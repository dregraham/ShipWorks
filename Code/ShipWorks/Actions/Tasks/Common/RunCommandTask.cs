using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Templates.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;


namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for running a command
    /// </summary>
    [ActionTask("Run a command", "RunCommand")]
    public class RunCommandTask : ActionTask
    {
        static readonly ILog log = LogManager.GetLogger(typeof(RunCommandTask));

        static string commandLogFolder;
        static int nextRunNumber = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunCommandTask"/> class.
        /// </summary>
        public RunCommandTask()
        {
            // Provide a default timeout value that coincides with minimum timeout 
            // allowed by the task editor
            CommandTimeoutInMinutes = 1;
        }

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

        private static string CommandLogFolder
        {
            get
            {
                if (null == commandLogFolder)
                {
                    ActionTaskDescriptor descriptor = ActionTaskManager.GetDescriptor(typeof(RunCommandTask));

                    commandLogFolder = Path.Combine(LogSession.LogFolder, descriptor.Identifier);

                    Directory.CreateDirectory(commandLogFolder);
                }

                return commandLogFolder;
            }
        }

        static string GetNextCommandLogPathWithoutExtension()
        {
            var fileName = string.Format("{0:0000}", Interlocked.Increment(ref nextRunNumber));

            return Path.Combine(CommandLogFolder, fileName);
        }

        /// <summary>
        /// Gets the command that should be executed
        /// </summary>
        /// <param name="inputKey">Id of object to use for merging tokens</param>
        /// <returns></returns>
        private string GetProcessedCommand(long inputKey)
        {
            return "@echo off" + Environment.NewLine + TemplateTokenProcessor.ProcessTokens(Command, inputKey, false);
        }

        /// <summary>
        /// Run the task
        /// </summary>
        protected override void Run(List<long> inputKeys)
        {
            // Get the time that should be used for timing out the process
            DateTime timeoutDate = DateTime.Now.AddMinutes(CommandTimeoutInMinutes);

            foreach (long key in inputKeys)
            {
                RunCommand(key, timeoutDate);
            }
        }

        string PrefixLines(string prefix, string value)
        {
            if (null == value)
                return null;

            return Regex.Replace(value, @"(?m)^(.*)$", prefix + "$1");
        }

        /// <summary>
        /// Run the command for each object
        /// </summary>
        /// <param name="inputKey">Id of the object for which to run the command</param>
        /// <param name="timeoutDate">Date after which the command should be timed out</param>
        private void RunCommand(long inputKey, DateTime timeoutDate)
        {
            string executionCommand = GetProcessedCommand(inputKey);
            string commandFileName = Path.GetRandomFileName() + ".cmd";
            string commandPath = Path.Combine(GetTempPath(), commandFileName);
            string commandLogBasePath = GetNextCommandLogPathWithoutExtension();

            string commandLogPath = commandLogBasePath + ".command";
            log.InfoFormat("Command log path: \n{0}", commandLogPath);

            string outputLogPath = commandLogBasePath + ".output";
            log.InfoFormat("Output log path: {0}", outputLogPath);

            // Save the command text for both logging and execution
            File.WriteAllText(commandLogPath, executionCommand);
            File.WriteAllText(commandPath, executionCommand);

            try
            {
                using (var commandLogWriter = File.CreateText(outputLogPath))
                {
                    commandLogWriter.AutoFlush = true;

                    using (Process process = new Process())
                    {
                        process.StartInfo = new ProcessStartInfo
                        {
                            CreateNoWindow = true,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            RedirectStandardInput = true,
                            UseShellExecute = false,
                            WorkingDirectory = GetTempPath(),
                            FileName = commandPath
                        };

                        // Wire up the handlers that will take care of logging output and errors
                        process.OutputDataReceived += (s, e) => commandLogWriter.WriteLine(PrefixLines("O> ", e.Data));
                        process.ErrorDataReceived += (s, e) => commandLogWriter.WriteLine(PrefixLines("E> ", e.Data));

                        process.Start();

                        // Start reading the output and error streams
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.StandardInput.Close();

                        bool wasResponding = true;

                        do
                        {
                            if (!process.HasExited)
                            {
                                if (!process.Responding && wasResponding)
                                {
                                    log.Warn("Command is not responding");
                                }

                                wasResponding = process.Responding;

                                if (ShouldStopCommandOnTimeout && timeoutDate < DateTime.Now)
                                {
                                    KillProcessTree(process);
                                    throw new ActionTaskRunException(string.Format("The program took longer than {0} minute{1} to run.",
                                                                                   CommandTimeoutInMinutes, CommandTimeoutInMinutes > 1 ? "s" : ""));
                                }
                            }
                        } while (!process.WaitForExit(500));

                        // Wait for asynchronous output processing to complete
                        process.WaitForExit();

                        // Verify that the command completed without errors
                        if (process.ExitCode > 0)
                        {
                            log.ErrorFormat("The command exited with code {0}", process.ExitCode);
                            throw new ActionTaskRunException("The program failed. See the log for details.");
                        }
                    }
                }
            }
            finally
            {
                File.Delete(commandPath);
            }
        }

        /// <summary>
        /// Recursively kills a process and all of its children
        /// </summary>
        /// <param name="process">Process that should be killed</param>
        private static void KillProcessTree(Process process)
        {
            // Kill the process if it's still running
            if (!process.HasExited)
            {
                log.ErrorFormat("Killing process [{0}] {1}", process.Id, process.ProcessName);
                process.Kill();
            }

            // Kill the children of the process, if there are any
            foreach (Process child in GetChildren(process.Id))
            {
                KillProcessTree(child);
            }

            process.WaitForExit(10000);
        }

        /// <summary>
        /// Gets the children of the specified process
        /// </summary>
        /// <param name="processId">Id of the process for which to retrieve children</param>
        /// <returns></returns>
        public static IEnumerable<Process> GetChildren(int processId)
        {
            return Process.GetProcesses().Where(x => GetParentProcess(x.Id) == processId);
        }

        /// <summary>
        /// Gets the id of the parent process
        /// </summary>
        /// <param name="id">Id of the process whose parent we want to find</param>
        /// <returns></returns>
        private static int GetParentProcess(int id)
        {
            using (ManagementObject mo = new ManagementObject(string.Format("win32_process.handle='{0}'", id)))
            {
                try
                {
                    mo.Get();
                    return Convert.ToInt32(mo["ParentProcessId"]);
                }
                catch (ManagementException)
                {
                    return -1;
                }
            }
        }
    }
}
