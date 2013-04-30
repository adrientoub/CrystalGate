using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalGateEditor
{
    public class Text
    {
        string text;
        bool showPlain;
        string plainText;
        TimeSpan lastGet;
        public static System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();

        public Text(string nameText)
        {
            text = nameText;
            showPlain = false;
            lastGet = new TimeSpan();
        }

        public Text(string nameText, bool isPlainString)
        {
            text = nameText;
            showPlain = isPlainString;
            if (showPlain)
                plainText = text;
            lastGet = new TimeSpan();
        }

        public Text()
        {
            text = "";
            showPlain = false;
            lastGet = new TimeSpan();
        }

        public string get()
        {
            if (!showPlain)
            {
                if (GameText.lastReinit >= lastGet)
                {
                    plainText = GameText.getText(text);
                    lastGet = Text.time.Elapsed;
                }
            }
            return plainText;
        }

        public override string ToString()
        {
            return get();
        }
    }
}
