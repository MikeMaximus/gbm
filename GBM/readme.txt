Game Backup Monitor v1.2.5 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

January 21, 2021

New in 1.2.5

All Platforms:

- Overhauled the user interface.
	- The "Start-Up Wizard" and "Add Game Wizard" instructions are now easier to read with a larger font.
	- Most buttons in the application are now larger with icons.
	- The redundant "Close" button has been removed from many forms.
	- The "Main Window" now has a collapsible game list with a search filter, this allows quicker access to features such backup, restore and game launching.
		- These new features can be disabled or hidden by default in Settings -> User Interface.
	- The "Game Manager" has been slimmed down for ease of use.
		- The size of the form has been reduced and the sections have been split into tabs.
		- Certain buttons and checkboxes have been moved into sub-menus.
	- The "Settings" form has been reorganized into more sections.
	- The "Quick Launch" feature introduced in v1.2.4 has been removed.
- Added the option to detect games using a window title instead of a process name.
	- This adds another option for detecting games that all run from the same executable. Such as emulated games, browser games, or cloud service games.
	- In addition to the process ending, GBM will also end monitoring when the window title no longer matches the game configuration.
	- This feature is not supported in Linux, see the "Known Issues" section for details.
- Added option to show a notification when a backup operation has been completed.
	- This option is available in the "Backup and Restore" section of the Settings window, it defaults to disabled.
	- A single notification will be shown when a backup operation is completed, even if the operation included multiple games.
- Added "Detection Speed" option.
	- Changes how quickly GBM detects games, faster settings have slightly higher CPU usage.
	- Allows users monitoring for hundreds or even thousands of games at once to greatly lower CPU usage at the cost of detection speed.
	- The average user with a normal sized game list should never need to adjust this setting, even the "Fast" setting will use less than 1% of the CPU.
	- This option is available in the "General" section of the Settings window, it defaults to "Fast".
- The "Icon" field now supports the use of Environment and Custom Path variables.
- The "Search" filter on the Main Window and Game Manager now supports filtering by tag.
	- Tags must be an exact text match.
- AbsolutePath is now a calculated field and no longer stored.
	- This will break official lists for prior versions of GBM (v1.1.5 - v1.2.4).  The last official list(s) compatible with these versions will be archived so they can still be accessed.
- Application settings are now handled more efficiently.
- Added missing code to properly update or delete existing launch data when certain configuration changes are made.
- Fixed issues with changing the Game ID when that configuration had existing backup files.

Windows:

- The icons from game executables are now cached after a session.
	- The "Main Window" and "Game Manager" will display the cached icon when viewing the game details, unless a custom icon has been set.
	- Cached icons are stored using the PNG format in the GBM temporary folder (can be customized in Settings).
	
Known Issues (Linux):

- Detecting games by window title does not work.
	- Mono hasn't implemented the "MainWindowTitle" property in System.Diagnostics.Process, so this feature is not supported in Linux at this time.
	
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html