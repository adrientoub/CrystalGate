using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalGate
{
            [Serializable]
    public class Text
    {
        string text;
        bool showPlain;
        string plainText;
        TimeSpan lastGet;

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
                    lastGet = EffetSonore.time.Elapsed;
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
