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

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for playing a sound
    /// </summary>
    [ActionTask("Play a sound", "PlaySound")]
    public class PlaySoundTask : ActionTask
    {
        long resourceReferenceID = 0;
        string pendingSoundFile = null;

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
            // InvalidOperationException is what is thrown if the sound file is "PCM" or whatever that means.
            catch (InvalidOperationException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Input is not required to play a sound
        /// </summary>
        public override ActionTaskInputRequirement RequiresInput
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
        protected override void SaveExtraState(ActionEntity action, SqlAdapter adapter)
        {
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

            SoundPlayer player = null;

            if (pendingSoundFile != null)
            {
                player = new SoundPlayer(pendingSoundFile);
            }
            else
            {
                DataResourceReference resource = DataResourceManager.LoadResourceReference(resourceReferenceID);
                if (resource != null)
                {
                    player = new SoundPlayer(resource.GetCachedFilename());
                }
            }

            if (player != null)
            {
                player.Play();
            }
        }
    }
}
