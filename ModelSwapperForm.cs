using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public partial class ModelSwapperForm : Form
    {
        private string _gameDirectory;
        private string _backupDirectory;
        private List<MGSModel> ModelsToSwapIn { get; set; } = new List<MGSModel>();
        private List<MGSModel> ModelsToSwapOut { get; set; } = new List<MGSModel>();
        private List<string> filesModded = new();

        private List<string> GetFaceTexturesFromListOfIds(List<string> idList, string triFile, string newResidentId)
        {
            DirectoryInfo stagesDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "stage"));
            FileInfo[] bpAssetsList = stagesDirectory.GetFiles("bp_assets.txt", SearchOption.AllDirectories);
            
            List<string> textureList = new List<string>();
            foreach (string id in idList)
            {
                //Get resource string for desired asset
                string firstFileContainingId = (from file in bpAssetsList
                                                let fileText = File.ReadAllText(file.FullName)
                                                where fileText.Contains(id, StringComparison.CurrentCultureIgnoreCase)
                                                select file.FullName).FirstOrDefault();
                string lineContainingId = (from line in File.ReadAllLines(firstFileContainingId)
                                           where line.Contains(id, StringComparison.CurrentCultureIgnoreCase)// && line.Contains(triFile, StringComparison.CurrentCultureIgnoreCase) 
                                           select line).FirstOrDefault();
                //"textures/flatlist/ema_arm_sub_alp_ovl.bmp.ctxr,stage/a16a/cache/ema_arm_sub_alp_ovl.bmp.ctxr,eu/stage/a16a/cache/00535469/000361b9.ctxr

                string assetName = lineContainingId.Split('/', StringSplitOptions.RemoveEmptyEntries)[2].Split(',', StringSplitOptions.None)[0];

                //Create new resource string for asset in proper resident format
                textureList.Add($"textures/flatlist/{assetName},face/{newResidentId}/cache/{assetName},eu/face/{newResidentId}/cache/{triFile}/{id.ToLower()}.ctxr");
            }

            return textureList;
        }

        private List<string> GetCacheTexturesFromListOfIds(List<string> idList, string triFile, string newStageId)
        {
            DirectoryInfo stagesDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "stage"));
            FileInfo[] bpAssetsList = stagesDirectory.GetFiles("bp_assets.txt", SearchOption.AllDirectories);
            List<string> textureList = new List<string>();
            foreach (string id in idList)
            {
                string firstFileContainingId = (from file in bpAssetsList
                                                let fileText = File.ReadAllText(file.FullName)
                                                where fileText.Contains(id, StringComparison.CurrentCultureIgnoreCase)
                                                select file.FullName).FirstOrDefault();
                string lineContainingId = (from line in File.ReadAllLines(firstFileContainingId)
                                           where line.Contains(id, StringComparison.CurrentCultureIgnoreCase)// && line.Contains(triFile, StringComparison.CurrentCultureIgnoreCase) 
                                           select line).FirstOrDefault();

                string assetName = lineContainingId.Split('/', StringSplitOptions.RemoveEmptyEntries)[2].Split(',', StringSplitOptions.None)[0];

                textureList.Add($"textures/flatlist/{assetName},stage/{newStageId}/cache/{assetName},eu/stage/{newStageId}/cache/{triFile}/{id.ToLower()}.ctxr");
            }

            return textureList;
        }

        private List<string> GetResidentTexturesFromListOfIds(List<string> idList, string triFile, string newResidentId)
        {
            DirectoryInfo stagesDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "stage"));
            FileInfo[] bpAssetsList = stagesDirectory.GetFiles("bp_assets.txt", SearchOption.AllDirectories);
            List<string> textureList = new List<string>();
            foreach (string id in idList)
            {
                //Get resource string for desired asset
                string firstFileContainingId = (from file in bpAssetsList
                                                let fileText = File.ReadAllText(file.FullName)
                                                where fileText.Contains(id, StringComparison.CurrentCultureIgnoreCase)
                                                select file.FullName).FirstOrDefault();
                string lineContainingId = (from line in File.ReadAllLines(firstFileContainingId)
                                           where line.Contains(id, StringComparison.CurrentCultureIgnoreCase)// && line.Contains(triFile, StringComparison.CurrentCultureIgnoreCase) 
                                           select line).FirstOrDefault();
                //"textures/flatlist/ema_arm_sub_alp_ovl.bmp.ctxr,stage/a16a/cache/ema_arm_sub_alp_ovl.bmp.ctxr,eu/stage/a16a/cache/00535469/000361b9.ctxr

                string assetName = lineContainingId.Split('/', StringSplitOptions.RemoveEmptyEntries)[2].Split(',', StringSplitOptions.None)[0];

                //Create new resource string for asset in proper resident format
                textureList.Add($"textures/flatlist/{assetName},stage/{newResidentId}/resident/{assetName},eu/stage/{newResidentId}/resident/{triFile}/{id.ToLower()}.ctxr");
            }

            return textureList;
        }

        #region Models To Swap IN
        private void BuildModelsToSwapInList()
        {
            /* Template
             * 
            ModelsToSwapIn.Add(new MGSModel
            {
                Name = "",
                LoLoDId = "",
                LoLodKms = "",
                LoLoDTri = "",
                HiLoDId = "",
                HiLodKms = "",
                HiLoDTri = "",
                CutsceneEvm = "",
                CutsceneTri = "",
                CutsceneId = "",
                UseCutsceneAsCodec = true,
                LoLoDTextures = new(),
                HiLoDTextures = new(),
                CutsceneTextures = new()
            });
             */
            ModelsToSwapIn.Add(new MGSModel //done
            {
                Name = "Emma",
                LoLoDId = "00b35469",
                LoLodKms = "assets/kms/us/ema_def_sh_mt.kms,us/stage/XXXX/resident/00b35469.kms,resident/00b35469.kms",
                LoLoDTri = "assets/tri/us/ema_def_mh_mt.tri,us/stage/r_plt0/resident/00535469.tri,resident/00535469.tri", //is this actually a cutscene model instead?
                HiLoDId = "00535469",
                HiLodKms = "assets/kms/us/ema_def_sh_mt.kms,us/stage/XXXX/resident/00b35469.kms,resident/00b35469.kms", //reusing other LoD
                HiLoDTri = "assets/tri/us/ema_def_sh_mt.tri,us/stage/r_plt0/resident/00b35469.tri,resident/00b35469.tri",
                Headpiece = "assets/evm/us/ema_hair_mh_stage_a16a.evm,us/stage/XXXX/cache/00ce9e72.evm,cache/00ce9e72.evm",
                HeadpieceId = "00ce9e72",
                HeadpieceTri = "assets/tri/us/ema_hair_mh.tri,us/stage/r_plt0/resident/00ce9e72.tri,resident/00ce9e72.tri",
                CodecEvm = "assets/evm/us/ema_radio_mh_mt.evm,us/face/f04d/cache/00ad3007.evm,cache/00ad3007.evm",
                CodecId = "00ad3007",
                CodecTri = "assets/tri/us/ema_radio_mh_mt.tri,us/face/f04d/cache/00ad3007.tri,cache/00ad3007.tri",
                CutsceneEvm = "assets/evm/us/ema_def_mh_mt.evm,us/stage/w31d/cache/00535469.evm,cache/00535469.evm",
                CutsceneTri = "assets/tri/us/ema_def_mh_mt.tri,us/stage/w31d/cache/00535469.tri,cache/00535469.tri",
                CutsceneId = "00535469",
                HeadpieceTextures = new List<string>
                {
                    "0034CE8A",
            "00981182",
            "0000E994",
            "00FAD987",
            "00C3F987",
            "00DBDDC6",
            "008AEB0A",
            "00917197",
            "004E8B20",
            "002ED51B"
                },
                LoLoDTextures = new List<string>
                {
                    "000361B9",
            "00F33158",
            "00995033",
            "001ADA6E",
            "00A901BB",
            "00256123",
            "00B33B34",
            "00B34158",
            "0034CE8A",
            "00CE23D8",
            "00BF280E",
            "00741A88",
            "00980D82",
            "00D42992",
            "0019862E",
            "00980F82",
            "00681CDE",
            "00127870",
            "008B6076",
            "0000E994",
            "00114762",
            "00FAD987",
            "00D672E9",
            "0058B6D5",
            "00415727",
            "007A021C",
            "00ADDBA2",
            "003B9DA8",
            "00917197",
            "000577FC",
            "000E1816",
            "0025614B",
            "006E5D1F"
                },
                HiLoDTextures = new List<string>
                {
                    "000361B9",
            "00F33158",
            "00995033",
            "001ADA6E",
            "00A901BB",
            "00256123",
            "00B33B34",
            "00B34158",
            "00CE23D8",
            "00BF280E",
            "00741A88",
            "00980D82",
            "00D42B43",
            "0034962E",
            "00980F82",
            "00681CDE",
            "00127870",
            "008B6076",
            "00114762",
            "00FAD987",
            "00D672E9",
            "0058B6D5",
            "00415727",
            "007A021C",
            "00ADDBA2",
            "0071E293",
            "003B9DA8",
            "000577FC",
            "000E1816",
            "0025614B",
            "002ED62F",
            "00F87996",
            "00F87997",
            "00F87998",
            "00F87999",
            "00F87BB6",
            "00F87BB7",
            "00F87BB8",
            "00F87BB9",
            "006E5D1F"
                },
                CutsceneTextures = new()
                {
                    "000361B9",
            "00F33158",
            "00995033",
            "001ADA6E",
            "00A901BB",
            "00256123",
            "00B33B34",
            "00B34158",
            "00CE23D8",
            "00BF280E",
            "00741A88",
            "00980D82",
            "00D42B43",
            "0034962E",
            "00980F82",
            "00681CDE",
            "00127870",
            "008B6076",
            "00114762",
            "00FAD987",
            "00D672E9",
            "0058B6D5",
            "00415727",
            "007A021C",
            "00ADDBA2",
            "0071E293",
            "003B9DA8",
            "000577FC",
            "000E1816",
            "0025614B",
            "002ED62F",
            "00F87996",
            "00F87997",
            "00F87998",
            "00F87999",
            "00F87BB6",
            "00F87BB7",
            "00F87BB8",
            "00F87BB9",
            "006E5D1F"
                },
                CodecTextures = new()
                {
                    "00F33158",
            "0066ABB8",
            "001ADA6E",
            "005132DA",
            "00B34158",
            "0034CE8A",
            "00CE23D8",
            "00BF280E",
            "00741A88",
            "00980D82",
            "00D42B43",
            "0034962E",
            "00681CDE",
            "00127870",
            "008B6076",
            "00981182",
            "0087ABB8",
            "0000E994",
            "00114762",
            "00FAD987",
            "00D672E9",
            "00C3F987",
            "00DBDDC6",
            "008AEB0A",
            "0071E293",
            "003B9DA8",
            "00917197",
            "004E8B20",
            "002ED51B",
            "002ED62F",
            "00F87996",
            "00F87997",
            "00F87998",
            "00F87999",
            "00F87BB6",
            "00F87BB7",
            "00F87BB8",
            "00F87BB9"
                }
            }); //Emma: done
            ModelsToSwapIn.Add(new MGSModel
            {
                Name = "Fatman",
                LoLoDId = "00b35eb5",
                LoLodKms = "assets/kms/us/fat_def_sh_mt.kms,us/stage/a20c/resident/00b35eb5.kms,resident/00b35eb5.kms",
                LoLoDTri = "assets/tri/us/fat_def_sh_mt.tri,us/stage/a41b/resident/00b35eb5.tri,resident/00b35eb5.tri",
                HiLoDId = "00b35eb5",
                HiLodKms = "assets/kms/us/fat_def_sh_mt.kms,us/stage/a20c/resident/00b35eb5.kms,resident/00b35eb5.kms",
                HiLoDTri = "assets/tri/us/fat_def_sh_mt.tri,us/stage/a41b/resident/00b35eb5.tri,resident/00b35eb5.tri",
                CutsceneEvm = "assets/evm/us/fat_def_mh_mt.evm,us/stage/a20c/cache/00535eb5.evm,cache/00535eb5.evm",
                CutsceneTri = "assets/tri/us/fat_def_mh_mt.tri,us/stage/a20c/cache/00535eb5.tri,cache/00535eb5.tri",
                CutsceneId = "00535eb5",
                UseCutsceneAsCodec = true,
                LoLoDTextures = new()
                {
                    "005029C5",
            "00C5BF10",
            "0025EB22",
            "00BDEF11",
            "00E1EDF6",
            "00AFDC46",
            "00E38E80",
            "003BBF61",
            "009780A9",
            "00F44053",
            "002BA3B2",
            "003028BA",
            "0018898F",
            "00631F66",
            "001C759D",
            "0004FBE7",
            "0047D422",
            "00265B14",
            "00E7D00B",
            "00F2B069",
            "002808CB",
            "00050AB6",
            "00EA0794",
            "000DAAD0",
            "00699063",
            "00E8C343",
            "00246B67",
            "00300F97",
            "0072747C",
            "00FB1E91",
            "0008473B",
            "001B6A77",
            "003F8FDF",
            "00EBB033",
            "00C2A6A3",
            "0041B69E",
            "0041B69F",
            "0041B6A0",
            "0041B6A1",
            "0041B8BE",
            "0041B8BF",
            "0041B8C0",
            "0041B8C1",
            "004B8A60",
            "0035DB4F",
            "00728753",
            "0064CDA8",
            "00F58944",
            "005E485B",
            "007B6CBA",
            "00BB4354",
            "00C26257",
            "00AA8E47",
            "008D19B4",
            "00461CC0",
            "0062A1FA",
            "00BE8E1F",
            "000B589A"
                },
                HiLoDTextures = new(), //same as LoD
                CutsceneTextures = new()
                {
                    "005029C5",
            "00C5BF10",
            "0025EB22",
            "00BDEF11",
            "00E1EDF6",
            "00AFDC46",
            "00E38E80",
            "003BBF61",
            "009780A9",
            "00F44053",
            "00F58944",
            "002BA3B2",
            "003028BA",
            "0018898F",
            "00631F66",
            "001C759D",
            "0004FBE7",
            "0047D422",
            "00265B14",
            "00E7D00B",
            "00F2B069",
            "002808CB",
            "00050AB6",
            "00EA0794",
            "000DAAD0",
            "00699063",
            "00E8C343",
            "00246B67",
            "00300F97",
            "0072747C",
            "00FB1E91",
            "0008473B",
            "001B6A77",
            "003F8FDF",
            "00EBB033",
            "00C2A6A3",
            "0041B69E",
            "0041B69F",
            "0041B6A0",
            "0041B6A1",
            "0041B8BE",
            "0041B8BF",
            "0041B8C0",
            "0041B8C1",
            "004B8A60",
            "0035DB4F",
            "00728753",
            "0064CDA8",
            "005E485B",
            "007B6CBA",
            "00BB4354",
            "00C26257",
            "00AA8E47",
            "008D19B4",
            "00461CC0",
            "0062A1FA",
            "00BE8E1F",
            "000B589A"
                }
            }); //Fatman: done
            ModelsToSwapIn.Add(new MGSModel //done
            {
                Name = "Fortune",
                LoLoDId = "0051376b",
                LoLodKms = "assets/kms/us/for_def_sh_mt.kms,us/stage/XXXX/resident/00b365ad.kms,resident/00b365ad.kms",
                LoLoDTri = "assets/tri/us/for_def.tri,us/stage/r_plt0/resident/0051376b.tri,resident/0051376b.tri",
                HiLoDId = "0004ad70",
                HiLodKms = "assets/kms/us/for_def_sh_mt.kms,us/stage/XXXX/resident/00b365ad.kms,resident/00b365ad.kms", //reusing other LoD
                HiLoDTri = "assets/tri/us/fortune_mh_mt.tri,us/stage/r_plt0/resident/0004ad70.tri,resident/0004ad70.tri",
                CutsceneEvm = "assets/evm/us/for_def_mh_mt.evm,us/stage/d080p01/cache/005365ad.evm,cache/005365ad.evm",
                CutsceneTri = "assets/tri/us/for_def_stage_d078p01.tri,us/stage/d080p01/cache/005365ad.tri,cache/005365ad.tri",
                CutsceneId = "005365ad",
                //ShadowKms = "assets/kms/us/for_cort_shadow.kms,us/stage/d012p01/resident/006d2838.kms,resident/006d2838.kms",
                CodecEvm = "assets/evm/us/for_def_mh_mt.evm,us/stage/d080p01/resident/005365ad.evm,resident/005365ad.evm",
                CodecId = "005365ad",
                CodecTri = "assets/tri/us/for_def_stage_d078p01.tri,us/stage/d080p01/resident/005365ad.tri,resident/005365ad.tri",
                LoLoDTextures = new()
                {
                    "00512D12",
            "00517DFE",
            "002007A5",
            "00AF0DCB",
            "001FDB23",
            "00DA8B4C",
            "00D31DDD",
            "006EF8B3",
            "006EF8B4",
            "006FF37B",
            "000D0276",
            "00787367",
            "005EFDFF",
            "00574B69",
            "002816FE",
            "00744D49",
            "0057A390",
            "0028DECE",
            "001BD9F7",
            "00F3CC28",
            "00A347A5",
            "00BF5036",
            "0070DB5E",
            "00CAEC9E",
            "00CAECAF",
            "0074A7CD",
            "0013B767",
            "007D47E7",
            "00F71E00",
            "00B7B37E",
            "007A52EE",
            "002029A5",
            "005174BD",
            "00CC2E02",
            "005178AA"
                },
                HiLoDTextures = new()
                {
                    "00512D12",
            "00517DFE",
            "002007A5",
            "00AF0DCB",
            "001FDB23",
            "00DA8B4C",
            "00D31DDD",
            "006EF8B3",
            "006EF8B4",
            "006FF37B",
            "00138BFD",
            "003001DC",
            "000D0276",
            "005EFDFF",
            "00574B69",
            "002816FE",
            "00744D49",
            "00F3CC28",
            "00A347A5",
            "001AA2F9",
            "00545F55",
            "0054D480",
            "0054D481",
            "0054D482",
            "00BF5036",
            "0070DB5E",
            "00CAEC9E",
            "00CAECAF",
            "0074A7CD",
            "0013B767",
            "007D47E7",
            "00F71E00",
            "00B7B37E",
            "007A52EE",
            "002029A5",
            "005174BD",
            "00CC2E02",
            "00EB6E02",
            "00EBCE02",
            "005178AA",
            "008F00C7"
                },
                CodecTextures = new()
                {
                    "00512D12",
            "00517DFE",
            "002007A5",
            "00AF0DCB",
            "001FDB23",
            "00DA8B4C",
            "00D31DDD",
            "006EF8B3",
            "006EF8B4",
            "006FF37B",
            "00138BFD",
            "003001DC",
            "000D0276",
            "005EFDFF",
            "00574B69",
            "002816FE",
            "00744D49",
            "00F3CC28",
            "00A347A5",
            "001AA2F9",
            "00545F55",
            "0054D480",
            "0054D481",
            "0054D482",
            "00BF5036",
            "0070DB5E",
            "00CAEC9E",
            "00CAECAF",
            "0074A7CD",
            "0013B767",
            "007D47E7",
            "00F71E00",
            "00B7B37E",
            "007A52EE",
            "002029A5",
            "005174BD",
            "00CC2E02",
            "00EB6E02",
            "00EBCE02",
            "005178AA",
            "008F00C7"
                },
                CutsceneTextures = new()
                {
                    "00512D12",
            "00517DFE",
            "002007A5",
            "00AF0DCB",
            "001FDB23",
            "00DA8B4C",
            "00D31DDD",
            "006EF8B3",
            "006EF8B4",
            "000D0276",
            "005EFDFF",
            "00574B69",
            "002816FE",
            "00744D49",
            "0028DECE",
            "001BD9F7",
            "00F3CC28",
            "00A347A5",
            "00BF5036",
            "0070DB5E",
            "00CAEC9E",
            "00CAECAF",
            "0074A7CD",
            "0013B767",
            "007D47E7",
            "00F71E00",
            "00B7B37E",
            "007A52EE",
            "002029A5",
            "005174BD",
            "00CC2E02",
            "005178AA"
                }
            }); //Fortune: done
            ModelsToSwapIn.Add(new MGSModel //working now 8)
            {
                Name = "MGS1 Snake(new)",
                LoLoDId = "007c918b",
                LoLodKms = "assets/kms/us/sna_oss_sh_mt.kms,us/stage/r_plt10/resident/00bfa829.kms,resident/00bfa829.kms",
                LoLoDTri = "assets/tri/us/txd_oss_nin_sh_mt.tri,us/stage/r_plt10/resident/007c918b.tri,resident/007c918b.tri",
                HiLoDId = "007c918b",
                HiLodKms = "assets/kms/us/sna_oss_sh_mt.kms,us/stage/r_plt10/resident/00bfa829.kms,resident/00bfa829.kms",
                HiLoDTri = "assets/tri/us/txd_oss_nin_sh_mt.tri,us/stage/r_plt10/resident/007c918b.tri,resident/007c918b.tri",
                HandsId = "0055aab4",
                HandsTri = "assets/tri/us/sna_mgs_mt.tri,us/stage/r_vr_1/resident/007c2ac3.tri,resident/007c2ac3.tri",
                ArmsEvm = "snh_def_mh_mt_stage_r_vr_1_r.evm",
                ArmsId = "0055aab4",
                ArmsTri = "assets/tri/us/sna_def_mw_stage_r_vr_1_r.tri,us/stage/r_vr_1/resident/0055aab4.tri,resident/0055aab4.tri",
                CutsceneEvm = "assets/evm/us/sna_oss_mh_mt.evm,us/stage/r_plt10/cache/005fa829.evm,cache/005fa829.evm",
                CutsceneId = "007c918b",
                CutsceneTri = "assets/tri/us/txd_oss_nin_sh_mt.tri,us/stage/r_plt10/cache/007c918b.tri,cache/007c918b.tri",
                CutsceneTextures = new()
                {
                    "00645593",
            "00316CAF",
            "00316D7C",
            "0063CB6A",
            "00094F12",
            "002C0E2D",
            "0028CDB7",
            "00E17BD9",
            "00495322",
            "00080E08",
            "0009382A",
            "000A382A",
            "007480B7",
            "00B9B2FD",
            "0056B429",
            "00C2EE56",
            "00D6A54F",
            "00D55655",
            "00DDA6B1",
            "00B6B443",
            "00DCD475",
            "00FEC8DE",
            "00161B31",
            "00A80EC9",
            "00980F43",
            "00981143",
            "00DB58DD",
            "00AA8506",
            "00CB4A34",
            "0028BB78",
            "0036E597",
            "0051F44D",
            "00AE666B",
            "00C1F536",
            "00C3020F",
            "008D5B4B",
            "00D7CD75",
            "00ADCB61",
            "009A840D",
            "009A840E",
            "009A840F",
            "009A8410",
            "009A862D",
            "009A862E",
            "009A862F",
            "009A8630",
            "008DF696",
            "00A51D74",
            "0013A13E",
            "009BCD67",
            "00A599E9",
            "001EF345",
            "00853930",
            "0013613F",
            "00854417",
            "00C1D13F",
            "0073B05A",
            "007BAFAB",
            "007C5074",
            "00B94140",
            "00C8CD7C",
            "00AA9D74",
            "00AB71BB",
            "00FFEC06",
            "0011C9B2",
            "00B973FE",
            "00C04482",
            "00FD249B",
            "00AE1D74",
            "0057A3CD",
            "00ECC009",
            "00FE30E9",
            "008EF8CE",
            "002C853C",
            "002C893C",
            "008CE8B2",
            "008CECB2",
            "000A7840",
            "0028323E",
            "00A88C36",
            "002B83BA",
            "00D3DDD3",
            "00BBFBA3",
            "004F5CE7",
            "00CDD240",
            "00DB6963",
            "007182F5",
            "00C4F509",
            "00AE2857",
            "0005D3CE",
            "00AF9CE6"
                },
                UseCutsceneAsCodec = true,
                ArmTextures = new()
                {
                    "001B7235",
            "006E46D5",
            "0034DAFA",
            "0034DAFB",
            "008DF696",
            "0013A13E",
            "009BCD67"
                },
                HandTextures = new()
                {
                    "0028CDB7",
            "00E17BD9",
            "00495322",
            "00980F43",
            "00981143",
            "007152E3",
            "006FB16D",
            "00DB58DD",
            "00AA8506",
            "00AB8506",
            "00CB4A34",
            "0028BB78",
            "0036E597",
            "0051F44D",
            "00AE666B",
            "00C1F536",
            "00C3020F",
            "008D5B4B",
            "00D7CD75",
            "008DF696",
            "00A51D74",
            "0013A13E",
            "009BCD67",
            "00A599E9",
            "001EF345",
            "00853930",
            "0013613F",
            "00854417",
            "00C1D13F",
            "0073B05A",
            "007BAFAB",
            "007C5074",
            "00B94140",
            "00C8CD7C",
            "00AA9D74",
            "00AB71BB",
            "00FFEC06",
            "0011C9B2",
            "00B973FE",
            "00C04482",
            "00FD249B",
            "00AE1D74",
            "0057A3CD",
            "00ECC009",
            "00FE30E9",
            "008EF8CE",
            "002C853C",
            "002C893C",
            "008CE8B2",
            "008CECB2"
                },
                LoLoDTextures = new()
                {
                    "00645593",
            "00316CAF",
            "00316D7C",
            "0063CB6A",
            "00094F12",
            "002C0E2D",
            "0028CDB7",
            "00E17BD9",
            "00495322",
            "00080E08",
            "0009382A",
            "000A382A",
            "007480B7",
            "00B9B2FD",
            "0056B429",
            "00C2EE56",
            "00D6A54F",
            "00D55655",
            "00DDA6B1",
            "00B6B443",
            "00DCD475",
            "00FEC8DE",
            "00161B31",
            "00A80EC9",
            "00980F43",
            "00981143",
            "00DB58DD",
            "00AA8506",
            "00CB4A34",
            "0028BB78",
            "0036E597",
            "0051F44D",
            "00AE666B",
            "00C1F536",
            "00C3020F",
            "008D5B4B",
            "00D7CD75",
            "00ADCB61",
            "009A840D",
            "009A840E",
            "009A840F",
            "009A8410",
            "009A862D",
            "009A862E",
            "009A862F",
            "009A8630",
            "008DF696",
            "00A51D74",
            "0013A13E",
            "009BCD67",
            "00A599E9",
            "001EF345",
            "00853930",
            "0013613F",
            "00854417",
            "00C1D13F",
            "0073B05A",
            "007BAFAB",
            "007C5074",
            "00B94140",
            "00C8CD7C",
            "00AA9D74",
            "00AB71BB",
            "00FFEC06",
            "0011C9B2",
            "00B973FE",
            "00C04482",
            "00FD249B",
            "00AE1D74",
            "0057A3CD",
            "00ECC009",
            "00FE30E9",
            "008EF8CE",
            "002C853C",
            "002C893C",
            "008CE8B2",
            "008CECB2",
            "000A7840",
            "0028323E",
            "00A88C36",
            "002B83BA",
            "00D3DDD3",
            "00BBFBA3",
            "004F5CE7",
            "00CDD240",
            "00DB6963",
            "007182F5",
            "00C4F509",
            "00AE2857",
            "0005D3CE",
            "00AF9CE6"
                },
                HiLoDTextures = new()
            }); //MGS1 Snake: done
            //TODO: guards
            //      tengu, seal, genolas, nypd, cit_maley_ctg_sh, hostages
            //      cap_dead(only 1 bone, will not work)
            /*
            //Easy models to add with highish value:
                - Vamp
                - Ames(ric_def)
                - Ocelot (1998)
                - Ocelot (Tanker)
                - Ocelot (Plant)
                - Stillman
                - Johnson
                - Olga (Ninja) : 006e1ff7 is olga ninja?

            //Easy models to add with low value:
                - Scott Dolph
                - Genome Soldiers
                - Marines
                - tnc_def

            //Harder models to add with highish value:
                - Olga (Tanker) (hair)
                - Olga (Plant) (hair)
                - Meryl (hair)
                - Any Raiden model (hair)

            //Currently impossible:
                - Rose(no kms)
            */
            ModelsToSwapIn.Add(new MGSModel //working, reusing some of the other MGS1 snake's assets to fill in holes
            {
                Name = "MGS1 Snake(1998)",
                LoLoDId = "006e1ff7",
                LoLodKms = "assets/kms/us/sna_mgs1.kms,us/stage/r_plt10/resident/002bded9.kms,resident/002bded9.kms",
                LoLoDTri = "assets/tri/us/sna_mgs1_mh.tri,us/stage/r_plt10/resident/006e1ff7.tri,resident/006e1ff7.tri",//none?
                HiLoDId = "006e1ff7",
                HiLodKms = "assets/kms/us/sna_mgs1.kms,us/stage/r_plt10/resident/002bded9.kms,resident/002bded9.kms",
                HiLoDTri = "assets/tri/us/sna_mgs1_mh.tri,us/stage/r_plt10/resident/006e1ff7.tri,resident/006e1ff7.tri", //none?
                HandsId = "0055aab4",
                HandsTri = "assets/tri/us/sna_mgs_mt.tri,us/stage/r_vr_1/resident/007c2ac3.tri,resident/007c2ac3.tri",
                ArmsEvm = "snh_def_mh_mt_stage_r_vr_1_r.evm",
                ArmsId = "0055aab4",
                ArmsTri = "assets/tri/us/sna_def_mw_stage_r_vr_1_r.tri,us/stage/r_vr_1/resident/0055aab4.tri,resident/0055aab4.tri",
                CutsceneId = "006e1ff7",
                CutsceneTri = "assets/tri/us/sna_mgs1_mh.tri,us/stage/r_plt10/cache/006e1ff7.tri,cache/006e1ff7.tri",
                CutsceneEvm = "assets/evm/us/sna_mgs1_mh.evm,us/stage/r_plt10/cache/006e1ff7.evm,cache/006e1ff7.evm",
                UseCutsceneAsCodec = true,
                LoLoDTextures = new()
                {
                    //"0051D033", //cit3_shoes
            //"00926A67", //htc
            //"007AEAE9", //htc
            //"00482F73", //htc 
            //"00DD8A7B", //htc
            //"00085B45", //htc
            "000B52F7",
            //"00EBB045", //htc
            //"0084B7A6", //htc
            //"0025E761", //ocelot
            //"0025E762", //ocelot
            //"0025E763", //ocelot
            //"0025E764", //ocelot
            //"0025E765", //ocelot
            //"0025E766", //ocelot
            //"0025E767", //ocelot
            //"00265BE4", //ocelot
            //"00FA9B4B", //ocelot
            //"00FA9B4C", //ocelot
            //"00FA9B4D", //ocelot
            //"00FA9B4E", //ocelot
            //"00FA9B4F", //ocelot
            //"00F1229D", //ocelot
            //"00F1A29D", //ocelot
            //"00BFA5E0", //ocelot
            //"00044AD6", //ocelot
            //"00044AD7", //ocelot 
            //"002476B6", //ocelot
            //"002476B7", //ocelot
            //"002476B8", //ocelot
            //"002476B9", //ocelot
            //"002476BA", //ocelot
            //"00264476", //ocelot
            //"002943C1", //ocelot
            //"002943C2", //ocelot
            //"002943C3", //ocelot
            //"002943C4", //ocelot
            //"002B32A1", //ocelot
            //"002B32A2", //ocelot
            //"002B32A3", //ocelot
            //"002B32A4", //ocelot
            //"00864B96", //ocelot
            //"00864B97", //ocelot
            //"00864B98", //ocelot
            //"002C976C", //ocelot
            //"002C976D", //ocelot
            //"002C976E", //ocelot
            //"002C976F", //ocelot
            //"002C9770", //ocelot
            //"00BA5916", //ocelot
            //"002C9772", //ocelot
            //"00850209", //olga ninja
            //"005AD87C", //olga ninja
            //"005AE87C", //olga ninja
            //"0060399C", //olga ninja
            //"00D053A9", //olga ninja
            //"0060399D", //olga ninja
            //"0090B4EF", //olga ninja
            //"007495A3", //olga ninja
            //"0090C4EF", //olga ninja
            //"00762542", //olga ninja
            //"008AB508", //olga ninja
            //"00762543", //olga ninja
            //"008AC508", //olga ninja
            //"00762544", //olga ninja
            //"008AD508", //olga ninja
            //"001E0413", //olga ninja
            //"00996069", //olga ninja
            //"00EDE1C0", //olga ninja
            "00260A19",
            "00260A1A",
            "00267E9C",
            "00857B4C",
            "00857B4D",
            "00857B4E",
            "00857B4F",
            "001BA5F0",
            "001BA5F1",
            "0027C6B9",
            "0027C6BA",
            "00414330",
            "0028663A",
            "0028663B",
            "0029466C",
            "0028CDB7",
            "00414AA6",
            "00296679",
            "0029667A",
            "002B5559",
            "002B555A",
            "002B555B",
            "002B555C",
            "00415E93",
            "00415E94",
            "00415E95",
            "002C5513",
            "008AA297",
            //"00A7CDAE", //cti_1
            //"00FA292D", //cti_1
            //"00A7F808", //cti_1
            //"00372AFD", //cti_1
            //"00E55FD8", //cti_1
            //"00C7CDAE", //cti_3
            //"00452670", //cti_3
            //"009A86A2", //cti_3
            //"0050D485", //cti_3
            //"00FF014A", //cti_3
            //"005339D4", //cti_3
            //"00C12C5B", //cti_arm
            //"001A80B0", //cti_1
            //"001A80B2", //cti_3
            //"00AC7538", //cti_3
            //"00DF06AD", //hos
            /*"00CF3B99", //htc
            "00E19CF1", //htc
            "00E19E02", //htc
            "006ED893", //htc
            "00641E16", //htc
            "006A3B68", //htc
            "004A1EFA", //htc
            "00F73B0E", //htc
            "00EDBCE3", //htc
            "00F772AF", //htc
            "0077EAE9", //htc
            "00206E79", //htc
            "0023EE79", //htc
            "00FC3BAE", //htc
            "007E3BF4", //htc
            "003FA0CF", //htc
            "007E0837", //htc
            "00810837", //htc
            "00285D8A", //htc
            "004DF3C4", //htc
            "008F2B58", //htc
            "00F8634B", //htc
            "00A96BE9", //htc
            "004634E7", //olga ninja
            "004634E8", //olga ninja
            "00851209", //olga ninja
            "004634E9", //olga ninja
            "00852209", //olga ninja
            "004634EA", //olga ninja
            "00853209", //olga ninja
            "005AF87C", //olga ninja
            "002E2F47", //olga ninja
            "002E2F48", //olga ninja
            "002E2F49", //olga ninja
            "007092ED", //olga ninja
            "006564AF", //olga ninja
            "00185B6F", //olga ninja
            "00ED7F2B", //olga ninja
            "0060399E", //olga ninja
            "00D073A9", //olga ninja
            "007495A2", //olga ninja
            "007495A4", //olga ninja
            "0090D4EF", //olga ninja
            "007495A5", //olga ninja
            "0090E4EF", //olga ninja
            "00DE69A8", //olga ninja
            "00D11B8C", //olga ninja
            "00DE69A9", //olga ninja
            "00D12B8C", //olga ninja
            "00798B05", //olga ninja
            "00E6E53E", //olga ninja
            "001E2413", //olga ninja
            "0046899A", //olga ninja
            "00D0320E", //olga ninja
            "0046899B", //olga ninja
            "00D0420E", //olga ninja
            "0046899C", //olga ninja
            "0046899D", //olga ninja
            "00D0620E", //olga ninja
            "0046899E", //olga ninja
            "00D0720E", //olga ninja
            "007C4291", //olga ninja
            "0041AB75", //olga ninja
            "0041AB76", //olga ninja
            "00EDF1C0", //olga ninja
            "003C40D0", //olga ninja
            "002A81C2" //cit hair 1*/
                },
                HiLoDTextures = new(),
                ArmTextures = new()
                {
                    "001B7235",
            "006E46D5",
            "0034DAFA",
            "0034DAFB",
            "008DF696",
            "0013A13E",
            "009BCD67"
                },
                HandTextures = new()
                {
                    "0028CDB7",
            "00E17BD9",
            "00495322",
            "00980F43",
            "00981143",
            "007152E3",
            "006FB16D",
            "00DB58DD",
            "00AA8506",
            "00AB8506",
            "00CB4A34",
            "0028BB78",
            "0036E597",
            "0051F44D",
            "00AE666B",
            "00C1F536",
            "00C3020F",
            "008D5B4B",
            "00D7CD75",
            "008DF696",
            "00A51D74",
            "0013A13E",
            "009BCD67",
            "00A599E9",
            "001EF345",
            "00853930",
            "0013613F",
            "00854417",
            "00C1D13F",
            "0073B05A",
            "007BAFAB",
            "007C5074",
            "00B94140",
            "00C8CD7C",
            "00AA9D74",
            "00AB71BB",
            "00FFEC06",
            "0011C9B2",
            "00B973FE",
            "00C04482",
            "00FD249B",
            "00AE1D74",
            "0057A3CD",
            "00ECC009",
            "00FE30E9",
            "008EF8CE",
            "002C853C",
            "002C893C",
            "008CE8B2",
            "008CECB2"
                },
                CutsceneTextures = new()
                {
                    //"0051D033", //cit3_shoes
            //"00926A67", //htc
            //"007AEAE9", //htc
            //"00482F73", //htc 
            //"00DD8A7B", //htc
            //"00085B45", //htc
            "000B52F7",
            //"00EBB045", //htc
            //"0084B7A6", //htc
            //"0025E761", //ocelot
            //"0025E762", //ocelot
            //"0025E763", //ocelot
            //"0025E764", //ocelot
            //"0025E765", //ocelot
            //"0025E766", //ocelot
            //"0025E767", //ocelot
            //"00265BE4", //ocelot
            //"00FA9B4B", //ocelot
            //"00FA9B4C", //ocelot
            //"00FA9B4D", //ocelot
            //"00FA9B4E", //ocelot
            //"00FA9B4F", //ocelot
            //"00F1229D", //ocelot
            //"00F1A29D", //ocelot
            //"00BFA5E0", //ocelot
            //"00044AD6", //ocelot
            //"00044AD7", //ocelot 
            //"002476B6", //ocelot
            //"002476B7", //ocelot
            //"002476B8", //ocelot
            //"002476B9", //ocelot
            //"002476BA", //ocelot
            //"00264476", //ocelot
            //"002943C1", //ocelot
            //"002943C2", //ocelot
            //"002943C3", //ocelot
            //"002943C4", //ocelot
            //"002B32A1", //ocelot
            //"002B32A2", //ocelot
            //"002B32A3", //ocelot
            //"002B32A4", //ocelot
            //"00864B96", //ocelot
            //"00864B97", //ocelot
            //"00864B98", //ocelot
            //"002C976C", //ocelot
            //"002C976D", //ocelot
            //"002C976E", //ocelot
            //"002C976F", //ocelot
            //"002C9770", //ocelot
            //"00BA5916", //ocelot
            //"002C9772", //ocelot
            //"00850209", //olga ninja
            //"005AD87C", //olga ninja
            //"005AE87C", //olga ninja
            //"0060399C", //olga ninja
            //"00D053A9", //olga ninja
            //"0060399D", //olga ninja
            //"0090B4EF", //olga ninja
            //"007495A3", //olga ninja
            //"0090C4EF", //olga ninja
            //"00762542", //olga ninja
            //"008AB508", //olga ninja
            //"00762543", //olga ninja
            //"008AC508", //olga ninja
            //"00762544", //olga ninja
            //"008AD508", //olga ninja
            //"001E0413", //olga ninja
            //"00996069", //olga ninja
            //"00EDE1C0", //olga ninja
            "00260A19",
            "00260A1A",
            "00267E9C",
            "00857B4C",
            "00857B4D",
            "00857B4E",
            "00857B4F",
            "001BA5F0",
            "001BA5F1",
            "0027C6B9",
            "0027C6BA",
            "00414330",
            "0028663A",
            "0028663B",
            "0029466C",
            "0028CDB7",
            "00414AA6",
            "00296679",
            "0029667A",
            "002B5559",
            "002B555A",
            "002B555B",
            "002B555C",
            "00415E93",
            "00415E94",
            "00415E95",
            "002C5513",
            "008AA297",
            //"00A7CDAE", //cti_1
            //"00FA292D", //cti_1
            //"00A7F808", //cti_1
            //"00372AFD", //cti_1
            //"00E55FD8", //cti_1
            //"00C7CDAE", //cti_3
            //"00452670", //cti_3
            //"009A86A2", //cti_3
            //"0050D485", //cti_3
            //"00FF014A", //cti_3
            //"005339D4", //cti_3
            //"00C12C5B", //cti_arm
            //"001A80B0", //cti_1
            //"001A80B2", //cti_3
            //"00AC7538", //cti_3
            //"00DF06AD", //hos
            /*"00CF3B99", //htc
            "00E19CF1", //htc
            "00E19E02", //htc
            "006ED893", //htc
            "00641E16", //htc
            "006A3B68", //htc
            "004A1EFA", //htc
            "00F73B0E", //htc
            "00EDBCE3", //htc
            "00F772AF", //htc
            "0077EAE9", //htc
            "00206E79", //htc
            "0023EE79", //htc
            "00FC3BAE", //htc
            "007E3BF4", //htc
            "003FA0CF", //htc
            "007E0837", //htc
            "00810837", //htc
            "00285D8A", //htc
            "004DF3C4", //htc
            "008F2B58", //htc
            "00F8634B", //htc
            "00A96BE9", //htc
            "004634E7", //olga ninja
            "004634E8", //olga ninja
            "00851209", //olga ninja
            "004634E9", //olga ninja
            "00852209", //olga ninja
            "004634EA", //olga ninja
            "00853209", //olga ninja
            "005AF87C", //olga ninja
            "002E2F47", //olga ninja
            "002E2F48", //olga ninja
            "002E2F49", //olga ninja
            "007092ED", //olga ninja
            "006564AF", //olga ninja
            "00185B6F", //olga ninja
            "00ED7F2B", //olga ninja
            "0060399E", //olga ninja
            "00D073A9", //olga ninja
            "007495A2", //olga ninja
            "007495A4", //olga ninja
            "0090D4EF", //olga ninja
            "007495A5", //olga ninja
            "0090E4EF", //olga ninja
            "00DE69A8", //olga ninja
            "00D11B8C", //olga ninja
            "00DE69A9", //olga ninja
            "00D12B8C", //olga ninja
            "00798B05", //olga ninja
            "00E6E53E", //olga ninja
            "001E2413", //olga ninja
            "0046899A", //olga ninja
            "00D0320E", //olga ninja
            "0046899B", //olga ninja
            "00D0420E", //olga ninja
            "0046899C", //olga ninja
            "0046899D", //olga ninja
            "00D0620E", //olga ninja
            "0046899E", //olga ninja
            "00D0720E", //olga ninja
            "007C4291", //olga ninja
            "0041AB75", //olga ninja
            "0041AB76", //olga ninja
            "00EDF1C0", //olga ninja
            "003C40D0", //olga ninja
            "002A81C2" //cit hair 1*/
                }
            }); //Snake(1998): done
            ModelsToSwapIn.Add(new MGSModel //working
            {
                Name = "Ninja Raiden",
                LoLoDId = "00bd87ad",
                LoLodKms = "assets/kms/us/rai_nin_sh_mt_fg.kms,us/stage/r_plt6/resident/00466847.kms,resident/00466847.kms",
                LoLoDTri = "assets/tri/us/for_nin_sh_mt_stage_r_plt6_r.tri,us/stage/r_plt6/resident/00bd87ad.tri,resident/00bd87ad.tri",
                HiLoDId = "00bd87ad",
                ShadowKms = "rai_nin_shadow.kms",
                ShadowTri = "assets/tri/us/rai_shadow.tri,us/stage/r_vr_b/resident/00567f12.tri,resident/00567f12.tri",
                ShadowId = "00567f12",
                HandsId = "0031ead1",
                HandsTri = "assets/tri/us/rai_tex_mt_stage_r_vr_b_r.tri,us/stage/r_vr_b/resident/0031ead1.tri,resident/0031ead1.tri",
                ArmsEvm = "rah_def_mh_mt_stage_r_vr_b_r.evm",
                ArmsId = "0031ead4",
                ArmsTri = "assets/tri/us/rai_tex_mw_stage_r_vr_b_r.tri,us/stage/r_vr_b/resident/0031ead4.tri,resident/0031ead4.tri",
                HiLodKms = "assets/kms/us/rai_nin_sh_mt_fg.kms,us/stage/r_plt6/resident/00466847.kms,resident/00466847.kms",
                HiLoDTri = "assets/tri/us/for_nin_sh_mt_stage_r_plt6_r.tri,us/stage/r_plt6/resident/00bd87ad.tri,resident/00bd87ad.tri",
                CutsceneEvm = "assets/evm/us/rai_nin_mh_mt.evm,us/stage/r_plt10/resident/005e4089.evm,resident/005e4089.evm",
                CutsceneId = "00bd87ad",
                CutsceneTri = "assets/tri/us/for_nin_sh_mt.tri,us/stage/r_plt10/resident/00bd87ad.tri,resident/00bd87ad.tri",
                UseCutsceneAsCodec = true,
                ArmTextures = new()
                {
                    "00EACC11",
            "00A5EDE8",
            "00F7FC98",
            "00CFCD5B",
            "00F7FCB8",
            "00D9950B",
            "0032A1AD",
            "000FE152",
            "006EE7D7",
            "00EACC10",
            "00A5ECE8",
            "00C12FF5",
            "0009D1BF",
            "00D1C6EE",
            "008BBAFE",
            "00455786",
            "000D202A",
            "00A76EA7",
            "00586F0E",
            "00432751",
            "00A0519E",
            "00CFF7FA",
            "000268C0",
            "00B36926",
            "00A26A5C",
            "00CEE796",
            "0045578A",
            "002D202A",
            "00A76EAB",
            "00586F12",
            "004327D1",
            "008C2EFE",
            "005F6AE4",
            "0044C05D"
                },
                HandTextures = new()
                {
                    "00621F4B",
            "00E38F6A",
            "0096C0CB",
            "000F57E5",
            "00EF98F2",
            "00C12FF5",
            "0009D1BF",
            "00267211",
            "00C850DB",
            "002AB7C6",
            "004BED24",
            "007449F1",
            "0028736C",
            "00CDBCE2",
            "0096BECB",
            "00CDBCE3",
            "0096BFCB",
            "00CDBCE4",
            "004D4826",
            "008BBAFE",
            "00293B3C",
            "00ECFCE5",
            "00ECFCE6",
            "00D6C2EA",
            "00ECFCE7",
            "00D6C3EA",
            "00ECFCE8",
            "00D6C4EA",
            "00151827",
            "00B0857E",
            "004C64E9",
            "00C0C1EE",
            "00EF98EF",
            "00EF98F0",
            "00EF98F1",
            "00EF98F3"
                },
                LoLoDTextures = new()
                {
                    "00621F4B",
            "00E38F6A",
            "0096C0CB",
            "008BF724",
            "000F57E5",
            "00EF98F2",
            "0025EB9E",
            "00CBD8D7",
            "00A80EC9",
            "0027EB1E",
            "0000D3B5",
            "00838EFF",
            "00A810C9",
            "0043C780",
            "004B6496",
            "0096EBBC",
            "0043E879",
            "009FC6D4",
            "00B59038",
            "004CD8A8",
            "002EE7F0",
            "007C1B13",
            "00C9BDAA",
            "002C3697",
            "005FCAC1",
            "00A883AA",
            "00D6DF15",
            "00D89A98",
            "00AF440D",
            "002F27F3",
            "00387C1F",
            "00387C20",
            "00387C21",
            "00387C22",
            "00387E3F",
            "00387E40",
            "00387E41",
            "00387E42",
            "000ED1C5",
            "00C12FF5",
            "0009D1BF",
            "00267211",
            "00C850DB",
            "002AB7C6",
            "004BED24",
            "008B94FE",
            "0028736C",
            "00CDBCE2",
            "0096BECB",
            "00CDBCE3",
            "0096BFCB",
            "00CDBCE4",
            "004D4826",
            "008BBAFE",
            "00293B3C",
            "00ECFCE5",
            "00ECFCE6",
            "00D6C2EA",
            "00ECFCE7",
            "00D6C3EA",
            "00ECFCE8",
            "00D6C4EA",
            "00151827",
            "00B0857E",
            "00D773F5",
            "004C64E9",
            "00C0C1EE",
            "00544B61",
            "0041040E",
            "0041040F",
            "00410410",
            "0041062F",
            "00410630",
            "00EF98EF",
            "00EF98F0",
            "00EF98F1",
            "00EF98F3",
            "00EE8376",
            "00EE8387"
                },
                HiLoDTextures = new(),
                CutsceneTextures = new()
                {
                    "00512D12",
            "00517DFE",
            "00621F4B",
            "00E38F6A",
            "0096C0CB",
            "008BF724",
            "000F57E5",
            "00EF98F2",
            "002007A5",
            "00AF0DCB",
            "001FDB23",
            "00DA8B4C",
            "00D31DDD",
            "006EF8B3",
            "006EF8B4",
            "000D0276",
            "005EFDFF",
            "00574B69",
            "002816FE",
            "00744D49",
            "0028DECE",
            "001BD9F7",
            "00F3CC28",
            "00A347A5",
            "00BF5036",
            "0070DB5E",
            "00CAEC9E",
            "00CAECAF",
            "0074A7CD",
            "0013B767",
            "007D47E7",
            "00F71E00",
            "00B7B37E",
            "007A52EE",
            "00F16803",
            "002029A5",
            "005174BD",
            "00CC2E02",
            "005178AA",
            "00C12FF5",
            "0009D1BF",
            "00267211",
            "00C850DB",
            "002AB7C6",
            "004BED24",
            "008B94FE",
            "0028736C",
            "00CDBCE2",
            "0096BECB",
            "00CDBCE3",
            "0096BFCB",
            "00CDBCE4",
            "004D4826",
            "008BBAFE",
            "00293B3C",
            "00ECFCE5",
            "00ECFCE6",
            "00D6C2EA",
            "00ECFCE7",
            "00D6C3EA",
            "00ECFCE8",
            "00D6C4EA",
            "00151827",
            "00B0857E",
            "00D773F5",
            "004C64E9",
            "00C0C1EE",
            "00544B61",
            "0041040E",
            "0041040F",
            "00410410",
            "0041062F",
            "00410630",
            "00EF98EF",
            "00EF98F0",
            "00EF98F1",
            "00EF98F3",
            "00EE8376",
            "00EE8387"
                },
                ShadowTextures = new()
                {
                    "00F7FC98",
            "00CFCD5B",
            "00F7FC9C",
            "00F7FCB8",
            "000D202A",
            "00CFF7FA",
            "0011202A",
            "002D202A"
                }
            }); //Ninja Raiden: done
            ModelsToSwapIn.Add(new MGSModel //working but looks like shit lmao
            {
                //Has hair we could use
                Name = "Otacon",
                LoLoDId = "00b3f7f1",
                LoLodKms = "assets/kms/us/otc_def_sh_mt.kms,us/stage/d070p01/resident/00b3f7f1.kms,resident/00b3f7f1.kms",
                LoLoDTri = "assets/tri/us/otc_def_sh_mt.tri,us/stage/d070p01/resident/00b3f7f1.tri,resident/00b3f7f1.tri",
                HiLoDId = "00b3f7f1",
                HiLodKms = "assets/kms/us/otc_def_sh_mt.kms,us/stage/d070p01/resident/00b3f7f1.kms,resident/00b3f7f1.kms",
                HiLoDTri = "assets/tri/us/otc_def_sh_mt.tri,us/stage/d070p01/resident/00b3f7f1.tri,resident/00b3f7f1.tri",
                CutsceneEvm = "assets/evm/us/otc_def_mh_mt.evm,us/stage/d070p01/cache/0053f7f1.evm,cache/0053f7f1.evm",
                CutsceneTri = "assets/tri/us/otc_def_sh_mt.tri,us/stage/d070p01/cache/00b3f7f1.tri,cache/00b3f7f1.tri",
                CutsceneId = "00b3f7f1",
                CodecEvm = "assets/evm/us/otc_radio_mh_mt.evm,us/face/f00a/cache/003b500a.evm,cache/003b500a.evm",
                CodecTri = "assets/tri/us/otc_radio_mh_mt.tri,us/face/f00a/cache/003b500a.tri,cache/003b500a.tri",
                CodecId = "003b500a",
                LoLoDTextures = new()
                {
                    "00ED0DCD",
            "0001AEEC",
            "00838EFF",
            "00316CAF",
            "00316D7C",
            "004CD8A8",
            "0063CB6A",
            "005FCAC1",
            "00A883AA",
            "00981182",
            "003F8FDF",
            "00EBB033",
            "0025EB9E",
            "0030043F",
            "00CBD8D7",
            "00A80EC9",
            "0027EB1E",
            "00614033",
            "00FFC9F1",
            "00A810C9",
            "00282691",
            "002A8137",
            "00748C74",
            "00FA703B",
            "009FC6D4",
            "002EE7F0",
            "002C3697",
            "00D89A98",
            "000ED1C5"
                },
                HiLoDTextures = new(),
                CutsceneTextures = new()
                {
                    "00ED0DCD",
            "0001AEEC",
            "00838EFF",
            "00316CAF",
            "00316D7C",
            "004CD8A8",
            "0063CB6A",
            "005FCAC1",
            "00A883AA",
            "00981182",
            "003F8FDF",
            "00EBB033",
            "0025EB9E",
            "0030043F",
            "00CBD8D7",
            "00A80EC9",
            "0027EB1E",
            "00614033",
            "00FFC9F1",
            "00A810C9",
            "00282691",
            "002A8137",
            "00748C74",
            "00FA703B",
            "009FC6D4",
            "002EE7F0",
            "002C3697",
            "00D89A98",
            "000ED1C5"
                },
                CodecTextures = new()
                {
                    "0025EB9E",
            "00CBD8D7",
            "00A80EC9",
            "0027EB1E",
            "0000D3B5",
            "00838EFF",
            "00A810C9",
            "0043C780",
            "0096EBBC",
            "0043E879",
            "004CD8A8",
            "001B353D",
            "00030858",
            "009B225E",
            "009B225F",
            "007C1B13",
            "00C9BDAA",
            "002C3697",
            "005FCAC1",
            "002F27F3",
            "00387C1F",
            "00387C20",
            "00387C21",
            "00387C22",
            "00387E3F",
            "00387E40",
            "00387E41",
            "00387E42",
            "000ED1C5"
                }
            }); //Otacon: done
            ModelsToSwapIn.Add(new MGSModel
            {
                Name = "Pigeon",
                LoLoDId = "001139db",
                LoLodKms = "assets/kms/us/pgn_def.kms,us/stage/d080p08/resident/001139db.kms,resident/001139db.kms",
                LoLoDTri = "assets/tri/us/pgn_def.tri,us/stage/d080p08/resident/001139db.tri,resident/001139db.tri",
                HiLoDId = "001139db",
                HiLodKms = "assets/kms/us/pgn_def.kms,us/stage/d080p08/resident/001139db.kms,resident/001139db.kms",
                HiLoDTri = "assets/tri/us/pgn_def.tri,us/stage/d080p08/resident/001139db.tri,resident/001139db.tri",
                LoLoDTextures = new()
                {
                    "0026639B",
            "002864F6",
            "002B3BF5",
            "002F2C2E",
            "005E403C",
            "00293B26",
            "006F403C"
                },
                HiLoDTextures = new()
            }); //pigeon: xdd
            ModelsToSwapIn.Add(new MGSModel //working PogBones
            {
                Name = "Pliskin",
                LoLoDId = "00b39721",
                LoLodKms = "assets/kms/us/iro_def_sh_mt.kms,us/stage/d014p01/resident/00b39721.kms,resident/00b39721.kms",
                LoLoDTri = "assets/tri/us/iro_def_mt.tri,us/stage/w14a/resident/001a1ab0.tri,resident/001a1ab0.tri",
                HiLoDId = "00b39721",
                HiLodKms = "assets/kms/us/iro_def_sh_mt.kms,us/stage/d014p01/resident/00b39721.kms,resident/00b39721.kms",
                HiLoDTri = "assets/tri/us/iro_def_sh_mt.tri,us/stage/d014p01/resident/00b39721.tri,resident/00b39721.tri",
                CutsceneEvm = "assets/evm/us/iro_def_mh_mt_stage_d014p01.evm,us/stage/d014p01/cache/00539721.evm,cache/00539721.evm",
                CutsceneTri = "assets/tri/us/iro_def_mh_mt.tri,us/stage/d014p01/cache/00539721.tri,cache/00539721.tri",
                CutsceneId = "00539721",
                CodecEvm = "assets/evm/us/iro_radio_mh_mt.evm,us/face/f01c/cache/00b81008.evm,cache/00b81008.evm",
                CodecTri = "assets/tri/us/iro_radio_mh_mt.tri,us/face/f01c/cache/00b81008.tri,cache/00b81008.tri",
                CodecId = "00b81008",
                ArmsEvm = "snh_def_mh_mt_stage_r_vr_p_r.evm",
                ArmsTri = "assets/tri/us/iro_def_mw.tri,us/stage/r_vr_p/resident/001a1ab3.tri,resident/001a1ab3.tri",
                ArmsId = "001a1ab3",
                HandsTri = "assets/tri/us/iro_def_mt_stage_r_vr_p_r.tri,us/stage/r_vr_p/resident/001a1ab0.tri,resident/001a1ab0.tri",
                HandsId = "001a1ab0",
                LoLoDTextures = new()
                {
                    "00B76CD0",
            "00B76CD6",
            "0025F675",
            "0025F676",
            "0025F677",
            "0025F678",
            "0025F679",
            "0025F67A",
            "00260758",
            "00262E5D",
            "00C5F777",
            "0027F629",
            "00E2EB6A",
            "00BD2DB2",
            "0009382A",
            "000A382A",
            "007480B7",
            "0028F788",
            "00215832",
            "002BF6F0",
            "00DCD475",
            "0057236C",
            "00617DC6",
            "00BECA14",
            "0087FC71",
            "002E9EF2",
            "00FF37E6",
            "00FEC8DE"
                },
                HiLoDTextures = new()
                {
                    "00B76CD0",
            "00B76CD6",
            "0025F675",
            "0025F676",
            "0025F677",
            "0025F678",
            "0025F679",
            "0025F67A",
            "00260758",
            "00262E5D",
            "00C5F777",
            "00080E08",
            "00BD2DB2",
            "0009382A",
            "000A382A",
            "007480B7",
            "0028F788",
            "00B9B2FD",
            "0056B429",
            "00C2EE56",
            "00D6A54F",
            "00D55655",
            "00215832",
            "002BF6F0",
            "00DCD475",
            "0057236C",
            "00617DC6",
            "00BECA14",
            "0087FC71",
            "002E9EF2",
            "00FF37E6",
            "00FEC8DE"
                },
                CutsceneTextures = new()
                {
                    "00B76CD0",
            "00B76CD6",
            "0025F675",
            "0025F676",
            "0025F677",
            "0025F678",
            "0025F679",
            "0025F67A",
            "00260758",
            "00262E5D",
            "00C5F777",
            "00080E08",
            "00BD2DB2",
            "0009382A",
            "000A382A",
            "007480B7",
            "0028F788",
            "00B9B2FD",
            "0056B429",
            "00C2EE56",
            "00D6A54F",
            "00D55655",
            "00DDA6B1",
            "00B6B443",
            "00215832",
            "002BF6F0",
            "00DCD475",
            "0057236C",
            "00617DC6",
            "00BECA14",
            "0087FC71",
            "002E9EF2",
            "00FF37E6",
            "00FEC8DE"
                },
                CodecTextures = new()
                {
                    "00B76CD0",
            "00B76CD6",
            "0025F675",
            "00262E5D",
            "00080E08",
            "0009382A",
            "000A382A",
            "007480B7",
            "00B9B2FD",
            "0056B429",
            "00C2EE56",
            "00D6A54F",
            "00D55655",
            "00DDA6B1",
            "00B6B443",
            "00DCD475",
            "0057236C",
            "00617DC6",
            "00BECA14",
            "0087FC71",
            "00FF37E6",
            "00FEC8DE"
                },
                ArmTextures = new()
                {
                    "00B76CD0",
            "00ED9B69",
            "001AD46D",
            "005A8DD5",
            "00F4D60B",
            "00F4D60C",
            "002E9EF2"
                },
                HandTextures = new()
                {
                    "00B76CD0",
            "00B76CD6",
            "0025F675",
            "0025F676",
            "0025F677",
            "0025F678",
            "0025F679",
            "0025F67A",
            "00260758",
            "00262E5D",
            "00C5F777",
            "00080E08",
            "0027F629",
            "00E2EB6A",
            "00BD2DB2",
            "0009382A",
            "000A382A",
            "007480B7",
            "0028F788",
            "00B9B2FD",
            "0056B429",
            "00C2EE56",
            "00D6A54F",
            "00D55655",
            "00215832",
            "002BF6F0",
            "00DCD475",
            "0057236C",
            "00617DC6",
            "00BECA14",
            "0087FC71",
            "002E9EF2",
            "00FF37E6",
            "00FEC8DE"
                }
            }); //Pliskin: done
            ModelsToSwapIn.Add(new MGSModel
            {
                Name = "RAY",
                LoLoDId = "0029a441",
                LoLodKms = "assets/kms/us/pdray_def_mt.kms,us/stage/w46a/resident/0029a441.kms,resident/0029a441.kms",
                LoLoDTri = "assets/tri/us/pdray_def_mt.tri,us/stage/w46a/resident/0029a441.tri,resident/0029a441.tri",
                HiLoDId = "0029a441",
                HiLodKms = "assets/kms/us/pdray_def_mt.kms,us/stage/w46a/resident/0029a441.kms,resident/0029a441.kms",
                HiLoDTri = "assets/tri/us/pdray_def_mt.tri,us/stage/w46a/resident/0029a441.tri,resident/0029a441.tri",
                LoLoDTextures = new()
                {
                    "00841D78",
            "00FD24F8",
            "00A49F92",
            "005E0815",
            "005E0816",
            "00DF8964",
            "00DF8984",
            "0056F55D",
            "0037B3CB",
            "00214FCA",
            "00EDE4A3",
            "004096DC",
            "00EFB655",
            "00458ABE",
            "00D43656",
            "006B15C0",
            "00929457",
            "006B19C0",
            "00D29457",
            "006BD01C",
            "00926445",
            "00074E8E",
            "001F6E1B",
            "000750AE",
            "00416E1B",
            "00417E1B",
            "00436E1B",
            "00CE7684",
            "00B6D16E",
            "0059F07B",
            "00433AA0",
            "00D492C1",
            "0062AAEF",
            "00C6F174",
            "00341025",
            "008E6562",
            "00B3EC87",
            "001B3EF4",
            "001F11F8",
            "009AE34E",
            "00A4D7E6",
            "00B4F7F3",
            "000EBDDE",
            "00146E92",
            "00A9CB22",
            "009B85C8",
            "000EBFFE",
            "00F838E9",
            "006E73BA",
            "009A8452",
            "009A7AF3",
            "00D4D63F",
            "009A8AF3",
            "00B148F0",
            "0011DC7E",
            "002D51B3",
            "000750AF",
            "000750CE",
            "00A9AA0E",
            "006EA326",
            "002D5236",
            "002DD236",
            "00478156",
            "005A8645",
            "000EC01E",
            "000EC03E",
            "007BB74E",
            "00D4D63E"
                },
                HiLoDTextures = new()
            }); //ray: xdd
            ModelsToSwapIn.Add(new MGSModel //works, but ofc no hair
            {
                Name = "Raiden",
                LoLoDId = "00b41e89",
                LoLodKms = "assets/kms/us/rai_def_sh_mt.kms,us/stage/d014p01/resident/00b41e89.kms,resident/00b41e89.kms",
                LoLoDTri = "assets/tri/us/rai_def_sh_mt.tri,us/stage/d014p01/resident/00b41e89.tri,resident/00b41e89.tri",
                HiLoDId = "00b41e89",
                HiLodKms = "assets/kms/us/rai_def_sh_mt.kms,us/stage/d014p01/resident/00b41e89.kms,resident/00b41e89.kms",
                HiLoDTri = "assets/tri/us/rai_def_sh_mt.tri,us/stage/d014p01/resident/00b41e89.tri,resident/00b41e89.tri",
                CutsceneEvm = "assets/evm/us/rai_def_mh_mt.evm,us/stage/a45a/cache/00541e89.evm,cache/00541e89.evm",
                CutsceneTri = "assets/tri/us/raiden.tri,us/stage/d070p01/cache/00541e89.tri,cache/00541e89.tri",
                CutsceneId = "00541e89",
                CodecEvm = "assets/evm/us/rai_radio_mh_mt.evm,us/face/f04d/cache/00d5b00a.evm,cache/00d5b00a.evm",
                CodecTri = "assets/tri/us/rai_radio_mh_mt.tri,us/face/f02a/cache/00d5b00a.tri,cache/00d5b00a.tri",
                CodecId = "00d5b00a",
                LoLoDTextures = new()
                {
                    "00E38F6A",
            "0070AB9D",
            "0035C82C",
            "008D3897",
            "0009D1BF",
            "00D1C6EE",
            "00EF4607",
            "00E1C6EE",
            "00EF4707",
            "0067F633",
            "00F3ACCE",
            "004D4826",
            "008BBAFE",
            "001B1F05",
            "0070AA9D",
            "002B1F05",
            "00B0857E",
            "004C64E9",
            "00C0C1EE",
            "0047BA4D",
            "00252D67",
            "00A7BA4D",
            "00253367",
            "00F40B56",
            "00A0096B",
            "00B0096B",
            "003CCAC6",
            "003CCBC6",
            "0052D5D3",
            "00DD8E18",
            "0052D5D4",
            "00DD9E18"
                },
                HiLoDTextures = new(),
                CutsceneTextures = new()
                {
                    "00E38F6A",
            "0070AB9D",
            "008BF724",
            "0035C82C",
            "008D3897",
            "0009D1BF",
            "00D1C6EE",
            "00EF4607",
            "00E1C6EE",
            "00EF4707",
            "008B94FE",
            "0067F633",
            "00F3ACCE",
            "004D4826",
            "008BBAFE",
            "001B1F05",
            "0070AA9D",
            "002B1F05",
            "00B0857E",
            "00D773F5",
            "004C64E9",
            "00C0C1EE",
            "00544B61",
            "0041040E",
            "0041040F",
            "00410410",
            "0041062F",
            "00410630",
            "0047BA4D",
            "00252D67",
            "00A7BA4D",
            "00253367",
            "00EE8376",
            "00EE8387",
            "00F40B56",
            "00A0096B",
            "00B0096B",
            "003CCAC6",
            "003CCBC6",
            "0052D5D3",
            "00DD8E18",
            "0052D5D4",
            "00DD9E18"
                },
                CodecTextures = new()
                {
                    "00F7FC9C",
            "008BF724",
            "0091333C",
            "008D3897",
            "0009D1BF",
            "00D1C6EE",
            "00EF4607",
            "00E1C6EE",
            "00EF4707",
            "008BBAFE",
            "00C55786",
            "0011202A",
            "00276EA8",
            "00D86F0E",
            "00CEE796",
            "00432761",
            "00B0857E",
            "004C64E9",
            "00C0C1EE",
            "00544B61",
            "0041040D",
            "0041040E",
            "0041040F",
            "00410410",
            "0041062D",
            "0041062E",
            "0041062F",
            "00410630",
            "008C2EFE"
                }

            }); //Raiden: done, but no hair or arms cuz i forgor
            ModelsToSwapIn.Add(new MGSModel //Snake: works for raiden, but gives second pistol and holster... hmmm
            {
                Name = "Snake",
                LoLoDId = "0055aab1",
                LoLodKms = "assets/kms/us/sna_def_sh_stage_r_plt10_r.kms,us/stage/r_plt10/resident/0055ab65.kms,resident/0055ab65.kms",
                LoLoDTri = "assets/tri/us/sna_def_mt.tri,us/stage/r_plt10/resident/0055aab1.tri,resident/0055aab1.tri",
                HiLoDId = "0055aab1",
                HiLodKms = "assets/kms/us/sna_def_sh_stage_r_plt10_r.kms,us/stage/r_plt10/resident/0055ab65.kms,resident/0055ab65.kms",
                HiLoDTri = "assets/tri/us/sna_def_mt.tri,us/stage/r_plt10/resident/0055aab1.tri,resident/0055aab1.tri",
                ArmsEvm = "assets/evm/us/snh_def_mh_mt.evm,us/stage/r_tnk0/resident/00543505.evm,resident/00543505.evm",
                ArmsTri = "assets/tri/us/sna_def_mw.tri,us/stage/r_vr_s/resident/0055aab4.tri,resident/0055aab4.tri",
                ArmsId = "0055aab4",
                HandsTri = "assets/tri/us/sna_def_mt_stage_r_plt_s_r.tri,us/stage/r_vr_s/resident/0055aab1.tri,resident/0055aab1.tri",
                HandsId = "0055aab1",
                CutsceneEvm = "assets/evm/us/sna_def_mh.evm,us/stage/w01e/cache/0055aaa5.evm,cache/0055aaa5.evm",
                CutsceneTri = "assets/tri/us/sna_def_mh.tri,us/stage/w01e/cache/0055aaa5.tri,cache/0055aaa5.tri",
                CutsceneId = "0055aaa5",
                CodecEvm = "assets/evm/us/sna_radio_mh_mt.evm,us/face/f00a/cache/002f300b.evm,cache/002f300b.evm",
                CodecTri = "assets/tri/us/sna_radio_mh_mt.tri,us/face/f00a/cache/002f300b.tri,cache/002f300b.tri",
                CodecId = "002f300b",
                LoLoDTextures = new()
                {
                    "0074BD1F",
            "0028CDB7",
            "00E17BD9",
            "00495322",
            "00833F12",
            "00A5217E",
            "009D3B34",
            "00ADC198",
            "00B83347",
            "009D72D5",
            "00854CFE",
            "00292B6B",
            "00980943",
            "00F3A26A",
            "000CDB9F",
            "0032F2D5",
            "00980F43",
            "00981143",
            "007152E3",
            "006FB16D",
            "00DB58DD",
            "00AA8506",
            "00AB8506",
            "00CB4A34",
            "0028BB78",
            "0036E597",
            "0051F44D",
            "00AE666B",
            "00C1F536",
            "00C3020F",
            "008D5B4B",
            "00D7CD75",
            "002AED01",
            "008F7C09",
            "0040F1E3",
            "00562B80",
            "002C853C",
            "002C893C",
            "00369635",
            "00E3AC54",
            "008CE8B2",
            "008CECB2"
                },
                HiLoDTextures = new(),
                CutsceneTextures = new()
                {
                    "002C0E2D",
            "0028CDB7",
            "00E17BD9",
            "00495322",
            "00A5217E",
            "009D3B34",
            "00ADC198",
            "00B83347",
            "009D72D5",
            "00854CFE",
            "00292B6B",
            "00980943",
            "00F3A26A",
            "000CDB9F",
            "0032F2D5",
            "00980F43",
            "00981143",
            "00DB58DD",
            "00AA8506",
            "00CB4A34",
            "0028BB78",
            "0036E597",
            "0051F44D",
            "00AE666B",
            "00C1F536",
            "00C3020F",
            "008D5B4B",
            "00D7CD75",
            "00ADCB61",
            "009A840D",
            "009A840E",
            "009A840F",
            "009A8410",
            "009A862D",
            "009A862E",
            "009A862F",
            "009A8630",
            "002AED01",
            "008F7C09",
            "0040F1E3",
            "00562B80",
            "002C853C",
            "002C893C",
            "00369635",
            "00E3AC54",
            "008CE8B2",
            "008CECB2"
                },
                CodecTextures = new()
                {
                    "002C0E2D",
            "00E17BD9",
            "00854CFE",
            "00292B6B",
            "00980943",
            "000CDB9F",
            "0032F2D5",
            "00980F43",
            "00981143",
            "00AA8506",
            "00CB4A34",
            "0028BB78",
            "0051F44D",
            "00AE666B",
            "00C1F536",
            "00C3020F",
            "008D5B4B",
            "00D7CD75",
            "00ADCB61",
            "009A840D",
            "009A840E",
            "009A840F",
            "009A8410",
            "009A862D",
            "009A862E",
            "009A862F",
            "009A8630",
            "00369635",
            "00E3AC54"
                },
                ArmTextures = new()
                {
                    "00854CFE",
            "00292B6B",
            "00980943",
            "001B7235",
            "006E46D5",
            "0034DAFA",
            "0034DAFB"
                },
                HandTextures = new()
                {
                    "0028CDB7",
            "00E17BD9",
            "00495322",
            "00A5217E",
            "009D3B34",
            "00ADC198",
            "00B83347",
            "009D72D5",
            "00854CFE",
            "00292B6B",
            "00980943",
            "00F3A26A",
            "000CDB9F",
            "0032F2D5",
            "00980F43",
            "00981143",
            "007152E3",
            "006FB16D",
            "00DB58DD",
            "00AA8506",
            "00AB8506",
            "00CB4A34",
            "0028BB78",
            "0036E597",
            "0051F44D",
            "00AE666B",
            "00C1F536",
            "00C3020F",
            "008D5B4B",
            "00D7CD75",
            "002AED01",
            "008F7C09",
            "0040F1E3",
            "00562B80",
            "002C853C",
            "002C893C",
            "00369635",
            "00E3AC54",
            "008CE8B2",
            "008CECB2"
                }
            }); //Snake: done
            ModelsToSwapIn.Add(new MGSModel //working
            {
                Name = "Solidus",
                LoLoDId = "00851f3a",
                LoLodKms = "assets/kms/us/sol_def_sh_mt.kms,us/stage/w24e/resident/00b43595.kms,resident/00b43595.kms",
                LoLoDTri = "assets/tri/us/sol_def_mh_mt.tri,us/stage/w24e/resident/00851f3a.tri,resident/00851f3a.tri",
                HiLoDId = "00851f3a",
                HiLodKms = "assets/kms/us/sol_def_sh_mt.kms,us/stage/w24e/resident/00b43595.kms,resident/00b43595.kms",
                HiLoDTri = "assets/tri/us/sol_def_mh_mt.tri,us/stage/w24e/resident/00851f3a.tri,resident/00851f3a.tri",
                CutsceneEvm = "assets/evm/us/sol_def_mh_mt.evm,us/stage/w46a/cache/00543595.evm,cache/00543595.evm",
                CutsceneTri = "assets/tri/us/sol_def_mh_mt.tri,us/stage/w46a/cache/00543595.tri,cache/00543595.tri",
                CutsceneId = "00543595",
                UseCutsceneAsCodec = true,
                LoLoDTextures = new()
                {
                    "004A9C1F",
            "0074D41C",
            "0059CF15",
            "009CFEF0",
            "000636DB",
            "00EE094E",
            "000636DC",
            "00A44E09",
            "007B9239",
            "0070D165",
            "0096AFF8",
            "0070D166",
            "0096BFF8",
            "0070D167",
            "0096CFF8",
            "0070D168",
            "0096DFF8",
            "0070D169",
            "0096EFF8",
            "0070D16A",
            "0096FFF8",
            "008B38C1",
            "00F00F44",
            "00CF3472",
            "008B34C1",
            "0060F068",
            "0074D41B",
            "00C21038",
            "00C22038",
            "00CBD2AD",
            "00CCD2AD",
            "00629D97",
            "00B3C66B",
            "004B0E8C",
            "00430225",
            "00A894BF",
            "00A8A4BF",
            "000B2126",
            "000B2127",
            "006E4647",
            "0052578F",
            "0052D78F",
            "0053578F",
            "0053D78F",
            "00625790",
            "0062D790",
            "00635790",
            "0063D790",
            "00068B8F",
            "00394954",
            "00068B90",
            "00395954",
            "00068B91",
            "00396954",
            "00068B92",
            "00397954",
            "00068B93",
            "00EFBE0E",
            "007EB3CC",
            "002C0E76",
            "00A23E39",
            "0063F30F"
                },
                HiLoDTextures = new(),
                CutsceneTextures = new()
                {
                    "004A9C1F",
            "0074D41C",
            "0059CF15",
            "009CFEF0",
            "000636DB",
            "00EE094E",
            "000636DC",
            "00A44E09",
            "007B9239",
            "0070D165",
            "0096AFF8",
            "0070D166",
            "0096BFF8",
            "0070D167",
            "0096CFF8",
            "0070D168",
            "0096DFF8",
            "0070D169",
            "0096EFF8",
            "0070D16A",
            "0096FFF8",
            "008B38C1",
            "00F00F44",
            "00CF3472",
            "008B34C1",
            "0060F068",
            "0074D41B",
            "00C21038",
            "00C22038",
            "00CBD2AD",
            "00CCD2AD",
            "00629D97",
            "00B3C66B",
            "004B0E8C",
            "00430225",
            "00A894BF",
            "00A8A4BF",
            "000B2126",
            "000B2127",
            "006E4647",
            "0052578F",
            "0052D78F",
            "0053578F",
            "0053D78F",
            "00625790",
            "0062D790",
            "00635790",
            "0063D790",
            "00068B8F",
            "00394954",
            "00068B90",
            "00395954",
            "00068B91",
            "00396954",
            "00068B92",
            "00397954",
            "00068B93",
            "00EFBE0E",
            "007EB3CC",
            "002C0E76",
            "00A23E39",
            "0063F30F"
                }
            }); //Solidus: done
            ModelsToSwapIn.Add(new MGSModel //works... kinda, def has issues. prolly bounding issues
            {
                Name = "Tuxedo Snake",
                LoLoDId = "001de6cb",
                LoLodKms = "assets/kms/us/sna_txd_sh_mt.kms,us/stage/r_plt10/resident/00c4cc69.kms,resident/00c4cc69.kms",
                LoLoDTri = "assets/tri/us/txd_oss_otc_mh_mt.tri,us/stage/r_plt10/resident/001de6cb.tri,resident/001de6cb.tri",
                HiLoDId = "001de6cb",
                HiLodKms = "assets/kms/us/sna_txd_sh_mt.kms,us/stage/r_plt10/resident/00c4cc69.kms,resident/00c4cc69.kms",
                HiLoDTri = "assets/tri/us/txd_oss_otc_mh_mt.tri,us/stage/r_plt10/resident/001de6cb.tri,resident/001de6cb.tri",
                LoLoDTextures = new()
                {
                    "0025EB9E",
            "00CBD8D7",
            "00A80EC9",
            "0027EB1E",
            "0000D3B5",
            "00838EFF",
            "00A810C9",
            "0043C780",
            "004B6496",
            "0096EBBC",
            "0043E879",
            "009FC6D4",
            "00B59038",
            "004CD8A8",
            "002EE7F0",
            "007C1B13",
            "00C9BDAA",
            "002C3697",
            "005FCAC1",
            "00A883AA",
            "00D6DF15",
            "00D89A98",
            "00AF440D",
            "002F27F3",
            "00387C1F",
            "00387C20",
            "00387C21",
            "00387C22",
            "00387E3F",
            "00387E40",
            "00387E41",
            "00387E42",
            "000ED1C5",
            "0010072B",
            "00F309EB",
            "00D3DDD3",
            "00BBFBA3",
            "00DB6963",
            "007182F5"
                },
                HiLoDTextures = new()
            }); //Tuxedo Snake: problem-city central.
        }
        #endregion

        #region Models To Swap OUT
        private void BuildModelsToSwapOutList()
        {
            ModelsToSwapOut.Add(new MGSModel
            {
                Name = "Raiden (Story)",
                LoLodKms = "assets/kms/us/rai_def.kms,us/stage/r_plt0/resident/00c13a4e.kms,resident/00c13a4e.kms",
                HiLodKms = "assets/kms/us/rai_def_sh_mt_stage_r_plt0_r.kms,us/stage/r_plt0/resident/00b41e89.kms,resident/00b41e89.kms",
                CutsceneEvm = "assets/evm/us/rai_def.evm,us/stage/d021p01/cache/00541e89.evm,cache/00541e89.evm", //technically this doesnt exist, but using a fake name so we can replace ALL the raiden evms
                CodecEvm = "assets/evm/us/rai_radio_mh_mt.evm,us/face/f04d/cache/00d5b00a.evm,cache/00d5b00a.evm",
                ArmsEvm = "rah_def_mh_mt.evm",
                ShadowKms = "rai_shadow.kms",
                Headpiece = "assets/evm/us/rai_hair_mh_mt_stage_r_plt0_r.evm,us/stage/r_plt0/resident/00543563.evm,resident/00543563.evm",
                ResidentStage = "r_plt0"
            });
            ModelsToSwapOut.Add(new MGSModel
            {
                Name = "Snake (Story)",
                LoLodKms = "assets/kms/us/sna_def.kms,us/stage/r_tnk0/resident/00413aa8.kms,resident/00413aa8.kms", //technically invalid, but using as a spoof so we can replace ALL the snake kms files
                HiLodKms = "assets/kms/us/sna_def_sh_stage_r_plt10_r.kms,us/stage/r_tnk0/resident/0055ab65.kms,resident/0055ab65.kms",
                Headpiece = "sna_bdn1_stage_r_plt_s_r.kms",
                ArmsEvm = "snh_def_mh_mt.evm",
                CutsceneEvm = "assets/evm/us/sna_def_mh.evm,us/stage/d01t/cache/0055aaa5.evm,cache/0055aaa5.evm", //"assets/evm/us/sna_def_mh.evm,us/stage/r_tnk0/resident/00543505.evm,resident/00543505.evm",
                CodecEvm = "assets/evm/us/sna_radio_mh_mt.evm,us/face/f00a/cache/002f300b.evm,cache/002f300b.evm",
                ShadowKms = "sna_shadow_stage_r_plt_s_r.kms",
                ResidentStage = "r_tnk0"
            });
        }

        private MGSModel fatman_raiden = new()
        {
            Name = "Raiden - Story (r_plt3)",
            LoLodKms = "assets/kms/us/rai_def.kms,us/stage/r_plt3/resident/00c13a4e.kms,resident/00c13a4e.kms",
            HiLodKms = "assets/kms/us/rai_def_sh_mt_stage_r_plt0_r.kms,us/stage/r_plt0/resident/00b41e89.kms,resident/00b41e89.kms",
            Headpiece = "assets/evm/us/rai_hair_mh_mt_stage_r_plt0_r.evm,us/stage/r_plt0/resident/00543563.evm,resident/00543563.evm",
            ResidentStage = "r_plt3"
        };
        private MGSModel diver_raiden = new()
        {
            Name = "Raiden - Story (r_plt1)",
            LoLodKms = "assets/kms/us/rai_def_stage_r_plt1_r.kms,us/stage/r_plt1/resident/00c13a4e.kms,resident/00c13a4e.kms",
            HiLodKms = "assets/kms/us/rai_def_sh_mt_stage_r_plt1_r.kms,us/stage/r_plt1/resident/00b41e89.kms,resident/00b41e89.kms",
            CutsceneEvm = "assets/evm/us/rai_diver.evm,us/stage/d005p01/cache/00e79093.evm,cache/00e79093.evm", //technically doesnt exist, but using a fake name so we can replace ALL the diver raiden emvs
            CodecEvm = "assets/evm/us/rai_radio_diver_mh_mt.evm,us/face/f01b/cache/00a84cea.evm,cache/00a84cea.evm",
            Headpiece = "assets/evm/us/rai_hair_mh_mt_stage_r_plt0_r.evm,us/stage/r_plt0/resident/00543563.evm,resident/00543563.evm",
            ResidentStage = "r_plt1"
        };
        private MGSModel naked_raiden = new()
        {
            Name = "Raiden - Story (r_plt2)",
            LoLodKms = "assets/kms/us/rai_naked.kms,us/stage/r_plt2/resident/00c13a4e.kms,resident/00c13a4e.kms",
            HiLodKms = "assets/kms/us/rai_naked_sh_stage_r_plt2_r.kms,us/stage/r_plt2/resident/00064e76.kms,resident/00064e76.kms",
            CutsceneEvm = "assets/evm/us/rai_naked.evm,us/stage/d070px9/cache/00dc8d3a.evm,cache/00dc8d3a.evm", //technically doesnt exist, but using a fake name so we can replace ALL the naked raiden evms
            CodecEvm = "assets/evm/us/rai_radio_naked_mh_mt.evm,us/face/f03b/cache/009d4991.evm,cache/009d4991.evm",
            Headpiece = "assets/evm/us/rai_hair_mh_mt_stage_r_plt0_r.evm,us/stage/r_plt0/resident/00543563.evm,resident/00543563.evm",
            ResidentStage = "r_plt2"
        };
        private MGSModel hostage_room_raiden = new()
        {
            Name = "Raiden - Story (r_plt4)",
            ResidentStage = "r_plt4"
        };
        private MGSModel vr_snake = new()
        {
            Name = "Snake - VR",
            ResidentStage = "r_vr_s"
        };
        private MGSModel snaketales_snake = new()
        {
            Name = "Snake - Snake Tales",
            ResidentStage = "r_vr_sp"
        };
        private MGSModel vr_raiden = new()
        {
            Name = "Raiden - VR",
            ResidentStage = "r_vr_r"
        };
        private MGSModel special_raiden = new()
        {
            Name = "Raiden - Special",
            ResidentStage = "r_vr_rp"
        };
        #endregion

        public ModelSwapperForm(string gameDirectory, string modToolsDirectory)
        {
            InitializeComponent();
            BuildModelsToSwapInList();
            BuildModelsToSwapOutList();
            modelToSwapInComboBox.Items.AddRange(ModelsToSwapIn.ToArray());
            modelToSwapOutComboBox.Items.AddRange(ModelsToSwapOut.ToArray());
            _gameDirectory = gameDirectory;
            _backupDirectory = Path.Combine(modToolsDirectory, "Model Swap Backups", "MGS2 Models");
            if (!Directory.Exists(_backupDirectory))
            {
                Directory.CreateDirectory(_backupDirectory);
                BackupModels();
            }
        }

        private void SwapModelIn(MGSModel modelToSwapOut, MGSModel modelToSwapIn)
        {
            //replacing headpiece does NOT work, it seems. since snake doesnt have one, gonna just leave it
            //*MIGHT* work if i did bounding shit, but it seems like way more pain than i want to deal with, at least for now
            DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));
            DirectoryInfo kmsCmdlDirectory = new(Path.Combine(kmsDirectory.FullName, "_win"));
            DirectoryInfo evmDirectory = new(Path.Combine(_gameDirectory, "assets", "evm", "us"));
            DirectoryInfo evmCmdlDirectory = new(Path.Combine(evmDirectory.FullName, "_win"));
            string loLodFileNameToSwapOut = modelToSwapOut.LoLodKms.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
            string hiLodFileNameToSwapOut = modelToSwapOut.HiLodKms.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
            string evmFileNameToSwapOut = modelToSwapOut.CutsceneEvm.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
            string codecFileNametoSwapOut = modelToSwapOut.CodecEvm.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
            FileInfo[] kmsFiles = kmsDirectory.GetFiles();
            FileInfo[] kmsCmdlFiles = kmsCmdlDirectory.GetFiles();
            FileInfo[] evmFiles = evmDirectory.GetFiles();
            FileInfo[] evmCmdlFiles = evmCmdlDirectory.GetFiles();


            string loLodFileNameToSwapIn = modelToSwapIn.LoLodKms.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
            string hiLodFileNameToSwapIn = modelToSwapIn.HiLodKms.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];

            //string headpieceNameToSwapIn = modelToSwapIn.Headpiece.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];

            DirectoryInfo kmsBackupDirectory = new(Path.Combine(_backupDirectory, "kms"));
            DirectoryInfo kmsCmdlBackupDirectory = new(Path.Combine(kmsBackupDirectory.FullName, "_win"));
            DirectoryInfo evmBackupDirectory = new(Path.Combine(_backupDirectory, "evm"));
            DirectoryInfo evmCmdlBackupDirectory = new(Path.Combine(kmsBackupDirectory.FullName, "_win"));

            foreach(FileInfo playerModel in kmsFiles.Where(x => x.Name.Contains(loLodFileNameToSwapOut)))
            {
                if (playerModel.Name.Contains("r_vr_1") || playerModel.Name.Contains("r_vr_t")) //this is only a problem with snake and this is a lazy solution to a problem created by laziness, but o well
                    continue;
                ReplaceModel(playerModel.Name.Replace(playerModel.Extension, ""), loLodFileNameToSwapIn, true);
            }
            //ReplaceModel(loLodFileNameToSwapOut, loLodFileNameToSwapIn, true);
            //ReplaceModel(hiLodFileNameToSwapOut, hiLodFileNameToSwapIn, true); //Technically not needed at all, as the loLods should handle everything now

            if (!string.IsNullOrEmpty(modelToSwapIn.CutsceneEvm) && cutsceneCheckbox.Checked)
            {
                ReplaceCutsceneEvms(modelToSwapIn, evmFileNameToSwapOut, evmFiles, evmCmdlFiles);
                if (modelToSwapOut.ResidentStage == "r_plt0")
                {
                    ReplaceCutsceneEvms(modelToSwapIn, "rai_gbs_body_mh_mt", evmFiles, evmCmdlFiles);
                    //ReplaceCutsceneEvms(modelToSwapIn, "rai_gbs_body_mh_mt_stage_r_plt4_r", evmFiles, evmCmdlFiles); //above already does this
                }
                else if (modelToSwapOut.ResidentStage == "r_tnk0")
                {
                    ReplaceCutsceneEvms(modelToSwapIn, "sna_def", evmFiles, evmCmdlFiles);
                }
            }
            if ((!string.IsNullOrEmpty(modelToSwapIn.CodecEvm) || modelToSwapIn.UseCutsceneAsCodec) && codecCheckBox.Checked)
            {
                if (modelToSwapIn.UseCutsceneAsCodec)
                {
                    modelToSwapIn.CodecEvm = modelToSwapIn.CutsceneEvm.Replace("/cache/","/resident/");
                    modelToSwapIn.CodecId = modelToSwapIn.CutsceneId;
                    modelToSwapIn.CodecTri = modelToSwapIn.CutsceneTri.Replace("/cache/", "/resident/");
                    modelToSwapIn.CodecTextures = modelToSwapIn.CutsceneTextures;
                }
                ReplaceCodecEvms(modelToSwapIn, codecFileNametoSwapOut, evmFiles, evmCmdlFiles);
                if (modelToSwapOut.ResidentStage == "r_plt0")
                {
                    ReplaceCodecEvms(modelToSwapIn, "rai_radio", evmFiles, evmCmdlFiles);
                    //The above should take care of all of the below
                    //ReplaceCodecEvms(modelToSwapIn, "rai_radio_diver_mh_mt", evmFiles, evmCmdlFiles);
                    //ReplaceCodecEvms(modelToSwapIn, "rai_radio_gbsbody_mh_mt", evmFiles, evmCmdlFiles);
                    //ReplaceCodecEvms(modelToSwapIn, "rai_radio_gbshead_mh", evmFiles, evmCmdlFiles);
                    //ReplaceCodecEvms(modelToSwapIn, "rai_radio_naked_mh_mt", evmFiles, evmCmdlFiles);
                }
            }
            if (!string.IsNullOrEmpty(modelToSwapIn.ShadowKms) && shadowCheckBox.Checked)
            {
                ReplaceShadow(modelToSwapIn, modelToSwapOut);
            }
            if (!string.IsNullOrEmpty(modelToSwapIn.ArmsEvm) && armsCheckbox.Checked)
            {
                ReplaceArms(modelToSwapIn, modelToSwapOut);
                if (modelToSwapOut.ResidentStage == "r_plt0")
                {
                    ReplaceModel("rah_naked_mh_mt", modelToSwapIn.ArmsEvm.Replace(".evm", ""), false);
                    ReplaceModel("rah_naked_mh_mt_stage_r_plt2_r", modelToSwapIn.ArmsEvm.Replace(".evm", ""), false);
                    ReplaceModel("rah_gbs_mh", modelToSwapIn.ArmsEvm.Replace(".evm", ""), false);
                }
            }
            if (modelToSwapOut.ResidentStage == "r_plt0")
            {
                if (!string.IsNullOrEmpty(modelToSwapIn.CutsceneEvm) && cutsceneCheckbox.Checked)
                {
                    string evmFileNameToSwapIn = modelToSwapIn.CutsceneEvm.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
                    ReplaceModel("rai_gbs_mt", loLodFileNameToSwapIn, true);
                    ReplaceModel("rai_gbs_sh_mt", hiLodFileNameToSwapIn, true);
                    ReplaceModel("rai_def_stage_r_plt1_r", loLodFileNameToSwapIn, true);
                    ReplaceModel("rai_def_sh_mt_stage_r_plt1_r", hiLodFileNameToSwapIn, true);
                    ReplaceModel("rai_naked_sh", loLodFileNameToSwapIn, true);
                    ReplaceModel("rai_naked_stage_r_plt2_r", loLodFileNameToSwapIn, true);
                    ReplaceModel("rai_naked_sh_stage_r_plt2_r", hiLodFileNameToSwapIn, true);
                    ReplaceModel("rai_naked_sh", hiLodFileNameToSwapIn, true);

                    ReplaceModel("rai_gbs_addhand_mh_mt", evmFileNameToSwapIn, false);
                    ReplaceModel("rai_gbshead_addhand_mh_mt", evmFileNameToSwapIn, false);
                    ReplaceModel("rai_diver_addhand_mh_mt", evmFileNameToSwapIn, false);
                    ReplaceModel("rai_diver_mh_mt", evmFileNameToSwapIn, false);
                    ReplaceModel("rai_diver_mh_mt_stage_r_plt1_r", evmFileNameToSwapIn, false);
                    ReplaceModel("rai_naked_mh_mt", evmFileNameToSwapIn, false);
                    ReplaceModel("rai_naked_p073_mh_mt", evmFileNameToSwapIn, false);
                    //OKAY NEVERMIND, THIS IS SO FUNNY. no shot i'm replacing the head ahhaha
                }
            }
            if (modelToSwapOut.ResidentStage == "r_tnk0" && cutsceneCheckbox.Checked)
            {
                ReplaceModel("sna_dive_sh_mt", hiLodFileNameToSwapIn, true);
            }
        }

        private void ReplaceArms(MGSModel modelToSwapIn, MGSModel modelToSwapOut)
        {
            ReplaceModel(modelToSwapOut.ArmsEvm.Replace(".evm", ""), modelToSwapIn.ArmsEvm.Replace(".evm", ""), false);
        }

        private void ReplaceShadow(MGSModel modelToSwapIn, MGSModel modelToSwapOut)
        {
            ReplaceModel(modelToSwapOut.ShadowKms.Replace(".kms", ""), modelToSwapIn.ShadowKms.Replace(".kms", ""), true);
        }

        private void ReplaceCodecEvms(MGSModel modelToSwapIn, string codecFileNametoSwapOut, FileInfo[] evmFiles, FileInfo[] evmCmdlFiles)
        {
            //TODO: reduce code duplication with ReplaceCutsceneEvms
            InsertCodecFiles(modelToSwapIn);
            string codecFileNameToSwapIn = modelToSwapIn.CodecEvm.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
            foreach (var evmFile in evmFiles.Where(x => x.Name.Contains(codecFileNametoSwapOut)))
            {
                File.Copy(evmFiles.FirstOrDefault(x => x.Name.Replace(x.Extension, "") == codecFileNameToSwapIn).FullName, evmFile.FullName, true);
                if (!filesModded.Contains(evmFile.FullName))
                {
                    filesModded.Add(evmFile.FullName);
                }
            }
            foreach (var evmCmdlFile in evmCmdlFiles.Where(x => x.Name.Contains(codecFileNametoSwapOut)))
            {
                File.Copy(evmCmdlFiles.FirstOrDefault(x => x.Name.Replace(x.Extension, "") == codecFileNameToSwapIn).FullName, evmCmdlFile.FullName, true);
                if (!filesModded.Contains(evmCmdlFile.FullName))
                {
                    filesModded.Add(evmCmdlFile.FullName);
                }
            }
        }

        private void ReplaceCutsceneEvms(MGSModel modelToSwapIn, string evmFileNameToSwapOut, FileInfo[] evmFiles, FileInfo[] evmCmdlFiles)
        {
            //TODO: reduce code duplication with ReplaceCodecEvms
            InsertCutsceneFiles(modelToSwapIn);
            string evmFileNameToSwapIn = modelToSwapIn.CutsceneEvm.Split('/', StringSplitOptions.RemoveEmptyEntries)[3].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
            foreach (var evmFile in evmFiles.Where(x => x.Name.Contains(evmFileNameToSwapOut)))
            {
                File.Copy(evmFiles.FirstOrDefault(x => x.Name.Replace(x.Extension, "") == evmFileNameToSwapIn).FullName, evmFile.FullName, true);
                if (!filesModded.Contains(evmFile.FullName))
                {
                    filesModded.Add(evmFile.FullName);
                }
            }
            foreach (var evmCmdlFile in evmCmdlFiles.Where(x => x.Name.Contains(evmFileNameToSwapOut)))
            {
                File.Copy(evmCmdlFiles.FirstOrDefault(x => x.Name.Replace(x.Extension, "") == evmFileNameToSwapIn).FullName, evmCmdlFile.FullName, true);
                if (!filesModded.Contains(evmCmdlFile.FullName))
                {
                    filesModded.Add(evmCmdlFile.FullName);
                }
            }
        }

        private void ReplaceModel(string fileToReplace, string newFile, bool isKms = true)
        {
            if (isKms)
            {
                DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));
                DirectoryInfo kmsCmdlDirectory = new(Path.Combine(kmsDirectory.FullName, "_win"));
                DirectoryInfo kmsBackupDirectory = new(Path.Combine(_backupDirectory, "kms"));
                DirectoryInfo kmsCmdlBackupDirectory = new(Path.Combine(kmsBackupDirectory.FullName, "_win"));
                string kmsDestination = Path.Combine(kmsDirectory.FullName, $"{fileToReplace}.kms");
                string cmdlDestination = Path.Combine(kmsCmdlDirectory.FullName, $"{fileToReplace}.cmdl");
                File.Copy(Path.Combine(kmsBackupDirectory.FullName, $"{newFile}.kms"), kmsDestination, true);
                File.Copy(Path.Combine(kmsCmdlBackupDirectory.FullName, $"{newFile}.cmdl"), cmdlDestination, true);

                if (!filesModded.Contains(kmsDestination))
                {
                    filesModded.Add(kmsDestination);
                }
                if (!filesModded.Contains(cmdlDestination))
                {
                    filesModded.Add(cmdlDestination);
                }
            }
            else
            {
                DirectoryInfo evmDirectory = new(Path.Combine(_gameDirectory, "assets", "evm", "us"));
                DirectoryInfo evmCmdlDirectory = new(Path.Combine(evmDirectory.FullName, "_win"));
                DirectoryInfo evmBackupDirectory = new(Path.Combine(_backupDirectory, "evm"));
                DirectoryInfo evmCmdlBackupDirectory = new(Path.Combine(evmBackupDirectory.FullName, "_win"));
                string evmDestination = Path.Combine(evmDirectory.FullName, $"{fileToReplace}.evm");
                string cmdlDestination = Path.Combine(evmCmdlDirectory.FullName, $"{fileToReplace}.cmdl");
                File.Copy(Path.Combine(evmBackupDirectory.FullName, $"{newFile}.evm"), evmDestination, true);
                File.Copy(Path.Combine(evmCmdlBackupDirectory.FullName, $"{newFile}.cmdl"), cmdlDestination, true);

                if (!filesModded.Contains(evmDestination))
                {
                    filesModded.Add(evmDestination);
                }
                if (!filesModded.Contains(cmdlDestination))
                {
                    filesModded.Add(cmdlDestination);
                }
            }
        }

        private void InsertCodecFiles(MGSModel model)
        {

            List<string> codecTextureList = GetFaceTexturesFromListOfIds(model.CodecTextures, model.CodecId, "XXXX");
            InsertTexturesIntoFaceFiles(codecTextureList);
            InsertTriIntoFaceFiles(model.CodecTri);
        }

        private void InsertCutsceneFiles(MGSModel model)
        {
            List<string> cutsceneTextureList = GetCacheTexturesFromListOfIds(model.CutsceneTextures, model.CutsceneId, "XXXX");
            InsertTexturesIntoStageFiles(cutsceneTextureList);
            InsertTriIntoStageFiles(model.CutsceneTri);
        }

        private void NullRaidenMags()
        {
            DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));

            FileInfo[] kmsFiles = kmsDirectory.GetFiles("rai_mag*");

            foreach (FileInfo mag in kmsFiles)
            {
                ReplaceModel(mag.Name.Replace(mag.Extension, ""), "null", true);
            }
        }

        private void UnNullRaidenMags()
        {
            DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));

            FileInfo[] kmsFiles = kmsDirectory.GetFiles("rai_mag*");

            foreach (FileInfo mag in kmsFiles)
            {
                ReplaceModel(mag.Name.Replace(mag.Extension, ""), mag.Name.Replace(mag.Extension, ""), true);
            }
        }

        private void NullSnakeMags()
        {
            DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));

            FileInfo[] kmsFiles = kmsDirectory.GetFiles("sna_mag*");

            foreach (FileInfo mag in kmsFiles)
            {
                ReplaceModel(mag.Name.Replace(mag.Extension, ""), "null", true);
            }
        }

        private void UnNullSnakeMags()
        {
            DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));

            FileInfo[] kmsFiles = kmsDirectory.GetFiles("sna_mag*");

            foreach (FileInfo mag in kmsFiles)
            {
                ReplaceModel(mag.Name.Replace(mag.Extension, ""), mag.Name.Replace(mag.Extension,""), true);
            }
        }

        private void UnNullBandana()
        {
            DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));

            FileInfo[] kmsFiles = kmsDirectory.GetFiles("sna_bdn*");

            foreach(FileInfo bandana in kmsFiles)
            {
                string extensionlessName = bandana.Name.Replace(bandana.Extension, "");
                ReplaceModel(extensionlessName, extensionlessName, true);
            }
        }

        private void NullBandana()
        {
            DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));

            FileInfo[] kmsFiles = kmsDirectory.GetFiles("sna_bdn*");

            foreach (FileInfo bandana in kmsFiles)
            {
                ReplaceModel(bandana.Name.Replace(bandana.Extension, ""), "null", true);
            }
        }

        private void UnNullRaidenHair()
        {
            DirectoryInfo evmDirectory = new(Path.Combine(_gameDirectory, "assets", "evm", "us"));

            FileInfo[] evmFiles = evmDirectory.GetFiles("rai_hair*");
            
            foreach(FileInfo raiHair in evmFiles)
            {
                string extensionlessName = raiHair.Name.Replace(raiHair.Extension, "");
                ReplaceModel(extensionlessName, extensionlessName, false);
            }
        }

        private void UnNullRaidenHeads()
        {
            DirectoryInfo evmDirectory = new(Path.Combine(_gameDirectory, "assets", "evm", "us"));

            string raidenHead1 = "rai_gbs_gbshead_mh_mt";
            string raidenHead2 = "rai_gbs_gbshead_mh_mt_stage_r_plt4_r";
            string raidenHead3 = "rai_gbs_raihead_mh_mt";
            string raidenHead4 = "rai_gbs_raihead_mh_mt_stage_r_plt5_r";

            ReplaceModel(raidenHead1, raidenHead1, false);
            ReplaceModel(raidenHead2, raidenHead2, false);
            ReplaceModel(raidenHead3, raidenHead3, false);
            ReplaceModel(raidenHead4, raidenHead4, false);
        }

        private void NullRaidenHeads()
        {
            DirectoryInfo evmDirectory = new(Path.Combine(_gameDirectory, "assets", "evm", "us"));

            FileInfo[] evmFiles = evmDirectory.GetFiles();

            string raidenHead1 = "rai_gbs_gbshead_mh_mt";
            string raidenHead2 = "rai_gbs_gbshead_mh_mt_stage_r_plt4_r";
            string raidenHead3 = "rai_gbs_raihead_mh_mt";
            string raidenHead4 = "rai_gbs_raihead_mh_mt_stage_r_plt5_r";

            ReplaceModel(raidenHead1, "headnull", false);
            ReplaceModel(raidenHead2, "headnull", false);
            ReplaceModel(raidenHead3, "headnull", false);
            ReplaceModel(raidenHead4, "headnull", false);
        }

        private void NullRaidenHair()
        {
            DirectoryInfo evmDirectory = new(Path.Combine(_gameDirectory, "assets", "evm", "us"));

            FileInfo[] evmFiles = evmDirectory.GetFiles("rai_hair*");

            foreach (FileInfo raiHair in evmFiles)
            {
                ReplaceModel(raiHair.Name.Replace(raiHair.Extension, ""), "null", false);
            }
        }

        private void BackupModels()
        {
            DirectoryInfo backupDirectory = new(_backupDirectory);
            DirectoryInfo kmsDirectory = new(Path.Combine(_gameDirectory, "assets", "kms", "us"));
            DirectoryInfo evmDirectory = new(Path.Combine(_gameDirectory, "assets", "evm", "us"));
            DirectoryInfo stageDirectory = new(Path.Combine(_gameDirectory, "eu", "stage"));
            DirectoryInfo faceDirectory = new(Path.Combine(_gameDirectory, "eu", "face"));

            FileInfo[] kmsFiles = kmsDirectory.GetFiles();
            FileInfo[] kmsCmdlFiles = kmsDirectory.GetDirectories("_win")[0].GetFiles();
            FileInfo[] evmFiles = evmDirectory.GetFiles();
            FileInfo[] evmCmdlFiles = evmDirectory.GetDirectories("_win")[0].GetFiles();
            FileInfo[] stageFiles = stageDirectory.GetFiles("", SearchOption.AllDirectories);
            FileInfo[] faceFiles = faceDirectory.GetFiles("", SearchOption.AllDirectories);

            DirectoryInfo kmsBackupDirectory = backupDirectory.CreateSubdirectory("kms");
            DirectoryInfo kmsCmdlBackupDirectory = kmsBackupDirectory.CreateSubdirectory("_win");
            DirectoryInfo evmBackupDirectory = backupDirectory.CreateSubdirectory("evm");
            DirectoryInfo evmCmdlBackupDirectory = evmBackupDirectory.CreateSubdirectory("_win");
            DirectoryInfo stageBackupDirectory = backupDirectory.CreateSubdirectory("stage");
            DirectoryInfo faceBackupDirectory = backupDirectory.CreateSubdirectory("face");

            File.Copy(Path.Combine("Resources", "headnull.evm"), Path.Combine(evmBackupDirectory.FullName, "headnull.evm"));
            File.Copy(Path.Combine("Resources", "headnull.evm.cmdl"), Path.Combine(evmCmdlBackupDirectory.FullName, "headnull.cmdl"));
            File.Copy(Path.Combine("Resources", "null.evm"), Path.Combine(evmBackupDirectory.FullName, "null.evm"));
            File.Copy(Path.Combine("Resources", "null.evm.cmdl"), Path.Combine(evmCmdlBackupDirectory.FullName, "null.cmdl"));

            foreach (FileInfo kmsFile in kmsFiles)
            {
                File.Copy(kmsFile.FullName, Path.Combine(kmsBackupDirectory.FullName, kmsFile.Name));
            }
            foreach (FileInfo kmsCmdlFile in kmsCmdlFiles)
            {
                File.Copy(kmsCmdlFile.FullName, Path.Combine(kmsCmdlBackupDirectory.FullName, kmsCmdlFile.Name));
            }
            foreach (FileInfo evmFile in evmFiles)
            {
                File.Copy(evmFile.FullName, Path.Combine(evmBackupDirectory.FullName, evmFile.Name));
            }
            foreach (FileInfo evmCmdlFile in evmCmdlFiles)
            {
                File.Copy(evmCmdlFile.FullName, Path.Combine(evmCmdlBackupDirectory.FullName, evmCmdlFile.Name));
            }
            foreach (FileInfo stageFile in stageFiles)
            {
                string specificStageDirectory = Path.Combine(stageBackupDirectory.FullName, stageFile.Directory.Name);
                if (!Directory.Exists(specificStageDirectory))
                {
                    Directory.CreateDirectory(specificStageDirectory);
                }
                File.Copy(stageFile.FullName, Path.Combine(specificStageDirectory, stageFile.Name));
            }
            foreach (FileInfo faceFile in faceFiles)
            {
                if (faceFile.Name == "pages.txt")
                {
                    File.Copy(faceFile.FullName, Path.Combine(faceBackupDirectory.FullName, faceFile.Name));
                    continue;
                }
                string specificFaceDirectory = Path.Combine(faceBackupDirectory.FullName, faceFile.Directory.Name);
                if (!Directory.Exists(specificFaceDirectory))
                {
                    Directory.CreateDirectory(specificFaceDirectory);
                }
                File.Copy(faceFile.FullName, Path.Combine(specificFaceDirectory, faceFile.Name));
            }
        }

        private void LoadTexturesAndTris(MGSModel modelToSwapIn, MGSModel modelToSwapOut)
        {
            List<string> loLodTextureList = GetResidentTexturesFromListOfIds(modelToSwapIn.LoLoDTextures, modelToSwapIn.LoLoDId, modelToSwapOut.ResidentStage);
            List<string> hiLodTextureList = GetResidentTexturesFromListOfIds(modelToSwapIn.HiLoDTextures, modelToSwapIn.HiLoDId, modelToSwapOut.ResidentStage);
            List<string> shadowTextureList = GetResidentTexturesFromListOfIds(modelToSwapIn.ShadowTextures, modelToSwapIn.ShadowId, modelToSwapOut.ResidentStage);
            List<string> armsTextureList = GetResidentTexturesFromListOfIds(modelToSwapIn.ArmTextures, modelToSwapIn.ArmsId, modelToSwapOut.ResidentStage);
            List<string> handsTextureList = GetResidentTexturesFromListOfIds(modelToSwapIn.HandTextures, modelToSwapIn.HandsId, modelToSwapOut.ResidentStage);
            //List<string> headPieceTextureList = GetTexturesFromListOfIds(modelToSwapIn.HeadpieceTextures, modelToSwapIn.HeadpieceId, modelToSwapOut.ResidentStage);
            InsertTexturesIntoResidentFile(loLodTextureList, modelToSwapOut.ResidentStage);
            InsertTexturesIntoResidentFile(hiLodTextureList, modelToSwapOut.ResidentStage);
            InsertTexturesIntoResidentFile(shadowTextureList, modelToSwapOut.ResidentStage);
            InsertTexturesIntoResidentFile(armsTextureList, modelToSwapOut.ResidentStage);
            InsertTexturesIntoResidentFile(handsTextureList, modelToSwapOut.ResidentStage);

            //InsertTexturesIntoResidentFile(headPieceTextureList, modelToSwapOut.ResidentStage);

            InsertTriIntoResidentFile(modelToSwapIn.LoLoDTri, modelToSwapOut.ResidentStage); //Are the tris necessary? Let's find out :) - tested it, and yes they are lmao
            InsertTriIntoResidentFile(modelToSwapIn.HiLoDTri, modelToSwapOut.ResidentStage);
            if (!string.IsNullOrEmpty(modelToSwapIn.ShadowTri))
            {
                InsertTriIntoResidentFile(modelToSwapIn.ShadowTri, modelToSwapOut.ResidentStage);
            }
            if (!string.IsNullOrEmpty(modelToSwapIn.ArmsTri))
            {
                InsertTriIntoResidentFile(modelToSwapIn.ArmsTri, modelToSwapOut.ResidentStage);
            }
            if (!string.IsNullOrEmpty(modelToSwapIn.HandsTri))
            {
                InsertTriIntoResidentFile(modelToSwapIn.HandsTri, modelToSwapOut.ResidentStage);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TODO: Pliskin got green-faced in harrier fight?
            //Considered instituting a forced rebase whenever clicking this button, but that will mean that it is impossible to have
            //multiple models swapped at the same time, which I don't really want to enforce...
            swapInNewModelButton.Text = "Swapping models...";
            Application.DoEvents();
            MGSModel modelToSwapIn = modelToSwapInComboBox.SelectedItem as MGSModel;
            MGSModel modelToSwapOut = modelToSwapOutComboBox.SelectedItem as MGSModel;

            List<Task> swapTasks = new ();
            swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, modelToSwapOut)));
            if (modelToSwapOut.ResidentStage == "r_plt0")
            {
                swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, diver_raiden))); //r_plt1 is used for the start of the plant sequence as diver raiden
                swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, naked_raiden))); //r_plt2 is used at the start of arsenal while raiden is naked
                swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, fatman_raiden))); //r_plt3 is used for the fatman fight for some reason, so we need to replace it as well.
                swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, hostage_room_raiden))); //r_plt4 is used for the hostage room for some reason, so we need to replace it as well.
                swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, vr_raiden))); //r_vr_r is used for vr missions
                swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, special_raiden))); //r_vr_rp is used for... something maybe?
            }
            else if(modelToSwapOut.ResidentStage == "r_tnk0")
            {
                swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, vr_snake))); //r_vr_s is used for vr missions
                swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, snaketales_snake))); //r_vr_sp is used for external gazer snake tale
                //swapTasks.Add(Task.Run(() => LoadTexturesAndTris(modelToSwapIn, mgs1_snake))); //r_vr_1 is used for confidential legacy snake tale and vr(gets broken 
            }

            swapTasks.Add(Task.Run(() => SwapModelIn(modelToSwapOut, modelToSwapIn)));

            if (modelToSwapOut.ResidentStage == "r_plt0")
            {
                if (extrasCheckBox.Checked)
                {
                    swapTasks.Add(Task.Run(UnNullRaidenMags));
                    swapTasks.Add(Task.Run(UnNullRaidenHeads));
                    swapTasks.Add(Task.Run(UnNullRaidenHair));
                }
                else
                {
                    swapTasks.Add(Task.Run(NullRaidenMags));
                    swapTasks.Add(Task.Run(NullRaidenHair));
                    swapTasks.Add(Task.Run(NullRaidenHeads));
                }
            }
            if (modelToSwapOut.ResidentStage == "r_tnk0")
            {
                if (extrasCheckBox.Checked)
                {
                    swapTasks.Add(Task.Run(UnNullBandana));
                    swapTasks.Add(Task.Run(UnNullSnakeMags));
                }
                else
                {
                    if(modelToSwapIn.Name != "MGS1 Snake(new)")
                        swapTasks.Add(Task.Run(NullBandana));
                    swapTasks.Add(Task.Run(NullSnakeMags));
                }
            }
            Task.WaitAll(swapTasks.ToArray());

            if (createModPackCheckBox.Checked)
            {
                string desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                DirectoryInfo modPackDirectory = Directory.CreateDirectory(Path.Combine(desktopDirectory, $"{modelToSwapIn.Name} Over {modelToSwapOut.Name} ModPack"));
                DirectoryInfo euDirectory = Directory.CreateDirectory(Path.Combine(modPackDirectory.FullName, "eu"));
                if (codecCheckBox.Checked)
                {
                    DirectoryInfo faceDirectory = Directory.CreateDirectory(Path.Combine(euDirectory.FullName, "face"));
                    foreach(var file in filesModded.Where(x => x.Contains("\\eu\\face\\")))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        Directory.CreateDirectory(Path.Combine(faceDirectory.FullName, fileInfo.Directory.Name));
                    }
                }
                DirectoryInfo stageDirectory = Directory.CreateDirectory(Path.Combine(euDirectory.FullName, "stage"));
                foreach(var file in filesModded.Where(x => x.Contains("\\eu\\stage\\")))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Directory.CreateDirectory(Path.Combine(stageDirectory.FullName, fileInfo.Directory.Name));
                }
                DirectoryInfo assetsDirectory = Directory.CreateDirectory(Path.Combine(modPackDirectory.FullName, "assets"));
                DirectoryInfo kmsDirectory = Directory.CreateDirectory(Path.Combine(assetsDirectory.FullName, "kms", "us"));
                DirectoryInfo kmsCmdlDirectory = Directory.CreateDirectory(Path.Combine(kmsDirectory.FullName, "_win"));

                if(codecCheckBox.Checked || cutsceneCheckbox.Checked || !extrasCheckBox.Checked || armsCheckbox.Checked)
                {
                    DirectoryInfo evmDirectory = Directory.CreateDirectory(Path.Combine(assetsDirectory.FullName, "evm", "us"));
                    DirectoryInfo evmCmdlDirectory = Directory.CreateDirectory(Path.Combine(evmDirectory.FullName, "_win"));
                }
                foreach (string modifiedFile in filesModded)
                {
                    string modifiedFilePath = modifiedFile.Replace(_gameDirectory, "");
                    string modPackDestination = modPackDirectory.FullName + modifiedFilePath;
                    File.Copy(modifiedFile, modPackDestination, true);
                }
            }
            filesModded.Clear();
            swapInNewModelButton.Text = "Swap In New Model";
            Application.DoEvents();
            string message = "Finished swapping in new model!";
            if (createModPackCheckBox.Checked)
            {
                message += " Modpack is now available on your Desktop.";
            }
            MessageBox.Show(message);
        }

        private void InsertTriIntoStageFiles(string newTriFile)
        {
            DirectoryInfo stageDirectory = new(Path.Combine(_gameDirectory, "eu", "stage"));
            foreach (DirectoryInfo subStageDirectory in stageDirectory.GetDirectories().Where(x => x.Name.StartsWith('d') ||
                                                                                            (x.Name.StartsWith('w') && x.Name.Length == 4)))
            {
                FileInfo manifest = subStageDirectory.GetFiles("manifest.txt").FirstOrDefault();
                string[] manifestContents = File.ReadAllLines(manifest.FullName);
                List<string> triFiles = manifestContents.Where(x => x.Contains(".tri")).ToList();
                List<string> otherAssetsList = manifestContents.Where(x => !x.Contains(".tri")).ToList();
                triFiles.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                otherAssetsList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                if (!triFiles.Contains(newTriFile))
                    triFiles.Add(newTriFile);
                triFiles.Sort();

                string newResidentFileContents = "";
                foreach (string triFile in triFiles)
                {
                    newResidentFileContents += $"{triFile.Trim()}\r\r\n";
                }
                foreach (string otherAsset in otherAssetsList)
                {
                    newResidentFileContents += $"{otherAsset.Trim()}\r\r\n";
                }

                File.WriteAllText(manifest.FullName, newResidentFileContents);

                if (!filesModded.Contains(manifest.FullName))
                {
                    filesModded.Add(manifest.FullName);
                }
            }
        }

        private void InsertTriIntoFaceFiles(string newTriFile)
        {
            DirectoryInfo faceDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "face"));
            foreach (DirectoryInfo subFaceDirectory in faceDirectory.GetDirectories().Where(x => x.Name.StartsWith('f')))
            {
                FileInfo manifest = subFaceDirectory.GetFiles("manifest.txt").FirstOrDefault();
                string[] manifestContents = File.ReadAllLines(manifest.FullName);
                List<string> triFiles = manifestContents.Where(x => x.Contains(".tri")).ToList();
                List<string> otherAssetsList = manifestContents.Where(x => !x.Contains(".tri")).ToList();
                triFiles.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                otherAssetsList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                if (!triFiles.Contains(newTriFile))
                    triFiles.Add(newTriFile);
                triFiles.Sort();

                string newResidentFileContents = "";
                foreach (string triFile in triFiles)
                {
                    newResidentFileContents += $"{triFile.Trim()}\r\r\n";
                }
                foreach (string otherAsset in otherAssetsList)
                {
                    newResidentFileContents += $"{otherAsset.Trim()}\r\r\n";
                }

                File.WriteAllText(manifest.FullName, newResidentFileContents);

                if (!filesModded.Contains(manifest.FullName))
                {
                    filesModded.Add(manifest.FullName);
                }
            }
        }

        private void InsertTriIntoResidentFile(string newTriFile, string resident)
        {
            DirectoryInfo residentDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "stage", resident));
            FileInfo manifest = residentDirectory.GetFiles("manifest.txt").FirstOrDefault();
            string[] manifestContents = File.ReadAllLines(manifest.FullName);
            List<string> triFiles = manifestContents.Where(x => x.Contains(".tri")).ToList();
            List<string> otherAssetsList = manifestContents.Where(x => !x.Contains(".tri")).ToList();
            triFiles.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            otherAssetsList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            if (!triFiles.Contains(newTriFile))
                triFiles.Add(newTriFile);
            triFiles.Sort();

            string newResidentFileContents = "";
            foreach (string triFile in triFiles)
            {
                newResidentFileContents += $"{triFile.Trim()}\r\r\n";
            }
            foreach (string otherAsset in otherAssetsList)
            {
                newResidentFileContents += $"{otherAsset.Trim()}\r\r\n";
            }

            File.WriteAllText(manifest.FullName, newResidentFileContents);

            if (!filesModded.Contains(manifest.FullName))
            {
                filesModded.Add(manifest.FullName);
            }
        }

        private void InsertTexturesIntoStageFiles(List<string> textureList)
        {
            DirectoryInfo stageDirectory = new(Path.Combine(_gameDirectory, "eu", "stage"));
            foreach (DirectoryInfo subStageDirectory in stageDirectory.GetDirectories().Where(x => x.Name.StartsWith('d') ||
                                                                                            (x.Name.StartsWith('w') && x.Name.Length == 4)))
            {
                FileInfo bpAssets = subStageDirectory.GetFiles("bp_assets.txt").FirstOrDefault();

                string[] bpAssetsContents = File.ReadAllLines(bpAssets.FullName);
                List<string> texturesList = bpAssetsContents.Where(x => x.Contains(".ctxr")).ToList();
                List<string> otherAssetsList = bpAssetsContents.Where(x => !x.Contains(".ctxr")).ToList();
                texturesList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                otherAssetsList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                foreach (string texture in textureList)
                {
                    string textureName = texture.Replace("XXXX", subStageDirectory.Name);
                    if (!texturesList.Contains(textureName))
                    {
                        texturesList.Add(textureName);
                    }
                }
                texturesList.Sort();

                string newResidentFileContents = "";
                foreach (string texture in texturesList)
                {
                    newResidentFileContents += $"{texture.Trim()}\r\r\n";
                }
                foreach (string otherAsset in otherAssetsList)
                {
                    newResidentFileContents += $"{otherAsset.Trim()}\r\r\n";
                }

                File.WriteAllText(bpAssets.FullName, newResidentFileContents);

                if (!filesModded.Contains(bpAssets.FullName))
                {
                    filesModded.Add(bpAssets.FullName);
                }
            }
        }

        private void InsertTexturesIntoFaceFiles(List<string> textureList)
        {
            DirectoryInfo faceDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "face"));
            foreach (DirectoryInfo subFaceDirectory in faceDirectory.GetDirectories().Where(x => x.Name.StartsWith('f')))
            {
                FileInfo bpAssets = subFaceDirectory.GetFiles("bp_assets.txt").FirstOrDefault();
                string[] bpAssetsContents = File.ReadAllLines(bpAssets.FullName);
                List<string> texturesList = bpAssetsContents.Where(x => x.Contains(".ctxr")).ToList();
                List<string> otherAssetsList = bpAssetsContents.Where(x => !x.Contains(".ctxr")).ToList();
                texturesList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                otherAssetsList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                foreach (string texture in textureList)
                {
                    string textureName = texture.Replace("XXXX", subFaceDirectory.Name);
                    if (!texturesList.Contains(textureName))
                    {
                        texturesList.Add(textureName);
                    }
                }
                texturesList.Sort();

                string newResidentFileContents = "";
                foreach (string texture in texturesList)
                {
                    newResidentFileContents += $"{texture.Trim()}\r\r\n";
                }
                foreach (string otherAsset in otherAssetsList)
                {
                    newResidentFileContents += $"{otherAsset.Trim()}\r\r\n";
                }

                File.WriteAllText(bpAssets.FullName, newResidentFileContents);

                if (!filesModded.Contains(bpAssets.FullName))
                {
                    filesModded.Add(bpAssets.FullName);
                }
            }
        }

        private void InsertTexturesIntoResidentFile(List<string> textureList, string resident)
        {
            DirectoryInfo residentDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "stage", resident));
            FileInfo bpAssets = residentDirectory.GetFiles("bp_assets.txt").FirstOrDefault();
            string[] bpAssetsContents = File.ReadAllLines(bpAssets.FullName);
            List<string> texturesList = bpAssetsContents.Where(x => x.Contains(".ctxr")).ToList();
            List<string> otherAssetsList = bpAssetsContents.Where(x => !x.Contains(".ctxr")).ToList();
            texturesList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            otherAssetsList.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            foreach (string texture in textureList)
            {
                if (!texturesList.Contains(texture))
                {
                    texturesList.Add(texture);
                }
            }
            texturesList.Sort();

            string newResidentFileContents = "";
            foreach (string texture in texturesList)
            {
                newResidentFileContents += $"{texture.Trim()}\r\r\n";
            }
            foreach (string otherAsset in otherAssetsList)
            {
                newResidentFileContents += $"{otherAsset.Trim()}\r\r\n";
            }

            File.WriteAllText(bpAssets.FullName, newResidentFileContents);

            if (!filesModded.Contains(bpAssets.FullName))
            {
                filesModded.Add(bpAssets.FullName);
            }
        }

        private void modelToSwapOutComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            modelToSwapInComboBox.Enabled = true;
            //TODO: select currently swapped in model in modelToSwapInComboBox? -- probably too complex to implement tbqh
        }

        private void RestoreModelsFromBackup()
        {
            //TODO: could make this more efficient by always replacing the cmdl when replacing the kms
            DirectoryInfo backupKmsDirectory = new DirectoryInfo(Path.Combine(_backupDirectory, "kms"));
            DirectoryInfo backupKmsCmdlDirectory = new DirectoryInfo(Path.Combine(_backupDirectory, "kms", "_win"));
            DirectoryInfo backupEvmDirectory = new DirectoryInfo(Path.Combine(_backupDirectory, "evm"));
            DirectoryInfo backupEvmCmdlDirectory = new DirectoryInfo(Path.Combine(_backupDirectory, "evm", "_win"));
            DirectoryInfo backupStageDirectory = new DirectoryInfo(Path.Combine(_backupDirectory, "stage"));
            DirectoryInfo backupFaceDirectory = new DirectoryInfo(Path.Combine(_backupDirectory, "face"));

            DirectoryInfo gameKmsDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "assets", "kms", "us"));
            DirectoryInfo gameKmsCmdlDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "assets", "kms", "us", "_win"));
            DirectoryInfo gameEvmDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "assets", "evm", "us"));
            DirectoryInfo gameEvmCmdlDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "assets", "evm", "us", "_win"));
            DirectoryInfo gameStageDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "stage"));
            DirectoryInfo gameFaceDirectory = new DirectoryInfo(Path.Combine(_gameDirectory, "eu", "face"));

            Task kmsTask = Task.Run(() =>
            {
                bool fileReplaced = CompareAndReplaceDifferences(backupKmsDirectory, gameKmsDirectory);
                if (fileReplaced)
                    CompareAndReplaceDifferences(backupKmsCmdlDirectory, gameKmsCmdlDirectory);
            });

            Task evmTask = Task.Run(() =>
            {
                bool fileReplaced = CompareAndReplaceDifferences(backupEvmDirectory, gameEvmDirectory);
                if (fileReplaced)
                    CompareAndReplaceDifferences(backupEvmCmdlDirectory, gameEvmCmdlDirectory);
            });

            Task stageTask = Task.Run(() =>
            {
                foreach (DirectoryInfo subDirectory in backupStageDirectory.GetDirectories())
                    CompareAndReplaceDifferences(subDirectory, gameStageDirectory.GetDirectories(subDirectory.Name)[0]);
            });

            Task faceTask = Task.Run(() =>
            {
                foreach (DirectoryInfo subDirectory in backupFaceDirectory.GetDirectories())
                    CompareAndReplaceDifferences(subDirectory, gameFaceDirectory.GetDirectories(subDirectory.Name)[0]);
                CompareTwoFiles(new FileInfo(Path.Combine(backupFaceDirectory.FullName, "pages.txt")), new FileInfo(Path.Combine(gameFaceDirectory.FullName, "pages.txt")));
            });

            Task.WaitAll(kmsTask, evmTask, stageTask, faceTask);
        }

        private void restoreModelsButton_Click(object sender, EventArgs e)
        {            
            restoreModelsButton.Text = "Restoring models...";
            Application.DoEvents();
            RestoreModelsFromBackup();
            restoreModelsButton.Text = "Restore Models From Backup";
            Application.DoEvents();
            MessageBox.Show("Finished restoring files from backup!");
        }

        private object lockObj = new();

        private bool CompareAndReplaceDifferences(DirectoryInfo backupDirectory, DirectoryInfo gameDirectory)
        {
            bool fileReplaced = false;
            
            lock (lockObj)
            {
                foreach (FileInfo backupFile in backupDirectory.GetFiles())
                {
                    FileInfo gameFile = gameDirectory.GetFiles(backupFile.Name, SearchOption.TopDirectoryOnly).FirstOrDefault();
                    if (!CompareTwoFiles(backupFile, gameFile))
                    {
                        //backupFile.Replace(gameFile.FullName, null);
                        File.Copy(backupFile.FullName, gameFile.FullName, true);
                        fileReplaced = true;
                    }
                }
            }

            return fileReplaced;
        }

        private bool CompareTwoFiles(FileInfo file1, FileInfo file2)
        {
            if (file2 != null)
            {
                if (file1.Length != file2.Length)
                    return false;

                using (FileStream file1Stream = File.OpenRead(file1.FullName))
                {
                    using (FileStream file2Stream = File.OpenRead(file2.FullName))
                    {
                        while (file1Stream.Position != file1Stream.Length)
                        {
                            if (file1Stream.ReadByte() != file2Stream.ReadByte())
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        private void UncheckAndDisableCheckboxes()
        {
            cutsceneCheckbox.Checked = false;
            cutsceneCheckbox.Enabled = false;
            codecCheckBox.Checked = false;
            codecCheckBox.Enabled = false;
            armsCheckbox.Checked = false;
            armsCheckbox.Enabled = false;
            shadowCheckBox.Checked = false;
            shadowCheckBox.Enabled = false;
            extrasCheckBox.Checked = false;
            extrasCheckBox.Enabled = false;
        }

        private bool ModelMatch(MGSModel model1, MGSModel model2)
        {
            if(model1.ResidentStage == "r_tnk0")
            {
                return model2.Name.Contains("Snake") || model2.Name.Contains("Pliskin");
            }
            else if(model1.ResidentStage == "r_plt0")
            {
                return model2.Name.Contains("Raiden");
            }

            return false;
        }

        private void modelToSwapInComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MGSModel selectedReplacementModel = modelToSwapInComboBox.SelectedItem as MGSModel;
            UncheckAndDisableCheckboxes();

            if (!string.IsNullOrEmpty(selectedReplacementModel.CutsceneEvm))
            {
                cutsceneCheckbox.Enabled = true;
                cutsceneCheckbox.Checked = true;
                codecCheckBox.Enabled = true;
                codecCheckBox.Checked = true;
            }
            if (!string.IsNullOrEmpty(selectedReplacementModel.ArmsEvm))
            {
                armsCheckbox.Enabled = true;
                if(ModelMatch((modelToSwapOutComboBox.SelectedItem as MGSModel), selectedReplacementModel))
                    armsCheckbox.Checked = true;
            }
            if (!string.IsNullOrEmpty(selectedReplacementModel.ShadowKms))
            {
                shadowCheckBox.Enabled = true;
                shadowCheckBox.Checked = true;
            }
            extrasCheckBox.Enabled = true;
        }
    }
}
