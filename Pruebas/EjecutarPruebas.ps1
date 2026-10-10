$ErrorActionPreference = 'Stop'
$raiz = Split-Path $PSScriptRoot -Parent
$salida = Join-Path $raiz 'BMTech\bin\Debug'
$compilador = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$roslyn = Join-Path ${env:ProgramFiles} 'Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\Roslyn\csc.exe'
if (Test-Path -LiteralPath $roslyn) { $compilador = $roslyn }
$destino = Join-Path $salida 'PruebasBasicas.exe'
& $compilador /nologo /target:exe "/out:$destino" "/r:$salida\mb506.BEBMTech.dll" "/r:$salida\mb506.BLLBMTech.dll" "/r:$salida\mb506.ServiciosBMTech.dll" (Join-Path $PSScriptRoot 'PruebasBasicas.cs')
if ($LASTEXITCODE -ne 0) { throw 'No se pudieron compilar las pruebas. Compile BMTech.sln primero.' }
Copy-Item -LiteralPath (Join-Path $salida 'mb506.BMTech.exe.config') -Destination ($destino + '.config') -Force
& $destino
if ($LASTEXITCODE -ne 0) { throw 'Hay una prueba fallida.' }
$exportaciones = Join-Path $salida 'PruebasExportacion.exe'
& $compilador /nologo /target:exe "/out:$exportaciones" "/r:$salida\mb506.BMTech.exe" "/r:$salida\mb506.BEBMTech.dll" "/r:$salida\PdfSharp-gdi.dll" (Join-Path $PSScriptRoot 'PruebasExportacion.cs')
if ($LASTEXITCODE -ne 0) { throw 'No se pudieron compilar las pruebas de exportacion.' }
Copy-Item -LiteralPath (Join-Path $salida 'mb506.BMTech.exe.config') -Destination ($exportaciones + '.config') -Force
& $exportaciones
if ($LASTEXITCODE -ne 0) { throw 'Hay una prueba de exportacion fallida.' }
