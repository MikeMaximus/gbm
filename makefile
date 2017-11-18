install: GBM.exe gbm_16x16.png gbm_24x24.png gbm_32x32.png gbm_48x48.png gbm_64x64.png gbm_72x72.png gbm_96x96.png gbm_128x128.png gbm_256x256.png
	#This needs xdg-utils
	chmod +x GBM.exe
	cp GBM.exe /usr/local/bin/gbm
	for i in {16,24,32,48,64,72,96,128,256};do xdg-icon-resource install --mode system --novendor --noupdate --size $${i} gbm_$${i}x$${i}.png gbm; done
	xdg-icon-resource forceupdate --mode system
	xdg-desktop-menu install --mode system --novendor gbm.desktop

uninstall: /usr/local/bin/gbm
	#This needs xdg-utils
	rm /usr/local/bin/gbm
	for i in {16,24,32,48,64,72,96,128,256};do xdg-icon-resource uninstall --mode system --novendor --noupdate --size $${i} gbm; done
	xdg-icon-resource forceupdate --mode system
	xdg-desktop-menu uninstall --mode system --novendor gbm.desktop

gbm_256x256.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[0]' '$@'

gbm_128x128.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[1]' '$@'

gbm_96x96.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[2]' '$@'

gbm_72x72.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[3]' '$@'

gbm_64x64.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[4]' '$@'

gbm_48x48.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[5]' '$@'

gbm_32x32.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[6]' '$@'

gbm_24x24.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[7]' '$@'

gbm_16x16.png: gbm.ico
	#This needs imagemagick
	convert 'gbm.ico[8]' '$@'
