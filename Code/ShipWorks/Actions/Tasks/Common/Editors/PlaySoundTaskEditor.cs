using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using System.Media;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Editor for the PlaySound task
    /// </summary>
    public partial class PlaySoundTaskEditor : ActionTaskEditor
    {
        PlaySoundTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlaySoundTaskEditor(PlaySoundTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (task.PendingSoundFile != null)
            {
                soundFile.Text = Path.GetFileName(task.PendingSoundFile);
            }
            else
            {
                DataResourceReference resource = DataResourceManager.GetResourceReference(task.ResourceReferenceID);
                if (resource != null)
                {
                    soundFile.Text = resource.Label;
                }
            }
        }

        /// <summary>
        /// Browse for the sound file to play
        /// </summary>
        private void OnBrowse(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "WAVE Audio File (*.wav)|*.wav";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    soundFile.Text = Path.GetFileName(dlg.FileName);
                    task.PendingSoundFile = dlg.FileName;
                }
            }
        }

        /// <summary>
        /// Play the selected sound file
        /// </summary>
        private void OnPlay(object sender, EventArgs e)
        {
            try
            {
                task.PlaySound();
            }
            catch (InvalidOperationException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
