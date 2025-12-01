Game Backup Monitor v1.4.3 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

December 1, 2025

New in 1.4.3

General:

- Added Japanese language support.
	- Thank you @nihongo-helper0119 for providing the Japanese translation.
- Added the ability to manually set the user interface language.
	- GBM will use your operating system language by default.
	- The Settings -> User Interface panel now allows you to manually set one of the available languages. (Operating System (Default), English, Chinese Simplified, Japanese).
- Added the ability to run GBM in a fully portable mode.
	- In portable mode, GBM will store all user and temporary data in the same location as the app instead of using the user's OS profile folder.
	- You must have write permissions to the application folder for GBM to start in portable mode.
	- GBM does not automatically move any of your existing data when switching between normal mode and portable mode.
	- To enable portable mode:
		- Run the "Toggle Portable Mode" script in the application folder, then start GBM.
			- "Toggle Portable Mode.bat" is for Windows and "Toggle Portable Mode.sh" is for Linux.
		- You can also manually create an empty file named "portable.ini" (case-sensitive depending on the OS) in the application folder to enable portable mode.				
			- Delete the "portable.ini" file to return to normal mode.
	- A new indicator on the main window status bar shows which mode you are currently in.
- The Start-Up Wizard will now allow you to choose a language.
- The Start-Up Wizard will now suggest the application folder as a default backup location when running in portable mode.	
- Fixed some long standing issues with GBM's initialization sequence.
- A GUI update pass was done across the entire application to better support multiple languages.
	- All buttons are now slightly larger.
	- Some forms have increased in size.
- Updated Components
	- YamlDotNet 16.1.3 -> 16.3.0
	- SQLite 3.50.2 -> 3.50.4 (Windows)
	- 7-Zip 25.00 -> 25.01 (Windows)

Known Issues:

- GBM may not properly display the characters for a language that does not match the operating system language while running on Mono (Linux).  To solve this issue:
	- Set your operating system language to the desired language, then restart GBM.
	- Consult the documentation for your Window Manager and/or Linux distribution for help.
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html