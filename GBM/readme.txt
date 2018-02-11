Game Backup Monitor v1.0.8 Pre-Release Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

February 11, 2018

New in 1.0.8

Disclaimer:

1.  This is pre-release software intended for testing. 
2.  Database files from this version (gbm.s3db) may not be compatible with the full release.  GBM makes automatic backups of your database files if you need to revert to a prior version.
3.  Do not make external links to this release, it will be available for a limited time.

All Platforms:

- Added Regular Expression support for game detection
	- This feature allows GBM to detect games based on a pattern instead of a single process name.
	- This allows GBM to better support games that run from multiple executables and games that use interpreters or emulators.
	- Use the new "Regular Expression" checkbox on the Game Manager and enter the pattern in the "Process" field.
	- GBM will validate patterns and offer to help troubleshoot (using regexr.com) when validation fails.
- Fixed a potential problem with detecting parameters.

Windows Only:

- Updated 7-Zip to v18.01
	
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html