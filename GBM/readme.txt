Game Backup Monitor v1.4.3 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

November 28, 2025

New in 1.4.3

General:

- Added Japanese language support.
	- Thank you @nihongo-helper0119 for providing the Japanese translation.
	- The GUI language is chosen based on your operating system language, it cannot currently be set manually. (I may add a manual language setting before releasing v143.)
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
- Fixed the poor coding practices in the app's initialization sequence.
- A GUI update pass was done across the entire application for better support of multiple languages.
	- All buttons are now generally larger.
	- Some forms have slightly increased in size.
- Updated Components
	- YamlDotNet 16.1.3 -> 16.3.0
	- SQLite 3.50.2 -> 3.50.4 (Windows)
	- 7-Zip 25.00 -> 25.01 (Windows)
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html