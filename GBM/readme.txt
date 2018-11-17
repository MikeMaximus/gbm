Game Backup Monitor v1.1.6 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

November 17, 2018

New in 1.1.6

All Platforms:

- Added support for games that save to the Windows %PROGRAMDATA% location.
- The Game Manager now automatically selects the last detected game when opened.
- Backup files can no longer be imported into a "Monitor Only" configuration.
- Optimized the memory and cpu usage of Regular Expressions when monitoring for games.
- Added a "Recurse sub-folders" option
	- This option is set per game configuration, it is available on Include/Exclude window of the Game Manager and Add Game Wizard.
	- It is enabled by default on all current and new configurations.
	- Disabling this option prevents 7-Zip and GBM from scanning every sub-folder and file of a save path.  This useful when backing up specific files inside an extremely large folder.
- Fixed a possible issue with backups when the configuration uses folder path includes.
	
Linux:

- Fixed a memory leak issue with Mono and Regular Expressions.
- Fixed some issues when detecting the prefix of Wine/Proton games.
	- The default prefix (~/.wine) will be assumed when a detected game does not have the WINEPREFIX variable set.
	- A prefix can now be fully detected when it contains spaces.
- Windows configurations that use a relative save path are now properly converted when detected in Wine/Proton.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html