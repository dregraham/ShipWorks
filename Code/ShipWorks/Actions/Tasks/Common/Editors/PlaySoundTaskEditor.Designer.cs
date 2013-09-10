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
            this.browse = new System.Windows.Forms.Button();
            this.labelStopAfter = new System.Windows.Forms.Label();
            this.stopPlayingDuration = new System.Windows.Forms.NumericUpDown();
            this.stopPlaying = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.play = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.stopPlayingDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSound
            // 
            this.labelSound.AutoSize = true;
            this.labelSound.Location = new System.Drawing.Point(21, 3);
            this.labelSound.Name = "labelSound";
            this.labelSound.Size = new System.Drawing.Size(41, 13);
            this.labelSound.TabIndex = 0;
            this.labelSound.Text = "Sound:";
            // 
            // soundFile
            // 
            this.soundFile.Location = new System.Drawing.Point(68, 0);
            this.soundFile.Name = "soundFile";
            this.soundFile.ReadOnly = true;
            this.soundFile.Size = new System.Drawing.Size(229, 21);
            this.soundFile.TabIndex = 1;
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(335, 0);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 3;
            this.browse.Text = "Browse...";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // labelStopAfter
            // 
            this.labelStopAfter.AutoSize = true;
            this.labelStopAfter.Location = new System.Drawing.Point(10, 29);
            this.labelStopAfter.Name = "labelStopAfter";
            this.labelStopAfter.Size = new System.Drawing.Size(52, 13);
            this.labelStopAfter.TabIndex = 4;
            this.labelStopAfter.Text = "Duration:";
            // 
            // stopPlayingDuration
            // 
            this.stopPlayingDuration.Location = new System.Drawing.Point(180, 27);
            this.stopPlayingDuration.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.stopPlayingDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.stopPlayingDuration.Name = "stopPlayingDuration";
            this.stopPlayingDuration.Size = new System.Drawing.Size(66, 21);
            this.stopPlayingDuration.TabIndex = 5;
            this.stopPlayingDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // stopPlaying
            // 
            this.stopPlaying.AutoSize = true;
            this.stopPlaying.Location = new System.Drawing.Point(68, 28);
            this.stopPlaying.Name = "stopPlaying";
            this.stopPlaying.Size = new System.Drawing.Size(112, 17);
            this.stopPlaying.TabIndex = 6;
            this.stopPlaying.Text = "Stop playing after";
            this.stopPlaying.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "seconds";
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
            // PlaySoundTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stopPlaying);
            this.Controls.Add(this.stopPlayingDuration);
            this.Controls.Add(this.labelStopAfter);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.play);
            this.Controls.Add(this.soundFile);
            this.Controls.Add(this.labelSound);
            this.Name = "PlaySoundTaskEditor";
            this.Size = new System.Drawing.Size(418, 60);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.stopPlayingDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSound;
        private System.Windows.Forms.TextBox soundFile;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Label labelStopAfter;
        private System.Windows.Forms.NumericUpDown stopPlayingDuration;
        private System.Windows.Forms.CheckBox stopPlaying;
        private System.Windows.Forms.Label label1;
    }
}
