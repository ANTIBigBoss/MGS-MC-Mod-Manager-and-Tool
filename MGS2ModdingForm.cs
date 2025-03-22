using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IOSearchOption = System.IO.SearchOption;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public partial class MGS2ModdingForm : Form
    {
        private ConfigSettings config;
        private RichTextBox modInfoRichTextBox;
        private FlowLayoutPanel modListPanel;
        private FileExplorerManager fileExplorerManager;
        private ModListManager modListManager;

        private readonly string[] expectedPaths = new string[]
        {
            "EngineSupport",
            "EngineSupport/Fonts",
            "EngineSupport/Fonts/_win",
            "EngineSupport/Shaders/_win",
            "Misc",
            "Misc/loading/_win",
            "Misc/splashscreen/_win",
            "Misc/us",
            "MonoBleedingEdge/EmbedRuntime",
            "MonoBleedingEdge/etc/mono",
            "MonoBleedingEdge/etc/mono/2.0",
            "MonoBleedingEdge/etc/mono/2.0/Browsers",
            "MonoBleedingEdge/etc/mono/4.0",
            "MonoBleedingEdge/etc/mono/4.0/Browsers",
            "MonoBleedingEdge/etc/mono/4.5",
            "MonoBleedingEdge/etc/mono/4.5/Browsers",
            "MonoBleedingEdge/etc/mono/mconfig",
            "assets/anm/us",
            "assets/cv2/us",
            "assets/evm/us",
            "assets/evm/us/_win",
            "assets/far/us",
            "assets/gcx/eu/_bp",
            "assets/gcx/us/_bp",
            "assets/hzx/eu",
            "assets/hzx/us",
            "assets/kms/eu",
            "assets/kms/eu/_win",
            "assets/kms/us",
            "assets/kms/us/_win",
            "assets/lt2/eu",
            "assets/lt2/us",
            "assets/mar/us",
            "assets/o2d/eu",
            "assets/o2d/us",
            "assets/row/eu",
            "assets/row/us",
            "assets/sar/us",
            "assets/tri/eu",
            "assets/tri/us",
            "assets/var/eu",
            "assets/var/us",
            "assets/zms/us/cbx/_win",
            "assets/zms/us/cbx_futa1/_win",
            "assets/zms/us/cbx_futa2/_win",
            "assets/zms/us/cbx_futa3/_win",
            "assets/zms/us/cbx_futa4/_win",
            "eu/codec/_bp",
            "eu/demo/_bp",
            "eu/demo2/_bp",
            "eu/face",
            "eu/face/capture",
            "eu/face/f00a",
            "eu/face/f01a",
            "eu/face/f01b",
            "eu/face/f01c",
            "eu/face/f01d",
            "eu/face/f01e",
            "eu/face/f01f",
            "eu/face/f02a",
            "eu/face/f03a",
            "eu/face/f03b",
            "eu/face/f04a",
            "eu/face/f04b",
            "eu/face/f04c",
            "eu/face/f04d",
            "eu/face/f04e",
            "eu/face/f05a",
            "eu/face/f06a",
            "eu/face/mobile",
            "eu/face/node",
            "eu/face/photo",
            "eu/movie/_bp",
            "eu/movievr/_bp",
            "eu/stage/a00a",
            "eu/stage/a00b",
            "eu/stage/a00c",
            "eu/stage/a01a",
            "eu/stage/a01b",
            "eu/stage/a01c",
            "eu/stage/a01d",
            "eu/stage/a01e",
            "eu/stage/a01f",
            "eu/stage/a02a",
            "eu/stage/a02b",
            "eu/stage/a03a",
            "eu/stage/a03b",
            "eu/stage/a12a",
            "eu/stage/a12b",
            "eu/stage/a13a",
            "eu/stage/a13b",
            "eu/stage/a13c",
            "eu/stage/a14a",
            "eu/stage/a14b",
            "eu/stage/a15a",
            "eu/stage/a15b",
            "eu/stage/a16a",
            "eu/stage/a17a",
            "eu/stage/a18a",
            "eu/stage/a19a",
            "eu/stage/a20a",
            "eu/stage/a20b",
            "eu/stage/a20c",
            "eu/stage/a20e",
            "eu/stage/a21a",
            "eu/stage/a21b",
            "eu/stage/a22a",
            "eu/stage/a22b",
            "eu/stage/a23a",
            "eu/stage/a23b",
            "eu/stage/a24a",
            "eu/stage/a24b",
            "eu/stage/a24c",
            "eu/stage/a24d",
            "eu/stage/a24f",
            "eu/stage/a24g",
            "eu/stage/a25a",
            "eu/stage/a25d",
            "eu/stage/a28a",
            "eu/stage/a31a",
            "eu/stage/a31b",
            "eu/stage/a31c",
            "eu/stage/a41a",
            "eu/stage/a41b",
            "eu/stage/a42a",
            "eu/stage/a43a",
            "eu/stage/a45a",
            "eu/stage/a46a",
            "eu/stage/a61a",
            "eu/stage/boss",
            "eu/stage/d001p01",
            "eu/stage/d001p02",
            "eu/stage/d005p01",
            "eu/stage/d005p03",
            "eu/stage/d00t",
            "eu/stage/d010p01",
            "eu/stage/d012p01",
            "eu/stage/d014p01",
            "eu/stage/d01t",
            "eu/stage/d021p01",
            "eu/stage/d036p03",
            "eu/stage/d036p05",
            "eu/stage/d045p01",
            "eu/stage/d046p01",
            "eu/stage/d04t",
            "eu/stage/d053p01",
            "eu/stage/d055p01",
            "eu/stage/d05t",
            "eu/stage/d063p01",
            "eu/stage/d065p02",
            "eu/stage/d070p01",
            "eu/stage/d070p09",
            "eu/stage/d070px9",
            "eu/stage/d078p01",
            "eu/stage/d080p01",
            "eu/stage/d080p06",
            "eu/stage/d080p07",
            "eu/stage/d080p08",
            "eu/stage/d082p01",
            "eu/stage/d10t",
            "eu/stage/d11t",
            "eu/stage/d12t",
            "eu/stage/d12t3",
            "eu/stage/d12t4",
            "eu/stage/d13t",
            "eu/stage/d14t",
            "eu/stage/ending",
            "eu/stage/init",
            "eu/stage/mselect",
            "eu/stage/museum",
            "eu/stage/n_title",
            "eu/stage/r_plt0",
            "eu/stage/r_plt1",
            "eu/stage/r_plt10",
            "eu/stage/r_plt11",
            "eu/stage/r_plt12",
            "eu/stage/r_plt13",
            "eu/stage/r_plt2",
            "eu/stage/r_plt3",
            "eu/stage/r_plt4",
            "eu/stage/r_plt5",
            "eu/stage/r_plt6",
            "eu/stage/r_plt7",
            "eu/stage/r_plt8",
            "eu/stage/r_plt9",
            "eu/stage/r_plt_s",
            "eu/stage/r_rai_b",
            "eu/stage/r_sna_b",
            "eu/stage/r_title",
            "eu/stage/r_tnk0",
            "eu/stage/r_tnk_r",
            "eu/stage/r_vr_1",
            "eu/stage/r_vr_b",
            "eu/stage/r_vr_p",
            "eu/stage/r_vr_r",
            "eu/stage/r_vr_rp",
            "eu/stage/r_vr_s",
            "eu/stage/r_vr_sp",
            "eu/stage/r_vr_t",
            "eu/stage/r_vr_x",
            "eu/stage/select",
            "eu/stage/sp01a",
            "eu/stage/sp02a",
            "eu/stage/sp03a",
            "eu/stage/sp06a",
            "eu/stage/sp07a",
            "eu/stage/sp08a",
            "eu/stage/sp21a",
            "eu/stage/sp22a",
            "eu/stage/sp24a",
            "eu/stage/sp25a",
            "eu/stage/sselect",
            "eu/stage/st01a",
            "eu/stage/st02a",
            "eu/stage/st03a",
            "eu/stage/st04a",
            "eu/stage/st05a",
            "eu/stage/ta00a",
            "eu/stage/ta01a",
            "eu/stage/ta01b",
            "eu/stage/ta01c",
            "eu/stage/ta01d",
            "eu/stage/ta01e",
            "eu/stage/ta01f",
            "eu/stage/ta02a",
            "eu/stage/ta12a",
            "eu/stage/ta20a",
            "eu/stage/ta22a",
            "eu/stage/ta24a",
            "eu/stage/ta31a",
            "eu/stage/ta42a",
            "eu/stage/tales",
            "eu/stage/tsp03a",
            "eu/stage/tvs03a",
            "eu/stage/tvs05a",
            "eu/stage/tvs06a",
            "eu/stage/tvs08a",
            "eu/stage/twp03a",
            "eu/stage/twp34a",
            "eu/stage/twp43a",
            "eu/stage/vs01a",
            "eu/stage/vs02a",
            "eu/stage/vs03a",
            "eu/stage/vs04a",
            "eu/stage/vs05a",
            "eu/stage/vs06a",
            "eu/stage/vs07a",
            "eu/stage/vs08a",
            "eu/stage/vs09a",
            "eu/stage/vs10a",
            "eu/stage/w00a",
            "eu/stage/w00b",
            "eu/stage/w00c",
            "eu/stage/w01a",
            "eu/stage/w01b",
            "eu/stage/w01c",
            "eu/stage/w01d",
            "eu/stage/w01e",
            "eu/stage/w01f",
            "eu/stage/w02a",
            "eu/stage/w03a",
            "eu/stage/w03b",
            "eu/stage/w04a",
            "eu/stage/w04b",
            "eu/stage/w04c",
            "eu/stage/w11a",
            "eu/stage/w11b",
            "eu/stage/w11c",
            "eu/stage/w12a",
            "eu/stage/w12b",
            "eu/stage/w12c",
            "eu/stage/w13a",
            "eu/stage/w13b",
            "eu/stage/w14a",
            "eu/stage/w15a",
            "eu/stage/w15b",
            "eu/stage/w16a",
            "eu/stage/w16b",
            "eu/stage/w17a",
            "eu/stage/w18a",
            "eu/stage/w19a",
            "eu/stage/w20a",
            "eu/stage/w20b",
            "eu/stage/w20c",
            "eu/stage/w20d",
            "eu/stage/w21a",
            "eu/stage/w21b",
            "eu/stage/w22a",
            "eu/stage/w23a",
            "eu/stage/w23b",
            "eu/stage/w24a",
            "eu/stage/w24b",
            "eu/stage/w24c",
            "eu/stage/w24d",
            "eu/stage/w24e",
            "eu/stage/w25a",
            "eu/stage/w25b",
            "eu/stage/w25c",
            "eu/stage/w25d",
            "eu/stage/w28a",
            "eu/stage/w31a",
            "eu/stage/w31b",
            "eu/stage/w31c",
            "eu/stage/w31d",
            "eu/stage/w31f",
            "eu/stage/w32a",
            "eu/stage/w32b",
            "eu/stage/w41a",
            "eu/stage/w42a",
            "eu/stage/w43a",
            "eu/stage/w44a",
            "eu/stage/w45a",
            "eu/stage/w46a",
            "eu/stage/w51a",
            "eu/stage/w61a",
            "eu/stage/webdemo",
            "eu/stage/wmovie",
            "eu/stage/wp01a",
            "eu/stage/wp02a",
            "eu/stage/wp03a",
            "eu/stage/wp04a",
            "eu/stage/wp05a",
            "eu/stage/wp11a",
            "eu/stage/wp12a",
            "eu/stage/wp13a",
            "eu/stage/wp14a",
            "eu/stage/wp15a",
            "eu/stage/wp21a",
            "eu/stage/wp22a",
            "eu/stage/wp23a",
            "eu/stage/wp24a",
            "eu/stage/wp25a",
            "eu/stage/wp31a",
            "eu/stage/wp32a",
            "eu/stage/wp33a",
            "eu/stage/wp34a",
            "eu/stage/wp35a",
            "eu/stage/wp41a",
            "eu/stage/wp42a",
            "eu/stage/wp43a",
            "eu/stage/wp44a",
            "eu/stage/wp45a",
            "eu/stage/wp51a",
            "eu/stage/wp52a",
            "eu/stage/wp53a",
            "eu/stage/wp54a",
            "eu/stage/wp55a",
            "eu/stage/wp61a",
            "eu/stage/wp62a",
            "eu/stage/wp63a",
            "eu/stage/wp64a",
            "eu/stage/wp65a",
            "eu/stage/wp71a",
            "eu/stage/wp72a",
            "eu/stage/wp73a",
            "eu/stage/wp74a",
            "eu/stage/wp75a",
            "eu/vox/_bp",
            "eu/vox2/_bp",
            "hqmovie/us/movie/p010_02_m01",
            "hqmovie/us/movie/p010_06_m02",
            "hqmovie/us/movie/p010_07_m03",
            "hqmovie/us/movie/p010_08_m04",
            "hqmovie/us/movie/p014_02_m01",
            "hqmovie/us/movie/p014_04_m02",
            "hqmovie/us/movie/p014_07_m03",
            "hqmovie/us/movie/p040_06_r02",
            "hqmovie/us/movie/p049_04_m02",
            "hqmovie/us/movie/p049_06_m03",
            "hqmovie/us/movie/p049_07_m04",
            "hqmovie/us/movie/p049_09_m05",
            "hqmovie/us/movie/p050_05_m03",
            "hqmovie/us/movie/p055_03_m01",
            "hqmovie/us/movie/p058_05_m01",
            "hqmovie/us/movie/p059_02_m02",
            "hqmovie/us/movie/p062_02_m01",
            "hqmovie/us/movie/p062_04_m02",
            "hqmovie/us/movie/p070_06_m01",
            "hqmovie/us/movie/p070_11_m02",
            "hqmovie/us/movie/p070_14_m03",
            "hqmovie/us/movie/p070_19_m05",
            "hqmovie/us/movie/p080_02_m01",
            "hqmovie/us/movie/p080_22_m09",
            "hqmovie/us/movie/p080_24_m10",
            "hqmovie/us/movievr/ending",
            "hqmovie/us/movievr/opening",
            "hqmovie/us/movievr/p080_06_m02",
            "hqmovie/us/movievr/p080_08_m03",
            "hqmovie/us/movievr/p080_12_m05",
            "hqmovie/us/movievr/p080_15_m06",
            "hqmovie/us/movievr/p080_17_m07",
            "hqmovie/us/movievr/p080_27_m11",
            "hqmovie/us/movievr/p082_03_m01",
            "hqmovie/us/movievr/p082_09_m04",
            "launcher_Data",
            "launcher_Data/Managed",
            "launcher_Data/Plugins",
            "launcher_Data/Plugins/x86_64",
            "launcher_Data/Resources",
            "launcher_Data/StreamingAssets",
            "launcher_Data/StreamingAssets/aa",
            "launcher_Data/StreamingAssets/aa/AddressablesLink",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/bonusassetsmgs2_assets_mgs2",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/bonusassetsmgs2_assets_mgs2/bonus",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/bonusassetsmgs2_assets_mgs2/bonus/image",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/animations",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/animations/guide",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/animations/title",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/bgm",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/fonts",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/materials",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/materials/ui",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/prefabs",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/prefabs/legalprivacy",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/se",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/device",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/guide",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/other_en",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/other_fr",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/other_gr",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/other_it",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/other_jp",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/other_sp",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/ps_en",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/ps_fr",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/ps_gr",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/ps_it",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/ps_jp",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/ps_sp",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_launcher/textures/title",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_material",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_module",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_module/fonts",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_module/sounddatamanagement",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_module/sounddatamanagement/sounddatas",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_module/textdata",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_shader",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_textmeshpro",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_assets_textmeshpro/shaders",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_scenes_module",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_scenes_module/scene",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_scenes_module/scene/sceneassets",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/defaultlocalgroup_scenes_scenes",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_launcher",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_launcher/bgm",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_launcher/bgm/mgs2",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_launcher/prefabs",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_launcher/prefabs/mgs2",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_launcher/textures",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_launcher/textures/mgs2",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_mgs2",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_mgs2/asset",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_module",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_module/credits",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_module/license",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_module/textdata",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_scenarioapp",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_scenarioapp/mgs2",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsmgs2_assets_scenarioapp/mgs2/bgm",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsrc_assets_launcher",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/packedassetsrc_assets_launcher/bgm",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/scenarioassetsmgs2_assets_mgs2",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/scenarioassetsmgs2_assets_mgs2/scenario",
            "launcher_Data/StreamingAssets/aa/StandaloneWindows64/scenarioassetsmgs2_assets_mgs2/scenario/image",
            "mgs2_savedata_win",
            "textures/flatlist/_win",
            "textures/flatlist/ovr_PS3/_win",
            "textures/flatlist/ovr_eu/_win",
            "textures/flatlist/ovr_stm/_win",
            "textures/flatlist/ovr_stm/ovr_eu/_win",
            "textures/flatlist/ovr_stm/ctrltype_kbd/_win",
            "textures/flatlist/ovr_stm/ctrltype_nx/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps4/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps4/ovr_eu/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps4/ovr_jp/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps5/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps5/ovr_eu/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps5/ovr_jp/_win",
            "textures/flatlist/ovr_stm/ctrltype_stmd/_win",
            "textures/flatlist/ovr_stm/ctrltype_xs/_win",
            "us/demo",
            "us/demo/_bp",
            "us/demo/p004_01_p01",
            "us/demo/p070_01_p01",
            "us/demo/t10a1D",
            "us/demo/t12a1D",
            "us/demo/t12a3D",
            "us/demo2/_bp",
            "us/movie",
            "us/movie/_bp",
            "us/movie/p010_02_m01",
            "us/movie/p010_06_m02",
            "us/movie/p010_07_m03",
            "us/movie/p010_08_m04",
            "us/movie/p014_02_m01",
            "us/movie/p014_04_m02",
            "us/movie/p014_07_m03",
            "us/movie/p014_14_m04",
            "us/movie/p036_09_m01",
            "us/movie/p040_06_r02",
            "us/movie/p049_04_m02",
            "us/movie/p049_06_m03",
            "us/movie/p049_07_m04",
            "us/movie/p049_09_m05",
            "us/movie/p050_03_m02",
            "us/movie/p050_05_m03",
            "us/movie/p055_03_m01",
            "us/movie/p058_05_m01",
            "us/movie/p059_02_m02",
            "us/movie/p062_02_m01",
            "us/movie/p062_04_m02",
            "us/movie/p070_06_m01",
            "us/movie/p070_11_m02",
            "us/movie/p070_14_m03",
            "us/movie/p070_19_m05",
            "us/movie/p080_02_m01",
            "us/movie/p080_22_m09",
            "us/movie/p080_24_m10",
            "us/movie/rmv001",
            "us/movie/vc003507",
            "us/movie/vc070002",
            "us/movie/vc070003",
            "us/movie/vc070004",
            "us/movie/vc070005",
            "us/movie/vc070006",
            "us/movie/vc070007",
            "us/movie/vc070008",
            "us/movie/vc070009",
            "us/movie/vc070010",
            "us/movie/vc070011",
            "us/movie/vc070012",
            "us/movie/vc070015",
            "us/movie/vc070016",
            "us/movie/vc070017",
            "us/movie/vc070018",
            "us/movie/vc070019",
            "us/movie/vc070020",
            "us/movie/vc070021",
            "us/movie/vc070022",
            "us/movie/vc070023",
            "us/movie/vc070024",
            "us/movie/vc070025",
            "us/movie/vc070026",
            "us/movie/vc070029",
            "us/movie/vc070030",
            "us/movie/vc070031",
            "us/movie/vc070100",
            "us/movie/vc070101",
            "us/movie/vc070102",
            "us/movie/vc070103",
            "us/movie/vc070104",
            "us/movie/vc070105",
            "us/movie/vc070106",
            "us/movie/vc070107",
            "us/movie/vc070108",
            "us/movie/vc070110",
            "us/movie/vc070111",
            "us/movie/vc070112",
            "us/movie/vc070113",
            "us/movie/vc070114",
            "us/movie/vc070121",
            "us/movie/vc070122",
            "us/movie/vc070123",
            "us/movie/vc070130",
            "us/movie/vc070132",
            "us/movie/vc070133",
            "us/movie/vc070134",
            "us/movie/vc070135",
            "us/movie/vc070136",
            "us/movie/vc070137",
            "us/movie/vc070138",
            "us/movie/vc070139",
            "us/movie/vc070140",
            "us/movie/vc070144",
            "us/movie/vc070200",
            "us/movie/vc080102",
            "us/movie/vc080402",
            "us/movie/vc080404",
            "us/movie/vc110107",
            "us/movie/vc110111",
            "us/movie/vc110304",
            "us/movie/vc110407",
            "us/movie/vc110802",
            "us/movie/vc110902",
            "us/movie/vc112408",
            "us/movie/vc114702",
            "us/movie/vc117102",
            "us/movie/vc260771",
            "us/movie/vc261722",
            "us/movie/vc266521",
            "us/movie/vc266611",
            "us/movie/vc266641",
            "us/movievr",
            "us/movievr/_bp",
            "us/movievr/baa01",
            "us/movievr/baa02",
            "us/movievr/baa03",
            "us/movievr/baa04",
            "us/movievr/baa05",
            "us/movievr/baa06",
            "us/movievr/baa07",
            "us/movievr/baa08-a",
            "us/movievr/baa08-b",
            "us/movievr/bab01-a",
            "us/movievr/bab01-b",
            "us/movievr/bab02",
            "us/movievr/bab03",
            "us/movievr/bab04",
            "us/movievr/bab05",
            "us/movievr/bab06",
            "us/movievr/bab07",
            "us/movievr/bab08",
            "us/movievr/bac01",
            "us/movievr/bac02",
            "us/movievr/bac03",
            "us/movievr/bac04",
            "us/movievr/bac05",
            "us/movievr/bac06",
            "us/movievr/bac07",
            "us/movievr/bac08",
            "us/movievr/ending",
            "us/movievr/opening",
            "us/movievr/p080_06_m02",
            "us/movievr/p080_08_m03",
            "us/movievr/p080_10_m04",
            "us/movievr/p080_12_m05",
            "us/movievr/p080_15_m06",
            "us/movievr/p080_17_m07",
            "us/movievr/p080_27_m11",
            "us/movievr/p082_03_m01",
            "us/movievr/p082_09_m04",
            "us/movievr/vr_bomb_r",
            "us/movievr/vr_bomb_s",
            "us/movievr/vr_el_r",
            "us/movievr/vr_el_s",
            "us/movievr/vr_fpv_r",
            "us/movievr/vr_fpv_s",
            "us/movievr/vr_ho_r",
            "us/movievr/vr_ho_s",
            "us/movievr/vr_photo_r",
            "us/movievr/vr_photo_s",
            "us/movievr/vr_sn_r",
            "us/movievr/vr_sn_s",
            "us/movievr/vr_va_r",
            "us/movievr/vr_va_s",
            "us/movievr/vr_wp_r",
            "us/movievr/vr_wp_s",
            "us/stage/a00a",
            "us/stage/a00b",
            "us/stage/a00c",
            "us/stage/a01a",
            "us/stage/a01b",
            "us/stage/a01c",
            "us/stage/a01d",
            "us/stage/a01e",
            "us/stage/a01f",
            "us/stage/a02a",
            "us/stage/a02b",
            "us/stage/a03a",
            "us/stage/a03b",
            "us/stage/a12a",
            "us/stage/a12b",
            "us/stage/a13a",
            "us/stage/a13b",
            "us/stage/a13c",
            "us/stage/a14a",
            "us/stage/a14b",
            "us/stage/a15a",
            "us/stage/a15b",
            "us/stage/a16a",
            "us/stage/a17a",
            "us/stage/a18a",
            "us/stage/a19a",
            "us/stage/a20a",
            "us/stage/a20b",
            "us/stage/a20c",
            "us/stage/a20e",
            "us/stage/a21a",
            "us/stage/a21b",
            "us/stage/a22a",
            "us/stage/a22b",
            "us/stage/a23a",
            "us/stage/a23b",
            "us/stage/a24a",
            "us/stage/a24b",
            "us/stage/a24c",
            "us/stage/a24d",
            "us/stage/a24f",
            "us/stage/a24g",
            "us/stage/a25a",
            "us/stage/a25d",
            "us/stage/a28a",
            "us/stage/a31a",
            "us/stage/a31b",
            "us/stage/a31c",
            "us/stage/a41a",
            "us/stage/a41b",
            "us/stage/a42a",
            "us/stage/a43a",
            "us/stage/a45a",
            "us/stage/a46a",
            "us/stage/a61a",
            "us/stage/d001p02",
            "us/stage/d005p01",
            "us/stage/d00t",
            "us/stage/d012p01",
            "us/stage/d01t",
            "us/stage/d021p01",
            "us/stage/mselect",
            "us/stage/n_title",
            "us/stage/r_plt0",
            "us/stage/r_plt1",
            "us/stage/r_plt2",
            "us/stage/r_plt3",
            "us/stage/r_plt4",
            "us/stage/r_plt5",
            "us/stage/r_title",
            "us/stage/r_tnk0",
            "us/stage/select",
            "us/stage/sp01a",
            "us/stage/sp02a",
            "us/stage/sp03a",
            "us/stage/sp06a",
            "us/stage/sp07a",
            "us/stage/sp08a",
            "us/stage/sp21a",
            "us/stage/sp22a",
            "us/stage/sp24a",
            "us/stage/sp25a",
            "us/stage/sselect",
            "us/stage/st01a",
            "us/stage/st02a",
            "us/stage/st03a",
            "us/stage/st04a",
            "us/stage/st05a",
            "us/stage/ta00a",
            "us/stage/ta01a",
            "us/stage/ta01b",
            "us/stage/ta01c",
            "us/stage/ta01d",
            "us/stage/ta01e",
            "us/stage/ta01f",
            "us/stage/ta02a",
            "us/stage/ta12a",
            "us/stage/ta20a",
            "us/stage/ta22a",
            "us/stage/ta24a",
            "us/stage/ta31a",
            "us/stage/ta42a",
            "us/stage/tales",
            "us/stage/tsp03a",
            "us/stage/tvs03a",
            "us/stage/tvs05a",
            "us/stage/tvs06a",
            "us/stage/tvs08a",
            "us/stage/twp03a",
            "us/stage/twp34a",
            "us/stage/twp43a",
            "us/stage/vs01a",
            "us/stage/vs02a",
            "us/stage/vs03a",
            "us/stage/vs04a",
            "us/stage/vs05a",
            "us/stage/vs06a",
            "us/stage/vs07a",
            "us/stage/vs08a",
            "us/stage/vs09a",
            "us/stage/vs10a",
            "us/stage/w00a",
            "us/stage/w00b",
            "us/stage/w00c",
            "us/stage/w01a",
            "us/stage/w01b",
            "us/stage/w01c",
            "us/stage/w01d",
            "us/stage/w01e",
            "us/stage/w01f",
            "us/stage/w02a",
            "us/stage/w03a",
            "us/stage/w03b",
            "us/stage/w04a",
            "us/stage/w04b",
            "us/stage/w04c",
            "us/stage/w11a",
            "us/stage/w11b",
            "us/stage/w11c",
            "us/stage/w12a",
            "us/stage/w12b",
            "us/stage/w12c",
            "us/stage/w13a",
            "us/stage/w13b",
            "us/stage/w14a",
            "us/stage/w15a",
            "us/stage/w15b",
            "us/stage/w16a",
            "us/stage/w16b",
            "us/stage/w17a",
            "us/stage/w18a",
            "us/stage/w19a",
            "us/stage/w20a",
            "us/stage/w20b",
            "us/stage/w20c",
            "us/stage/w20d",
            "us/stage/w21a",
            "us/stage/w21b",
            "us/stage/w22a",
            "us/stage/w23a",
            "us/stage/w23b",
            "us/stage/w24a",
            "us/stage/w24b",
            "us/stage/w24c",
            "us/stage/w24d",
            "us/stage/w24e",
            "us/stage/w25a",
            "us/stage/w25b",
            "us/stage/w25c",
            "us/stage/w25d",
            "us/stage/w28a",
            "us/stage/w31a",
            "us/stage/w31b",
            "us/stage/w31c",
            "us/stage/w31d",
            "us/stage/w31f",
            "us/stage/w32a",
            "us/stage/w32b",
            "us/stage/w41a",
            "us/stage/w42a",
            "us/stage/w43a",
            "us/stage/w44a",
            "us/stage/w45a",
            "us/stage/w46a",
            "us/stage/w51a",
            "us/stage/w61a",
            "us/stage/wp01a",
            "us/stage/wp02a",
            "us/stage/wp03a",
            "us/stage/wp04a",
            "us/stage/wp05a",
            "us/stage/wp11a",
            "us/stage/wp12a",
            "us/stage/wp13a",
            "us/stage/wp14a",
            "us/stage/wp15a",
            "us/stage/wp21a",
            "us/stage/wp22a",
            "us/stage/wp23a",
            "us/stage/wp24a",
            "us/stage/wp25a",
            "us/stage/wp31a",
            "us/stage/wp32a",
            "us/stage/wp33a",
            "us/stage/wp34a",
            "us/stage/wp35a",
            "us/stage/wp41a",
            "us/stage/wp42a",
            "us/stage/wp43a",
            "us/stage/wp44a",
            "us/stage/wp45a",
            "us/stage/wp51a",
            "us/stage/wp52a",
            "us/stage/wp53a",
            "us/stage/wp54a",
            "us/stage/wp55a",
            "us/stage/wp61a",
            "us/stage/wp62a",
            "us/stage/wp63a",
            "us/stage/wp64a",
            "us/stage/wp65a",
            "us/stage/wp71a",
            "us/stage/wp72a",
            "us/stage/wp73a",
            "us/stage/wp74a",
            "us/stage/wp75a",
            "us/vox",
            "us/vox/_bp"
        };

        public MGS2ModdingForm()
        {
            InitializeComponent();

            modInfoRichTextBox = new RichTextBox
            {
                Multiline = true,
                ReadOnly = true,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ScrollBars = RichTextBoxScrollBars.None,
                WordWrap = true,
                MaximumSize = new Size(312, 91),
                Size = new Size(312, 91),
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold)
            };
            this.Controls.Add(modInfoRichTextBox);

            this.FormClosing += new FormClosingEventHandler(MGS2ModdingForm_FormClosing);
        }

        private void MGS2ModdingForm_Load(object sender, EventArgs e)
        {
            this.Location = GuiManager.GetLastFormLocation();
            config = ConfigManager.LoadSettings();

            if (!CheckAndPromptForFolderPaths())
                return;

            string gameInstallPath = config.GamePaths["MGS2"];
            if (string.IsNullOrEmpty(gameInstallPath) || !Directory.Exists(gameInstallPath))
            {
                gameInstallPath = FindMGS2Installation();
                if (!string.IsNullOrEmpty(gameInstallPath))
                {
                    config.GamePaths["MGS2"] = gameInstallPath;
                    ConfigManager.SaveSettings(config);
                }
            }

            modListPanel = new FlowLayoutPanel
            {
                AutoScroll = true,
                Size = new Size((int)(this.Width / 1.5), this.Height - 80 - 80),
                Location = new Point(this.Width - (int)(this.Width / 1.5) - 50, 80),
                BackColor = ColorTranslator.FromHtml("#0f3930"),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 10, 20, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            this.Controls.Add(modListPanel);

            fileExplorerManager = new FileExplorerManager(config, this, modListPanel, "MGS2", expectedPaths);
            fileExplorerManager.SetupBackupFolders();
            fileExplorerManager.SetupModFolder();

            modListManager = new ModListManager(modListPanel);

            LoadModsIntoUI();

            if (!config.Backup.MGS2BackupCompleted)
            {
                fileExplorerManager.BackupVanillaFiles(gameInstallPath);
                config.Backup.MGS2BackupCompleted = true;
                ConfigManager.SaveSettings(config);
            }
        }

        #region First Time Setup

        private bool CheckAndPromptForFolderPaths()
        {
            if (!config.MGS2VanillaFolderSet)
            {
                DialogResult res = MessageBox.Show(
                    "Before you can modify the files we need to make a backup of your MGS2 Files.\n\nDo you want to use the default location for the MGS2 Vanilla Files folder?\n\nClick 'No' if you'd like to select your own location" +
                    "\nDefault location:\n" + config.MGS2VanillaFolderPath,
                    "Vanilla Files Location", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Cancel)
                {
                    GoBackToMainMenu();
                    return false;
                }
                else if (res == DialogResult.No)
                {
                    using (FolderBrowserDialog fbd = new FolderBrowserDialog()
                    {
                        SelectedPath = config.MGS2VanillaFolderPath,
                        Description = "Select a folder where the 'MGS2 Vanilla Files' folder will be created."
                    })
                    {
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            config.MGS2VanillaFolderPath = Path.Combine(fbd.SelectedPath, "MGS2 Vanilla Files");
                        }
                        else
                        {
                            GoBackToMainMenu();
                            return false;
                        }
                    }
                }
                config.MGS2VanillaFolderSet = true;
            }

            if (!config.MGS2ModFolderSet)
            {
                DialogResult res = MessageBox.Show(
                    "Now we need to set up a location where your mods will be stored.\n\nDo you want to use the default location for the MGS2 Mods folder?\n\nClick 'No' if you'd like to select your own location" +
                    "\nDefault location:\n" + config.MGS2ModFolderPath,
                    "MGS2 Mods Folder Location", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Cancel)
                {
                    GoBackToMainMenu();
                    return false;
                }
                else if (res == DialogResult.No)
                {
                    using (FolderBrowserDialog fbd = new FolderBrowserDialog()
                    {
                        SelectedPath = config.MGS2ModFolderPath,
                        Description = "Select a folder where the 'MGS2 Mods' folder will be created."
                    })
                    {
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            config.MGS2ModFolderPath = Path.Combine(fbd.SelectedPath, "MGS2 Mods");
                        }
                        else
                        {
                            GoBackToMainMenu();
                            return false;
                        }
                    }
                }
                config.MGS2ModFolderSet = true;
            }
            ConfigManager.SaveSettings(config);
            return true;
        }

        private string FindMGS2Installation()
        {
            string[] commonPaths = {
                @"C:\Program Files (x86)\Steam\steamapps\common\MGS2",
                @"A:\SteamLibrary\steamapps\common\MGS2",
                @"B:\SteamLibrary\steamapps\common\MGS2",
                @"C:\SteamLibrary\steamapps\common\MGS2",
                @"D:\SteamLibrary\steamapps\common\MGS2",
                @"E:\SteamLibrary\steamapps\common\MGS2",
                @"F:\SteamLibrary\steamapps\common\MGS2",
                @"G:\SteamLibrary\steamapps\common\MGS2",
            };
            return commonPaths.FirstOrDefault(Directory.Exists) ?? "";
        }

        private void GoBackToMainMenu()
        {
            LoggingManager.Instance.Log("User did not complete folder selection. Returning to Main Menu.\n");
            MainMenuForm mainMenu = new MainMenuForm();
            mainMenu.Show();
            this.FormClosing -= MGS2ModdingForm_FormClosing;
            this.Close();
        }

        #endregion

        #region Mod List GUI Setup

        private void LoadModsIntoUI()
        {
            if (!Directory.Exists(fileExplorerManager.ModFolder))
                return;
            string[] modDirs = Directory.GetDirectories(fileExplorerManager.ModFolder);
            List<ModListManager.ModItem> modItems = new List<ModListManager.ModItem>();

            foreach (string modPath in modDirs)
            {
                string modName = Path.GetFileName(modPath);
                bool isEnabled = config.Mods.ActiveMods.ContainsKey(modName) && config.Mods.ActiveMods[modName];
                bool isHDfix = fileExplorerManager.IsMGSHDFixMod(modPath);
                modItems.Add(new ModListManager.ModItem
                {
                    ModName = modName,
                    ModPath = modPath,
                    IsEnabled = isEnabled,
                    IsHDfix = isHDfix
                });
            }

            Point savedScroll = modListManager.ModListPanel.AutoScrollPosition;

            modListManager.LoadMods(modItems,
                onToggle: ToggleModAction,
                onRename: RenameModAction,
                onDelete: DeleteModAction,
                onSettings: SettingsAction,
                onHoverEnter: (modName, ctrl) => ShowModImage(modName, ctrl),
                onHoverLeave: () => HideModImage());

            modListManager.ModListPanel.AutoScrollPosition = new Point(-savedScroll.X, -savedScroll.Y);
        }

        #endregion

        #region Delegate Callback Methods

        private async void ToggleModAction(string modName)
        {
            string gameInstallPath = config.GamePaths["MGS2"];
            if (!Directory.Exists(gameInstallPath))
            {
                MessageBox.Show("Game installation not found, cannot apply mods.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Point savedScroll = modListManager.ModListPanel.AutoScrollPosition;
                await fileExplorerManager.ToggleModStateByNameAsync(modName, gameInstallPath);
                LoadModsIntoUI();
                modListManager.ModListPanel.AutoScrollPosition = new Point(-savedScroll.X, -savedScroll.Y);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenameModAction(string modName)
        {
            RenameModByName(modName);
            LoadModsIntoUI();
        }

        private void DeleteModAction(string modName)
        {
            Button dummy = new Button { Tag = modName };
            fileExplorerManager.DeleteMod(dummy, EventArgs.Empty);
            LoadModsIntoUI();
        }

        private void SettingsAction(string modName)
        {
            string modPath = Path.Combine(fileExplorerManager.ModFolder, modName);
            string iniPath = Path.Combine(modPath, "MGSHDFix.ini");
            if (!File.Exists(iniPath))
            {
                MessageBox.Show("MGSHDFix.ini not found in mod folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MGSHDFixSettingsForm settingsForm = new MGSHDFixSettingsForm(iniPath, this);
            settingsForm.ShowDialog();
            this.ActiveControl = modListManager.ModListPanel;
        }

        private void RenameModByName(string modName)
        {
            Button dummy = new Button { Tag = modName };
            RenameMod(dummy, EventArgs.Empty);
        }

        #endregion

        #region Hover Methods

        private void ShowModImage(string modName, Control modControl)
        {
            string modImagePath = Path.Combine(fileExplorerManager.ModFolder, modName, "Mod Details", "Mod Image.png");
            string modInfoPath = Path.Combine(fileExplorerManager.ModFolder, modName, "Mod Details", "Mod Info.txt");
            if (File.Exists(modImagePath))
                hoverPictureBox.Image = Image.FromFile(modImagePath);
            else
            {
                Bitmap blankImage = new Bitmap(312, 265);
                using (Graphics g = Graphics.FromImage(blankImage))
                    g.Clear(Color.White);
                hoverPictureBox.Image = blankImage;
            }
            hoverPictureBox.Size = new Size(312, 265);
            hoverPictureBox.Location = new Point(12, 193);
            hoverPictureBox.Visible = true;
            hoverPictureBox.BringToFront();
            ShowModInfo(modInfoPath);
        }

        private void ShowModInfo(string modInfoPath)
        {
            if (File.Exists(modInfoPath))
                modInfoRichTextBox.Text = File.ReadAllText(modInfoPath);
            else
                modInfoRichTextBox.Text = string.Empty;
            modInfoRichTextBox.Location = new Point(12, hoverPictureBox.Bottom);
            modInfoRichTextBox.Visible = true;
            modInfoRichTextBox.BringToFront();
        }

        private void HideModImage()
        {
            if (hoverPictureBox.Image != null)
            {
                hoverPictureBox.Image.Dispose();
                hoverPictureBox.Image = null;
            }
            hoverPictureBox.Visible = false;
            modInfoRichTextBox.Visible = false;
        }

        #endregion

        #region Mod Renaming and Editing

        private void RenameMod(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            string oldModName = button.Tag.ToString();
            string oldModPath = Path.Combine(fileExplorerManager.ModFolder, oldModName);
            string newModPath = oldModPath;
            if (MessageBox.Show("Do you want to rename the mod?", "Rename Mod", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string newModName = PromptForModName(oldModName);
                if (string.IsNullOrWhiteSpace(newModName) || newModName == oldModName)
                    return;
                newModPath = Path.Combine(fileExplorerManager.ModFolder, newModName);
                if (Directory.Exists(newModPath))
                {
                    MessageBox.Show($"A mod with the name '{newModName}' already exists.", "Rename Mod", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    Directory.CreateDirectory(newModPath);
                    Directory.CreateDirectory(Path.Combine(newModPath, "Mod Details"));
                    foreach (string dirPath in Directory.GetDirectories(oldModPath, "*", System.IO.SearchOption.AllDirectories))
                    {
                        string newDirPath = dirPath.Replace(oldModPath, newModPath);
                        if (!newDirPath.EndsWith("Mod Details", StringComparison.OrdinalIgnoreCase))
                            Directory.CreateDirectory(newDirPath);
                    }
                    foreach (string filePath in Directory.GetFiles(oldModPath, "*.*", System.IO.SearchOption.AllDirectories))
                    {
                        string newFilePath = filePath.Replace(oldModPath, newModPath);
                        if (!newFilePath.Contains(Path.Combine(newModPath, "Mod Details")))
                            File.Move(filePath, newFilePath);
                    }
                    FileSystem.DeleteDirectory(oldModPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    UpdateModNameInConfig(oldModName, newModName);
                    oldModPath = newModPath;
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show($"Access denied while renaming mod '{oldModName}':\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error renaming mod '{oldModName}':\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            string modDetailsPath = Path.Combine(oldModPath, "Mod Details");
            if (!Directory.Exists(modDetailsPath))
                Directory.CreateDirectory(modDetailsPath);
            if (MessageBox.Show("Do you want to select a new mod image?", "Select Mod Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                SelectModImage(modDetailsPath);
            if (MessageBox.Show("Do you want to edit the mod description?", "Edit Mod Description", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                EditModDescription(modDetailsPath);
            LoadModsIntoUI();
        }

        private string PromptForModName(string currentName)
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 400;
                prompt.Height = 150;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = "Rename Mod";
                prompt.StartPosition = FormStartPosition.CenterParent;
                Label textLabel = new Label() { Left = 20, Top = 20, Text = "Enter a new name for the mod:" };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340, Text = currentName };
                Button confirmation = new Button() { Text = "OK", Left = 280, Width = 80, Top = 80, DialogResult = DialogResult.OK };
                prompt.AcceptButton = confirmation;
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                return (prompt.ShowDialog() == DialogResult.OK) ? textBox.Text.Trim() : null;
            }
        }

        private void SelectModImage(string modDetailsPath)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Select a Mod Image";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = ofd.FileName;
                    string destinationImagePath = Path.Combine(modDetailsPath, "Mod Image.png");
                    File.Copy(selectedImagePath, destinationImagePath, true);
                }
            }
        }

        private void EditModDescription(string modDetailsPath)
        {
            string modInfoPath = Path.Combine(modDetailsPath, "Mod Info.txt");
            using (Form prompt = new Form())
            {
                prompt.Width = 400;
                prompt.Height = 300;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = "Edit Mod Description";
                prompt.StartPosition = FormStartPosition.CenterParent;
                Label textLabel = new Label() { Left = 20, Top = 20, Text = "Enter a new description for the mod:" };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340, Height = 150, Multiline = true, ScrollBars = ScrollBars.Vertical };
                if (File.Exists(modInfoPath))
                    textBox.Text = File.ReadAllText(modInfoPath);
                Button confirmation = new Button() { Text = "OK", Left = 280, Width = 80, Top = 220, DialogResult = DialogResult.OK };
                prompt.AcceptButton = confirmation;
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                if (prompt.ShowDialog() == DialogResult.OK)
                    File.WriteAllText(modInfoPath, textBox.Text.Trim());
            }
        }

        private void UpdateModNameInConfig(string oldModName, string newModName)
        {
            bool isActive = config.Mods.ActiveMods.ContainsKey(oldModName) && config.Mods.ActiveMods[oldModName];
            config.Mods.ActiveMods.Remove(oldModName);
            config.Mods.ActiveMods[newModName] = isActive;
            if (config.Mods.ModMappings.ContainsKey(oldModName))
            {
                var mappings = config.Mods.ModMappings[oldModName];
                config.Mods.ModMappings.Remove(oldModName);
                config.Mods.ModMappings[newModName] = mappings;
            }
            if (config.Mods.ReplacedFiles.ContainsKey(oldModName))
            {
                var replacedFiles = config.Mods.ReplacedFiles[oldModName];
                config.Mods.ReplacedFiles.Remove(oldModName);
                config.Mods.ReplacedFiles[newModName] = replacedFiles;
            }
            ConfigManager.SaveSettings(config);
        }

        #endregion

        #region Other Event Handlers

        private void MGS2ModdingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoggingManager.Instance.Log("User exiting the Mod Manager.\nEnd of log for this session.\n\n");
            Application.Exit();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("Going back to Main Menu from MGS2.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "MGS2ModdingForm");
            MainMenuForm mainMenu = new MainMenuForm();
            mainMenu.Show();
            this.Hide();
        }

        private void RefreshMods_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("Refreshing mods list in MGS2 form.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "MGS2ModdingForm");
            MGS2ModdingForm newForm = new MGS2ModdingForm();
            newForm.Show();
            this.Hide();
        }

        private void MoveVanillaFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                SelectedPath = config.MGS2VanillaFolderPath,
                Description = "Select a new location for the MGS2 Vanilla Files folder."
            })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string newFolderPath = Path.Combine(fbd.SelectedPath, "MGS2 Vanilla Files");
                    try
                    {
                        Directory.Move(config.MGS2VanillaFolderPath, newFolderPath);
                        fileExplorerManager.SetupBackupFolders();
                        config.MGS2VanillaFolderPath = newFolderPath;
                        ConfigManager.SaveSettings(config);
                        MessageBox.Show("Vanilla Files folder moved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error moving Vanilla Files folder:\n" + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RebuildVanillaFiles_Click(object sender, EventArgs e)
        {
            string gameInstallPath = config.GamePaths["MGS2"];
            if (string.IsNullOrEmpty(gameInstallPath) || !Directory.Exists(gameInstallPath))
            {
                MessageBox.Show("Game installation not found. Please set the game path first.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            fileExplorerManager.BackupVanillaFiles(gameInstallPath);
            config.Backup.MGS2BackupCompleted = true;
            ConfigManager.SaveSettings(config);
            MessageBox.Show("Vanilla files have been rebuilt successfully.",
                "Rebuild Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MoveMgs2ModFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                SelectedPath = config.MGS2ModFolderPath,
                Description = "Select a new location for the MGS2 Mods folder."
            })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string newFolderPath = Path.Combine(fbd.SelectedPath, "MGS2 Mods");
                    try
                    {
                        Directory.Move(fileExplorerManager.ModFolder, newFolderPath);
                        config.MGS2ModFolderPath = newFolderPath;
                        fileExplorerManager.SetupModFolder();
                        ConfigManager.SaveSettings(config);
                        MessageBox.Show("MGS2 Mods folder moved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error moving MGS2 Mods folder:\n" + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AddMods_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you adding a single mod?",
                "Add Mod(s)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
                return;
            using (FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                SelectedPath = fileExplorerManager.ModFolder,
                Description = (result == DialogResult.Yes) ?
                    "Select the mod folder you want to add." :
                    "Select the folder containing the mods you want to add."
            })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    if (result == DialogResult.Yes)
                        fileExplorerManager.ProcessModFolder(fbd.SelectedPath);
                    else if (result == DialogResult.No)
                    {
                        string[] modDirs = Directory.GetDirectories(fbd.SelectedPath);
                        if (modDirs.Length == 0)
                        {
                            MessageBox.Show("The selected folder does not contain any mod folders.",
                                "No Mods Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        foreach (string modPath in modDirs)
                        {
                            fileExplorerManager.ProcessModFolder(modPath);
                        }
                    }
                    ConfigManager.SaveSettings(config);
                    LoadModsIntoUI();
                }
            }
        }

        private void DownloadModsMGS2_Click(object sender, EventArgs e)
        {
            DownloadForm downloadManager = new DownloadForm();
            downloadManager.ShowDialog();
        }

        #endregion
    }
}
