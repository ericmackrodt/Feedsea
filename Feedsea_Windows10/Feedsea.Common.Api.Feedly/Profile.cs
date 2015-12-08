using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Api.Feedly
{
    [DataContract]
    public class Profile
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "wave")]
        public string Wave { get; set; }

        [DataMember(Name = "picture")]
        public string Picture { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "fullName")]
        public string FullName { get; set; }

        [DataMember(Name = "twitterConnected")]
        public bool TwitterConnected { get; set; }

        [DataMember(Name = "dropboxConnected")]
        public bool DropboxConnected { get; set; }

        [DataMember(Name = "windowsLiveConnected")]
        public bool WindowsLiveConnected { get; set; }

        [DataMember(Name = "facebookConnected")]
        public bool FacebookConnected { get; set; }

        [DataMember(Name = "evernoteConnected")]
        public bool EvernoteConnected { get; set; }

        [DataMember(Name = "pocketConnected")]
        public bool PocketConnected { get; set; }

        [DataMember(Name = "googleConnected")]
        public bool GoogleConnected { get; set; }        
        
        [DataMember(Name = "locale")]
        public string Locale { get; set; }
        
        [DataMember(Name = "google")]
        public string Google { get; set; }

        [DataMember(Name = "twitter")]
        public string Twitter { get; set; }

        [DataMember(Name = "windowsLiveId")]
        public string WindowsLiveID { get; set; }
    }
}
