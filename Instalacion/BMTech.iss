; Instalador de BMTech
; Se compila con Inno Setup 6 desde CrearDistribucion.ps1

#define MiApp "BMTech"
#define MiVersion "1.0"
#define Origen "..\Distribucion\BMTech"

[Setup]
AppId={{5E2B7C41-8A3D-4F6E-9B10-2C7D8E4F1A65}
AppName={#MiApp}
AppVersion={#MiVersion}
AppPublisher=Maite Benitez
DefaultDirName={autopf}\{#MiApp}
DefaultGroupName={#MiApp}
DisableProgramGroupPage=yes
OutputDir=..\Distribucion\Instalador
OutputBaseFilename=BMTech_Setup
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
UninstallDisplayName={#MiApp}

[Languages]
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "datosprueba"; Description: "Cargar datos de prueba (clientes, productos, una venta y ordenes de servicio)"; Flags: unchecked
Name: "escritorio"; Description: "Crear un acceso directo en el escritorio"

[Files]
Source: "{#Origen}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "SQLServer\SQLEXPR_x64_ESN.exe"; DestDir: "{tmp}"; Flags: dontcopy

[Icons]
Name: "{autoprograms}\{#MiApp}"; Filename: "{app}\mb506.BMTech.exe"
Name: "{autodesktop}\{#MiApp}"; Filename: "{app}\mb506.BMTech.exe"; Tasks: escritorio

[UninstallDelete]
Type: files; Name: "{app}\InstalacionBase.log"

[Run]
Filename: "{app}\mb506.BMTech.exe"; Description: "Abrir BMTech"; Flags: nowait postinstall skipifsilent

[Code]
var
  ServidorNuevo: String;

// .NET Framework 4.8 o posterior
function TieneNet48(): Boolean;
var
  Version: Cardinal;
begin
  Result := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', Version) and (Version >= 528040);
end;

// Alguna instancia de SQL Server instalada
function TieneSqlServer(): Boolean;
var
  Nombres: TArrayOfString;
begin
  Result := RegGetValueNames(HKLM64, 'SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL', Nombres) and (GetArrayLength(Nombres) > 0);
end;

function InitializeSetup(): Boolean;
begin
  Result := TieneNet48();
  if not Result then
    MsgBox('BMTech necesita .NET Framework 4.8. Instalelo desde Windows Update y vuelva a ejecutar el instalador.', mbError, MB_OK);
end;

// Instala SQL Server Express solo si el equipo no tiene SQL Server
function InstalarSqlServer(): Boolean;
var
  Codigo: Integer;
  Carpeta: String;
begin
  Result := True;
  ServidorNuevo := '';
  if TieneSqlServer() then exit;

  WizardForm.StatusLabel.Caption := 'Instalando SQL Server Express. Este paso puede tardar varios minutos...';
  ExtractTemporaryFile('SQLEXPR_x64_ESN.exe');
  Carpeta := ExpandConstant('{tmp}\sqlexpress');
  Exec(ExpandConstant('{tmp}\SQLEXPR_x64_ESN.exe'), '/q /x:"' + Carpeta + '"', '', SW_HIDE, ewWaitUntilTerminated, Codigo);
  Exec(Carpeta + '\SETUP.EXE',
    '/QS /ACTION=Install /FEATURES=SQLEngine /INSTANCENAME=SQLEXPRESS /IACCEPTSQLSERVERLICENSETERMS /UPDATEENABLED=False ' +
    '/SQLSYSADMINACCOUNTS="' + ExpandConstant('{computername}') + '\' + GetUserNameString() + '"',
    '', SW_SHOW, ewWaitUntilTerminated, Codigo);
  // 3010 indica que la instalacion termino y pide reiniciar
  Result := (Codigo = 0) or (Codigo = 3010);
  if Result then
    ServidorNuevo := '.\SQLEXPRESS'
  else
    MsgBox('No se pudo instalar SQL Server Express (codigo ' + IntToStr(Codigo) + '). Revise los registros en C:\Program Files\Microsoft SQL Server\160\Setup Bootstrap\Log.', mbError, MB_OK);
end;

// Crea la base BMTech con PrepararBase.ps1 y guarda el resultado en un archivo
procedure CrearBase();
var
  Codigo: Integer;
  Parametros: String;
begin
  WizardForm.StatusLabel.Caption := 'Creando la base de datos BMTech...';
  Parametros := '/c powershell -NoProfile -ExecutionPolicy Bypass -File "' + ExpandConstant('{app}\PrepararBase.ps1') + '"';
  if ServidorNuevo <> '' then
    Parametros := Parametros + ' -Servidor "' + ServidorNuevo + '"';
  if WizardIsTaskSelected('datosprueba') then
    Parametros := Parametros + ' -ConDatosDemo';
  Parametros := Parametros + ' > "' + ExpandConstant('{app}\InstalacionBase.log') + '" 2>&1';
  Exec(ExpandConstant('{cmd}'), Parametros, ExpandConstant('{app}'), SW_HIDE, ewWaitUntilTerminated, Codigo);
  if Codigo <> 0 then
    MsgBox('No se pudo crear la base de datos. El detalle esta en ' + ExpandConstant('{app}\InstalacionBase.log') + '.', mbError, MB_OK);
end;

procedure CurStepChanged(Paso: TSetupStep);
begin
  if Paso = ssPostInstall then
    if InstalarSqlServer() then
      CrearBase();
end;
