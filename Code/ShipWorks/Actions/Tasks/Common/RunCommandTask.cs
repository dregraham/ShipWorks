using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Web.Services3.Mime;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for running a command
    /// </summary>
    [ActionTask("Run a command", "RunCommand", ActionTriggerClassifications.Scheduled)]
    public class RunCommandTask : ActionTask
    {
        /// <summary>
        /// Creates the editor that will be used to modify the task
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new RunCommandTaskEditor(this);
        }

        /// <summary>
        /// This task requires input
        /// </summary>
        public override bool RequiresInput
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the text to use for the input label
        /// </summary>
        public override string InputLabel
        {
            get { return "Run command using:"; }
        }

        /// <summary>
        /// Gets or sets the command that should be executed
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets whether the command should be stopped after the timeout has elapsed
        /// </summary>
        public bool ShouldStopCommandOnTimeout { get; set; }

        /// <summary>
        /// Gets or sets how long to wait before timing out the command
        /// </summary>
        public int CommandTimeoutInMinutes { get; set; }

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            string executionCommand = TemplateTokenProcessor.ProcessTokens(Command, inputKeys);
            string commandFileName = Path.GetRandomFileName();
            string commandPath = Path.Combine(DataPath.ShipWorksTemp, commandFileName);

            File.WriteAllText(commandPath, executionCommand);

            Process process = null;

            try
            {
                process = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        WorkingDirectory = DataPath.ShipWorksTemp,
                        FileName = commandPath
                    }
                };

                //process.OutputDataReceived += (sender, args) =>


                do
                {
                    if (!process.HasExited)
                    {

                    }
                } while (!process.WaitForExit(1000));
            }
            catch (Exception)
            {
                if (process != null)
                {
                    process.Close();
                }
            }
            finally
            {
                
            }
        }
    }
}
