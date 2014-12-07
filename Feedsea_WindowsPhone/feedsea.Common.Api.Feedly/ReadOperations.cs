using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api.Feedly
{
    [DataContract]
    public class ReadOperations
    {
        [DataMember(Name = "entries")]
        public string[] Entries { get; set; }
        [DataMember(Name = "feeds")]
        public ReadOperationsFeed[] Feeds { get; set; }    
    }

    public class ReadOperationsFeed 
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "asOf")]
        public long AsOf { get; set; }
    }
}
