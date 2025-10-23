Game Backup Monitor v1.4.3 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

October 23, 2025

New in 1.4.3

General:

- Added the ability to run GBM in a fully portable mode.	
	- In portable mode, GBM will store all user and temporary data in the same location as the app instead of using the current user's OS profile folder.
	- To enable portable mode, create an empty text file named "portable.ini" (case-sensitive depending on the OS) in the application folder.
		- You MUST have write permissions to the application folder for portable mode to function.
		- This works in both Windows and Linux.		
		- Delete the "portable.ini" file to return to the default mode.
	- The Start-Up Wizard will now use the current app folder as a default backup location when running in portable mode.
	- GBM will NOT automatically move your data when switching between the default mode and portable mode.	
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html