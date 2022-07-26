Game Backup Monitor v1.3.2 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

July 26, 2022

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
- Added a "Lock" feature to the Game Manager.
	- Locked configurations cannot be modified until it is "Unlocked".
	- Locked configurations are ignored when updates are available during an import.
- Added a "Copy" feature to the Game Manager.
	- This will make a copy of the currently selected game configuration.
	- The Core Configuration, Game Information, Tags, Proccesses and Configuration Links are included in the copy.
- Performance Improvements:
	- Some functions that blocked the UI now run on a seperate thread.
	- Optimized various database queries.
- Interface Improvements:
	- All windows can now be closed and/or canceled out of edit mode by using the "Escape" key.
	- More windows will now ask for confirmation before closing if changes aren't yet saved.
	- You can now use the "Enter" key to confirm the "Ludusavi Options" window.
	- You can now filter by "Tag" specifically in some search fields by using a hashtag in the search term. Ex. #Steam
		- In prior versions you didn't need to use a hashtag in the search term to filter by tag, but the results included both name and tag matches.
		- This feature is supported on the main window, the Game Manager, and the Import window.
	- Some options have been renamed for clarification.
		- The "Monitor this game" option has been renamed to "Allow monitoring".
		- The "Monitor only" option has been renamed to "No backup when game ends".
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html