Game Backup Monitor v1.2.3 Readme
http://mikemaximus.github.io/gbm-web/
gamebackupmonitor@gmail.com

October 2, 2020

Important Notices:

- v1.2.3 is still in development.  This file will be updated as changes are made.

New in 1.2.3

All Platforms:

- Improvements to the "Choose Game" window	
	- It should now always be displayed in front of other windows after closing a game.
	- It now appears in the taskbar, just in case it does get hidden behind other windows.
	- The GBM icon is now displayed on this window.
- Improvements to the "Import Game Configurations" window	
	- Added "Show Only Selected" option.  
		- This option will filter the current list to show only currently selected configurations.
		- This option can be combined with the text filter.
	- Added "Detect Saved Games" button.
		- This button allows you to detect configurations with saved games.  This has always been automatically done when the form is opened, but the button is useful if you need to start over.
		- Using this button will not unselect any configurations you manually selected.	
	- Configurations in an import list that use only a special folder(or custom variable) as the save path, will no longer be incorrectly selected when detecting saved games.
		- These types of configurations cannot currently be detected, this only fixes them being selected by mistake.
	- You can now maximize or minimize this window.
	- Increased the default size of the window.
	- The GBM icon is now displayed on this window and it now appears in the taskbar.
	

Windows:

- Simplified the code used to hide the main window.  
	- Prevents the weird flicker effect that could occur when clicking the close button.
- Updated SQLite to 3.33.0
		
The entire version history of GBM releases is available at http://mikemaximus.github.io/gbm-web/versionhistory.html