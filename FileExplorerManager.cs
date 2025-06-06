﻿using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IOSearchOption = System.IO.SearchOption;
using System.Threading.Tasks;

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

        public async Task ToggleModStateByNameAsync(string modName, string gameInstallPath)
        {
            bool isEnabled = config.Mods.ActiveMods.TryGetValue(modName, out var enabled) && enabled;
            string modPath = Path.Combine(modFolder, modName);

            await Task.Run(() =>
            {
                if (isEnabled)
                {
                    RestoreVanillaFiles(modPath, gameInstallPath);
                    config.Mods.ActiveMods[modName] = false;
                    config.Mods.ActiveVariants.Remove(modName);
                }
                else
                {
                    string selectedVariant = config.Mods.ActiveVariants.TryGetValue(modName, out var variant) ? variant : null;
                    ApplyModFiles(modPath, gameInstallPath, selectedVariant);
                    config.Mods.ActiveMods[modName] = true;
                }
            });

            ConfigManager.SaveSettings(config);
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

            if (Directory.Exists(destinationPath))
            {
                MessageBox.Show($"The mod '{modName}' is already in the list.",
                    "Mod Already Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DirectoryCopy(modPath, destinationPath, true);
            config.Mods.ActiveMods[modName] = false;
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirName}");
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);
            foreach (FileInfo file in dir.GetFiles())
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
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
