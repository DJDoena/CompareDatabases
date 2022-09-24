[Setup]
AppName=Compare DVD Profiler Databases
AppId=CompareDatabases
AppVerName=Compare DVD Profiler Databases 2.0.0.2
AppCopyright=Copyright © Doena Soft. 2013 - 2022
AppPublisher=Doena Soft.
AppPublisherURL=http://doena-journal.net/en/dvd-profiler-tools/
DefaultDirName={commonpf32}\Doena Soft.\CompareDatabases
DefaultGroupName=Compare DVD Profiler Databases
DirExistsWarning=No
SourceDir=..\CompareDatabases\bin\x86\CompareDatabases
Compression=zip/9
AppMutex=InvelosDVDPro
OutputBaseFilename=CompareDatabasesSetup
OutputDir=..\..\..\..\CompareDatabasesSetup\Setup\CompareDatabases
MinVersion=0,6.1sp1
PrivilegesRequired=admin
WizardStyle=modern
DisableReadyPage=yes
ShowLanguageDialog=no
VersionInfoCompany=Doena Soft.
VersionInfoCopyright=2013 - 2022
VersionInfoDescription=Compare DVD Profiler Databases Setup
VersionInfoVersion=2.0.0.2
UninstallDisplayIcon={app}\djdsoft.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Messages]
WinVersionTooLowError=This program requires Windows XP or above to be installed.%n%nWindows 9x, NT and 2000 are not supported.

[Types]
Name: "full"; Description: "Full installation"

[Files]
Source: "djdsoft.ico"; DestDir: "{app}"; Flags: ignoreversion

Source: "DoenaSoft.DVDProfilerHelper.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.DVDProfilerXML.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "CompareDatabases.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "CompareDatabases.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.CompareDatabases.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.CompareDatabases.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.WindowsAPICodePack.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.WindowsAPICodePack.Shell.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.ToolBox.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "..\..\..\..\CompareDatabasesPlugin\bin\x86\CompareDatabasesPlugin\DoenaSoft.CompareDatabases.Plugin.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\..\..\CompareDatabasesPlugin\bin\x86\CompareDatabasesPlugin\DoenaSoft.CompareDatabases.Plugin.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\..\..\CompareDatabasesPlugin\bin\x86\CompareDatabasesPlugin\de\DoenaSoft.CompareDatabases.Plugin.resources.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "de\DoenaSoft.DVDProfilerHelper.resources.dll"; DestDir: "{app}\de"; Flags: ignoreversion
Source: "de\DoenaSoft.CompareDatabases.resources.dll"; DestDir: "{app}\de"; Flags: ignoreversion
Source: "de\DoenaSoft.CompareDatabases.resources.dll"; DestDir: "{app}\de"; Flags: ignoreversion

Source: "ReadMe\readme.html"; DestDir: "{app}\ReadMe"; Flags: ignoreversion

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\Compare DVD Profiler Databases"; Filename: "{app}\CompareDatabases.exe"; WorkingDir: "{app}"; IconFilename: "{app}\djdsoft.ico"
Name: "{userdesktop}\Compare DVD Profiler Databases"; Filename: "{app}\CompareDatabases.exe"; WorkingDir: "{app}"; IconFilename: "{app}\djdsoft.ico"

[Run]
Filename: "{win}\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"; Parameters: "/codebase ""{app}\DoenaSoft.CompareDatabases.Plugin.dll"""; Flags: runhidden

;[UninstallDelete]

[UninstallRun]
Filename: "{win}\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"; Parameters: "/u ""{app}\DoenaSoft.CompareDatabases.Plugin.dll"""; Flags: runhidden

[Registry]
; Register - Cleanup ahead of time in case the user didn't uninstall the previous version.
Root: HKCR; Subkey: "CLSID\{{01CD7B58-DD10-47D7-B15A-899431CF5157}"; Flags: dontcreatekey deletekey
Root: HKCR; Subkey: "DoenaSoft.DVDProfiler.CompareDatabases.Plugin"; Flags: dontcreatekey deletekey
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler\Plugins\Identified"; ValueType: none; ValueName: "{{01CD7B58-DD10-47D7-B15A-899431CF5157}"; ValueData: "0"; Flags: deletevalue
Root: HKLM; Subkey: "Software\Classes\CLSID\{{01CD7B58-DD10-47D7-B15A-899431CF5157}"; Flags: dontcreatekey deletekey
Root: HKLM; Subkey: "Software\Classes\DoenaSoft.DVDProfiler.CompareDatabases.Plugin"; Flags: dontcreatekey deletekey
; Unregister
Root: HKCR; Subkey: "CLSID\{{01CD7B58-DD10-47D7-B15A-899431CF5157}"; Flags: dontcreatekey uninsdeletekey
Root: HKCR; Subkey: "DoenaSoft.DVDProfiler.CompareDatabases.Plugin"; Flags: dontcreatekey uninsdeletekey
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler\Plugins\Identified"; ValueType: none; ValueName: "{{01CD7B58-DD10-47D7-B15A-899431CF5157}"; ValueData: "0"; Flags: uninsdeletevalue
Root: HKLM; Subkey: "Software\Classes\CLSID\{{01CD7B58-DD10-47D7-B15A-899431CF5157}"; Flags: dontcreatekey uninsdeletekey
Root: HKLM; Subkey: "Software\Classes\DoenaSoft.DVDProfiler.CompareDatabases.Plugin"; Flags: dontcreatekey uninsdeletekey

[Code]
function IsDotNET40Detected(): boolean;
// Function to detect dotNet framework version 4.0
// Returns true if it is available, false it's not.
var
dotNetStatus: boolean;
begin
dotNetStatus := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4');
Result := dotNetStatus;
end;

function InitializeSetup(): Boolean;
// Called at the beginning of the setup package.
begin

if not IsDotNET40Detected then
begin
MsgBox( 'The Microsoft .NET Framework version 4.0 is not installed. Please install it and try again.', mbInformation, MB_OK );
Result := false;
end
else
Result := true;
end;

