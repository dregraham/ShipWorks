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
using ShipWorks.ApplicationCore.Logging;
using System.Threading;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for running a command
    /// </summary>
    [ActionTask("Run a command", "RunCommand", ActionTriggerClassifications.Scheduled)]
    public class RunCommandTask : ActionTask
    {
        static readonly ILog log = LogManager.GetLogger(typeof(RunCommandTask));

        static string commandLogFolder;
        static int nextRunNumber = 0;

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

        static string CommandLogFolder
        {
            get
            {
                if (null == commandLogFolder)
                {
                    var descriptor = ActionTaskManager.GetDescriptor(typeof(RunCommandTask));

                    commandLogFolder = Path.Combine(LogSession.LogFolder, descriptor.Identifier);

                    Directory.CreateDirectory(commandLogFolder);
                }

                return commandLogFolder;
            }
        }

        static string GetNextCommandLogPath()
        {
            var fileName = string.Format("{0:0000}.txt", Interlocked.Increment(ref nextRunNumber));

            return Path.Combine(CommandLogFolder, fileName);
        }

        /// <summary>
        /// Gets the command that should be executed
        /// </summary>
        /// <param name="inputKeys">List of ids of objects to use for merging tokens</param>
        /// <returns></returns>
        private string GetProcessedCommand(IEnumerable<long> inputKeys)
        {
            return
                "@echo off" + Environment.NewLine +
                inputKeys.Select(x => TemplateTokenProcessor.ProcessTokens(Command, x, false)).Aggregate((x, y) => x + Environment.NewLine + y);
        }

        /// <summary>
        /// Run the task
        /// </summary>
        protected override void Run(List<long> inputKeys)
        {
            string executionCommand = GetProcessedCommand(inputKeys);
            string commandFileName = Path.GetRandomFileName() + ".cmd";
            string commandPath = Path.Combine(GetTempPath(), commandFileName);
            string commandLogPath = GetNextCommandLogPath();

            log.InfoFormat("Command: \n{0}", executionCommand);
            log.InfoFormat("Command output log: {0}", commandLogPath);

            // Save the command as a batch file so we can execute it
            File.WriteAllText(commandPath, executionCommand);

            try
            {
                using (var commandLogWriter = File.CreateText(commandLogPath))
                {
                    commandLogWriter.AutoFlush = true;

                    using (var process = new Process())
                    {
                        process.StartInfo = new ProcessStartInfo() {
                            CreateNoWindow = true,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            RedirectStandardInput = true,
                            UseShellExecute = false,
                            WorkingDirectory = GetTempPath(),
                            FileName = commandPath
                        };

                        // Wire up the handlers that will take care of logging output and errors
                        process.OutputDataReceived += (s, e) => commandLogWriter.WriteLine(e.Data);
                        process.ErrorDataReceived += (s, e) => commandLogWriter.WriteLine(e.Data);

                        process.Start();

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.StandardInput.Close();

                        // Get the time that should be used for timing out the process
                        DateTime timeoutDate = DateTime.Now.AddMinutes(CommandTimeoutInMinutes);
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

                        // Wait for asynchronous output processing to complete
                        process.WaitForExit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ActionException("An error ocurred while running the command.", ex);
            }
            finally
            {
                File.Delete(commandPath);
            }
        }
    }
}
