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
using ShipWorks.Properties;

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

            stopPlaying.Checked = task.StopAfter;
            stopPlayingDuration.Value = task.StopAfterSeconds;

            stopPlaying.CheckedChanged += OnStopAfterChanged;
            stopPlayingDuration.ValueChanged += OnStopAfterDurationChanged;

            task.AudioStopped += OnAudioStopped;

            UpdateAudioImage();
        }

        /// <summary>
        /// Audio has stopped playing
        /// </summary>
        void OnAudioStopped(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => OnAudioStopped(sender, e)));
                return;
            }

            UpdateAudioImage();
        }

        /// <summary>
        /// Duration to stop has changed
        /// </summary>
        void OnStopAfterDurationChanged(object sender, EventArgs e)
        {
            task.StopAfterSeconds = (int) stopPlayingDuration.Value;
        }

        /// <summary>
        /// Whether to stop or not has changed
        /// </summary>
        void OnStopAfterChanged(object sender, EventArgs e)
        {
            task.StopAfter = stopPlaying.Checked;
        }

        /// <summary>
        /// Browse for the sound file to play
        /// </summary>
        private void OnBrowse(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    soundFile.Text = Path.GetFileName(dlg.FileName);
                    task.PendingSoundFile = dlg.FileName;
                    task.StopSound(true);
                }
            }
        }

        /// <summary>
        /// Update the displayed audio image
        /// </summary>
        private void UpdateAudioImage()
        {
            if (task.IsOwnSoundPlaying)
            {
                play.Image = Resources.media_stop_red;
            }
            else
            {
                play.Image = Resources.media_play;
            }
        }

        /// <summary>
        /// Play the selected sound file
        /// </summary>
        private void OnPlay(object sender, EventArgs e)
        {
            if (task.IsOwnSoundPlaying)
            {
                task.StopSound(true);
            }
            else
            {
                try
                {
                    task.PlaySound();
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                }
            }

            UpdateAudioImage();
        }

        /// <summary>
        /// Task validation means we are closing, so stop the sound
        /// </summary>
        public override void ValidateTask(ICollection<TaskValidationError> errors)
        {
            base.ValidateTask(errors);

            task.StopSound();
            UpdateAudioImage();
        }
    }
}
