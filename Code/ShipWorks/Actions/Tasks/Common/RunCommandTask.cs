using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
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
        static readonly ILog log = LogManager.GetLogger(typeof(RunCommandTask));
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
        /// Gets the temporary path to which to save the command
        /// </summary>
        private static string GetTempPath()
        {
            return DataPath.ShipWorksTemp;
        }

        /// <summary>
        /// Gets the command that should be executed
        /// </summary>
        /// <param name="inputKeys">List of ids of objects to use for merging tokens</param>
        /// <returns></returns>
        private string GetProcessedCommand(IEnumerable<long> inputKeys)
        {
            return inputKeys.Select(x => TemplateTokenProcessor.ProcessTokens(Command, x)).Aggregate((x, y) => x + '\n' + y);
        }

        /// <summary>
        /// Handle data coming from standard error
        /// </summary>
        private static void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                log.Error(e.Data);
            }
        }

        /// <summary>
        /// Handle data coming from standard output
        /// </summary>
        private static void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                log.Info(e.Data);
            }
        }

        /// <summary>
        /// Log the command that will be executed
        /// </summary>
        /// <param name="command">Contents of the command that will be executed</param>
        private static void LogCommand(string command)
        {
            log.InfoFormat("Command: \n{0}", command);
        }

        /// <summary>
        /// Run the task
        /// </summary>
        protected override void Run(List<long> inputKeys)
        {
            string executionCommand = GetProcessedCommand(inputKeys);
            string commandFileName = Path.GetRandomFileName() + ".bat";
            string commandPath = Path.Combine(GetTempPath(), commandFileName);

            LogCommand(executionCommand);

            // Save the command as a batch file so we can execute it
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
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        WorkingDirectory = GetTempPath(),
                        FileName = commandPath
                    }
                };

                // Wire up the handlers that will take care of logging output and errors
                process.OutputDataReceived += ProcessOnOutputDataReceived;
                process.ErrorDataReceived += ProcessOnErrorDataReceived;

                // Get the time that should be used for timing out the process
                DateTime timeoutDate = DateTime.Now.AddMinutes(CommandTimeoutInMinutes);

                process.Start();

                //
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.StandardInput.Close();

                do
                {
                    if (!process.HasExited)
                    {
                        if (!process.Responding)
                        {
                            log.Warn("Command is not responding");
                        }

                        if (ShouldStopCommandOnTimeout && timeoutDate < DateTime.Now)
                        {
                            process.Kill();
                            throw new TimeoutException(string.Format("The command took longer than {0} minute(s) to execute.", CommandTimeoutInMinutes));
                        }
                    }
                } while (!process.WaitForExit(500));

            }
            catch (Exception ex)
            {
                if (process != null)
                {
                    process.Close();
                }

                throw new ActionException("An error ocurred while running the command.", ex);
            }
            finally
            {
                File.Delete(commandPath);
            }
        }
    }
}
