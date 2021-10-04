using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using System.Xml.Serialization;
using System.Media;
using ShipWorks.ApplicationCore;
using System.IO;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;
using NAudio.Wave;
using System.Threading;
using System.Diagnostics;
using ShipWorks.Common.Threading;
using NAudio;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for playing a sound
    /// </summary>
    [ActionTask("Play a sound", "PlaySound", ActionTaskCategory.External)]
    public class PlaySoundTask : ActionTask
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PlaySoundTask));

        long resourceReferenceID = 0;
        string pendingSoundFile = null;

        static object audioLock = new object();
        static WaveOut audioCurrent;

        WaveOut thisAudio;

        // In seconds
        bool stopAfter = true;
        int stopAfterSeconds = 5;

        /// <summary>
        /// Raised when the audio stops.  Probably on a background thread
        /// </summary>
        public event EventHandler AudioStopped;

        /// <summary>
        /// Create the editor for editing the task's settings
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new PlaySoundTaskEditor(this);
        }

        /// <summary>
        /// Run the task
        /// </summary>
        protected override void Run()
        {
            // This is so you can set the task to an invalid sound file to force an error, and then force it to 
            // pretend like there is no error later.
            if (InterapptiveOnly.MagicKeysDown)
            {
                return;
            }

            try
            {
                PlaySound();
            }
            catch (MmException ex)
            {
                if (ex.Result == MmResult.BadDeviceId)
                {
                    log.Warn("There doesn't appear to be a soundcard installed.", ex);
                }
                else
                {
                    log.Error("Failed to play sound.", ex);

                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
            // InvalidOperationException is what is thrown if the sound file is "PCM" or whatever that means.
            catch (Exception ex)
            {
                log.Error("Failed to play sound.", ex);

                throw new ActionTaskRunException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Input is not required to play a sound
        /// </summary>
        public override ActionTaskInputRequirement InputRequirement
        {
            get { return ActionTaskInputRequirement.None; }
        }

        /// <summary>
        /// The ID of the sound resource in the database
        /// </summary>
        public long ResourceReferenceID
        {
            get { return resourceReferenceID; }
            set { resourceReferenceID = value; }
        }

        /// <summary>
        /// Controls if audio should automatically stop playing after a set amount of time
        /// </summary>
        public bool StopAfter
        {
            get { return stopAfter; }
            set { stopAfter = value; }
        }

        /// <summary>
        /// The maximum seconds to play the sound before automatically stopping.
        /// </summary>
        public int StopAfterSeconds
        {
            get { return stopAfterSeconds; }
            set { stopAfterSeconds = value; }
        }

        /// <summary>
        /// Set the sound file that will be saved the next time the task is saved
        /// </summary>
        [XmlIgnore]
        public string PendingSoundFile
        {
            get { return pendingSoundFile; }
            set { pendingSoundFile = value; }
        }

        /// <summary>
        /// Save the sound file resource to the database
        /// </summary>
        protected override void SaveExtraState(ActionEntity action, ISqlAdapter adapter)
        {
            // If we are saving, we are closing or changing, so stop playing
            StopSound();

            if (pendingSoundFile == null)
            {
                return;
            }

            // Cleanup the old resource
            DeleteExtraState();

            try
            {
                // Create the resource
                resourceReferenceID = DataResourceManager.CreateFromFile(pendingSoundFile, Entity.ActionTaskID).ReferenceID;

                // Clear the pending now that it's saved
                pendingSoundFile = null;
            }
            catch (IOException ex)
            {
                throw new ActionSaveException(string.Format("The sound file '{0}' could not be accessed.", Path.GetFileName(pendingSoundFile)), ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ActionSaveException(string.Format("The sound file '{0}' could not be accessed.", Path.GetFileName(pendingSoundFile)), ex);
            }
        }

        /// <summary>
        /// Delete the sound file resource from the database
        /// </summary>
        protected override void DeleteExtraState()
        {
            if (resourceReferenceID != 0)
            {
                DataResourceManager.ReleaseResourceReference(resourceReferenceID);
            }
        }

        /// <summary>
        /// Play the sound configured for the task
        /// </summary>
        public void PlaySound()
        {
            // This is to test actions succeeding after failing, when using a bad sound file to force actions failures.
            if (InterapptiveOnly.MagicKeysDown)
            {
                return;
            }

            string fileToPlay = null;

            if (pendingSoundFile != null)
            {
                fileToPlay = pendingSoundFile;
            }
            else
            {
                DataResourceReference resource = DataResourceManager.LoadResourceReference(resourceReferenceID);
                if (resource != null)
                {
                    fileToPlay = resource.GetCachedFilename();
                }
            }

            if (fileToPlay != null)
            {
                lock (audioLock)
                {
                    if (audioCurrent != null && audioCurrent == thisAudio)
                    {
                        throw new InvalidOperationException("The sound is already playing.");
                    }

                    AbortAllSound();
                }

                if (fileToPlay.EndsWith("mp3"))
                {
                    PlayMp3(fileToPlay);
                }
                else
                {
                    PlayWav(fileToPlay);
                }
            }
        }

        /// <summary>
        /// Stop playing the sound for this task, if its currently playing
        /// </summary>
        public void StopSound()
        {
            lock (audioLock)
            {
                if (audioCurrent != null && audioCurrent == thisAudio)
                {
                    audioCurrent.Stop();
                }
            }
        }

        /// <summary>
        /// Stop playing any currently playing sound, for any task
        /// </summary>
        private static void AbortAllSound()
        {
            lock (audioLock)
            {
                if (audioCurrent != null)
                {
                    audioCurrent.Stop();
                }
            }
        }

        /// <summary>
        /// Indicates if the sound is currently playing as initiated and owned by this instance
        /// </summary>
        [XmlIgnore]
        public bool IsSoundPlaying 
        {
            get
            {
                lock (audioLock)
                {
                    return audioCurrent != null && audioCurrent == thisAudio;
                }
            }
        }

        /// <summary>
        /// Play a wave file
        /// </summary>
        private void PlayWav(string waveFile)
        {
            PlayAudio(new AudioFileReader(waveFile));
        }


        /// <summary>
        /// Play an mp3 file
        /// </summary>
        private void PlayMp3(string mp3File)
        {
            FileStream fileStream = File.OpenRead(mp3File);

            Mp3FileReader reader = new Mp3FileReader(fileStream);

            WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(reader);
            BlockAlignReductionStream baStream = new BlockAlignReductionStream(waveStream);

            PlayAudio(baStream);
        }

        /// <summary>
        /// Play the given wave provider
        /// </summary>
        private void PlayAudio(IWaveProvider waveProvider)
        {
            lock (audioLock)
            {
                thisAudio = new WaveOut(WaveCallbackInfo.FunctionCallback());

                thisAudio.PlaybackStopped += OnPlaybackStopped;
                thisAudio.Init(waveProvider);
                thisAudio.Play();

                audioCurrent = thisAudio;

                ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(WaitForAudioStop), Tuple.Create(thisAudio, stopAfter, stopAfterSeconds));
            }
        }

        /// <summary>
        /// Wait for the current audio file to stop
        /// </summary>
        private void WaitForAudioStop(object state)
        {
            var data = (Tuple<WaveOut, bool, int>) state;

            WaveOut localAudio = data.Item1;
            bool localStopAfter = data.Item2;
            int localStopAfterSeconds = data.Item3;

            Stopwatch timer = Stopwatch.StartNew();

            while (true)
            {
                lock (audioLock)
                {
                    if (localStopAfter && timer.Elapsed.TotalSeconds > localStopAfterSeconds)
                    {
                        localAudio.Stop();
                    }

                    if (localAudio.PlaybackState == PlaybackState.Stopped)
                    {
                        // If this was the current running audio, there is no more audio
                        if (localAudio == audioCurrent)
                        {
                            audioCurrent = null;
                        }

                        // If this was our audio, there is no more this audio
                        if (localAudio == thisAudio)
                        {
                            thisAudio = null;
                        }

                        localAudio.PlaybackStopped -= OnPlaybackStopped;
                        localAudio.Dispose();
                        localAudio = null;
                    }
                }

                if (localAudio == null)
                {
                    // If we owned this sound, notify the sender of the stop
                    if (AudioStopped != null)
                    {
                        AudioStopped(this, EventArgs.Empty);
                    }

                    return;
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Playback stopped
        /// </summary>
        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(new WaitCallback(data =>
                {
                    WaveOut waveOut = (WaveOut) sender;

                    lock (audioLock)
                    {
                        waveOut.PlaybackStopped -= OnPlaybackStopped;
                        waveOut.Dispose();
                    }
                })));
        }
    }
}
