;--------------------------------
; WARNINGS: 
; 1.  This script is only a template.  <DESTNAME>, <SOURCEDIR> and <VERSION> need to be set.
; 2.  UninstallLog.nsh needs to be added to the Include folder of the NSIS installation.
;-------------------------------- 

Unicode True
!include MUI2.nsh
!include UninstallLog.nsh

Name "Game Backup Monitor (32-bit)"
OutFile "<DESTNAME>"
InstallDir "$PROGRAMFILES\Game Backup Monitor"
InstallDirRegKey HKCU "Software\Game Backup Monitor (32-bit)" "Install Directory"
RequestExecutionLevel admin
SetCompressor /SOLID /FINAL lzma

!define MUI_ICON "<SOURCEDIR>\gbm.ico"
!define REG_ROOT "HKCU"
!define REG_APP_PATH "Software\Game Backup Monitor (32-bit)"
!define UNINSTALL_PATH "Software\Game Backup Monitor (32-bit)"
!define WINDOWS_UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\Game Backup Monitor (32-bit)"

Var StartMenuFolder

!define MUI_ABORTWARNING

!insertmacro MUI_PAGE_WELCOME
!define MUI_DIRECTORYPAGE_VARIABLE $INSTDIR
!insertmacro MUI_PAGE_DIRECTORY

!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\Game Backup Monitor (32-bit)"
!define MUI_STARTMENUPAGE_VALUENAME "Start Menu Folder"
!insertmacro MUI_PAGE_STARTMENU "Game" $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES


!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

;--------------------------------
; Configure UnInstall log to only remove what is installed
;-------------------------------- 
  ;Set the name of the uninstall log
    !define UninstLog "uninstall.log"
    Var UninstLog
 
  ;Uninstall log file missing.
    LangString UninstLogMissing ${LANG_ENGLISH} "${UninstLog} not found!$\r$\nUninstallation cannot proceed!"
 
  ;AddItem macro
    !define AddItem "!insertmacro AddItem"
 
  ;File macro
    !define File "!insertmacro File"
 
  ;CreateShortcut macro
    !define CreateShortcut "!insertmacro CreateShortcut"
 
  ;Copy files macro
    !define CopyFiles "!insertmacro CopyFiles"
 
  ;Rename macro
    !define Rename "!insertmacro Rename"
 
  ;CreateDirectory macro
    !define CreateDirectory "!insertmacro CreateDirectory"
 
  ;SetOutPath macro
    !define SetOutPath "!insertmacro SetOutPath"
 
  ;WriteUninstaller macro
    !define WriteUninstaller "!insertmacro WriteUninstaller"
 
  ;WriteRegStr macro
    !define WriteRegStr "!insertmacro WriteRegStr"
 
  ;WriteRegDWORD macro
    !define WriteRegDWORD "!insertmacro WriteRegDWORD" 
 
  Section -openlogfile
    CreateDirectory "$INSTDIR"
    IfFileExists "$INSTDIR\${UninstLog}" +3
      FileOpen $UninstLog "$INSTDIR\${UninstLog}" w
    Goto +4
      SetFileAttributes "$INSTDIR\${UninstLog}" NORMAL
      FileOpen $UninstLog "$INSTDIR\${UninstLog}" a
      FileSeek $UninstLog 0 END
  SectionEnd

Section "Game Installation" GameInstall

	${SetOutPath} "$INSTDIR"	
	${File} "<SOURCEDIR>" "GBM.exe"
	${File} "<SOURCEDIR>" "gbm.ico"
	${File} "<SOURCEDIR>" "Mono.Data.Sqlite.dll"
  ${File} "<SOURCEDIR>" "YamlDotNet.dll"
	${File} "<SOURCEDIR>" "readme.txt"
	${File} "<SOURCEDIR>" "sqlite3.dll"
	${CreateDirectory} "$INSTDIR\License"
	${SetOutPath} "$INSTDIR\License"
	${File} "<SOURCEDIR>\License" "7z license.txt"
	${File} "<SOURCEDIR>\License" "credits.txt"
	${File} "<SOURCEDIR>\License" "gpl-3.0.html"
	${File} "<SOURCEDIR>\License" "license.txt"
	${CreateDirectory} "$INSTDIR\zh"
	${SetOutPath} "$INSTDIR\zh"
	${File} "<SOURCEDIR>\zh" "GBM.resources.dll"
	${CreateDirectory} "$INSTDIR\Utilities"
	${CreateDirectory} "$INSTDIR\Utilities\x86"
	${SetOutPath} "$INSTDIR\Utilities\x86"
	${File} "<SOURCEDIR>\Utilities\x86" "7za.exe"

	${SetOutPath} "$INSTDIR"
	
	${WriteRegStr} "${REG_ROOT}" "${REG_APP_PATH}" "Install Directory" "$INSTDIR"
	${WriteRegStr} "${REG_ROOT}" "${UNINSTALL_PATH}" "UninstallString" "$INSTDIR\Uninstall.exe"
	
	WriteRegStr HKLM "${WINDOWS_UNINSTALL_PATH}" "DisplayName" "Game Backup Monitor (32-bit)"
	WriteRegStr HKLM "${WINDOWS_UNINSTALL_PATH}" "Publisher" "Michael J. Seiferling"
	WriteRegStr HKLM "${WINDOWS_UNINSTALL_PATH}" "HelpLink" "http://mikemaximus.github.io/gbm-web/manual.html"
	WriteRegStr HKLM "${WINDOWS_UNINSTALL_PATH}" "URLUpdateInfo" "https://github.com/MikeMaximus/gbm/releases"
	WriteRegStr HKLM "${WINDOWS_UNINSTALL_PATH}" "URLInfoAbout" "http://mikemaximus.github.io/gbm-web/"
	WriteRegStr HKLM "${WINDOWS_UNINSTALL_PATH}" "DisplayVersion" "<VERSION>"
	WriteRegStr HKLM "${WINDOWS_UNINSTALL_PATH}" "DisplayIcon" "$\"$INSTDIR\gbm.ico$\""
	WriteRegStr HKLM "${WINDOWS_UNINSTALL_PATH}" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
	WriteRegDWORD HKLM "${WINDOWS_UNINSTALL_PATH}" "EstimatedSize" "5120"
	WriteRegDWORD HKLM "${WINDOWS_UNINSTALL_PATH}" "NoModify" "1"
	WriteRegDWORD HKLM "${WINDOWS_UNINSTALL_PATH}" "NoRepair" "1"

	!insertmacro MUI_STARTMENU_WRITE_BEGIN Game
	${CreateDirectory} "$SMPROGRAMS\$StartMenuFolder"
	${CreateShortcut} "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "" ""
	${CreateShortcut} "$SMPROGRAMS\$StartMenuFolder\Game Backup Monitor (32-bit).lnk" "$INSTDIR\GBM.exe" "" "$INSTDIR\gbm.ico" ""
	!insertmacro MUI_STARTMENU_WRITE_END
	
	${WriteUninstaller} "$INSTDIR\Uninstall.exe"

SectionEnd

Section "Uninstall"

  ;Can't uninstall if uninstall log is missing!
  IfFileExists "$INSTDIR\${UninstLog}" +3
    MessageBox MB_OK|MB_ICONSTOP "$(UninstLogMissing)"
      Abort
 
  Push $R0
  Push $R1
  Push $R2
  SetFileAttributes "$INSTDIR\${UninstLog}" NORMAL
  FileOpen $UninstLog "$INSTDIR\${UninstLog}" r
  StrCpy $R1 -1
 
  GetLineCount:
    ClearErrors
    FileRead $UninstLog $R0
    IntOp $R1 $R1 + 1
    StrCpy $R0 $R0 -2
    Push $R0   
    IfErrors 0 GetLineCount
 
  Pop $R0
 
  LoopRead:
    StrCmp $R1 0 LoopDone
    Pop $R0
 
    IfFileExists "$R0\*.*" 0 +3
      RMDir $R0  #is dir
    Goto +9
    IfFileExists $R0 0 +3
      Delete $R0 #is file
    Goto +6
    StrCmp $R0 "${REG_ROOT} ${REG_APP_PATH}" 0 +3
      DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}" #is Reg Element
    Goto +3
    StrCmp $R0 "${REG_ROOT} ${UNINSTALL_PATH}" 0 +2
      DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}" #is Reg Element
 
    IntOp $R1 $R1 - 1
    Goto LoopRead
  LoopDone:
  FileClose $UninstLog
  Delete "$INSTDIR\${UninstLog}"
  DeleteRegKey HKLM "${WINDOWS_UNINSTALL_PATH}"
  RMDir "$INSTDIR"
  Pop $R2
  Pop $R1
  Pop $R0

SectionEnd