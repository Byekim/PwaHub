﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Voice.Request
{

    public class RequestBroadcast : RequestBroadcastHistory
    {

        public string title { get; set; }
    }
}
