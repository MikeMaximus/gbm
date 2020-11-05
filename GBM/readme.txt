Game Backup Monitor v1.2.4 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

Novermber 5, 2020

Important Notices:

- v1.2.4 is still in development.  This file will be updated as changes are made.

New in 1.2.4

All Platforms:

- Added the ability to launch recently played games.
	- Launch the last five games played from the system tray menu.
	- Games are launched using information GBM has automatically detected or can fully customized via the Game Manager.
	- This feature is optional and disabled by default, it must be enabled in Settings -> General.
		- This feature relies on "Session Tracking", it must be enabled to determine the recently played games.
- Added the ability to customize game launch options.
	- Added "Launch Settings..." to Game Manager		
		- Allows the configuration of another launcher (Steam, uPlay, EGS) to start the game instead of directly using an executable.
			- You can manage available launchers using the "Manage Launchers..." button on this window.  Popular launchers will be preconfigured.			
		- Allows the use of a custom executable, instead of using the process from the game configuration.
		- Allows setting launch arguments
			- Arguments only apply to game executables directly launched with GBM.
			- Arguments do NOT apply to games configured to use another launcher (Steam, EGS, etc).  You must set the arguments each game in those launchers instead.
- Double right-clicking the tray icon no longer triggers "Restore Window"
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html