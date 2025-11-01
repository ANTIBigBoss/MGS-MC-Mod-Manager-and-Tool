using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms;
using IOSearchOption = System.IO.SearchOption;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public class FileExplorerManager
    {
        public ConfigSettings config;
        public Form parentForm;
        public FlowLayoutPanel modListPanel;
        public string gameKey;
        public readonly string[] expectedPaths;
        private string backupRoot;
        private string modFolder;

        public string BackupRoot
        {
            get { return backupRoot; }
        }

        public string ModFolder
        {
            get { return modFolder; }
        }

        public FileExplorerManager(ConfigSettings config, Form parentForm, FlowLayoutPanel modListPanel, string gameKey, string[] expectedPaths)
        {
            this.config = config;
            this.parentForm = parentForm;
            this.modListPanel = modListPanel;
            this.gameKey = gameKey;
            this.expectedPaths = expectedPaths;

            if (gameKey.Equals("MG1andMG2", StringComparison.OrdinalIgnoreCase))
            {
                backupRoot = config.MG1andMG2VanillaFolderPath;
                modFolder = config.MG1andMG2ModFolderPath;
            }           
            else if (gameKey.Equals("MGS1", StringComparison.OrdinalIgnoreCase))
            {
                backupRoot = config.MGS1VanillaFolderPath;
                modFolder = config.MGS1ModFolderPath;
            }
            else if (gameKey.Equals("MGS2", StringComparison.OrdinalIgnoreCase))
            {
                backupRoot = config.MGS2VanillaFolderPath;
                modFolder = config.MGS2ModFolderPath;
            }
            else if (gameKey.Equals("MGS3", StringComparison.OrdinalIgnoreCase))
            {
                backupRoot = config.MGS3VanillaFolderPath;
                modFolder = config.MGS3ModFolderPath;
            }
            else
            {
                throw new ArgumentException("Unsupported game key provided.");
            }
        }

        public void SetupBackupFolders()
        {
            if (!Directory.Exists(backupRoot))
                Directory.CreateDirectory(backupRoot);
        }

        public void SetupModFolder()
        {
            if (!Directory.Exists(modFolder))
            {
                Directory.CreateDirectory(modFolder);
                config.Mods.ModFolderCreated = true;
                ConfigManager.SaveSettings(config);
            }
        }

        private bool CheckForResourceFileInMod(string modPath)
        {
            DirectoryInfo modDirectory = new DirectoryInfo(modPath);

            FileInfo[] bpAssetsFiles = modDirectory.GetFiles("bp_assets.txt", IOSearchOption.AllDirectories);
            FileInfo[] manifestFiles = modDirectory.GetFiles("manifest.txt", IOSearchOption.AllDirectories);

            return bpAssetsFiles.Length > 0 || manifestFiles.Length > 0;
        }

        private void UninstallModTouchingResourceFiles(string modPath, string gameInstallPath)
        {
            DirectoryInfo backupDirectory = new DirectoryInfo(backupRoot);

            List<string> linesAddedByOtherMods = new();

            //Need to check ALL installed mods, see if any touch resource files, and if they do, what resources they use.
            foreach (string mod in config.Mods.ActiveMods.Keys.Where(x => config.Mods.ActiveMods[x] == true)) //only care about currently installed mods
            {
                DirectoryInfo otherActiveModDirectory = new DirectoryInfo(Path.Combine(modFolder, mod));
                if (!otherActiveModDirectory.Exists || otherActiveModDirectory.FullName == modPath)
                    continue; //This means this mod isn't for the right game

                FileInfo[] bpAssetsOtherModFiles = otherActiveModDirectory.GetFiles("bp_assets.txt", IOSearchOption.AllDirectories);
                FileInfo[] manifestOtherModFiles = otherActiveModDirectory.GetFiles("manifest.txt", IOSearchOption.AllDirectories);
                foreach (FileInfo bpAssetsFile in bpAssetsOtherModFiles) 
                {
                    string trimmedPath = bpAssetsFile.FullName.Replace(otherActiveModDirectory.FullName, "");
                    FileInfo bpAssetsVanillaFile = new(backupDirectory.FullName + trimmedPath);
                    linesAddedByOtherMods.AddRange(DiffFiles(bpAssetsFile, bpAssetsVanillaFile));
                }
                foreach(FileInfo manifestFile in manifestOtherModFiles)
                {
                    string trimmedPath = manifestFile.FullName.Replace(otherActiveModDirectory.FullName, "");
                    FileInfo manifestVanillaFile = new(backupDirectory.FullName + trimmedPath);
                    linesAddedByOtherMods.AddRange(DiffFiles(manifestFile, manifestVanillaFile));
                }
            }

            DirectoryInfo modDirectory = new DirectoryInfo(modPath);
            Dictionary<FileInfo, string> resourceFiles = new();
            FileInfo[] bpAssetsModFiles = modDirectory.GetFiles("bp_assets.txt", IOSearchOption.AllDirectories);
            FileInfo[] manifestModFiles = modDirectory.GetFiles("manifest.txt", IOSearchOption.AllDirectories);

            //Then, remove any that are not ALREADY in vanilla and are not required by another, different mod.
            foreach (FileInfo bpAssetsModFile in bpAssetsModFiles)
            {
                resourceFiles.Add(bpAssetsModFile, bpAssetsModFile.FullName);
                string trimmedPath = bpAssetsModFile.FullName.Replace(modDirectory.FullName, "");
                FileInfo bpAssetsVanillaFile = new(backupDirectory.FullName + trimmedPath);
                List<string> linesAddedByThisMod = DiffFiles(bpAssetsModFile, bpAssetsVanillaFile);
                ResourceFileEditorSupport.BpAssetsFile gameBpAssetsFile = new(gameInstallPath + trimmedPath); 
                foreach(string lineAdded in linesAddedByThisMod.Where(x=> linesAddedByOtherMods.Contains(x) == false))
                {
                    if (lineAdded.Contains(".ctxr"))
                        gameBpAssetsFile.CtxrResources.Remove(gameBpAssetsFile.CtxrResources.First(x => x.Path == lineAdded));
                    else
                        gameBpAssetsFile.CmdlResources.Remove(gameBpAssetsFile.CmdlResources.First(x => x.Path == lineAdded));
                }
                gameBpAssetsFile.WriteToFile();
            }
            foreach (FileInfo manifestModFile in manifestModFiles)
            {
                resourceFiles.Add(manifestModFile, manifestModFile.FullName);
                string trimmedPath = manifestModFile.FullName.Replace(modDirectory.FullName, "");
                FileInfo manifestVanillaFile = new(backupDirectory.FullName + trimmedPath);
                List<string> linesAddedByThisMod = DiffFiles(manifestModFile, manifestVanillaFile);
                ResourceFileEditorSupport.ManifestFile gameManifestFile = new(gameInstallPath + trimmedPath);
                foreach (string lineAdded in linesAddedByThisMod.Where(x => linesAddedByOtherMods.Contains(x) == false))
                {
                    if (lineAdded.Contains(".tri"))
                        gameManifestFile.TriResources.Remove(gameManifestFile.TriResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".hzx"))
                        gameManifestFile.HzxResources.Remove(gameManifestFile.HzxResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".var"))
                        gameManifestFile.VarResources.Remove(gameManifestFile.VarResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".sar"))
                        gameManifestFile.SarResources.Remove(gameManifestFile.SarResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".row"))
                        gameManifestFile.RowResources.Remove(gameManifestFile.RowResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".o2d"))
                        gameManifestFile.O2dResources.Remove(gameManifestFile.O2dResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".mar"))
                        gameManifestFile.MarResources.Remove(gameManifestFile.MarResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".lt2"))
                        gameManifestFile.Lt2Resources.Remove(gameManifestFile.Lt2Resources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".kms"))
                        gameManifestFile.KmsResources.Remove(gameManifestFile.KmsResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".far"))
                        gameManifestFile.FarResources.Remove(gameManifestFile.FarResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".evm"))
                        gameManifestFile.EvmResources.Remove(gameManifestFile.EvmResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".cv2"))
                        gameManifestFile.Cv2Resources.Remove(gameManifestFile.Cv2Resources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".anm"))
                        gameManifestFile.AnmResources.Remove(gameManifestFile.AnmResources.First(x => x.Path == lineAdded));
                    else if (lineAdded.Contains(".gcx"))
                        gameManifestFile.GcxResources.Remove(gameManifestFile.GcxResources.First(x => x.Path == lineAdded));
                }
                gameManifestFile.WriteToFile();
            }

            //Take resource files out of the mod directory temporarily
            DirectoryInfo tempDirectory = Directory.CreateTempSubdirectory();
            foreach (FileInfo resourceFile in resourceFiles.Keys)
            {
                resourceFile.MoveTo(Path.Combine(tempDirectory.FullName, resourceFile.Directory.Name + resourceFile.Name));
            }

            //Uninstall all of the other remaining files normally
            RestoreVanillaFiles(modPath, gameInstallPath);

            //Move the resource files back into the mod directory for installation purposes later
            foreach (FileInfo resourceFile in resourceFiles.Keys)
            {
                resourceFile.MoveTo(resourceFiles[resourceFile], true);
            }

            //Be better than every other developer that uses Temp directories and actually clean up after myself.
            tempDirectory.Delete(true);
        }

        private List<string> DiffFiles(FileInfo file1, FileInfo file2)
        {
            List<string> fileDiff = new ();

            string[] file1Lines = File.ReadAllLines(file1.FullName).Distinct().Where(x => x != "").ToArray(); ; //filter out blanks
            string[] file2Lines = File.ReadAllLines(file2.FullName).Distinct().Where(x => x != "").ToArray(); ; //filter out blanks

            foreach (string line in file1Lines)
            {
                if (!file2Lines.Contains(line))
                {
                    fileDiff.Add(line);
                }
            }

            //I don't think we care about removed lines at all?
            return fileDiff;
        }

        private void InstallModTouchingResourceFiles(string modPath, string gameInstallPath, string selectedVariant)
        {
            DirectoryInfo backupDirectory = new DirectoryInfo(backupRoot);
            DirectoryInfo modDirectory = new DirectoryInfo(modPath);

            FileInfo[] bpAssetsModFiles = modDirectory.GetFiles("bp_assets.txt", IOSearchOption.AllDirectories); 
            FileInfo[] manifestModFiles = modDirectory.GetFiles("manifest.txt", IOSearchOption.AllDirectories);
            //FileInfo[] allOtherModFiles = modDirectory.GetFiles("*", IOSearchOption.AllDirectories).Where(x => (bpAssetsModFiles.Contains(x) == false) && (manifestModFiles.Contains(x) == false)).ToArray();

            Dictionary<FileInfo, string> resourceFiles = new();

            foreach(FileInfo bpAssetsModFile in bpAssetsModFiles)
            {
                resourceFiles.Add(bpAssetsModFile, bpAssetsModFile.FullName);
                string trimmedPath = bpAssetsModFile.FullName.Replace(modDirectory.FullName, "");
                FileInfo bpAssetsVanillaFile = new FileInfo(backupDirectory.FullName + trimmedPath);
                List<string> linesToAdd = DiffFiles(bpAssetsModFile, bpAssetsVanillaFile);
                ResourceFileEditorSupport.BpAssetsFile gameBpAssetsFile = new ResourceFileEditorSupport.BpAssetsFile(gameInstallPath + trimmedPath);
                foreach (string line in linesToAdd)
                {
                    if (line.Contains(".ctxr"))
                        gameBpAssetsFile.CtxrResources.Add(new ResourceFileEditorSupport.Ctxr(line));
                    else
                        gameBpAssetsFile.CmdlResources.Add(new ResourceFileEditorSupport.Cmdl(line));
                }
                gameBpAssetsFile.WriteToFile();
            }
            foreach (FileInfo manifestModFile in manifestModFiles)
            {
                resourceFiles.Add(manifestModFile, manifestModFile.FullName);
                string trimmedPath = manifestModFile.FullName.Replace(modDirectory.FullName, "");
                FileInfo manifestVanillaFile = new FileInfo(backupDirectory.FullName + trimmedPath);
                List<string> linesToAdd = DiffFiles(manifestModFile, manifestVanillaFile);
                ResourceFileEditorSupport.ManifestFile gameManifestFile = new ResourceFileEditorSupport.ManifestFile(gameInstallPath + trimmedPath);
                foreach (string line in linesToAdd)
                {
                    if (line.Contains(".tri"))
                        gameManifestFile.TriResources.Add(new(line));
                    else if(line.Contains(".hzx"))
                        gameManifestFile.HzxResources.Add(new(line));
                    else if (line.Contains(".var"))
                        gameManifestFile.VarResources.Add(new(line));
                    else if (line.Contains(".sar"))
                        gameManifestFile.SarResources.Add(new(line));
                    else if (line.Contains(".row"))
                        gameManifestFile.RowResources.Add(new(line));
                    else if (line.Contains(".o2d"))
                        gameManifestFile.O2dResources.Add(new(line));
                    else if (line.Contains(".mar"))
                        gameManifestFile.MarResources.Add(new(line));
                    else if (line.Contains(".lt2"))
                        gameManifestFile.Lt2Resources.Add(new(line));
                    else if (line.Contains(".kms"))
                        gameManifestFile.KmsResources.Add(new(line));
                    else if (line.Contains(".far"))
                        gameManifestFile.FarResources.Add(new(line));
                    else if (line.Contains(".evm"))
                        gameManifestFile.EvmResources.Add(new(line));
                    else if (line.Contains(".cv2"))
                        gameManifestFile.Cv2Resources.Add(new(line));
                    else if (line.Contains(".anm"))
                        gameManifestFile.AnmResources.Add(new(line));
                    else if (line.Contains(".gcx"))
                        gameManifestFile.GcxResources.Add(new(line));
                }
                gameManifestFile.WriteToFile();
            }

            //Take resource files out of the mod directory temporarily
            DirectoryInfo tempDirectory = Directory.CreateTempSubdirectory();
            foreach (FileInfo resourceFile in resourceFiles.Keys)
            {
                resourceFile.MoveTo(Path.Combine(tempDirectory.FullName, resourceFile.Directory.Name + resourceFile.Name));
            }

            //Apply all of the other remaining files normally
            ApplyModFiles(modPath, gameInstallPath, selectedVariant);

            //Move the resource files back into the mod directory for uninstallation purposes later
            foreach(FileInfo resourceFile in resourceFiles.Keys)
            {
                resourceFile.MoveTo(resourceFiles[resourceFile], true);
            }

            //Be better than every other developer that uses Temp directories and actually clean up after myself.
            tempDirectory.Delete(true);
        }

        public async Task<bool> ToggleModStateByNameAsync(string modName, string gameInstallPath)
        {
            bool isEnabled = config.Mods.ActiveMods.TryGetValue(modName, out var enabled) && enabled;
            string modPath = Path.Combine(modFolder, modName);

            try
            {
                await Task.Run(() =>
                {
                    bool modifiesAnyResourceFile = CheckForResourceFileInMod(modPath);
                    if (isEnabled)
                    {
                        if (modifiesAnyResourceFile)
                        {
                            UninstallModTouchingResourceFiles(modPath, gameInstallPath);
                        }
                        else
                        {
                            RestoreVanillaFiles(modPath, gameInstallPath);
                        }
                        config.Mods.ActiveMods[modName] = false;
                        config.Mods.ActiveVariants.Remove(modName);
                    }
                    else
                    {
                        string selectedVariant = config.Mods.ActiveVariants.TryGetValue(modName, out var variant) ? variant : null;
                        if (modifiesAnyResourceFile)
                        {
                            InstallModTouchingResourceFiles(modPath, gameInstallPath, selectedVariant);
                        }
                        else
                        {
                            ApplyModFiles(modPath, gameInstallPath, selectedVariant);
                        }
                        config.Mods.ActiveMods[modName] = true;
                    }
                });

                ConfigManager.SaveSettings(config);
                return true;
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.Log($"Error toggling mod state: {ex.Message}");
                return false;
            }
        }

        public List<string> FindVariantFolders(string modRoot)
        {
            var variants = new List<string>();

            foreach (var dir in Directory.GetDirectories(modRoot))
            {
                string dirName = Path.GetFileName(dir);

                if (dirName.Equals("Mod Details", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("assets", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("EngineSupport", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("fr", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("gr", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("hqmovie", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("mgs3_savedata_win", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("Misc", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("MonoBleedingEdge", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("sp", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("us", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("End", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("Fear", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("Pain", StringComparison.OrdinalIgnoreCase) ||
                    dirName.Equals("Save Menus (MSX + MGS3)", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                variants.Add(dirName);
            }

            return variants.Count > 1 ? variants : new List<string>();
        }

        public void ReplaceOrAppendModInfoLine(string modInfoPath, string newLine)
        {
            string directory = Path.GetDirectoryName(modInfoPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(modInfoPath))
            {
                File.WriteAllText(modInfoPath, string.Empty);
            }

            var lines = File.ReadAllLines(modInfoPath).ToList();
            bool replaced = false;
            string prefix = "This mod is currently replacing the ";

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(prefix))
                {
                    lines[i] = newLine;
                    replaced = true;
                    break;
                }
            }

            if (!replaced)
            {
                lines.Add(newLine);
            }
            File.WriteAllLines(modInfoPath, lines);
        }      

        public void DeleteMod(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            string modName = button.Tag.ToString();
            string modPath = Path.Combine(modFolder, modName);

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete the mod '{modName}'?\nIt will be moved to the Recycle Bin.",
                "Delete Mod", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool isEnabled = config.Mods.ActiveMods.ContainsKey(modName) && config.Mods.ActiveMods[modName];
                    if (isEnabled)
                    {
                        RestoreVanillaFiles(modPath, config.GamePaths[gameKey]);
                        config.Mods.ActiveMods[modName] = false;
                    }

                    config.Mods.ActiveMods.Remove(modName);
                    if (config.Mods.ModMappings.ContainsKey(modName))
                        config.Mods.ModMappings.Remove(modName);
                    if (config.Mods.ReplacedFiles.ContainsKey(modName))
                        config.Mods.ReplacedFiles.Remove(modName);

                    ConfigManager.SaveSettings(config);
                    FileSystem.DeleteDirectory(modPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting mod '{modName}':\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void ProcessModFolder(string modPath)
        {
            string modName = new DirectoryInfo(modPath).Name;
            string destinationPath = Path.Combine(modFolder, modName);

            // Normalize and append separator for exact prefix checks
            string sourceFull = Path.GetFullPath(modPath)
                                    .TrimEnd(Path.DirectorySeparatorChar)
                                + Path.DirectorySeparatorChar;
            string destFull = Path.GetFullPath(destinationPath)
                                    .TrimEnd(Path.DirectorySeparatorChar)
                                + Path.DirectorySeparatorChar;

            if (destFull.StartsWith(sourceFull, StringComparison.OrdinalIgnoreCase) ||
                sourceFull.StartsWith(destFull, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Cannot add this folder here: it would create an infinite loop of folders.\n\nSelect a folder that isn't MGS2 Mods or MGS3 Mods.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                return;
            }

            if (Directory.Exists(destinationPath))
            {
                MessageBox.Show(
                    $"The mod '{modName}' is already in the list.",
                    "Mod Already Added", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
                return;
            }

            DirectoryCopy(modPath, destinationPath, copySubDirs: true);
            config.Mods.ActiveMods[modName] = false;
        }


        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            var dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
                throw new DirectoryNotFoundException(
                    $"Source directory not found: {sourceDirName}"
                );

            Directory.CreateDirectory(destDirName);

            foreach (var file in dir.GetFiles())
            {
                string targetFile = Path.Combine(destDirName, file.Name);
                file.CopyTo(targetFile, overwrite: false);
            }

            if (!copySubDirs) return;

            string sourceRoot = Path.GetFullPath(sourceDirName)
                                      .TrimEnd(Path.DirectorySeparatorChar)
                                  + Path.DirectorySeparatorChar;
            string destRoot = Path.GetFullPath(destDirName)
                                      .TrimEnd(Path.DirectorySeparatorChar)
                                  + Path.DirectorySeparatorChar;

            foreach (var subdir in dir.GetDirectories())
            {
                string nextSource = subdir.FullName;
                string nextDest = Path.Combine(destDirName, subdir.Name);

                if (nextDest
                    .StartsWith(sourceRoot, StringComparison.OrdinalIgnoreCase) ||
                    nextSource
                    .StartsWith(destRoot, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                DirectoryCopy(nextSource, nextDest, copySubDirs);
            }
        }

        private void HandleResourceFileModification()
        {
            //
        }


        public string FindMGSHDFixRoot(string modPath)
        {
            var iniFiles = Directory.GetFiles(modPath, "MGSHDFix.ini", IOSearchOption.AllDirectories);
            return (iniFiles.Length > 0) ? Path.GetDirectoryName(iniFiles[0]) : modPath;
        }

        public bool IsMGSHDFixMod(string modPath)
        {
            string[] requiredFiles =
            {
                "d3d11.dll",
                "MGSHDFix.asi",
                "MGSHDFix.ini",
                "README.md",
                "UltimateASILoader_LICENSE.md"
            };
            var filesInRoot = Directory.GetFiles(modPath, "*", IOSearchOption.AllDirectories)
                .Select(f => Path.GetFileName(f))
                .ToList();
            return requiredFiles.All(reqFile =>
                filesInRoot.Any(f => string.Equals(f, reqFile, StringComparison.OrdinalIgnoreCase)));
        }

        public void ApplyModFiles(string modPath, string gameInstallPath, string selectedVariant = null)
        {
            string modInfoPath = Path.Combine(modPath, "modinfo.json");
            string modName = new DirectoryInfo(modPath).Name;
            List<string> newModReplacedFiles = new List<string>();

            if (File.Exists(modInfoPath))
            {
                string json = File.ReadAllText(modInfoPath);
                ModInfo modInfo = JsonConvert.DeserializeObject<ModInfo>(json);
                if (!config.Mods.ModMappings.ContainsKey(modName))
                {
                    config.Mods.ModMappings[modName] = modInfo.Files;
                    newModReplacedFiles = modInfo.Files.Select(m => m.TargetPath).ToList();
                    config.Mods.ReplacedFiles[modName] = new List<string>(newModReplacedFiles);
                    ConfigManager.SaveSettings(config);
                }
                foreach (var mapping in modInfo.Files)
                {
                    string sourceFile = Path.Combine(modPath, mapping.ModFile);
                    string destinationPath = Path.Combine(gameInstallPath, mapping.TargetPath);
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    CopyFileWithLogging(sourceFile, destinationPath);
                }
                List<ModMapping> fallbackMappings = new List<ModMapping>();
                List<string> fallbackReplacedFiles = new List<string>();
                foreach (string expected in expectedPaths)
                {
                    var matchingDirs = FindDirectoriesEndingWith(modPath, expected);
                    foreach (var dir in matchingDirs)
                    {
                        foreach (var file in Directory.GetFiles(dir, "*", IOSearchOption.AllDirectories))
                        {
                            string localRelPath = file.Substring(dir.Length).TrimStart(Path.DirectorySeparatorChar);
                            string effectiveRelPath = Path.Combine(expected.Replace('/', Path.DirectorySeparatorChar), localRelPath);
                            bool alreadyMapped = modInfo.Files.Any(m => string.Equals(m.TargetPath, effectiveRelPath, StringComparison.OrdinalIgnoreCase));
                            if (!alreadyMapped)
                            {
                                string destinationPath = Path.Combine(gameInstallPath, effectiveRelPath);
                                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                                CopyFileWithLogging(file, destinationPath);
                                fallbackMappings.Add(new ModMapping { ModFile = effectiveRelPath, TargetPath = effectiveRelPath });
                                fallbackReplacedFiles.Add(effectiveRelPath);
                            }
                        }
                    }
                }
                if (fallbackMappings.Count > 0)
                {
                    modInfo.Files.AddRange(fallbackMappings);
                    newModReplacedFiles.AddRange(fallbackReplacedFiles);
                    config.Mods.ModMappings[modName] = modInfo.Files;
                    if (config.Mods.ReplacedFiles.ContainsKey(modName))
                        config.Mods.ReplacedFiles[modName].AddRange(fallbackReplacedFiles);
                    else
                        config.Mods.ReplacedFiles[modName] = new List<string>(fallbackReplacedFiles);
                    ConfigManager.SaveSettings(config);
                }
            }
            else if (IsMGSHDFixMod(modPath))
            {
                string hdFixRoot = FindMGSHDFixRoot(modPath);
                foreach (var file in Directory.GetFiles(hdFixRoot, "*", IOSearchOption.TopDirectoryOnly))
                {
                    string fileName = Path.GetFileName(file);
                    string destinationPath = Path.Combine(gameInstallPath, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    CopyFileWithLogging(file, destinationPath);
                    newModReplacedFiles.Add(fileName);
                }
                config.Mods.ModMappings[modName] = Directory.GetFiles(hdFixRoot, "*", IOSearchOption.TopDirectoryOnly)
                    .Select(f => new ModMapping { ModFile = Path.GetFileName(f), TargetPath = Path.GetFileName(f) })
                    .ToList();
                config.Mods.ReplacedFiles[modName] = newModReplacedFiles;
                ConfigManager.SaveSettings(config);
            }
            else
            {
                List<ModMapping> fallbackMappings = new List<ModMapping>();
                List<string> replacedFiles = new List<string>();

                if (!string.IsNullOrEmpty(selectedVariant))
                {
                    string variantFolderPath = Path.Combine(modPath, selectedVariant);
                    if (Directory.Exists(variantFolderPath))
                    {
                        string targetDirectory = Path.Combine(gameInstallPath, @"textures\flatlist\ovr_stm\_win");

                        Directory.CreateDirectory(targetDirectory);

                        LoggingManager.Instance.Log($"Applying variant '{selectedVariant}' to: {targetDirectory}");

                        foreach (var file in Directory.GetFiles(variantFolderPath, "*.ctxr", IOSearchOption.AllDirectories))
                        {
                            string fileName = Path.GetFileName(file);
                            string destinationPath = Path.Combine(targetDirectory, fileName);

                            LoggingManager.Instance.Log($"Copying CTXR file:\nSource: {file}\nDestination: {destinationPath}");

                            File.Copy(file, destinationPath, true);

                            string gameRelativePath = Path.Combine(@"textures\flatlist\ovr_stm\_win", fileName);
                            replacedFiles.Add(gameRelativePath);
                            fallbackMappings.Add(new ModMapping
                            {
                                ModFile = Path.Combine(selectedVariant, fileName),
                                TargetPath = gameRelativePath
                            });
                        }

                        config.Mods.ModMappings[modName] = fallbackMappings;
                        config.Mods.ReplacedFiles[modName] = replacedFiles;
                        ConfigManager.SaveSettings(config);
                    }
                    else
                    {
                        LoggingManager.Instance.Log($"Variant folder not found: {variantFolderPath}");
                        MessageBox.Show($"Variant folder '{selectedVariant}' not found in mod directory!",
                                       "Missing Variant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    foreach (string expected in expectedPaths)
                    {
                        var matchingDirs = FindDirectoriesEndingWith(modPath, expected);
                        foreach (var dir in matchingDirs)
                        {
                            foreach (var file in Directory.GetFiles(dir, "*", IOSearchOption.AllDirectories))
                            {
                                string localRelPath = file.Substring(dir.Length).TrimStart(Path.DirectorySeparatorChar);
                                string effectiveRelPath = Path.Combine(expected.Replace('/', Path.DirectorySeparatorChar), localRelPath);
                                string destinationPath = Path.Combine(gameInstallPath, effectiveRelPath);
                                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                                CopyFileWithLogging(file, destinationPath);
                                fallbackMappings.Add(new ModMapping { ModFile = effectiveRelPath, TargetPath = effectiveRelPath });
                                replacedFiles.Add(effectiveRelPath);
                            }
                        }
                    }
                }

                if (fallbackMappings.Count > 0)
                {
                    config.Mods.ModMappings[modName] = fallbackMappings;
                    config.Mods.ReplacedFiles[modName] = replacedFiles;
                    ConfigManager.SaveSettings(config);
                }
            }
        }

        public void ApplyModFiles(string modPath, string gameInstallPath)
        {
            ApplyModFiles(modPath, gameInstallPath, null);
        }

        public List<string> FindDirectoriesEndingWith(string root, string expected)
        {
            List<string> matches = new List<string>();
            string expectedNormalized = expected.Replace('/', Path.DirectorySeparatorChar);
            foreach (var dir in Directory.GetDirectories(root, "*", IOSearchOption.AllDirectories))
            {
                if (dir.EndsWith(expectedNormalized, StringComparison.OrdinalIgnoreCase))
                    matches.Add(dir);
            }
            return matches;
        }

        public void ResolveModConflicts(string newModName, List<string> newModFiles)
        {
            foreach (var modEntry in config.Mods.ActiveMods)
            {
                if (!modEntry.Key.Equals(newModName, StringComparison.OrdinalIgnoreCase) && modEntry.Value)
                {
                    if (config.Mods.ReplacedFiles.ContainsKey(modEntry.Key))
                    {
                        List<string> olderFiles = config.Mods.ReplacedFiles[modEntry.Key];
                        var conflicts = olderFiles.Intersect(newModFiles, StringComparer.OrdinalIgnoreCase).ToList();
                        if (conflicts.Any())
                        {
                            olderFiles.RemoveAll(f => conflicts.Contains(f, StringComparer.OrdinalIgnoreCase));
                            config.Mods.ReplacedFiles[modEntry.Key] = olderFiles;
                            if (config.Mods.ModMappings.ContainsKey(modEntry.Key))
                            {
                                List<ModMapping> mappings = config.Mods.ModMappings[modEntry.Key];
                                mappings.RemoveAll(m => conflicts.Contains(m.TargetPath, StringComparer.OrdinalIgnoreCase));
                                config.Mods.ModMappings[modEntry.Key] = mappings;
                            }
                        }
                    }
                }
            }
            ConfigManager.SaveSettings(config);
        }

        public void RestoreVanillaFiles(string modPath, string gameInstallPath)
        {
            string modName = new DirectoryInfo(modPath).Name;
            if (config.Mods.ReplacedFiles.ContainsKey(modName))
            {
                foreach (string relativePath in config.Mods.ReplacedFiles[modName])
                {
                    string backupFilePath = Path.Combine(backupRoot, relativePath);
                    string destinationPath = Path.Combine(gameInstallPath, relativePath);
                    if (File.Exists(backupFilePath))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                        CopyFileWithLogging(backupFilePath, destinationPath);
                    }
                    else if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    RemoveEmptyDirectories(Path.GetDirectoryName(destinationPath));
                }
            }
            if (config.Mods.ActiveVariants.ContainsKey(modName))
            {
                config.Mods.ActiveVariants.Remove(modName);
            }
        }

        public void RemoveEmptyDirectories(string directory)
        {
            if (Directory.Exists(directory) && !Directory.EnumerateFileSystemEntries(directory).Any())
            {
                Directory.Delete(directory);
                RemoveEmptyDirectories(Path.GetDirectoryName(directory));
            }
        }

        public static string GetRelativePath(string relativeTo, string path)
        {
            var relativeToUri = new Uri(relativeTo.EndsWith(Path.DirectorySeparatorChar.ToString())
                                        ? relativeTo
                                        : relativeTo + Path.DirectorySeparatorChar);
            var pathUri = new Uri(path);
            var relativeUri = relativeToUri.MakeRelativeUri(pathUri);
            return Uri.UnescapeDataString(relativeUri.ToString())
                     .Replace('/', Path.DirectorySeparatorChar);
        }

        public string GetTargetRelativePath(string file, string modPath)
        {
            string relativePath = GetRelativePath(modPath, file);
            relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            foreach (string expected in expectedPaths)
            {
                string expectedNormalized = expected.Replace('/', Path.DirectorySeparatorChar);
                int index = relativePath.IndexOf(expectedNormalized, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                    return relativePath.Substring(index);
            }
            return null;
        }

        public void CopyFileWithLogging(string sourceFile, string destinationPath)
        {
            try
            {
                long sizeBefore = 0;
                bool fileExists = File.Exists(destinationPath);
                if (fileExists)
                    sizeBefore = new FileInfo(destinationPath).Length;
                File.Copy(sourceFile, destinationPath, true);
                long sizeAfter = new FileInfo(destinationPath).Length;
                string message = fileExists ?
                    $"OVERWRITTEN:\nDestination: {destinationPath}\nSource: {sourceFile}\nSize before: {sizeBefore} bytes\nSize after: {sizeAfter} bytes" :
                    $"COPIED NEW FILE:\nDestination: {destinationPath}\nSource: {sourceFile}\nSize: {sizeAfter} bytes";
                LoggingManager.Instance.Log(message);
            }
            catch (Exception ex)
            {
                LoggingManager.Instance.Log($"Error copying file:\nSource: {sourceFile}\nDestination: {destinationPath}\nError: {ex.Message}");
            }
        }

        public bool CheckBackupForCompleteness(string gameInstallPath)
        {
            if (string.IsNullOrEmpty(gameInstallPath))
            {
                MessageBox.Show("Game installation not found, cannot attempt modding.", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            foreach(var relativePath in expectedPaths)
            {
                string sourcePath = Path.Combine(gameInstallPath, relativePath);
                string targetPath = Path.Combine(backupRoot, relativePath);
                if (!Directory.Exists(sourcePath))
                    continue;
                if (!Directory.Exists(targetPath))
                    return false;
            }

            return true;
        }

        public void BackupVanillaFiles(string gameInstallPath)
        {
            if (string.IsNullOrEmpty(gameInstallPath))
            {
                MessageBox.Show("Game installation not found, cannot back up files.", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (var relativePath in expectedPaths)
            {
                string sourcePath = Path.Combine(gameInstallPath, relativePath);
                string targetPath = Path.Combine(backupRoot, relativePath);
                if (!Directory.Exists(sourcePath))
                    continue;
                Directory.CreateDirectory(targetPath);
                foreach (var file in Directory.GetFiles(sourcePath))
                {
                    string fileName = Path.GetFileName(file);
                    string destinationFile = Path.Combine(targetPath, fileName);
                    if (!File.Exists(destinationFile))
                        CopyFileWithLogging(file, destinationFile);
                }
            }
        }

        public (List<string> camos, List<string> boxes, List<string> faces) GetRecognizedTextures(string modPath)
        {
            List<string> foundCamos = new List<string>();
            List<string> foundBoxes = new List<string>();
            List<string> foundFaces = new List<string>();

            var ctxrFiles = Directory.GetFiles(modPath, "*.ctxr", IOSearchOption.AllDirectories);
            foreach (string file in ctxrFiles)
            {
                string fileNameNoExt = Path.GetFileNameWithoutExtension(file);
                if (MGS3TextureRenamer.CamoMappings.ContainsKey(fileNameNoExt))
                {
                    foundCamos.Add(file);
                }
                else if (MGS3TextureRenamer.BoxMappings.ContainsKey(fileNameNoExt))
                {
                    foundBoxes.Add(file);
                }
                else if (MGS3TextureRenamer.FaceMappings.ContainsKey(fileNameNoExt))
                {
                    foundFaces.Add(file);
                }
            }
            return (foundCamos, foundBoxes, foundFaces);
        }


    }
}
