using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public class DownloadManager
    {
        private static readonly HttpClient client = new HttpClient();
        private List<ModRepository> modRepositories;
        private List<Mod> gamebananaMods;

        public DownloadManager()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("ANTIBigBoss_MGS_Mod_Manager/1.0");
            InitializeModRepositories();
            InitializeGamebananaMods();
        }

        private void InitializeModRepositories()
        {
            modRepositories = new List<ModRepository>
            {
                new ModRepository { Owner = "cipherxof", Repo = "MGS3CrouchWalk", Name = "MGS3 Crouch Walk" },
                new ModRepository { Owner = "cipherxof", Repo = "MGSFPSUnlock", Name = "MGS FPS Unlock" },
                new ModRepository { Owner = "Lyall", Repo = "MGSHDFix", Name = "MGS HD Fix" },
                new ModRepository { Owner = "nuggslet", Repo = "MGSM2Fix", Name = "MGS M2 Fix" },
                new ModRepository { Owner = "ANTIBigBoss", Repo = "MGS3-Cheat-Trainer-GUI", Name = "MGS3 Cheat Trainer" },
                new ModRepository { Owner = "sagefantasma", Repo = "MGS2-Cheat-Trainer", Name = "MGS2 Cheat Trainer" },
                new ModRepository { Owner = "ANTIBigBoss", Repo = "MGS1-MC-Cheat-Trainer", Name = "MGS1-MC Cheat Trainer" }
            };
        }

        private void InitializeGamebananaMods()
        {
            gamebananaMods = new List<Mod>
            {
                new Mod { Name = "Dark-Haired Raiden", Author = "goostabo", Link = "https://gamebanana.com/dl/1084055", Description = "For Raiden, a simple brown recolor for Raiden's hair and eyebrows." },
                new Mod { Name = "Silver-Haired Raiden", Author = "goostabo", Link = "https://gamebanana.com/dl/1084055", Description = "For Raiden, a dashing silver recolor for Raiden's hair." },
                new Mod { Name = "The Pride Facepaint Package", Author = "Avaresst", Link = "https://gamebanana.com/dl/1105899", Description = "This package includes several facepaint texture variants (can only have 1 installed at a time)." },
                new Mod { Name = "Menu Screen and Loading Icon", Author = "Avaresst", Link = "https://gamebanana.com/dl/1105891", Description = "This mod replaces the old HD collection menu logo with a new Master Collection one that I've made. Also replaces a loading icon leftover from the HD collection." },
                new Mod { Name = "Save and Codec Menu Sprite Change", Author = "Avaresst", Link = "https://gamebanana.com/dl/1105913", Description = "This pack includes 15 sprites you can replace the fox logo from the save menu of both mgs3 and the msx games with and the codec / general menus from mgs3." },
                new Mod { Name = "Crafted", Author = "Avaresst", Link = "https://gamebanana.com/dl/1148464", Description = "Turns most of the games textures into Minecraft theme." },
                new Mod { Name = "Bloodmoon", Author = "Avaresst", Link = "https://gamebanana.com/dl/1311845", Description = "Thematically changes the game for the Halloween spirit with texture and animation changes." },
                new Mod { Name = "Peace Walker Sneaking Suit", Author = "Avaresst", Link = "https://gamebanana.com/dl/1115509", Description = "This mod replaces the mgs3 sneaking suit with the one from Peace Walker / Ground Zeroes." },
                new Mod { Name = "Metal Gear Jolly", Author = "Avaresst", Link = "https://gamebanana.com/dl/1109719", Description = "This mod makes most of the environment snowy and festive, replaces a bunch of enemies, allies, codec sprites, items here and there all to match a Christmas theme." },
                new Mod { Name = "Portable Ops USSR Soldiers", Author = "Avaresst", Link = "https://gamebanana.com/dl/1105980", Description = "This mod replaces the mgs3 Gru Soldiers with the yellow suit USSR Soldiers from portable ops." },
                new Mod { Name = "Portable Ops Sneaking Suit", Author = "Avaresst", Link = "https://gamebanana.com/dl/1105976", Description = "This mod replaces the mgs3 sneaking suit with the one from portable ops." },
                new Mod { Name = "More MGSV Accurate Parasites", Author = "Avaresst", Link = "https://gamebanana.com/dl/1105906", Description = "This mod replaces a few textures of the parasitic cobra members to make the Pain, the Fear and the End more accurate / fitting to MGSV's lore and parasites." },
                new Mod { Name = "Menu Screen and Loading Icon", Author = "Avaresst", Link = "https://gamebanana.com/dl/1105886", Description = "This mod replaces the old HD collection menu logo with a new Master Collection one that I've made. Also replaces a loading icon leftover from the HD collection." },
                new Mod { Name = "The Pride Bandana Package", Author = "Avaresst", Link = "https://gamebanana.com/dl/1105897", Description = "This package includes several bandanna texture variants (can only have 1 installed at a time)." },
                new Mod { Name = "Grey Fabric Bandana for Naked Snake", Author = "goostabo", Link = "https://gamebanana.com/dl/1083464", Description = "For Naked Snake, a simple grey fabric bandana neatly tucked in." }
            };
        }

        public List<ModRepository> GetModRepositories()
        {
            return modRepositories;
        }

        public List<Mod> GetGamebananaMods()
        {
            return gamebananaMods;
        }

        public async Task<string> GetModInfo(ModRepository modRepo)
        {
            try
            {
                string apiUrl = $"https://api.github.com/repos/{modRepo.Owner}/{modRepo.Repo}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject repoInfo = JObject.Parse(responseBody);

                string modName = modRepo.Name ?? repoInfo["name"]?.ToString();
                string modAuthor = repoInfo["owner"]?["login"]?.ToString();
                string modDescription = repoInfo["description"]?.ToString();

                return $"Name: {modName}\nAuthor: {modAuthor}\nDescription: {modDescription}";
            }
            catch (Exception ex)
            {
                return $"Error fetching mod info: {ex.Message}";
            }
        }

        public async Task<string> GetLatestReleaseDownloadUrl(ModRepository modRepo)
        {
            try
            {
                string apiUrl = $"https://api.github.com/repos/{modRepo.Owner}/{modRepo.Repo}/releases/latest";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Check for 404 Not Found
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("No releases found for this repository.");
                }

                // Throw an exception if the status is not successful
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject releaseInfo = JObject.Parse(responseBody);

                // Get the first asset in the release
                JArray assets = (JArray)releaseInfo["assets"];
                if (assets != null && assets.Count > 0)
                {
                    string downloadUrl = assets[0]["browser_download_url"]?.ToString();
                    return downloadUrl;
                }
                else
                {
                    throw new Exception("No assets found in the latest release.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting download URL: {ex.Message}");
            }
        }

        public async Task DownloadModFile(string downloadUrl, string destinationPath)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(downloadUrl);
                response.EnsureSuccessStatusCode();

                byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                System.IO.File.WriteAllBytes(destinationPath, fileBytes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error downloading mod file: {ex.Message}");
            }
        }

        public async Task EnsureModToolsDownloaded(string modToolsPath)
        {
            if (!Directory.Exists(modToolsPath))
            {
                Directory.CreateDirectory(modToolsPath);
                LoggingManager.Instance.Log($"Created mod tools folder: {modToolsPath}");
            }

            string ctxrToolPath = Path.Combine(modToolsPath, "CtxrTool.exe");
            string texconvPath = Path.Combine(modToolsPath, "texconv.exe");

            if (!File.Exists(ctxrToolPath))
            {
                LoggingManager.Instance.Log("CtxrTool.exe not found. Downloading...");
                await DownloadModFile("https://github.com/Jayveer/CtxrTool/releases/download/1.3/CtxrTool.exe", ctxrToolPath);
            }
            else
            {
                LoggingManager.Instance.Log("CtxrTool.exe already exists.");
            }

            if (!File.Exists(texconvPath))
            {
                LoggingManager.Instance.Log("texconv.exe not found. Downloading...");
                await DownloadModFile("https://github.com/Microsoft/DirectXTex/releases/latest/download/texconv.exe", texconvPath);
            }
            else
            {
                LoggingManager.Instance.Log("texconv.exe already exists.");
            }

            string assetsFolder = Path.Combine(modToolsPath, "3D Models and Textures");
            if (!Directory.Exists(assetsFolder))
            {
                LoggingManager.Instance.Log("Assets folder not found. Downloading assets zip...");
                string assetsZipUrl = "https://github.com/ANTIBigBoss/MGS-MC-Mod-Manager-and-Tool/releases/download/test/3D.Models.and.Textures.zip";
                string tempZipPath = Path.Combine(Path.GetTempPath(), "3DModelsAndTextures.zip");
                await DownloadModFile(assetsZipUrl, tempZipPath);
                LoggingManager.Instance.Log("Extracting assets zip...");
                ZipFile.ExtractToDirectory(tempZipPath, modToolsPath);

                string extractedFolder = Path.Combine(modToolsPath, "3D.Models.and.Textures");
                if (Directory.Exists(extractedFolder))
                {
                    Directory.Move(extractedFolder, assetsFolder);
                    LoggingManager.Instance.Log($"Renamed extracted folder to: {assetsFolder}");
                }
                File.Delete(tempZipPath);
                LoggingManager.Instance.Log("Deleted temporary assets zip.");
            }
            else
            {
                LoggingManager.Instance.Log("Assets folder already exists.");
            }
        }

    }
}

/*
 public async Task EnsureModToolsDownloaded(string modToolsPath)
        {
            if (!Directory.Exists(modToolsPath))
            {
                Directory.CreateDirectory(modToolsPath);
            }
            string ctxrToolPath = Path.Combine(modToolsPath, "CtxrTool.exe");
            string texconvPath = Path.Combine(modToolsPath, "texconv.exe");

            if (!File.Exists(ctxrToolPath))
            {
                await DownloadModFile("https://github.com/Jayveer/CtxrTool/releases/download/1.3/CtxrTool.exe", ctxrToolPath);
            }
            if (!File.Exists(texconvPath))
            {
                await DownloadModFile("https://github.com/Microsoft/DirectXTex/releases/latest/download/texconv.exe", texconvPath);
            }
        }
*/