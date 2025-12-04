Game Backup Monitor v1.4.3 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

December 4, 2025

New in 1.4.3

General:

- Added Japanese language support.
	- Thank you @nihongo-helper0119 for providing the Japanese translation.
- Added the ability to manually set the user interface language.
	- GBM will use your operating system language by default.
	- The Settings -> User Interface panel now allows you to choose from any of the available languages. 
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
	- The installer packages aren't meant for use with portable mode and do not include the toggle scripts.	
- Changed how GBM starts linked processes.
	- You can now set a start-up delay for each linkable process in the Process Manager.
	- Processes are now started on a seperate thread.
- Updated the Start-Up Wizard.
	- You can now choose the UI language on the first step.
	- The application folder is suggested as a default backup location if running in portable mode.
- Fixed some long standing issues with GBM's initialization sequence.
- Changed the size of some controls and forms to better support multiple languages.	
- Updated Components
	- YamlDotNet 16.1.3 -> 16.3.0
	- SQLite 3.50.2 -> 3.50.4 (Windows)
	- 7-Zip 25.00 -> 25.01 (Windows)

Known Issues:

- GBM may not properly display characters for a language that does not match the current desktop language while running in Mono (Linux).  To resolve this issue:
	1.  Set the LANG environment variable to the language you wish to use before running GBM.  Ex. LANG=ja_JP.utf-8
	2.  Ensure you have a language/font package installed for the language you wish to use.
	3.  Set your desktop language to the language you wish to use in GBM.
	4.  Consult the documentation for your desktop/linux distribution for more help.
- Some buttons and/or other controls still appear in English while GBM is set to another language.  To resolve this issue:
	1.  Set your operating system/desktop language to the language you wish to use in GBM. 
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html