Game Backup Monitor v1.4.5 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

May 20, 2026

New in 1.4.5

General:

- Added a friendly age descriptor (Ex. 15 minutes ago) to areas where it may be useful.
	- The age is rounded to the closest time unit, from seconds up to years.	
- GBM will no longer attempt to launch games using a "Window Title" configuration when it doesn't have the proper information to do so.
- GBM now launches game executables on a seperate thread.	
- Fixed an issue with the Ludusavi import that prevented certain valid game configurations from being available.
	- The conversion algorithm assumed certain data in the manifest was consistent for each game when it isn't.
- Updated Components
	- 7-Zip 25.01 -> 26.01
	- SQLite 3.50.4 -> 3.51.3

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html