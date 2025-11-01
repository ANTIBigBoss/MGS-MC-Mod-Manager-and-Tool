using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
            "eu/stage/d001p01",
            "eu/stage/d005p03",
            "assets/gcx/us/_bp",
            "assets/gcx/eu/_bp",
            "assets/hzx/eu",
            "assets/hzx/us",
            "assets/mar/us",
            "assets/evm/us",
            "assets/evm/us/_win",
            "assets/kms/us",
            "assets/kms/us/_win",
            "textures/flatlist/_win",
            "textures/flatlist/ovr_eu/_win",
            "textures/flatlist/ovr_PS3/_win",
            "textures/flatlist/ovr_stm/_win",
            "textures/flatlist/ovr_stm/ovr_eu/_win",
            "textures/flatlist/ovr_stm/ctrltype_kbd/_win",
            "textures/flatlist/ovr_stm/ctrltype_nx",
            "textures/flatlist/ovr_stm/ctrltype_ps4/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps4/ovr_eu/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps4/ovr_jp/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps5/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps5/ovr_eu/_win",
            "textures/flatlist/ovr_stm/ctrltype_ps5/ovr_jp/_win",
            "textures/flatlist/ovr_stm/ctrltype_stmd/_win",
            "textures/flatlist/ovr_stm/ctrltype_xs/_win",
            "eu/codec",
            "eu/demo",
            "eu/demo2",
            "eu/face",
            "eu/movie",
            "eu/movievr",
            "eu/stage",
            "eu/vox",
            "eu/vox2",
            "eu/codec/_bp",
            "eu/demo/_bp",
            "eu/demo2/_bp",
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
            "eu/vox2/_bp"
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
                BackColor = Color.FromArgb(15, 57, 48),
                ScrollBars = RichTextBoxScrollBars.None,
                WordWrap = true,
                MaximumSize = new Size(364, 133),
                Size = new Size(364, 133),
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold),
                ForeColor = SystemColors.Control,
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

            //Check to make sure the backup is not missing any directories from the expected paths list
            if (fileExplorerManager.CheckBackupForCompleteness(gameInstallPath))
            {
                fileExplorerManager.BackupVanillaFiles(gameInstallPath);
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
                @"D:\SteamLibrary\steamapps\common\MGS2",
                @"E:\SteamLibrary\steamapps\common\MGS2",
                @"F:\SteamLibrary\steamapps\common\MGS2",
                @"G:\SteamLibrary\steamapps\common\MGS2",
            };

            string foundPath = commonPaths.FirstOrDefault(Directory.Exists);
            if (!string.IsNullOrEmpty(foundPath))
            {
                return foundPath;
            }
            else
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select the installation folder for MGS2 - Master Collection.";
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        return fbd.SelectedPath;
                    }
                    else
                    {
                        GoBackToMainMenu();
                        return null;
                    }
                }
            }
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
                MessageBox.Show(
                    "Game installation not found, cannot apply mods.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (modName.Equals("MGSFPSUnlock", StringComparison.OrdinalIgnoreCase))
            {
                bool isEnabled = config.Mods.ActiveMods.TryGetValue(modName, out var on) && on;
                string fpsFolder = Path.Combine(config.MGS2ModFolderPath, modName);

                if (isEnabled)
                    MGSFPSUnlockManager.Disable(gameInstallPath, config);
                else
                    MGSFPSUnlockManager.Enable(fpsFolder, gameInstallPath, config);

                LoadModsIntoUI();
                return;
            }

            try
            {
                Point savedScroll = modListManager.ModListPanel.AutoScrollPosition;
                bool success = await fileExplorerManager.ToggleModStateByNameAsync(modName, gameInstallPath);

                if (success)
                {
                    // Reload config to ensure we have the latest state
                    config = ConfigManager.LoadSettings();
                    LoadModsIntoUI();
                    modListManager.ModListPanel.AutoScrollPosition = new Point(-savedScroll.X, -savedScroll.Y);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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
            MGSHDFixSettingsForm settingsForm = new MGSHDFixSettingsForm("MGS2", this);
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
                hoverPictureBox.Image = null;
            hoverPictureBox.Size = new Size(364, 270);
            hoverPictureBox.Location = new Point(14, 223);
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

        #endregion
    }
}
