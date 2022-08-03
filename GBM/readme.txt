Game Backup Monitor v1.3.2 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

August 3, 2022

New in 1.3.2

Disclaimer:

v1.3.2 is still in development, this file will be updated as changes are made.
Database changes made in development builds may not be compatible with the final release.  Use at your own risk!

All Platforms:

- Fixed an issue that could cause imports from the web to stop functioning after a failed download.
- Fixed an issue that caused changes to be lost if GBM was closed while actively working in certain forms like the Game Manager.
- Fixed an issue with 7z files not being displayed when importing backup files.
- Fixed a crash that occured when the "Configuration Links" window is opened on the Game Manager while adding a new configuration.
- Configurations that use duplicate names will now have more descriptive folder and file names.  Ex. Metro Exodus [8ac11c11-8d18-471b-8f0d-aa154dc77f0e]
- Added a "Copy" feature to the Game Manager.
	- This will make a copy of the currently selected game configuration.
	- The Core Configuration, Game Information, Tags, Proccesses and Configuration Links are included in the copy.
- Performance improvements:
	- Some functions that blocked the UI now run on a seperate thread.
	- Optimized various database queries.
- Interface improvements:
	- All windows can now be closed and/or canceled out of edit mode by using the "Escape" key.
	- More windows will now ask for confirmation before closing if changes aren't yet saved.
	- Double-clicking a game in the main window list will now open it in the Game Manager.
	- You can now use the "Enter" key to confirm the "Ludusavi Options" window.
	- You can now filter by "Tag" specifically in some search fields by using a hashtag in the search term. Ex. #Steam
		- In prior versions you didn't need to use a hashtag in the search term to filter by tag, but the results included both name and tag matches.
		- This feature is supported on the main window, the Game Manager, and the Import window.
	- Some configuration options have been renamed for clarification.
		- The "Monitor this game" option has been renamed to "Allow monitoring".
		- The "Monitor only" option has been renamed to "No backup when game ends".
	- Added buttons to open the current "Game Path" or "Save Path" in the Game Manager.
	- Added "Open Backup Folder" option to the File menu and system tray menu.
	- The "Backup limit" and "Full backup interval" fields will no longer reset to 0 in the Game Manager if toggled off during an edit.		
- Improvements to importing game configurations:
	- You can now "Ignore" (and "Unignore") any configurations on the import screen using the right-click menu.
		- This allows you to hide any configuration(s) that you never want to add or update during an import.
		- The ignored configurations are saved and persist between sessions.
		- You can show any hidden configurations by unticking the "Hide Ignored" checkbox.
	- Added new icons to indicate auto-detected and ignored configurations.
			
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html