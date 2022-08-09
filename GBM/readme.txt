Game Backup Monitor v1.3.2 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

August 9, 2022

New in 1.3.2

All Platforms:

- Fixed an issue that could cause imports from the web to stop functioning after a failed download.
- Fixed an issue that caused recent changes to be lost if GBM was unexpectedly closed while working in certain windows, like the Game Manager.
- Fixed a crash that occured when the "Configuration Links" window is opened on the Game Manager while adding a new configuration.
- Fixed various issues when importing backup files:
	- Backup files created by v1.2.8 can now be imported without errors.
	- Fixed the broken 7z file filter when importing backup files.	
	- Fixed an issue that could cause ManifestID constraint errors.
	- Fixed an issue could cause differential backup imports to always fail.
- Changes to folder and file naming:
	- Removed the "Use Game ID for folder and file names" option from Settings.
	- Configurations using duplicate names will now use a more descriptive name format:
		- Game Name [Game ID] Ex. Metro Exodus [8ac11c11-8d18-471b-8f0d-aa154dc77f0e]	
	- The length of folder and file names is now more strict to minimize issues with the Windows max path limitation.
		- Only the first 64 characters of a game name will be used when creating folder and file names.
	- Modifying the name of a configuration will no longer automatically rename existing backup folders and files.
	- The above changes are only applied when create new backup files.
	- I understand the recent naming changes may be frustrating to some users.  Please leave any feedback in the GitHub issues or discussions section.
- Improvements to the Game Manager:
	- Added a "Copy" feature.
		- This will make a copy of the currently selected game configuration(s).
		- The Core Configuration, Game Information, Tags, Proccess and Configuration Links are included in the copy.
	- Added buttons to quickly open the current "Game Path" or "Save Path".
	- Some game configuration options have been renamed for clarification.
		- The "Monitor this game" option has been renamed to "Allow monitoring".
		- The "Monitor only" option has been renamed to "No backup when game ends".
	- The "Backup limit" and "Full backup interval" fields will no longer reset to 0 in the Game Manager if toggled off during an edit.
- Improvements to Importing Game Configurations:
	- The list will now try to retain the last scroll position after being refreshed.
	- You can now "Ignore" (or "Unignore") any configurations on the import window using the right-click menu.
		- This allows you to hide any configuration(s) that you never want to add or update during an import.
		- The ignored configurations are saved and persist between sessions.
		- You can show any hidden configurations by unticking the "Hide Ignored" checkbox.
	- Added new icons to indicate auto-detected and ignored configurations.
- General Interface improvements:
	- Added an "Open Backup Folder" option to the File menu (Main window and system tray).
	- You can now filter by "Tag" specifically in some search fields by using a hashtag in the search term. Ex. #Steam
		- In prior versions you didn't need to use a hashtag in the search term to filter by tag, but the results included both name and tag matches.
		- This feature is supported on the Main window, the Game Manager, and the Import window.		 		
	- All windows can now be closed and/or canceled out of edit mode by using the "Escape" key.
	- More windows will now ask for confirmation before closing if changes aren't yet saved.
	- Double-clicking a game in the main window list will now open it in the Game Manager.
	- You can now use the "Enter" key to confirm the "Ludusavi Options" window.	
- General performance improvements:
	- Some features that caused the interface to become unresponsive now run on a seperate thread.
	- Optimized various database queries.
			
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html