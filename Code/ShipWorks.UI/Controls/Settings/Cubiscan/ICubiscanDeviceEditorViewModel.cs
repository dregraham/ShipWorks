using System;
using System.Collections.Generic;
using System.Windows.Input;
using Interapptive.Shared.IO.Hardware;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    public interface ICubiscanDeviceEditorViewModel
    {
        Action OnComplete { get; set; }
        ComputerEntity SelectedComputer { get; set; }
        DeviceModel SelectedModel { get; set; }
        string IPAddress { get; set; }
        string PortNumber { get; set; }
        IEnumerable<ComputerEntity> Computers { get; set; }
        Dictionary<DeviceModel, string> Models { get; set; }
        ICommand CancelCommand { get; }
        ICommand SaveCommand { get; }
    }
}