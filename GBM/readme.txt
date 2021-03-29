Game Backup Monitor v1.2.7 Readme
https://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

March 29, 2021

New in 1.2.7

Disclaimer:

v1.2.7 is still in development.  This file will be updated as changes are made.

All Platforms:

- Fixed unreliable logic when detecting a process.
- Added a new setting to control "Two-Pass Detection"
	- This setting is enabled by default, GBM has always used two-pass detection.	
	- When this setting is enabled, the same process needs to be detected on two seperate passes to trigger GBM.
		- This makes detection slower, but it prevents issues with the Windows UAC prompt and makes detection more reliable in general.
	- When this setting is disabled, a process is registered as detected after a single pass.
		- This makes detection faster, but may be unreliable in some rare situations.
			- Single pass detection is generally reliable on Linux, and on Windows when UAC is disabled.
			- Faster detection can be useful when using linked processes.	

The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html