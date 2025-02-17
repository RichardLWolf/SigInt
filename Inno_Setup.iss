[Setup]
AppName=SigInt
AppVersion=1.0.0.1
DefaultDirName={autopf}\SigInt
DefaultGroupName=SigInt
OutputDir="N:\Repository\SigInt\bin\Installer"
OutputBaseFilename=SigIntSetup
SetupIconFile=N:\Repository\SigInt\SigInt_Icon.ico

[Files]
Source: "N:\Repository\SigInt\bin\Release\net8.0-windows\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\SigInt"; Filename: "{app}\SigInt.exe"

[Run]
Filename: "{app}\SigInt.exe"; Description: "Launch SigInt"; Flags: nowait postinstall
