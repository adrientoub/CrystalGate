using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalGate
{
    public class Text
    {
        string text;
        bool showPlain;

        public Text(string nameText)
        {
            text = nameText;
            showPlain = false;
        }

        public Text(string nameText, bool isPlainString)
        {
            text = nameText;
            showPlain = isPlainString;
        }

        public Text()
        {
            text = "";
            showPlain = false;
        }

        public string get()
        {
            if (!showPlain)
                return GameText.getText(text);
            else
                return text;
        }

        public override string ToString()
        {
            if (!showPlain)
                return GameText.getText(text);
            else
                return text;
        }
    }
}
