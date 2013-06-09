using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalGate.Reseau
{
    class Message
    {
        public TimeSpan dateEnvoi;
        public string message;

        public Message()
        {
            message = "";
            dateEnvoi = TimeSpan.Zero;
        }

        public Message(TimeSpan dateEnvoi, string message)
        {
            this.dateEnvoi = dateEnvoi;
            this.message = message;
        }
    }
}
