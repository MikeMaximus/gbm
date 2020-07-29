Game Backup Monitor v1.2.2 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

August 1, 2020

New in 1.2.2

All Platforms:

- Added new settings to customize the UI
	- Added "Exit when closing main window"
		- Allows the user to exit the app completely when closing the main window, instead of hiding it.
		- This option defaults to enabled in Linux and disabled in Windows.		
	- Added "Exit without confirmation"
		- Allows the user to exit the app without the confirmation pop-up.
		- This defaults to disabled in both Linux and Windows.
- Attempted to unify how the UI functions in Windows / Linux and reduce the amount of platform specific work-arounds.	
	- You can now minimize the main window to the taskbar in Windows.
	- You can no longer double-click the tray icon or use the "Show / Hide" option to toggle the visibility of the main window.		
		- The "Show / Hide" option has been replaced by "Restore Window".
		- Double-clicking the tray icon or using "Restore Window" will always restore the app to a normal, visible state and give it focus.		
	- Unfortunately, the main window cannot be hidden in Linux.  In cases where it's supposed to be hidden, it will be minimized instead.
		- Hiding the main window in Linux requires too many work-arounds and my goal was to reduce the amount of platform specific code.
- Improved platform detection.
	- The "About Game Backup Monitor" window will now display which platform the app is running on (Mono or .NET) and the version.
- Reduced the amount of platform specific code when the app initalizes, this may improve performance.
- The "Start-Up Wizard" will now always appear in the center of screen, instead of sometimes appearing in a random location.
- The system tray menu is now disabled during the "Start-Up Wizard".
- Updated window titles on the custom folder browsers to give more concise instructions.
- Fixed the forced import of multiple backup files when using the Game Manager.
	- It will now create manifest entries for each file when appropriate, instead of just the newest file.

Linux:
	- Fixed the incorrect save path being stored in the metadata when making backups from games running in Proton / Wine.
	- Moved the wine path detection output into a debug mode instead of cluttering up the log.
	- The Include/Exclude builder on the Game Manager will now properly open to the Proton / Wine saved game path when possible.
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html