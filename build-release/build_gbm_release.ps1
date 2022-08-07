#Script Name: Game Backup Monitor - Release Package Builder
#Author: Michael J. Seiferling
#Revised: August 3, 2022
#Warning: This script only prepares Windows packages for release, it does not build GBM.

$7z_bin = "C:\Program Files\7-Zip\7z.exe"
$nsis_bin = "C:\Program Files (x86)\NSIS\makensis.exe"

$release_version = "1.3.2"
$base_project = "E:\Projects\gbm"
$x86_build_folder = $base_project + "\GBM\bin\x86\Release"
$x64_build_folder = $base_project + "\GBM\bin\x64\Release"
$base_release_folder = "$env:USERPROFILE\Desktop\$release_version"

$x86_7z_filename = "GBM.v$release_version.32-bit.7z"
$x64_7z_filename = "GBM.v$release_version.64-bit.7z"
$x86_installer_filename = "GBM.v$release_version.32-bit.Installer.exe"
$x64_installer_filename = "GBM.v$release_version.64-bit.Installer.exe"
$checksum_filename = "checksums"

$x86_7z_release_folder =  $base_release_folder + "\x86_7z"
$x64_7z_release_folder = $base_release_folder + "\x64_7z"
$x86_installer_release_folder =  $base_release_folder + "\x86_installer"
$x64_installer_release_folder = $base_release_folder + "\x64_installer"

$delete_files = "GBM.exe.config", "gbm.xml", "gbm.pdb", "YamlDotNet.xml"
$linux_files = "gbm.desktop", "gbm.sh", "makefile"

#Copy and Prepare Releases (x86)

Remove-Item -Recurse -Force $x86_7z_release_folder
md -Force $x86_7z_release_folder
Get-ChildItem -Path $x86_build_folder | Copy-Item -Destination $x86_7z_release_folder -Recurse -Container -Force
Set-Location $x86_7z_release_folder
Remove-Item $delete_files
Remove-Item -Recurse -Force $x86_installer_release_folder
md -Force $x86_installer_release_folder
Get-ChildItem -Path $x86_7z_release_folder | Copy-Item -Destination $x86_installer_release_folder -Recurse -Container -Force

#Prepare Linux Content (x86)

Get-ChildItem -Path $base_project\* -Include $linux_files | Copy-Item -Destination $x86_7z_release_folder -Recurse -Container -Force
Copy-Item $base_project\deb-package -Destination $x86_7z_release_folder -Recurse -Container -Force
Set-Location $x86_7z_release_folder\deb-package\gbm\DEBIAN
(Get-Content -path control -Raw) -replace '<MAINTAINER>','Michael J. Seiferling <mseiferling@gmail.com>' | Set-Content control -NoNewline
(Get-Content -path control -Raw) -replace '<VERSION>',"$release_version" | Set-Content control -NoNewline

#Copy and Prepare Releases (x64)

Remove-Item -Recurse -Force $x64_7z_release_folder
md -Force $x64_7z_release_folder
Get-ChildItem -Path $x64_build_folder | Copy-Item -Destination $x64_7z_release_folder -Recurse -Container -Force
Set-Location $x64_7z_release_folder
Remove-Item $delete_files
Remove-Item -Recurse -Force $x64_installer_release_folder
md -Force $x64_installer_release_folder
Get-ChildItem -Path $x64_7z_release_folder | Copy-Item -Destination $x64_installer_release_folder -Recurse -Container -Force

#Prepare Linux Content (x64)

Get-ChildItem -Path $base_project\* -Include $linux_files | Copy-Item -Destination $x64_7z_release_folder -Recurse -Container -Force
Copy-Item $base_project\deb-package -Destination $x64_7z_release_folder -Recurse -Container -Force
Set-Location $x64_7z_release_folder\deb-package\gbm\DEBIAN
(Get-Content -path control -Raw) -replace '<MAINTAINER>','Michael J. Seiferling <mseiferling@gmail.com>' | Set-Content control -NoNewline
(Get-Content -path control -Raw) -replace '<VERSION>',"$release_version" | Set-Content control -NoNewline

#Build 7z Release Packages

& $7z_bin a -t7z -mx9 $base_release_folder\$x86_7z_filename $x86_7z_release_folder\*
& $7z_bin a -t7z -mx9 $base_release_folder\$x64_7z_filename $x64_7z_release_folder\*

#Prepare NSIS Installer Scripts
Set-Location $base_release_folder
Copy-Item $base_project\build-release\*.nsi .
(Get-Content -path x86.nsi -Raw) -replace '<DESTNAME>',"$x86_installer_filename" | Set-Content x86.nsi
(Get-Content -path x86.nsi -Raw) -replace '<VERSION>',"$release_version" | Set-Content x86.nsi
(Get-Content -path x86.nsi -Raw) -replace '<SOURCEDIR>',"$x86_installer_release_folder" | Set-Content x86.nsi

(Get-Content -path x64.nsi -Raw) -replace '<DESTNAME>',"$x64_installer_filename" | Set-Content x64.nsi
(Get-Content -path x64.nsi -Raw) -replace '<VERSION>',"$release_version" | Set-Content x64.nsi
(Get-Content -path x64.nsi -Raw) -replace '<SOURCEDIR>',"$x64_installer_release_folder" | Set-Content x64.nsi

#Build Installers
& $nsis_bin x86.nsi
& $nsis_bin x64.nsi

#Checksums
$FileHash = Get-FileHash $base_release_folder\$x86_7z_filename
$FileHash.Hash + " *" + $x86_7z_filename + "`r`n" | Set-Content -NoNewline $base_release_folder\$checksum_filename
$FileHash = Get-FileHash $base_release_folder\$x64_7z_filename
$FileHash.Hash + " *" + $x64_7z_filename + "`r`n" | Add-Content -NoNewline $base_release_folder\$checksum_filename
$FileHash = Get-FileHash $base_release_folder\$x86_installer_filename
$FileHash.Hash + " *" + $x86_installer_filename + "`r`n" | Add-Content -NoNewline $base_release_folder\$checksum_filename
$FileHash = Get-FileHash $base_release_folder\$x64_installer_filename
$FileHash.Hash + " *" + $x64_installer_filename + "`r`n" | Add-Content -NoNewline $base_release_folder\$checksum_filename