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
            text = nameText;
        }

        public string get()
        {
            return GameText.getText(text);
        }

        public override string ToString()
        {
            return GameText.getText(text);
        }
    }
}
