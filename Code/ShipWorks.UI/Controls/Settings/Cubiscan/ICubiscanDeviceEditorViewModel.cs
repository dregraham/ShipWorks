using System;
using System.Collections.Generic;
using System.Windows.Input;
using Interapptive.Shared.IO.Hardware;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    /// <summary>
    /// ViewModel for a CubiscanDeviceEditor View
    /// </summary>
    public interface ICubiscanDeviceEditorViewModel
    {
        /// <summary>
        /// Action to run when the editor is completed
        /// </summary>
        Action OnComplete { get; set; }
        
        /// <summary>
        /// The currently selected computer
        /// </summary>
        ComputerEntity SelectedComputer { get; set; }
        
        /// <summary>
        /// The currently selected model
        /// </summary>
        DeviceModel SelectedModel { get; set; }
        
        /// <summary>
        /// The IP address
        /// </summary>
        string IPAddress { get; set; }
        
        /// <summary>
        /// The port number
        /// </summary>
        string PortNumber { get; set; }
        
        /// <summary>
        /// List of computers connected to ShipWorks
        /// </summary>
        IEnumerable<ComputerEntity> Computers { get; set; }
        
        /// <summary>
        /// List of models currently supported by ShipWorks 
        /// </summary>
        Dictionary<DeviceModel, string> Models { get; set; }
        
        /// <summary>
        /// Command to cancel editing
        /// </summary>
        ICommand CancelCommand { get; }
        
        /// <summary>
        /// Command to save new device
        /// </summary>
        ICommand SaveCommand { get; }
        
        /// <summary>
        /// Whether or not to show the spinner
        /// </summary>
        bool ShowSpinner { get; set; }
    }
}