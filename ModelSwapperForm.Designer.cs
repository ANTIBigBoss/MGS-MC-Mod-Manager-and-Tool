namespace ANTIBigBoss_MGS_Mod_Manager
{
    partial class ModelSwapperForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            modelToSwapOutComboBox = new System.Windows.Forms.ComboBox();
            modelToSwapLabel = new System.Windows.Forms.Label();
            modelToSwapInLabel = new System.Windows.Forms.Label();
            modelToSwapInComboBox = new System.Windows.Forms.ComboBox();
            swapInNewModelButton = new System.Windows.Forms.Button();
            restoreModelsButton = new System.Windows.Forms.Button();
            cutsceneCheckbox = new System.Windows.Forms.CheckBox();
            codecCheckBox = new System.Windows.Forms.CheckBox();
            armsCheckbox = new System.Windows.Forms.CheckBox();
            extrasCheckBox = new System.Windows.Forms.CheckBox();
            shadowCheckBox = new System.Windows.Forms.CheckBox();
            createModPackCheckBox = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // modelToSwapOutComboBox
            // 
            modelToSwapOutComboBox.FormattingEnabled = true;
            modelToSwapOutComboBox.Location = new System.Drawing.Point(54, 102);
            modelToSwapOutComboBox.Name = "modelToSwapOutComboBox";
            modelToSwapOutComboBox.Size = new System.Drawing.Size(225, 23);
            modelToSwapOutComboBox.TabIndex = 0;
            modelToSwapOutComboBox.SelectedIndexChanged += modelToSwapOutComboBox_SelectedIndexChanged;
            // 
            // modelToSwapLabel
            // 
            modelToSwapLabel.AutoSize = true;
            modelToSwapLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            modelToSwapLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            modelToSwapLabel.Location = new System.Drawing.Point(54, 67);
            modelToSwapLabel.Name = "modelToSwapLabel";
            modelToSwapLabel.Size = new System.Drawing.Size(204, 32);
            modelToSwapLabel.TabIndex = 1;
            modelToSwapLabel.Text = "Model To Change";
            // 
            // modelToSwapInLabel
            // 
            modelToSwapInLabel.AutoSize = true;
            modelToSwapInLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            modelToSwapInLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            modelToSwapInLabel.Location = new System.Drawing.Point(332, 67);
            modelToSwapInLabel.Name = "modelToSwapInLabel";
            modelToSwapInLabel.Size = new System.Drawing.Size(205, 32);
            modelToSwapInLabel.TabIndex = 3;
            modelToSwapInLabel.Text = "Model To Swap In";
            // 
            // modelToSwapInComboBox
            // 
            modelToSwapInComboBox.Enabled = false;
            modelToSwapInComboBox.FormattingEnabled = true;
            modelToSwapInComboBox.Location = new System.Drawing.Point(332, 102);
            modelToSwapInComboBox.Name = "modelToSwapInComboBox";
            modelToSwapInComboBox.Size = new System.Drawing.Size(205, 23);
            modelToSwapInComboBox.TabIndex = 2;
            modelToSwapInComboBox.SelectedIndexChanged += modelToSwapInComboBox_SelectedIndexChanged;
            // 
            // swapInNewModelButton
            // 
            swapInNewModelButton.Location = new System.Drawing.Point(246, 313);
            swapInNewModelButton.Name = "swapInNewModelButton";
            swapInNewModelButton.Size = new System.Drawing.Size(131, 23);
            swapInNewModelButton.TabIndex = 4;
            swapInNewModelButton.Text = "Swap In New Model";
            swapInNewModelButton.UseVisualStyleBackColor = true;
            swapInNewModelButton.Click += button1_Click;
            // 
            // restoreModelsButton
            // 
            restoreModelsButton.Location = new System.Drawing.Point(198, 362);
            restoreModelsButton.Name = "restoreModelsButton";
            restoreModelsButton.Size = new System.Drawing.Size(206, 23);
            restoreModelsButton.TabIndex = 5;
            restoreModelsButton.Text = "Restore Models From Backup";
            restoreModelsButton.UseVisualStyleBackColor = true;
            restoreModelsButton.Click += restoreModelsButton_Click;
            // 
            // cutsceneCheckbox
            // 
            cutsceneCheckbox.AutoSize = true;
            cutsceneCheckbox.Enabled = false;
            cutsceneCheckbox.ForeColor = System.Drawing.Color.White;
            cutsceneCheckbox.Location = new System.Drawing.Point(332, 131);
            cutsceneCheckbox.Name = "cutsceneCheckbox";
            cutsceneCheckbox.Size = new System.Drawing.Size(214, 19);
            cutsceneCheckbox.TabIndex = 6;
            cutsceneCheckbox.Text = "Replace Original's Cutscene Model?";
            cutsceneCheckbox.UseVisualStyleBackColor = true;
            // 
            // codecCheckBox
            // 
            codecCheckBox.AutoSize = true;
            codecCheckBox.Enabled = false;
            codecCheckBox.ForeColor = System.Drawing.Color.White;
            codecCheckBox.Location = new System.Drawing.Point(332, 156);
            codecCheckBox.Name = "codecCheckBox";
            codecCheckBox.Size = new System.Drawing.Size(199, 19);
            codecCheckBox.TabIndex = 7;
            codecCheckBox.Text = "Replace Original's Codec Model?";
            codecCheckBox.UseVisualStyleBackColor = true;
            // 
            // armsCheckbox
            // 
            armsCheckbox.AutoSize = true;
            armsCheckbox.Enabled = false;
            armsCheckbox.ForeColor = System.Drawing.Color.White;
            armsCheckbox.Location = new System.Drawing.Point(332, 181);
            armsCheckbox.Name = "armsCheckbox";
            armsCheckbox.Size = new System.Drawing.Size(216, 19);
            armsCheckbox.TabIndex = 8;
            armsCheckbox.Text = "Replace Original's FPV Arms Model?";
            armsCheckbox.UseVisualStyleBackColor = true;
            // 
            // extrasCheckBox
            // 
            extrasCheckBox.AutoSize = true;
            extrasCheckBox.Enabled = false;
            extrasCheckBox.ForeColor = System.Drawing.Color.White;
            extrasCheckBox.Location = new System.Drawing.Point(332, 231);
            extrasCheckBox.Name = "extrasCheckBox";
            extrasCheckBox.Size = new System.Drawing.Size(353, 34);
            extrasCheckBox.TabIndex = 9;
            extrasCheckBox.Text = "Keep Original's Extra Parts? \r\n(e.g. Raiden hair + extra mags, Snake's bandana + extra mags)";
            extrasCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            extrasCheckBox.UseVisualStyleBackColor = true;
            // 
            // shadowCheckBox
            // 
            shadowCheckBox.AutoSize = true;
            shadowCheckBox.Enabled = false;
            shadowCheckBox.ForeColor = System.Drawing.Color.White;
            shadowCheckBox.Location = new System.Drawing.Point(332, 206);
            shadowCheckBox.Name = "shadowCheckBox";
            shadowCheckBox.Size = new System.Drawing.Size(170, 19);
            shadowCheckBox.TabIndex = 10;
            shadowCheckBox.Text = "Replace Original's Shadow?";
            shadowCheckBox.UseVisualStyleBackColor = true;
            // 
            // createModPackCheckBox
            // 
            createModPackCheckBox.AutoSize = true;
            createModPackCheckBox.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            createModPackCheckBox.Location = new System.Drawing.Point(383, 316);
            createModPackCheckBox.Name = "createModPackCheckBox";
            createModPackCheckBox.Size = new System.Drawing.Size(194, 19);
            createModPackCheckBox.TabIndex = 11;
            createModPackCheckBox.Text = "Create Mod Manager Mod Pack";
            createModPackCheckBox.UseVisualStyleBackColor = true;
            // 
            // ModelSwapperForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(createModPackCheckBox);
            Controls.Add(shadowCheckBox);
            Controls.Add(extrasCheckBox);
            Controls.Add(armsCheckbox);
            Controls.Add(codecCheckBox);
            Controls.Add(cutsceneCheckbox);
            Controls.Add(restoreModelsButton);
            Controls.Add(swapInNewModelButton);
            Controls.Add(modelToSwapInLabel);
            Controls.Add(modelToSwapInComboBox);
            Controls.Add(modelToSwapLabel);
            Controls.Add(modelToSwapOutComboBox);
            Name = "ModelSwapperForm";
            Text = "ModelSwapperForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox modelToSwapOutComboBox;
        private System.Windows.Forms.Label modelToSwapLabel;
        private System.Windows.Forms.Label modelToSwapInLabel;
        private System.Windows.Forms.ComboBox modelToSwapInComboBox;
        private System.Windows.Forms.Button swapInNewModelButton;
        private System.Windows.Forms.Button restoreModelsButton;
        private System.Windows.Forms.CheckBox cutsceneCheckbox;
        private System.Windows.Forms.CheckBox codecCheckBox;
        private System.Windows.Forms.CheckBox armsCheckbox;
        private System.Windows.Forms.CheckBox extrasCheckBox;
        private System.Windows.Forms.CheckBox shadowCheckBox;
        private System.Windows.Forms.CheckBox createModPackCheckBox;
    }
}