using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Common.Threading
{
    // Used to invoke functions with single parameters and no return value
    delegate void MethodInvoker<T>(T param1);

    // Used to invoke functions with two parameters and no return value
    delegate void MethodInvoker<T1, T2>(T1 param1, T2 param2);

    // Used to invoke functions with three parameters and no return value
    delegate void MethodInvoker<T1, T2, T3>(T1 param1, T2 param2, T3 param3);

    // Used to invoke functions with four parameters and no return value
    delegate void MethodInvoker<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4);

    // Used to invoke functions with five parameters and no return value
    delegate void MethodInvoker<T1, T2, T3, T4, T5>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
}
