using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class UI
    {
        public Texture2D Bas { get; set; }
        public Texture2D Curseur { get; set; }
        public Rectangle BasPosition { get; set; }

        public Joueur joueur { get; set; }
        SpriteBatch spritebatch { get; set; }
        SpriteFont gamefont { get; set; }

        public UI(Joueur joueur, Texture2D bas, Texture2D curseur, SpriteBatch sp, SpriteFont gf)
        {
            this.joueur = joueur;
            Bas = bas;
            Curseur = curseur;
            BasPosition = new Rectangle(0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Bas.Height, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, Bas.Height);
            spritebatch = sp;
            gamefont = gf;
        }

        public void Update()
        {

        }

        public void Draw()
        {
            MouseState m = Mouse.GetState();
            spritebatch.Draw(Curseur, new Vector2(joueur.camera.Position.X + m.X, joueur.camera.Position.Y + m.Y), Color.White);
            string str = "Ici, faudrait une interface a la Diablo, ceci est juste un exemple";
            spritebatch.Draw(Bas, BasPosition, Color.White);
            spritebatch.DrawString(gamefont, str, new Vector2(BasPosition.X + System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 - gamefont.MeasureString(str).X / 2, BasPosition.Y + Bas.Height / 2), Color.White); 
        }
    }
}
