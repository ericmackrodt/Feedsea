using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{
    public enum MessageType
    {
        Alert,
        Error
    }

    public class ValidationMessage
    {
        public event EventHandler IsValidChanged;
        public event EventHandler MessageChanged;

        private bool isValid;
        private string message;
        
        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (isValid != value)
                {
                    isValid = value;
                    if (IsValidChanged != null)
                        IsValidChanged(this, new EventArgs());
                }
            }
        }
        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    if (MessageChanged != null)
                        MessageChanged(this, new EventArgs());
                }
            }
        }
        public MessageType Type { get; set; }

        public ValidationMessage()
        {
            isValid = true;
        }

        public void Set(string message)
        {
            Set(message, MessageType.Error);
        }

        public void Set(string message, MessageType type)
        {
            Type = type;
            Message = message;
            IsValid = false;
        }

        public void Clear()
        {
            IsValid = true;
            this.message = "";
        }
    }
}
