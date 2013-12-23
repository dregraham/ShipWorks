using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Interapptive.Shared.Data
{
    public class DatabaseVersionListAdapter
    {



        public string SerializeToDictionary(Dictionary<string, List<string>> deserailizedVersion)
        {
            return JsonConvert.SerializeObject(deserailizedVersion);
        }

    }
}
