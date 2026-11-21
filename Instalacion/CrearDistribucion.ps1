param([string]$Servidor = '.\SQLEXPRESS')
$ErrorActionPreference = 'Stop'
$raiz = Split-Path $PSScriptRoot -Parent
$msbuild = Join-Path ${env:ProgramFiles} 'Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe'
if (-not (Test-Path -LiteralPath $msbuild)) { throw 'Se necesita MSBuild de Visual Studio 2022 para preparar la distribucion.' }
& $msbuild (Join-Path $raiz 'BMTech.sln') /t:Build /p:Configuration=Release /v:minimal /nologo
if ($LASTEXITCODE -ne 0) { throw 'La solucion no compilo.' }
$salida = Join-Path $raiz 'BMTech\bin\Release'
$destino = Join-Path $raiz 'Distribucion\BMTech'
if (Test-Path -LiteralPath $destino) { Remove-Item -LiteralPath $destino -Recurse -Force }
[void](New-Item -ItemType Directory -Path $destino)
Copy-Item -LiteralPath (Join-Path $salida 'mb506.BMTech.exe') -Destination $destino
Copy-Item -LiteralPath (Join-Path $salida 'mb506.BMTech.exe.config') -Destination $destino
Get-ChildItem -LiteralPath $salida -Filter '*.dll' | Copy-Item -Destination $destino
foreach ($archivo in @('Ayuda.html','Ayuda.css','Operacion.html','Mantenimiento.html')) {
    Copy-Item -LiteralPath (Join-Path $salida $archivo) -Destination $destino
}
$sql = Join-Path $destino 'BaseDatos'
[void](New-Item -ItemType Directory -Path $sql)
foreach ($archivo in @('CrearBaseBMTech.sql','DatosDePruebaBMTech.sql')) {
    Copy-Item -LiteralPath (Join-Path $raiz ('BaseDatos\'+$archivo)) -Destination $sql
}
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'PrepararBase.ps1') -Destination $destino
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'LEEME.txt') -Destination $destino
[xml]$config = Get-Content -LiteralPath (Join-Path $destino 'mb506.BMTech.exe.config') -Raw
$entrada = $config.SelectSingleNode('/configuration/appSettings/add[@key="ServidorSql"]')
$entrada.SetAttribute('value',$Servidor)
$config.Save((Join-Path $destino 'mb506.BMTech.exe.config'))
Write-Output ('Distribucion preparada: '+$destino)

# Instalador: BMTech.iss toma los archivos de Distribucion\BMTech y deja BMTech_Setup.exe en Distribucion\Instalador
if (-not (Test-Path -LiteralPath (Join-Path $PSScriptRoot 'SQLServer\SQLEXPR_x64_ESN.exe'))) { throw 'Falta Instalacion\SQLServer\SQLEXPR_x64_ESN.exe.' }
$iscc = Join-Path $env:LOCALAPPDATA 'Programs\Inno Setup 6\ISCC.exe'
if (-not (Test-Path -LiteralPath $iscc)) { $iscc = Join-Path ${env:ProgramFiles(x86)} 'Inno Setup 6\ISCC.exe' }
if (-not (Test-Path -LiteralPath $iscc)) { throw 'Se necesita Inno Setup 6 para generar el instalador.' }
& $iscc (Join-Path $PSScriptRoot 'BMTech.iss')
if ($LASTEXITCODE -ne 0) { throw 'No se pudo generar el instalador.' }
Write-Output ('Instalador generado: ' + (Join-Path $raiz 'Distribucion\Instalador\BMTech_Setup.exe'))
