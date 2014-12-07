using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone.MVVM.Tombstone
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TombstoneAttribute : Attribute
    {
        public Type[] KnownTypes { get; set; }

        public TombstoneAttribute()
        {

        }

        public TombstoneAttribute(params Type[] knownTypes)
        {
            KnownTypes = knownTypes;
        }
    }
}
