using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Crashes;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Displayed when a background operation is preventing a user from continuing with a task.
    /// </summary>
    public partial class ApplicationBusyDlg : Form
    {
        string operationsFormat;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationBusyDlg(string goal)
        {
            InitializeComponent();

            operationsFormat = labelOperation.Text;
            labelGoal.Text = string.Format(labelGoal.Text, goal);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            CheckOperationStatus();

            timer.Start();
        }

        /// <summary>
        /// Check to see if the operations are done
        /// </summary>
        private void OnTimer(object sender, EventArgs e)
        {
            CheckOperationStatus();
        }

        /// <summary>
        /// Check to see if the operation is complete.
        /// </summary>
        private void CheckOperationStatus()
        {
            List<string> operations = ApplicationBusyManager.GetActiveOperations();

            if (operations.Count == 0)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            if (CrashDialog.IsApplicationCrashed)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            labelOperation.Text = string.Format(operationsFormat, GetOperationsText(operations));
        }

        /// <summary>
        /// Get the text to display for the active operations.
        /// </summary>
        private string GetOperationsText(List<string> operations)
        {
            if (operations.Count == 0)
            {
                throw new ArgumentException("operations cannot be empty");
            }

            else if (operations.Count == 1)
            {
                return operations[0];
            }

            else if (operations.Count == 2)
            {
                return string.Format("{0} and {1}", operations[0], operations[1]);
            }

            else
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < operations.Count - 1; i++)
                {
                    sb.AppendFormat("{0}, ", operations[i]);
                }

                sb.AppendFormat("and {0}", operations[operations.Count - 1]);

                return sb.ToString();
            }
        }
    }
}