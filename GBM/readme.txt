Game Backup Monitor v1.2.1 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

June 4, 2020

New in 1.2.1

All Platforms:

- Fixed a possible crash when renaming a game with existing backup files while a cloud client is monitoring the backup folder.
- Fixed a rare issue that could cause the wrong game to be pre-selected when opening the Game Manager.
- Fixed an issue causing the ampersand character to be hidden in certain controls.  
	- Ex. Mount & Blade II: Bannerlords will now be displayed correctly.
- Various changes for debugging and building releases.

Windows:

- Installers are now built with NSIS 3.05

Linux:

- The system tray icon is now enabled for desktop environments that support it (Cinnamon, LXDE).	
	- You'll need an up-to-date version of Mono for the tray icon to work, tested with Mono 6.8.0.	
- Fixed an issue that caused opening external apps to fail when using the latest versions of Mono.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html