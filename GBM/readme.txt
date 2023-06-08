Game Backup Monitor v1.3.6 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

June 8, 2023

New in 1.3.6

All Platforms:

- Added the ability to backup and restore saves for the currently monitored game by using the "Backup" and "Restore" buttons on the main window.
	- This allows you to save scum while a game is running, without interrupting the current session.
- Fixed issues that could cause the Ludusavi Manifest import to fail.
- Fixed a crash that could occur when cancelling detection and re-enabling while using the "Fast" detection speed.
- Changed how concurrent operations are handled.
	- A backup, restore or import operation cannot be queued while another is currently in progress, a warning notification is now displayed if this occurs.
	- Syncs triggered by another application and automatic backup restores can now execute while GBM is currently monitoring a session or paused while working in another window such as the Game Manager.
		- Automatic backup restores can still fail if they happen to queue during another operation in progress.
- Updated YamlDotNet to 13.1.0.

Windows:

- Added the ability to backup or restore saves for the last played, selected or currently monitored game by using global hotkeys.
	- This allows you to quickly backup or restore your save(s) while in-game, without the need to switch to GBM. 
	- Ctrl + Alt + F5 will trigger a backup.
	- Ctrl + Alt + F9 will trigger a restore.
	- The hotkeys currently cannot be rebound and only work in Windows.
- Added unique "Success" or "Failure" audio that will play when a backup or restore is triggered from a hot key.

Linux:

- The main window can now be hidden to the system tray by closing the window.
	- Uncheck "Exit when closing window" in Main Window Options of Settings -> User Interface to enable this feature.
	- It can also be hidden automatically on startup by checking the "Start minimized" option in Settings -> Startup.
- GBM no longer needs to wait 60 seconds before automatically restoring new backups (if enabled) when the app starts.

Linux Known Issues:

- After hiding and restoring the main window, the window may not redraw correctly.
	- Hide and restore the window again as a work-around.
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html