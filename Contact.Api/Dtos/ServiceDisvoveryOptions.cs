﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Dtos
{
    public class ServiceDisvoveryOptions
    {
        public string UserServiceName { get; set; }

        public ConsulOptions Consul { get; set; }

    }
}
