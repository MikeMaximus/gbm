Game Backup Monitor v1.1.8 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

March 10, 2019

New in 1.1.8

All Platforms:

- Added the ability to backup and restore saved games that are stored in the Windows registry:
	- This feature is also supported in Linux for Windows games running in Wine/Proton.
	- Please see the online manual for more details on how to use this feature.
- On startup, GBM now waits up to one minute for the current backup location to become available before displaying an error message.
	- The error message can now be canceled to continue waiting.
	- This is useful for delayed network shares on startup or if you forgot to plug in an external backup drive.
- The "Choose Game" window now displays tags in addition to the game name when multiple configurations are detected.
- The "Backup Confirmation" window should now always get top focus after a gaming session ends.
- The Game Manager and Custom Variable Manager now use a different folder browser where applicable.	
	- The new browser provides better usability for power users, other areas of the app will still use the classic folder browser.
	- The new browser allows Linux users to "Show Hidden Folders", which can't be done using the classic Folder Browser.
- When a UNC path is used for the backup folder, the disk space check prior to backups will be automatically disabled because it cannot be done.
- The disk space check prior to backups can now be disabled in the "Backup and Restore" settings.	
- Added "Operating System" field to game configurations:
	- "Windows" and "Linux" are the only valid choices at this time and only Linux users are allowed to change this field.
	
Windows:

- Updated SQLite to 3.27.2
- Updated 7-Zip to 19.00

Linux:

- You can now set GBM to automatically start on log-in via the "Startup" settings.
	- This requires that GBM be installed via a package manager or the makefile, so any required files are in a known location.  The option will be disabled otherwise.
- The "Start Minimized" option is now available in "Startup" settings.
- GBM now stores all Wine configuration data seperately from the core game configuration:
	- The core configuration is no longer altered in any way when a game is detected running in Wine or Proton.
	- The Wine configuration data is now automatically updated each time a game is detected, such as when running the game from a new prefix.
	- This data can be viewed and manually modified on the Game Manager.
- Fixed the handling of Wine/Proton games when multiple configurations are detected.
- Fixed a crash when using "Backup Only" and "New Backups Pending" filters on the Game Manager.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html