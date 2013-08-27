﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Templates.Tokens;
using log4net;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for running a command
    /// </summary>
    [ActionTask("Run a command", "RunCommand", ActionTaskCategory.External)]
    public class RunCommandTask : ActionTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RunCommandTask));

        private static string commandLogFolder;
        private static int nextRunNumber = 0;

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
        /// This task can use input
        /// </summary>
        public override ActionTaskInputRequirement RequiresInput
        {
            get { return ActionTaskInputRequirement.Optional; }
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
        /// Gets the command log folder.
        /// </summary>
        private static string CommandLogFolder
        {
            get
            {
                if (commandLogFolder == null)
                {
                    ActionTaskDescriptor descriptor = ActionTaskManager.GetDescriptor(typeof(RunCommandTask));
                    commandLogFolder = Path.Combine(LogSession.LogFolder, descriptor.Identifier);

                    Directory.CreateDirectory(commandLogFolder);
                }

                return commandLogFolder;
            }
        }

        /// <summary>
        /// Gets the next command log path without extension.
        /// </summary>
        /// <returns>The path of the next log file to be written.</returns>
        private static string GetNextCommandLogPathWithoutExtension()
        {
            string fileName = string.Format("{0:0000}", Interlocked.Increment(ref nextRunNumber));
            return Path.Combine(CommandLogFolder, fileName);
        }

        /// <summary>
        /// Gets the command that should be executed
        /// </summary>
        /// <param name="inputKey">Id of object to use for merging tokens</param>
        /// <returns></returns>
        private string GetProcessedCommand(long inputKey)
        {
            return TurnOffEcho(TemplateTokenProcessor.ProcessTokens(Command, inputKey, false));
        }

        /// <summary>
        /// Gets the command with echo turned off
        /// </summary>
        /// <param name="command">Command to suppress echo</param>
        /// <returns></returns>
        private static string TurnOffEcho(string command)
        {
            return "@echo off" + Environment.NewLine + command;
        }

        /// <summary>
        /// Run the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            // Get the time that should be used for timing out the process
            DateTime timeoutDate = DateTime.Now.AddMinutes(CommandTimeoutInMinutes);

            if ((ActionTaskInputSource) context.Step.InputSource == ActionTaskInputSource.Nothing)
            {
                RunCommand(TurnOffEcho(Command), timeoutDate);
            }
            else
            {
                foreach (long key in inputKeys)
                {
                    RunCommand(GetProcessedCommand(key), timeoutDate);
                }   
            }
        }

        /// <summary>
        /// Inserts the given prefix to the value provided.
        /// </summary>
        private static string PrefixLines(string prefix, string value)
        {
            if (value == null)
            {
                return null;
            }

            return Regex.Replace(value, @"(?m)^(.*)$", prefix + "$1");
        }

        /// <summary>
        /// Run the command for each object
        /// </summary>
        /// <param name="executionCommand">Command that should be executed</param>
        /// <param name="timeoutDate">Date after which the command should be timed out</param>
        private void RunCommand(string executionCommand, DateTime timeoutDate)
        {
            string commandFileName = Path.GetRandomFileName() + ".cmd";
            string commandPath = Path.Combine(GetTempPath(), commandFileName);
            string commandLogBasePath = GetNextCommandLogPathWithoutExtension();

            string commandLogPath = commandLogBasePath + ".command.log";
            log.InfoFormat("Command log path: \n{0}", commandLogPath);

            string outputLogPath = commandLogBasePath + ".output.log";
            log.InfoFormat("Output log path: {0}", outputLogPath);

            // Save the command text for both logging and execution
            File.WriteAllText(commandLogPath, executionCommand);
            File.WriteAllText(commandPath, executionCommand);

            try
            {
                using (StreamWriter commandLogWriter = File.CreateText(outputLogPath))
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