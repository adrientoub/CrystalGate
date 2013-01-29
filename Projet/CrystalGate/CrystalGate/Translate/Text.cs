using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalGate
{
    class Text
    {
        string text;

        public Text(string nameText)
        {
            text = GameText.getText(nameText);
        }

        public string get()
        {
            return text;
        }
    }
}
