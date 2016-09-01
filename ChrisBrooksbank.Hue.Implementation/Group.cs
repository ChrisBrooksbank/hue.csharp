using System;
using System.Collections.Generic;
using ChrisBrooksbank.Hue.Interfaces;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class Group : IGroup
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
