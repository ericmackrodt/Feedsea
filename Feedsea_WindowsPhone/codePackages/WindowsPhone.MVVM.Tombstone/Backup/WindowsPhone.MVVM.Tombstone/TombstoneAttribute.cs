using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone.MVVM.Tombstone
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TombstoneAttribute : Attribute
    {

    }
}
