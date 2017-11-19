ICONSIZES := 16 24 32 48 64 72 96 128 256
IMGMAGICK := $(shell command -v convert 2> /dev/null)
XDGUTILS := $(shell command -v xdg-desktop-menu 2> /dev/null)
define \n


endef

install: GBM.exe $(foreach size,$(ICONSIZES),gbm_$(size)x$(size).png)
ifndef XDGUTILS
	$(error "xdg-desktop-menu is not available, please install xdg-utils")
endif
#rename it in a way, it can easily started from terminal
	install gbm.sh /usr/local/bin/gbm;
	install -d /usr/local/share/gbm;
	install GBM.exe /usr/local/share/gbm/;
#install icon in different sizes
	$(foreach size,$(ICONSIZES),xdg-icon-resource install --mode system --novendor --noupdate --size $(size) gbm_$(size)x$(size).png gbm;$(\n))
	xdg-icon-resource forceupdate --mode system;
#install .desktop file, which is used for running gbm from desktop and menus
	xdg-desktop-menu install --mode system --novendor gbm.desktop;

uninstall: /usr/local/bin/gbm
ifndef XDGUTILS
	$(error "xdg-desktop-menu is not available, please install xdg-utils")
endif
	rm /usr/local/bin/gbm;
	rm -r /usr/local/share/gbm/;
	$(foreach size,$(ICONSIZES),xdg-icon-resource uninstall --mode system --novendor --noupdate --size $(size) gbm;$(\n))
	xdg-icon-resource forceupdate --mode system;
	xdg-desktop-menu uninstall --mode system --novendor gbm.desktop;

gbm_%.png: gbm.ico
ifndef IMGMAGICK
	$(error "convert is not available, please install imagemagick")
endif
#extracts the correct ico index appended to the filename from identify’s output
	$(eval INDEX := $(shell identify gbm.ico | grep $* | cut -d" " -f1;))
	convert '$(INDEX)' '$@';
