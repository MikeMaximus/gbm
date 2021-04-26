Game Backup Monitor v1.2.7 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

April 26, 2021

New in 1.2.7

Disclaimer:

v1.2.7 is still in development.  This file will be updated as changes are made.

All Platforms:

- Fixed unreliable logic when detecting a process.
- Fixed an unexpected error that could occur while monitoring games detected with a window title.
- Added a new setting to control "Two-Pass Detection"		
	- When this setting is enabled, the same process needs to be detected on two consecutive passes to trigger GBM.
		- This makes detection slower, but it prevents issues with the Windows UAC prompt and makes detection more reliable in general.
	- When this setting is disabled, a process is registered as detected after a single pass.
		- This makes detection faster, but may be unreliable in some rare situations.
			- Single pass detection is generally reliable on Linux, and on Windows when UAC is disabled.
			- Faster detection can be useful when using linked processes.
	- This setting is enabled by default, GBM has always used two-pass detection.
- Added better support for launching games using external game launchers
	- Launchers that use their own executable to launch games are now supported.
		- GOG Galaxy is now available as a preconfigured launcher if it's installed in the default location.
	- More variables are now available to use when configuring launcher commands and parameters. See the online manual for details.
	- The %ID% variable is no longer automatically appended to launch commands if it's missing.	
- GBM no longer attempts to start a linked process if it's already running.	
- GBM no longer needs to be running as Administrator to do Windows registry backups.
- Fixed and changed the "Kill process when game is closed" field on the "Process Manager" 
	- The form now properly changes to edit mode when this field is changed.
	- This field is no longer enabled by default when adding a new process.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html