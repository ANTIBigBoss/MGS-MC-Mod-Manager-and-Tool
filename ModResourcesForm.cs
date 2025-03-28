﻿using System;
using System.Windows.Forms;

namespace ANTIBigBoss_MGS_Mod_Manager
{
    public partial class ModResourcesForm : Form
    {
        public ModResourcesForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(ModResourcesForm_FormClosing);
        }

        private void ModResourcesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoggingManager.Instance.Log("User exiting the Mod Manager.\nEnd of log for this session.\n\n");
            Application.Exit();
        }

        private void ModResources_Load(object sender, EventArgs e)
        {
            this.Location = GuiManager.GetLastFormLocation();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("Going back to Main Menu from Mod Resources.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "ModResourcesForm");
            MainMenuForm form1 = new MainMenuForm();
            form1.Show();
            this.Hide();
        }

        private void TextureImportExportButton_Click(object sender, EventArgs e)
        {
            LoggingManager.Instance.Log("Changing to Texture and 3D Model form from the Mod Resources form.\n");
            GuiManager.UpdateLastFormLocation(this.Location);
            GuiManager.LogFormLocation(this, "TextureModelForm");
            TextureModelForm textureModelForm = new TextureModelForm();
            textureModelForm.Show();
            this.Hide();
        }

        // For your button:
        private void MGS3GuardRouteTool_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Geom Files (*.geom)|*.geom",
                InitialDirectory = "...",
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                if (GeomEditor.HasGroupedDefinition(path))
                    GeomEditor.EditGeomFileNextPrevGrouped(path);
                else
                    MessageBox.Show("No grouped definition for this file.", "Info");
            }
        }

        private void MGS2GuardRouteTool_Click(object sender, EventArgs e)
        {
            HzxEditor.EditHzxFile();
        }

    }
}