Game Backup Monitor v1.2.4 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

November 27, 2020

New in 1.2.4

All Platforms:

- Added the ability to launch games.
	- Games are launched using information GBM has automatically detected or can be fully customized via the Game Manager.
		- Most games will need to be detected at least once in v1.2.4 or higher before they can be automatically launched using this feature.
	- Launch any configured game using the "Quick Launcher".
		- The Quick Launcher is available in the File menu, system tray menu and by using "Ctrl + L" on the main window.
	- Launch any configured game directly from the Game Manager.
	- Launch the last five games played from the system tray menu
		- The "Session Tracking" feature needs to be enabled and have enough recorded data to display any recently played games in the system tray menu.
- Added the ability to customize game launch options.
	- Added "Launch Settings..." button to Game Manager.
		- Allows the configuration of another launcher (such as Steam) to start the game instead of directly using an executable.
			- You can manage available launchers using the Launcher Manager.  Some popular launchers will be preconfigured.			
		- Allows the use of an alternate executable or script.
		- Allows setting alternate launch parameters (or disabling them).		
- The process path is now always saved once a game has been detected.
	- Priors versions of GBM only saved the process path when it was required for backup or detection purposes.
	- A warning will now be displayed in the log when the process path cannot be determined.
- Session time (and total time played) will now update(once per minute) on the main window, instead of only being displayed after the game ends.
- Session time will now be displayed and update(once per minute) on the GBM tray icon tooltip.
- Disabled features will now have their options removed from all menus.  This currently only applies to session tracking and game launching.
- Fixed issues that prevented configuration save paths from being updated correctly when a custom variable is modified or deleted.
- Deleting a custom variable will now properly remove the environment variable in the current session and update any affected game configurations.
- Editing a configuration while using a Custom filter on the Game Manager will no longer lose the changes if any tags are modified before saving.
- Modifying tags on a single configuration while using a Custom filter on the Game Maanger will no longer unselect the current configuration.
- Any form that asks to save unchanged data on closing, will no longer just close and lose the changes if the form fails validation when choosing to save.
- Double right-clicking the tray icon no longer triggers "Restore Window".

Windows:

- GBM will no longer crash if the UAC prompt is cancelled while restarting as Administrator.

Known Issues (Linux):

- The "Quick Launcher" may not accept using the "Enter" key on the game list combo box to trigger closing the form and launching the game.  You need to hit "Tab" then "Enter".
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html