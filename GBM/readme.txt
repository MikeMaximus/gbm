Game Backup Monitor v1.1.9HF2 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

July 31, 2019

New in 1.1.9HF2

All Platforms:

- Multiple variables in the same path are now supported.
	- This feature is meant to allow the mix of relative path variables with a single absolute path variable when the situation requires it.
	- Multiple Custom Path Variables in the same path are now supported.
	- Mixing Environment Variables with relative Custom Path Variables is now supported.
- Allow saving a game configuration with an empty Process field.
	- This feature is meant to make life easier for users who want to use GBM for manual backups.
	- New users will receive a one-time warning when saving a configuration with no Process, current users may or may not receive this new warning.
	- Configurations with an empty Process field will be automatically excluded from monitoring, regardless of the "Monitor this Game" setting.

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html