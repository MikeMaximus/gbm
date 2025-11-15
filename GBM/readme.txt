Game Backup Monitor v1.4.3 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

November 15, 2025

New in 1.4.3

General:

- Added the ability to run GBM in a fully portable mode.  Supports Windows and Linux.
	- In portable mode, GBM will store all user and temporary data in the same location as the app instead of using the user's OS profile folder.
	- To enable portable mode, create an empty text file named "portable.ini" (case-sensitive depending on the OS) in the application folder.
		- You MUST have write permissions to the application folder for portable mode to function.		
		- Delete the "portable.ini" file to return to the default mode.
	- The Start-Up Wizard will now use the current app folder as a default backup location when running in portable mode.
	- GBM will NOT automatically move your data when switching between the default mode and portable mode.
- Fixed the bad coding practices in GBM's ancient initialization sequence.	
- Updated Components
	- YamlDotNet 16.1.3 -> 16.3.0
	- SQLite 3.50.2 -> 3.50.4 (Windows)
	- 7-Zip 25.00 -> 25.01 (Windows)
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html