using System;
using System.Collections.Generic;
using System.Text;

namespace RedAssistance.Models
{
    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
