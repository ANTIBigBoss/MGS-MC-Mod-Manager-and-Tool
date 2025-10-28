using System;
using System.Drawing;
using System.Windows.Forms;

namespace ANTIBigBoss_MGS_Mod_Manager
{


    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(MainMenuForm_FormClosing);
        }

        public static class AppInfo
        {
            public const string CurrentVersion = "1.0.1.0";
        }
        private void ShowChangelog()
        {
            using (Form changelogForm = new Form())
            {
                changelogForm.Text = "Changelog - What's New!";
                changelogForm.StartPosition = FormStartPosition.CenterParent;
                changelogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                changelogForm.MinimizeBox = false;
                changelogForm.MaximizeBox = false;
                changelogForm.ClientSize = new Size(595, 525);

                // Create a panel with AutoScroll enabled
                Panel scrollPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true
                };

                Label lbl = new Label
                {
                    Font = new Font("Segoe UI", 11f, FontStyle.Regular),
                    Text = BuildChangelogText(),
                    AutoSize = true,
                    TextAlign = ContentAlignment.TopLeft,
                    UseMnemonic = false
                };

                scrollPanel.Controls.Add(lbl);

                Button btn = new Button
                {
                    Text = "Close",
                    Dock = DockStyle.Bottom,
                    Height = 35
                };
                btn.Click += (_, __) => changelogForm.Close();

                changelogForm.Controls.Add(scrollPanel);
                changelogForm.Controls.Add(btn);

                changelogForm.ShowDialog(this);
            }
        }

        private string BuildChangelogText()
        {
            return
@"Thanks for using the Mod Manager and Tools! - ANTIBigBoss

 Changelog - 1.0.1.0
- Thanks to SageFantasma we've added in a model swapping 
feature with existing MGS2 models to the Modding Tools menu. 
Allows you to swap Snake and Raiden's model with a 
selection of available models in the dropdown.
- Fixed the MGS2 Guard Route Editor's set button as the
recent update to MGS2 2.0.2 broke that functionality

Changelog - v1.0.0.9
- Attempted a patch at MGS2 Mods not showing as enabled when a mod was enabled
- Fixed some logic issues in 3D Model and Texture Import/Export tool
- Created a new organization system for mods created in 
  3D Model and Texture Import/Export tool
- Mods should no longer falsely appear as enabled in MGS2/3 Mod folders 
  when a mod is made in the 3D Model and Texture Import/Export tool

Changelog - v1.0.0.8
- Added in the ability to make a mod from any model or texture 
(Would recommend still keeping them MGS2/3 related to avoid issues in naming) 

Changelog - v1.0.0.7
- Resource Editor added for MGS2 in the Mod Resources
- Added in some more models for MGS2
- QOL Tweaks to the MGS2 Guard editor
- Added more tweaks into the Mipmap issue (Still has some issues to be ironed out)

Changelog - v1.0.0.6
- Fixed a bug where the MGS3 Mods folder could be infinitely duplicated if 
  selected as a mod via 'Add Mod' in the MGS3 Modding Form
- Fixed a bug where textures that shouldn't have mip maps were being given
  mip maps and causing weird issues in the Texture Import/Export tool
- Fixed a bug in the Texture Import/Export tool where you could create an
  empty mod if no textures were selected
- Added support for MGS2 Manifest and Resource text files for future model swaps

Changelog - v1.0.0.5
- Added in over 100 MGS3 3D models to the 
  3D Model and Texture Import/Export tool
- Added in support for MGS2 mods made with the 
  SeaLouse Model Import/Export tool

Changelog - v1.0.0.4
- Added in more 3D models to the 3D Model and Texture Import/Export tool
- Added in a Help/FAQ button for the Texture Import/Export tool
- Bug fixes and UI improvements for Texture Import/Export tool
- MGSFPSUnlock support added to MGS2 and MGS3 Modding Forms
- MGS3CrouchWalk support added to MGS3 Modding Form
- UI Fixes where MGS3 Modding Form would start from 
  top of the page everytime a mod was activated or deactivated";
        }

        private void MainMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoggingManager.Instance.Log("User exiting the Mod Manager.\nEnd of log for this session.\n\n");
            Application.Exit();
        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            this.Location = GuiManager.GetLastFormLocation();

            var settings = ConfigManager.LoadSettings();

            if (settings.Settings.LastSeenVersion != AppInfo.CurrentVersion)
            {
                ShowChangelog();
                settings.Settings.LastSeenVersion = AppInfo.CurrentVersion;
                ConfigManager.SaveSettings(settings);
            }
        }


        private void MG1FormSwap_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("User is changing to the MG1 form from the Main Menu form.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "MG1ModdingForm");
            MG1andMG2ModdingForm form2 = new MG1andMG2ModdingForm();
            form2.Show();
            this.Hide();
        }

        private void MGS1FormSwap_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("User is changing to the MGS1 form from the Main Menu form.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "MGS1ModdingForm");
            MGS1ModdingForm form4 = new MGS1ModdingForm();
            form4.Show();
            this.Hide();
        }

        private void MGS2FormSwap_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("User is changing to the MGS2 form from the Main Menu form.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "MGS2ModdingForm");
            MGS2ModdingForm form5 = new MGS2ModdingForm();
            form5.Show();
            this.Hide();
        }

        private void MGS3FormSwap_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("User is changing to the MGS3 form from the Main Menu form.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "MGS3ModdingForm");
            MGS3ModdingForm form6 = new MGS3ModdingForm();
            form6.Show();
            this.Hide();
        }

        private void ModResourcesFormSwap_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("User is changing to the Mod Resources form from the Main Menu form.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "ModResourcesForm");
            ModResourcesForm form7 = new ModResourcesForm();
            form7.Show();
            this.Hide();
        }

        private void KoFiLink_Click(object sender, EventArgs e)
        {
            string url = "https://ko-fi.com/antibigboss";

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(psi);
        }

        private void HelpFaqButton_Click(object sender, EventArgs e)
        {
            ShowChangelog();
        }
    }
}
