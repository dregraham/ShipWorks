/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 4/9/2019
 * Time: 3:10 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace OrderCreation
{
    /// <summary>
    /// Description of Variables.
    /// </summary>
    [TestModule("5D5DDD69-8E8F-4A62-A4BB-CBFF725F054E", ModuleType.UserCode, 1)]
       public static class Variables
    {
        // You can use the "Insert New User Code Method" functionality from the context menu,
        // to add a new method with the attribute [UserCodeMethod].
	
		public static int NumberOfLoops = 1;
	}
}