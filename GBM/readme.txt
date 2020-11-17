Game Backup Monitor v1.2.4 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

November 17, 2020

Important Notices:

- v1.2.4 is still in development.  This file will be updated as changes are made.

New in 1.2.4

All Platforms:

- Added the ability to launch games.		
	- Launch the last five games played from the system tray menu.
	- Launch any configured game using the "Quick Launcher" window on the system tray menu.
	- Launch any configured game from the Game Manager.
	- Games are launched using information GBM has automatically detected or can be fully customized via the Game Manager.
	- This feature is optional and disabled by default, it must be enabled in Settings -> General.
		- This feature relies on "Session Tracking", it must be enabled to determine the recently played games.		
- Added the ability to customize game launch options.
	- Added "Launch Settings..." button to Game Manager.
		- Allows the configuration of another launcher (such as Steam) to start the game instead of directly using an executable.
			- You can manage available launchers using the Launcher Manager.  Some popular launchers will be preconfigured.			
		- Allows the use of an alternate executable or script.
		- Allows setting alternate launch parameters (or disabling them).		
- The process path is now always detected and saved after a game has been detected.
	- Priors versions of GBM only saved the process path when it was required for a backup.
	- A warning will now be displayed in the log when the process path cannot be determined.
- Disabled features will now have their options removed from all menus.  This currently only applies to session tracking and game launching.
- Deleting a custom variable will now properly remove the environment variable in the current session and update any affected game configurations.
- Editing a configuration while using a Custom filter on the Game Manager will no longer lose the changes if any tags are modified before saving.
- Modifying tags on a single configuration while using a Custom filter on the Game Maanger will no longer unselect the current configuration.
- Any form that asks to save unchanged data on closing, will no longer just close and lose the changes if the form fails validation when choosing to save.
- Double right-clicking the tray icon no longer triggers "Restore Window".

Windows:

- GBM will no longer crash if the UAC prompt is cancelled while restarting as Administrator.
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html