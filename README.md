﻿# FFXIV_PercentilePlugin (beta)
Percentile Plugin for ACT to allow overlays to display FFLogs percentile data in real time.

The plugin works by using data obtained from FFLogs and calculating the current percentile in real time using your current DPS. The data used should be fairly accurate +/- 1% (give or take) of the Historical Percentile if you use the latest data. You can view the percentiles in real time via the "Percentile" column that is added to ACT or via a compatible overlay.

Percentiles displayed in a modified Kagerou overlay (the Pct column):
![Example](https://i.imgur.com/lrgGFzG.png)

**Note:** The plugin is still in development and while fully functional for displaying percentiles for known fights, future fights and some current fights may need to be adjusted.

# How-To

1. Obtain the latest version (zip file) of the plugin from the [Releases](https://github.com/Liquidize/FFXIV_PercentilePlugin/releases)
2. Extract the zip file to where your "Advanced Combat Tracker.exe" is, usually something like "C:\Program Files (x86)\Advanced Combat Tracker"
3. Run ACT as administrator (incase of permissions errors for writing config files)
4. Go to the "Plugins" tab and the "PercentilePlugin.dll" as a plugin.
5. Go to the "PercentilePlugin" plugin tab, and click update.
6. Use a compatible overlay if you want to see the information in the overlay.

**Note:** I've forked and added percentile functionality to the popular [Kagerou](https://github.com/hibiyasleep/kagerou) overlay, simply change the overlay URL to: **https://liquidize.github.io/kagerou/overlay/** and you should be able to add "Percentile" as a column in your tabs in the config. You will need to completely reconfigure your overlay though, unfortunately.

Clicking the "Update" button will download the "parsedata.bin" file that is stored in this repository. This file contains all the data needed to calculate your parses, and is updated daily.

# 使用方法介绍 (How-to)
1. 下载插件的最新版本(.zip文件) -> [Releases](https://github.com/Liquidize/FFXIV_PercentilePlugin/releases)
2. 解压缩下载得到的.zip文件，将文件夹中的所有文件解压缩到 “Advanced Combat Tracker.exe” 所在目录。(通常情况类似于 “C:\Program Files (x86)\Advanced Combat Tracker” )
3. 以管理员身份运行ACT
4. 前往 “插件列表”，添加新插件 “PercentilePlugin.dll”
5. 前往 “PercentilePlugin.dll” 分页，点击 “update”
6. 使用可用的overlay就可以实时看到百分比信息

**提示:** 常见的 [Kagerou](https://github.com/hibiyasleep/kagerou) overlay, 可以通过更改URL为 **https://liquidize.github.io/kagerou/overlay/** 来将百分比添加为overlay中的一列 (但你可能需要重新调整kagerou的设置，注：Kagerou内部有设置导入导出功能)

点击 “Update” 按钮将重新下载最新的 “parsedata.bin” 文件，该文件保存了计算你的百分比所需的数据

使用另一种常见overlay “MopiMopi” 的用户可以自行参照下方 “Compatible Overlays” 进行操作
# Compatible Overlays

1. [Kagerou pct](https://github.com/Liquidize/kagerou) - Forked by me, this is the latest version of Kagerou with the added ability to add "Percentile" as a column to your tabs. Just like Kagerou you can easily set this as your overlay by using the following url: https://liquidize.github.io/kagerou/overlay/
2. [Fancy Detail Overlay](https://i.imgur.com/HGSZoQ6.png) - By 라그린네, I am unsure of the original name of this overlay a user of the plugin asked me to add support for it. You can find the overlay in the Overlays folder on the repository.
3. [MopiMopi Pct](https://github.com/Liquidize/mopimopi) - A fork of [MopiMopi](https://github.com/HAERUHAERU/mopimopi) by [HaeruHaeru](https://github.com/HAERUHAERU/) with percentile support added by myself. This overlay requires ActWebSocket, follow the guide [HERE](https://docs.google.com/presentation/d/1U7-Vgv6UA2_EFdvw3m8BI-5-9T91WeKTflDuR7rEx-U/edit). Click [HERE](https://i.imgur.com/bPvNkIQ.png) for a preview. Use [https://liquidize.github.io/mopimopi/](https://liquidize.github.io/mopimopi/) as the URL instead of the one in the guide.
4. [Horizon Overlay](https://github.com/unfaiyted/horizoverlay) - The Horizon Overlay with Percentile support by [Unfaiyted](https://github.com/unfaiyted/).

If you want your overlay listed here, please message me in game or on discord.

* **Character in game:** Kaliya Y'mhitra (Goblin - NA)
* **Discord:** Kaliya#0001

# Credits
* Dagaz / Dagaz Ai (Coeurl - NA) - Maintainer of the plugin at the moment
* Liquidize / Kaliya Y'mhitra (Goblin - NA) - Creator of this plugin, which I forked and added functionality to.
* Kaliph Soren (Goblin - NA) - Co-developer of Liquidize
* [Hibiyasleep](https://github.com/hibiyasleep) - Creator of the Kagerou overlay which Liquidize forked and added functionality to.
* The update functionality of the plugin was taken from Cactbot an open source Raid helper found [here](https://github.com/quisquous/cactbot).


