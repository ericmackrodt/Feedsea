using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace feedsea.Models
{
    public class ShareButtonModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public ICommand ShareCommand { get; set; }
    }
}
