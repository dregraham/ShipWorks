namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class PlaySoundTaskEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelSound = new System.Windows.Forms.Label();
            this.soundFile = new System.Windows.Forms.TextBox();
            this.play = new System.Windows.Forms.Button();
            this.browse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelSound
            // 
            this.labelSound.AutoSize = true;
            this.labelSound.Location = new System.Drawing.Point(3, 3);
            this.labelSound.Name = "labelSound";
            this.labelSound.Size = new System.Drawing.Size(41, 13);
            this.labelSound.TabIndex = 0;
            this.labelSound.Text = "Sound:";
            // 
            // soundFile
            // 
            this.soundFile.Location = new System.Drawing.Point(50, 0);
            this.soundFile.Name = "soundFile";
            this.soundFile.ReadOnly = true;
            this.soundFile.Size = new System.Drawing.Size(247, 21);
            this.soundFile.TabIndex = 1;
            // 
            // play
            // 
            this.play.Image = global::ShipWorks.Properties.Resources.media_play;
            this.play.Location = new System.Drawing.Point(299, 0);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(30, 23);
            this.play.TabIndex = 2;
            this.play.UseVisualStyleBackColor = true;
            this.play.Click += new System.EventHandler(this.OnPlay);
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(222, 23);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 3;
            this.browse.Text = "Browse...";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // PlaySoundTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browse);
            this.Controls.Add(this.play);
            this.Controls.Add(this.soundFile);
            this.Controls.Add(this.labelSound);
            this.Name = "PlaySoundTaskEditor";
            this.Size = new System.Drawing.Size(335, 50);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSound;
        private System.Windows.Forms.TextBox soundFile;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button browse;
    }
}
