Game Backup Monitor v1.3.9 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

May 21, 2024

New in 1.3.9

All Platforms:

- Game Backup Monitor is now built for .NET v4.8.
- Improved how GBM detects when other software has modified it's manifest database.
- Improved quick search (Main Window and Game Manager)
	- The default quick search is now wide, using most text fields and tags.
		- These fields currently include: Name, Process, Parameter, Path, Version, Company, Comments.
	- You can now do a refined quick search by prepending it with the field name (case-sensitive) and a colon.  Ex.  Name:Doom or Company:Sega
	- A refined tag search is still supported by prepending with a hashtag, Ex. #GOG
	- You cannot combine refined searches in a single quick search, use the "Custom Filter" on the Game Manager for complex queries.
- Updated NHotkey to 3.0.0.
- Updated YamlDotNet to 15.1.4.

Windows:

- Updated 7-Zip to 24.05.
- Updated SQLite to 3.45.3.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html