using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone.MVVM.Tombstone
{
    class TombstoneException : Exception
    {
        public TombstoneException(string message) : base(message) { }
    }
}
