namespace ShipWorks.ApplicationCore.Services
{
    partial class ShipWorksServiceBase
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
            this.checkInTimer = new System.Timers.Timer();
            this.tryStartTimer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.checkInTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tryStartTimer)).BeginInit();
            // 
            // checkInTimer
            // 
            this.checkInTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnCheckInTimerElapsed);
            // 
            // tryStartTimer
            // 
            this.tryStartTimer.AutoReset = false;
            this.tryStartTimer.Interval = 180000D;
            this.tryStartTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTryStartTimerElapsed);
            ((System.ComponentModel.ISupportInitialize)(this.checkInTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tryStartTimer)).EndInit();

        }

        #endregion

        private System.Timers.Timer checkInTimer;
        private System.Timers.Timer tryStartTimer;
    }
}
