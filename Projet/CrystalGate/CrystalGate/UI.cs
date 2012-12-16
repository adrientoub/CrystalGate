using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CrystalGate
{
    class UI
    {
        public Texture2D Bas { get; set; }
        public Rectangle BasPosition { get; set; }

        SpriteBatch spritebatch { get; set; }
        SpriteFont gamefont { get; set; }

        public UI(Texture2D bas, SpriteBatch sp, SpriteFont gf)
        {
            Bas = bas;
            BasPosition = new Rectangle(0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Bas.Height, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, Bas.Height);
            spritebatch = sp;
            gamefont = gf;
        }

        public void Draw()
        {
            string str = "Ici, faudrait une interface a la Diablo, ceci est juste un exemple";
            spritebatch.Draw(Bas, BasPosition, Color.White);
            spritebatch.DrawString(gamefont, str, new Vector2(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 - gamefont.MeasureString(str).X / 2, BasPosition.Y + Bas.Height / 2), Color.White); 
        }
    }
}
