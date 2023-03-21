Game Backup Monitor v1.3.5 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

March 21, 2023

Important Notice:

Game Backup Monitor will no longer be receiving any major new features or changes.  However I will continue to maintain it and apply minor changes or fixes as required.  

I'd like to thank everyone that made contributions to this project over the years!

New in 1.3.5

All Platforms:

- Fixed an issue that prevented GBM from properly refreshing the main window when the database file is updated by another app.
- Fixed issues that could cause an unnecessary sync operation to be triggered.
- GBM will no longer perform database syncs or automatically handle new backups while a game is being monitored or during other operations.
	- These actions running simultaneously could cause errors and/or leave the app in an broken state.	
- The remote database backup created when GBM is launched will now include the name of the PC in the file name.
	- The remote database is "gbm.s3db" in your backup folder.
	- This change makes the database backup more useful when using GBM on multiple PCs with a shared backup folder.
	- This change prevents conflict issues with cloud services, as multiple PCs could be overwriting the same backup file with varying versions.

Windows:

- Added a Setting (Settings -> Files & Folders) to toggle the deleting of files to the Recycling Bin.
	- This setting is enabled by default to match functionality of prior versions.

Linux:

- Fixed a crash that occured when attempting to automatically restore new backups on app start.
	- GBM will now wait 60 seconds before attempting to automatically handle new backups after the app starts.
			
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html