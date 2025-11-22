Game Backup Monitor v1.4.3 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

November 22, 2025

New in 1.4.3

General:

- Added the ability to run GBM in a fully portable mode.  Supports Windows and Linux.
	- In portable mode, GBM will store all user and temporary data in the same location as the app instead of using the user's OS profile folder.
	- You MUST have write permissions to the application folder for portable mode to function.
	- GBM does not automatically move any of your existing data when switching between normal mode and portable mode.
	- To enable portable mode:
		- Run the "Toggle Portable Mode" script in the application folder, then start GBM.
			- "Toggle Portable Mode.bat" is for Windows and "Toggle Portable Mode.sh" is for Linux.
		- You can also manually create an empty file named "portable.ini" (case-sensitive depending on the OS) in the application folder to enable portable mode.				
			- Delete the "portable.ini" file to return to normal mode.
- The Start-Up Wizard will now suggest the application folder as a default backup location when running in portable mode.	
- Fixed the bad coding practices in GBM's ancient initialization sequence.	
- Updated Components
	- YamlDotNet 16.1.3 -> 16.3.0
	- SQLite 3.50.2 -> 3.50.4 (Windows)
	- 7-Zip 25.00 -> 25.01 (Windows)
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html