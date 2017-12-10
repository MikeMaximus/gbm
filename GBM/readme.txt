Game Backup Monitor v1.0.5 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

December 8, 2017

New in 1.0.5

All Platforms:

- You can now add Comments to a game configuration.
- You can now exclude tags and use negative filters on the Game Manager.
- Fixed a bug causing backup size calculations to be incorrect when including sub-folders in a configuration.
- Fixed a bug causing GBM to calculate the backup size of an incorrect location when using a relative path configuration.  This could cause very long delays when a backup was running.
- XML export files now contain the date, version and amount of configurations.  The Import window will now display the date of the XML file in the title bar if applicable.
- Fixed a bug causing games not to be detected if more than one copy of the process was running.
- GBM can now save statistical data from each detected gaming session:	
	- This feature records the start and end time of each detected gaming session.  In future versions more data may be available.	
	- You can view session data using the new "Session Viewer" available in the Tools menu.
	- This feature is disabled by default.  It can be enabled on the Setting screen.
	- Session data is stored locally,  it is currently not synced with the backup folder.

Windows Only:

- Only one instance of GBM can now be running.

Linux Only:

- Added makefile for easy Linux installation.  Thanks basxto!
	
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html