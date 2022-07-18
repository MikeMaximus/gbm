Game Backup Monitor v1.3.2 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

July 18, 2022

New in 1.3.2

Disclaimer:

v1.3.2 is still in development, this file will be updated as changes are made.
Database changes made in development builds may not be compatible with the final release.  Use at your own risk!

All Platforms:

- Fixed an issue that caused changes to be lost if GBM was closed while actively working in certain forms like the Game Manager.
- Fixed an issue with 7z files not being displayed when importing backup files.
- Configurations that use duplicate names will now have more descriptive folder and file names.  Ex. Metro Exodus [8ac11c11-8d18-471b-8f0d-aa154dc77f0e]
- Performance Improvements:
	- Some functions that blocked the UI now run on a seperate thread.
	- Optimized various database queries.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html