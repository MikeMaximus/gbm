Game Backup Monitor v1.2.7 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

April 27, 2021

New in 1.2.7

All Platforms:

- Added more support for launching games using external game launchers
	- Launchers that use their own executable to launch games are now supported.
		- GOG Galaxy is now available as a preconfigured launcher if it's installed in the default location.
	- More variables are now available to use when configuring launcher commands and parameters. 
		- See the "Launching Games" section of the online manual for details.
	- The %ID% variable is no longer automatically appended to launch commands if it's missing.
- Added a new setting to control "Two-Pass Detection"
	- When enabled, the same process needs to be found on two consecutive detection passes to trigger GBM. This makes detection slower, but more reliable.  This has always been the default.
	- When disabled, a process is registered as detected after a single pass. This makes detection twice as fast, but it may be unreliable in certain situations.
- Incomplete backup operations and automatic restore operations will no longer run at the same time when starting GBM.
	- Automatic restore operations will now wait for incomplete backup operations to be finished.
- GBM no longer attempts to start a linked process if it's already running.
- GBM no longer needs to be running as Administrator to do Windows registry backups.
- Fixed unreliable logic when detecting a process.
- Fixed an unexpected error that could occur while monitoring games detected with a window title.
- Fixed and changed the "Kill process when game is closed" field on the "Process Manager" 
	- The form now properly changes to edit mode when this field is changed.
	- This field is no longer enabled by default when adding a new process.
- Fixed an issue with manually triggered backups being flagged as incomplete when they failed a prerequisite condition.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html