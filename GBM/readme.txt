Game Backup Monitor v1.3.6 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

May 26, 2023

New in 1.3.6

All Platforms:

- Fixed issues that could cause the Ludusavi Manifest import to fail.
- Fixed a crash that could occur when cancelling detection and re-enabling while using the "Fast" detection speed.
- Added the ability to backup and restore saves for the currently monitored game by using the "Backup" and "Restore" buttons on the main window.
	- This allows for quick save scumming while a game is running, without interrupting the current session.
- Updated YamlDotNet to 13.1.0.

Linux:

- The main window can now be hidden to the system tray by closing the window.
	- Uncheck "Exit when closing window" in Main Window Options of Settings -> User Interface to enable this feature.
	- It can also be hidden automatically on startup by checking the "Start minimized" option in Settings -> Startup.

Linux Known Issues:

- After hiding and restoring the main window, the window may not refresh correctly when being resized.
	- Hide and restore the window again after resizing as a work-around.
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html