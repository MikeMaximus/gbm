Game Backup Monitor v1.2.5 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

December 31, 2020

New in 1.2.5

Disclaimer:

v1.2.5 is still in development.  This file will be updated as changes are made.

All Platforms:

- Visit https://github.com/MikeMaximus/gbm/discussions/209 for details on the GUI overhaul currently in progress.
	- Overhaul of the Main Window interface
	- Overhaul of the Game Manager interface
- Added the option to detect games using a window title instead of a process name
	- This adds another option for detecting games that all run from the same executable. Such as emulated games, browser games, or cloud service games.
	- In addition to the process ending, GBM will also end monitoring when the window title no longer matches the game configuration.
- Added option to show a notification when a backup operation has been completed.
	- This option is available in the "Backup and Restore" section of the Settings window, it defaults to disabled.
	- Shows a single notification when a backup operation is completed, even if the operation included multiple games.
- Added "Detection Speed" option
	- This option is available in the "General" section of the Settings window, it defaults to "Fast".
	- Changes how quickly GBM detects games, faster settings have a slightly higher CPU usage.
	- Allows those monitoring for hundreds or even thousands of games at once to greatly lower CPU usage at the cost of detection speed.
	- The average user with a normal sized game list should never need to adjust this setting, even the "Fast" setting will use less than 1% of the CPU.
- AbsolutePath is now a calculated field and no longer stored.
	- This is a bigger change than it seems, but I feel it was worth it.  It never should have been stored and this will clean up some issues and prepare for future updates.
	- This will break future official lists for prior versions of GBM (v1.1.5 - v1.2.4).  The last official list(s) compatible with these prior versions will be archived so they can still be accessed.
- "Search" on the Main Window and Game Manager now supports searching by tag.
- Application settings are now handled more efficiently.
- Added missing code to properly update or delete existing launch data when certain configuration changes are made.
- Fixed issues with changing a configuration identifier when that configuration had existing backup files.

Windows:

- The icons from game executables are now cached after a session.
	- The main window and game manager will display the cached icon when viewing the game details, unless a custom icon has been set.
	- Cached icons are stored using the PNG format in the GBM temporary folder (can be customized in Settings).
	
Known Issues (Linux):

- Detecting games by window title does not work.
	- Mono hasn't implemented the "MainWindowTitle" property in System.Diagnostics.Process, so this feature is not supported in Linux at this time.
	
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html