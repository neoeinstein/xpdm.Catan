using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core
{
    enum DieResult
    {
        Two = 1,
        Three = 2,
        Four = 4,
        Five = 8,
        Six = 16,
        Seven = 32,
        Eight = 64,
        Nine = 128,
        Ten = 256,
        Eleven = 512,
        Twelve = 1024,
        Any = 2047,
        AnyButSeven = 2015,
    }
}
