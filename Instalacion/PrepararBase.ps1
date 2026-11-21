param(
    [string]$Servidor = '',
    [string]$BaseDatos = 'BMTech',
    [string]$DirectorioAplicacion = $PSScriptRoot,
    [switch]$ConDatosDemo
)
$ErrorActionPreference = 'Stop'
if ($BaseDatos -ne 'BMTech' -and $BaseDatos -notmatch '^BMTech_PruebasEntrega3_[a-f0-9]{12}$') {
    throw 'Solo se admite BMTech o una base aislada de las pruebas de Entrega 3.'
}
$dll = Join-Path $DirectorioAplicacion 'mb506.DALBMTech.dll'
$config = Join-Path $DirectorioAplicacion 'mb506.BMTech.exe.config'
if (-not (Test-Path -LiteralPath $dll) -or -not (Test-Path -LiteralPath $config)) { throw 'Faltan los archivos de la aplicacion.' }
$servidorAnterior = $env:BMTECH_SQL_SERVER
$baseAnterior = $env:BMTECH_SQL_DATABASE
$conexion = $null
$transaccion = $null
try {
    if ([string]::IsNullOrWhiteSpace($Servidor)) {
        $instancias = @()
        foreach ($ruta in @('HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL','HKLM:\SOFTWARE\WOW6432Node\Microsoft\Microsoft SQL Server\Instance Names\SQL')) {
            if (Test-Path -LiteralPath $ruta) {
                $clave = Get-Item -LiteralPath $ruta
                foreach ($nombre in $clave.GetValueNames()) {
                    $instancia = if ($nombre -eq 'MSSQLSERVER') { $env:COMPUTERNAME } else { $env:COMPUTERNAME + '\' + $nombre }
                    if ($instancias -notcontains $instancia) { $instancias += $instancia }
                }
            }
        }
        $disponibles = @()
        foreach ($instancia in $instancias) {
            $prueba = New-Object System.Data.SqlClient.SqlConnection("Server=$instancia;Database=master;Integrated Security=True;Connect Timeout=3")
            try { $prueba.Open(); $disponibles += $instancia } catch {} finally { $prueba.Dispose() }
        }
        if ($disponibles.Count -eq 0) { throw 'No se encontro una instancia SQL Server accesible. Instale o inicie SQL Server.' }
        # Con varias instancias se usa la principal, despues SQLEXPRESS y si no la primera
        $Servidor = $disponibles[0]
        if ($disponibles -contains ($env:COMPUTERNAME + '\SQLEXPRESS')) { $Servidor = $env:COMPUTERNAME + '\SQLEXPRESS' }
        if ($disponibles -contains $env:COMPUTERNAME) { $Servidor = $env:COMPUTERNAME }
    }
    $cadena = New-Object System.Data.SqlClient.SqlConnectionStringBuilder
    $cadena['Data Source'] = $Servidor
    $cadena['Initial Catalog'] = 'master'
    $cadena['Integrated Security'] = $true
    $cadena['Connect Timeout'] = 10
    $conexion = New-Object System.Data.SqlClient.SqlConnection($cadena.ConnectionString)
    $conexion.Open()
    $command = $conexion.CreateCommand()
    $command.CommandText = 'SELECT COUNT(*) FROM sys.databases WHERE name=@nombre'
    [void]$command.Parameters.AddWithValue('@nombre',$BaseDatos)
    $existe = [int]$command.ExecuteScalar() -eq 1
    $command.Parameters.Clear()
    if ($existe) {
        throw 'La base ya existe. La instalacion nueva requiere una base inexistente y no reemplaza datos.'
    } else {
        $command.CommandText = "CREATE DATABASE [$BaseDatos]"
        [void]$command.ExecuteNonQuery()
        $conexion.ChangeDatabase($BaseDatos)
        $carpetaSql = Join-Path $DirectorioAplicacion 'BaseDatos'
        # La base ya se creo arriba: del script se usa desde USE [BMTech] en adelante
        $estructura = [IO.File]::ReadAllText((Join-Path $carpetaSql 'CrearBaseBMTech.sql'))
        $inicio = $estructura.IndexOf('USE [BMTech];')
        if ($inicio -lt 0) { throw 'No se reconoce el script de la base.' }
        $scripts = @($estructura.Substring($inicio))
        if ($ConDatosDemo) { $scripts += [IO.File]::ReadAllText((Join-Path $carpetaSql 'DatosDePruebaBMTech.sql')) }
        $transaccion = $conexion.BeginTransaction()
        foreach ($script in $scripts) {
            $script = $script.Replace('USE [BMTech];', "USE [$BaseDatos];")
            foreach ($lote in [regex]::Split($script,'(?im)^\s*GO\s*(?:--[^\r\n]*)?$')) {
                if ([string]::IsNullOrWhiteSpace($lote)) { continue }
                $command = $conexion.CreateCommand()
                $command.Transaction = $transaccion
                $command.CommandText = $lote
                $command.CommandTimeout = 60
                [void]$command.ExecuteNonQuery()
            }
        }
        $transaccion.Commit()
        $transaccion.Dispose()
        $transaccion = $null
        $env:BMTECH_SQL_SERVER = $Servidor
        $env:BMTECH_SQL_DATABASE = $BaseDatos
        [void][Reflection.Assembly]::LoadFrom($dll)
        (New-Object mb506.DALBMTech.DALIntegridad).mb506RegenerarSistema()
        Write-Output 'Base nueva creada. Un administrador y un empleado, con clave inicial 1234.'
    }
    [xml]$xml = [IO.File]::ReadAllText($config)
    if ($null -eq $xml.configuration.appSettings) {
        [void]$xml.configuration.AppendChild($xml.CreateElement('appSettings'))
    }
    $entrada = $xml.SelectSingleNode('/configuration/appSettings/add[@key="ServidorSql"]')
    if ($null -eq $entrada) {
        $entrada = $xml.CreateElement('add')
        $entrada.SetAttribute('key','ServidorSql')
        [void]$xml.configuration.appSettings.AppendChild($entrada)
    }
    $entrada.SetAttribute('value',$Servidor)
    $xml.Save($config)
    Write-Output ('Configuracion guardada. Servidor: ' + $Servidor)
} catch {
    if ($null -ne $transaccion) { try { $transaccion.Rollback() } catch {} }
    throw
} finally {
    if ($null -ne $transaccion) { $transaccion.Dispose() }
    if ($null -ne $conexion) { $conexion.Dispose() }
    $env:BMTECH_SQL_SERVER = $servidorAnterior
    $env:BMTECH_SQL_DATABASE = $baseAnterior
}
