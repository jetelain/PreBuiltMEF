﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryA;

namespace PreBuiltMEFDemo.Samples
{
    [Export]
    internal class PartD : GenericExtPartBase<string>
    {

    }
}
