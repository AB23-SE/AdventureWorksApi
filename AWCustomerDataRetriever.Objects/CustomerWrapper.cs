﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWCustomerDataRetriever.Objects
{
    public class CustomerWrapper
    {
        public int TotalRecords { get; set; }
        public Customer[] Data { get; set; }
    }
}
