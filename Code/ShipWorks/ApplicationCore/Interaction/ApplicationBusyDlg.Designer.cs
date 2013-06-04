namespace ShipWorks.ApplicationCore.Interaction
{
    partial class ApplicationBusyDlg
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
            this.components = new System.ComponentModel.Container();
            this.hourglass = new System.Windows.Forms.PictureBox();
            this.labelOperation = new System.Windows.Forms.Label();
            this.labelGoal = new System.Windows.Forms.Label();
            this.labelWaiting = new System.Windows.Forms.Label();
            this.waitingIndicator = new System.Windows.Forms.PictureBox();
            this.cancel = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.hourglass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.waitingIndicator)).BeginInit();
            this.SuspendLayout();
            // 
            // hourglass
            // 
            this.hourglass.Image = global::ShipWorks.Properties.Resources.hourglass32;
            this.hourglass.Location = new System.Drawing.Point(12, 12);
            this.hourglass.Name = "hourglass";
            this.hourglass.Size = new System.Drawing.Size(32, 32);
            this.hourglass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.hourglass.TabIndex = 0;
            this.hourglass.TabStop = false;
            // 
            // labelOperation
            // 
            this.labelOperation.AutoSize = true;
            this.labelOperation.Location = new System.Drawing.Point(50, 12);
            this.labelOperation.Name = "labelOperation";
            this.labelOperation.Size = new System.Drawing.Size(136, 13);
            this.labelOperation.TabIndex = 1;
            this.labelOperation.Text = "ShipWorks is currently {0}.";
            // 
            // labelGoal
            // 
            this.labelGoal.Location = new System.Drawing.Point(50, 31);
            this.labelGoal.Name = "labelGoal";
            this.labelGoal.Size = new System.Drawing.Size(387, 37);
            this.labelGoal.TabIndex = 2;
            this.labelGoal.Text = "This activity must finish before you can {0}. You can wait for activity to finish" +
                ", or cancel and return to ShipWorks.";
            // 
            // labelWaiting
            // 
            this.labelWaiting.AutoSize = true;
            this.labelWaiting.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelWaiting.Location = new System.Drawing.Point(68, 67);
            this.labelWaiting.Name = "labelWaiting";
            this.labelWaiting.Size = new System.Drawing.Size(59, 13);
            this.labelWaiting.TabIndex = 3;
            this.labelWaiting.Text = "Waiting...";
            // 
            // waitingIndicator
            // 
            this.waitingIndicator.Image = global::ShipWorks.Properties.Resources.indiciator_green;
            this.waitingIndicator.Location = new System.Drawing.Point(51, 65);
            this.waitingIndicator.Name = "waitingIndicator";
            this.waitingIndicator.Size = new System.Drawing.Size(16, 16);
            this.waitingIndicator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.waitingIndicator.TabIndex = 4;
            this.waitingIndicator.TabStop = false;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(355, 62);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Interval = 250;
            this.timer.Tick += new System.EventHandler(this.OnTimer);
            // 
            // BackgroundOperationDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(442, 94);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.waitingIndicator);
            this.Controls.Add(this.labelWaiting);
            this.Controls.Add(this.labelGoal);
            this.Controls.Add(this.labelOperation);
            this.Controls.Add(this.hourglass);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackgroundOperationDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activity In Progress";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.hourglass)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.waitingIndicator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox hourglass;
        private System.Windows.Forms.Label labelOperation;
        private System.Windows.Forms.Label labelGoal;
        private System.Windows.Forms.Label labelWaiting;
        private System.Windows.Forms.PictureBox waitingIndicator;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Timer timer;
    }
}