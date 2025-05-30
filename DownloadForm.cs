﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public partial class DownloadForm : Form
    {
        private DownloadManager downloadManager;

        public DownloadForm()
        {
            InitializeComponent();
            downloadManager = new DownloadManager();
        }

        private async void DownloadForm_Load(object sender, EventArgs e)
        {
            this.Text = "Download Manager";
            this.Size = new Size(600, 800);
            this.BackColor = ColorTranslator.FromHtml("#95957d");

            Button closeButton = new Button
            {
                Text = "Close",
                Location = new Point(10, 10),
                Size = new Size(100, 30),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            closeButton.Click += (s, args) => this.Close();
            this.Controls.Add(closeButton);

            FlowLayoutPanel modInfoPanel = new FlowLayoutPanel
            {
                AutoScroll = true,
                Size = new Size(this.Width - 40, this.Height - 80),
                Location = new Point(20, 50), // Move down by 50px
                BackColor = ColorTranslator.FromHtml("#95957d"),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10)
            };
            this.Controls.Add(modInfoPanel);

            await LoadAndDisplayModInfo(modInfoPanel);
        }

        private async Task LoadAndDisplayModInfo(FlowLayoutPanel modInfoPanel)
        {
            List<ModRepository> modRepos = downloadManager.GetModRepositories();
            List<Mod> gamebananaMods = downloadManager.GetGamebananaMods();

            foreach (var modRepo in modRepos)
            {
                string modInfo = await downloadManager.GetModInfo(modRepo);
                DisplayModInfo(modInfoPanel, modRepo, modInfo);
            }

            foreach (var mod in gamebananaMods)
            {
                DisplayGamebananaModInfo(modInfoPanel, mod);
            }
        }

        private void DisplayModInfo(FlowLayoutPanel modInfoPanel, ModRepository modRepo, string modInfo)
        {
            string[] modInfoLines = modInfo.Split('\n');
            string modName = modInfoLines[0].Replace("Name: ", "");
            string modAuthor = modInfoLines[1].Replace("Author: ", "");
            string modDescription = modInfoLines[2].Replace("Description: ", "");

            Panel modPanel = new Panel
            {
                Width = modInfoPanel.Width - 25,
                Height = 180,
                BackColor = ColorTranslator.FromHtml("#95957d"),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };

            Label modTitleLabel = new Label
            {
                Text = modName,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 14, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 10)
            };
            modPanel.Controls.Add(modTitleLabel);

            Label modAuthorLabel = new Label
            {
                Text = $"Author: {modAuthor}",
                AutoSize = true,
                Font = new Font(Font.FontFamily, 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 40)
            };
            modPanel.Controls.Add(modAuthorLabel);

            Label modDescriptionLabel = new Label
            {
                Text = $"Description: {modDescription}",
                AutoSize = true,
                Font = new Font(Font.FontFamily, 10, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(10, 70),
                MaximumSize = new Size(modPanel.Width - 20, 0)
            };
            modPanel.Controls.Add(modDescriptionLabel);

            Button downloadButton = new Button
            {
                Text = "Download",
                Location = new Point(modPanel.Width - 90, 10),
                Size = new Size(80, 30),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            downloadButton.Click += async (s, args) => await DownloadModFile(modRepo);
            modPanel.Controls.Add(downloadButton);

            modInfoPanel.Controls.Add(modPanel);
        }

        private void DisplayGamebananaModInfo(FlowLayoutPanel modInfoPanel, Mod mod)
        {
            Panel modPanel = new Panel
            {
                Width = modInfoPanel.Width - 25,
                Height = 180,
                BackColor = ColorTranslator.FromHtml("#95957d"),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };

            Label modTitleLabel = new Label
            {
                Text = mod.Name,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 14, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 10)
            };
            modPanel.Controls.Add(modTitleLabel);

            Label modAuthorLabel = new Label
            {
                Text = $"Author: {mod.Author}",
                AutoSize = true,
                Font = new Font(Font.FontFamily, 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 40)
            };
            modPanel.Controls.Add(modAuthorLabel);

            Label modDescriptionLabel = new Label
            {
                Text = $"Description: {mod.Description}",
                AutoSize = true,
                Font = new Font(Font.FontFamily, 10, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(10, 70),
                MaximumSize = new Size(modPanel.Width - 20, 0)
            };
            modPanel.Controls.Add(modDescriptionLabel);

            Button downloadButton = new Button
            {
                Text = "Download",
                Location = new Point(modPanel.Width - 90, 10),
                Size = new Size(80, 30),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            downloadButton.Click += (s, args) => System.Diagnostics.Process.Start(mod.Link);
            modPanel.Controls.Add(downloadButton);

            modInfoPanel.Controls.Add(modPanel);
        }

        private async Task DownloadModFile(ModRepository modRepo)
        {
            try
            {
                string downloadUrl = await downloadManager.GetLatestReleaseDownloadUrl(modRepo);

                if (!string.IsNullOrEmpty(downloadUrl))
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Title = "Save Mod File",
                        Filter = "Zip Files (*.zip)|*.zip|All Files (*.*)|*.*",
                        FileName = $"{modRepo.Name ?? modRepo.Repo}.zip"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string destinationPath = saveFileDialog.FileName;
                        await downloadManager.DownloadModFile(downloadUrl, destinationPath);
                        MessageBox.Show("Mod downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Download URL not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading mod: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

