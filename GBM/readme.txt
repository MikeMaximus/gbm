Game Backup Monitor v1.3.5 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

February 18, 2023

Important Notice:

Game Backup Monitor will no longer be receiving any major new features or changes.  However I will continue to maintain it and apply minor changes or fixes as required.  

I'd like to thank everyone that made contributions to this project over the years!

New in 1.3.5

All Platforms:

- Fixed an issue that prevented GBM from properly reloading the main window after the database file is updated by another application, such as Dropbox.
- The remote database backup created when GBM is launched will now include the name of the PC in the file name.
	- The remote database is "gbm.s3db" in your backup folder.
	- This change makes the launch backup(s) more useful when using GBM on multiple PCs.
	- This change also prevents conflict issues with cloud services, as multiple PCs could be overwriting the same backup file with varying versions.
	
Windows:

- Added a toggle to control deleting files to the Recycling Bin.
	- This is enabled by default to match functionality of prior versions.
			
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html