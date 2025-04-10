﻿using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public class ConfigManager
    {
        private static readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NotificationsAndSettings.json");

        public static ConfigSettings LoadSettings()
        {
            if (!File.Exists(configPath))
            {
                ConfigSettings defaultSettings = new ConfigSettings();
                SaveSettings(defaultSettings);
                return defaultSettings;
            }
            string json = File.ReadAllText(configPath);
            return JsonConvert.DeserializeObject<ConfigSettings>(json) ?? new ConfigSettings();
        }

        public static void SaveSettings(ConfigSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }
    }

    public class ConfigSettings
    {
        public Dictionary<string, string> GamePaths { get; set; } = new Dictionary<string, string>
        {
            { "MG1", "" },
            { "MG2", "" },
            { "MGS1", "" },
            { "MGS2", "" },
            { "MGS3", "" }
        };

        public string MG1VanillaFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MG1", "MG1 Vanilla Files");

       public string MG2VanillaFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MG2", "MG2 Vanilla Files");

        public string MGS1VanillaFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MGS1", "MGS1 Vanilla Files");

        public string MGS2VanillaFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MGS2", "MGS2 Vanilla Files");

        public string MGS3VanillaFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MGS3", "MGS3 Vanilla Files");

        public string MG1ModFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MG1", "MG1 Mods");

        public string MG2ModFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MG2", "MG2 Mods");

        public string MGS1ModFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MGS1", "MGS1 Mods");

        public string MGS2ModFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MGS2", "MGS2 Mods");

        public string MGS3ModFolderPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MGS3", "MGS3 Mods");

        public string ModToolsPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MGS Modding Tools");

        public bool MG1VanillaFolderSet { get; set; } = false;
        public bool MG1ModFolderSet { get; set; } = false;
        public bool MG2VanillaFolderSet { get; set; } = false;
        public bool MG2ModFolderSet { get; set; } = false;
        public bool MGS1VanillaFolderSet { get; set; } = false;
        public bool MGS1ModFolderSet { get; set; } = false;
        public bool MGS1GamePathSet { get; set; } = false;

        public bool MGS2VanillaFolderSet { get; set; } = false;
        public bool MGS2ModFolderSet { get; set; } = false;
        public bool MGS3VanillaFolderSet { get; set; } = false;
        public bool MGS3ModFolderSet { get; set; } = false;
        public bool ModToolsFolderSet { get; set; } = false;

        // New assets configuration section.
        public AssetsConfig Assets { get; set; } = new AssetsConfig();

        public AppSettings Settings { get; set; } = new AppSettings();
        public BackupStatus Backup { get; set; } = new BackupStatus();
        public ModTracking Mods { get; set; } = new ModTracking();
        public Dictionary<string, string> AudioReplacements { get; set; } = new Dictionary<string, string>();
    }

    public class AssetsConfig
    {
        public string ModelsAndTexturesFolder { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                         "MGS Mod Manager and Trainer", "MGS Modding Tools", "3D Models and Textures");

        public Dictionary<string, string> FolderMapping { get; set; } = new Dictionary<string, string>
        {
            { "GRU", "GRU" },
            { "MGS3NakedSnake", "MGS3 Naked Snake" }
        };
    }
    public class BackupStatus
    {
        public bool MG1BackupCompleted { get; set; } = false;
        public bool MG2BackupCompleted { get; set; } = false;
        public bool MGS1BackupCompleted { get; set; } = false;
        public bool MGS2BackupCompleted { get; set; } = false;
        public bool MGS3BackupCompleted { get; set; } = false;
    }

    public class ModTracking
    {
        public bool ModFolderCreated { get; set; } = false;
        public Dictionary<string, bool> ActiveMods { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, List<ModMapping>> ModMappings { get; set; } = new Dictionary<string, List<ModMapping>>();
        public Dictionary<string, List<string>> ReplacedFiles { get; set; } = new Dictionary<string, List<string>>();
    }

    public class AppSettings
    {
        public bool AutoDetectGamePaths { get; set; } = true;
        public bool EnableLogging { get; set; } = true;
        public bool ShowNotifications { get; set; } = true;
    }

    public class ModInfo
    {
        public List<ModMapping> Files { get; set; }
    }

    public class ModMapping
    {
        public string ModFile { get; set; }
        public string TargetPath { get; set; }
    }

    public class ModRepository
    {
        public string Owner { get; set; }
        public string Repo { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
    }

    public class Mod
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Game { get; set; }
    }

    

    }