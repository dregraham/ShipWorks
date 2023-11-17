using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Ups.ShipEngine
{
    [Component(RegistrationType.Self, SingleInstance = true)]
    public class UpsGroundSaverState
    {
        public bool IsGroundSaverEnabled { get; set; }
    }
}
