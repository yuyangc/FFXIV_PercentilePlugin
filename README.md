# 最终幻想14-Logs百分比插件（测试版）
基于ACT的Logs百分比插件，使伤害美化插件实时显示您的输出在CN FFLogs中的百分比。

插件使用从CN FFLogs中拉取的数据，用您的实时DPS数据计算您的百分比。您可以通过ACT主表或美化插件中的"Percentile"进行查看。

提示：由于ACT统计DoT伤害的不准确性、比对方式的不同、以及更新数据的周期性，百分比基本准确，但可能与FFLogs上显示的数字略有不同。

使用修改过的Kagerou伤害美化显示百分比:
![Example](https://i.imgur.com/lrgGFzG.png)

**贴士:** 本插件仍在测试与研发中，部分功能可能仍需优化。

# 使用方法介绍
1. 下载插件的最新版本(.zip文件) -> [Releases](https://github.com/yuyangc/FFXIV_PercentilePlugin/releases)
2. 解压缩下载得到的.zip文件，将文件夹中的所有文件解压缩到 “Advanced Combat Tracker.exe” 所在目录。(通常情况类似于 “C:\Program Files (x86)\Advanced Combat Tracker” )
3. 以管理员身份运行ACT
4. 前往 “插件列表”，添加新插件 “PercentilePlugin.dll”
5. 前往 “PercentilePlugin.dll” 分页，点击 “update”
6. 使用可用的overlay就可以实时看到百分比信息

**提示:** 常见的 [Kagerou](https://github.com/hibiyasleep/kagerou) overlay, 可以通过更改URL为 **https://liquidize.github.io/kagerou/overlay/** 来将百分比添加为overlay中的一列 (但你可能需要重新调整kagerou的设置，注：Kagerou内部有设置导入导出功能)

点击 “Update” 按钮将重新下载最新的 “parsedata.bin” 文件，该文件保存了计算你的百分比所需的数据

使用另一种常见overlay “MopiMopi” 的用户可以自行参照下方 “Compatible Overlays” 进行操作

# 可用的伤害美化插件

1. [Kagerou pct](https://github.com/Liquidize/kagerou) - 由Liquidize复制并修改的Kagerou伤害美化，增加了"Pct"栏来显示百分比数据。使用如下网址将其设置为您的美化插件：https://liquidize.github.io/kagerou/overlay/

2. [MopiMopi Pct](https://github.com/Liquidize/mopimopi) -  [MopiMopi](https://github.com/HAERUHAERU/mopimopi) by [HaeruHaeru](https://github.com/HAERUHAERU/) 的修改版，由Liquidize增加了百分比支持。该插件需要ActWebSocket插件支持，点[此](https://docs.google.com/presentation/d/1U7-Vgv6UA2_EFdvw3m8BI-5-9T91WeKTflDuR7rEx-U/edit)查看引导. 使用 [https://liquidize.github.io/mopimopi/](https://liquidize.github.io/mopimopi/) 代替引导中提供的Url.

# 鸣谢
* 哀曜 (国服 - 拉诺西亚)
Dagaz / Dagaz Ai (Coeurl - NA) - 插件的CN本地化，目前的更新负责者
* [Liquidize](https://github.com/Liquidize) / Kaliya Y'mhitra (Goblin - NA) - 插件创始人
* Kaliph Soren (Goblin - NA) - 插件创始人的合作者
* [Hibiyasleep](https://github.com/hibiyasleep) - Kagerou伤害美化插件的制作者
* 插件的更新功能取自Cactbot，点[此](https://github.com/quisquous/cactbot)查看详情.


