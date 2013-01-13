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
        public Texture2D Portrait { get; set; }
        public Rectangle BasPosition { get; set; }
        public Rectangle PortraitPosition { get; set; }

        public Joueur joueur { get; set; }
        SpriteBatch spritebatch { get; set; }
        SpriteFont gamefont { get; set; }

        public UI(Joueur joueur, Texture2D bas, Texture2D curseur, Texture2D portrait, SpriteBatch sp, SpriteFont gf)
        {
            this.joueur = joueur;
            this.Portrait = portrait;
            Bas = bas;
            Curseur = curseur;
            BasPosition = new Rectangle(0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Bas.Height, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, Bas.Height);
            PortraitPosition = new Rectangle((int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 3.7f), System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Portrait.Height / 2, (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 10.2), (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 7.8));
            spritebatch = sp;
            gamefont = gf;
        }

        public void Update()
        {
            PortraitPosition = new Rectangle((int)(joueur.camera.Position.X + System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 3.7f), (int)joueur.camera.Position.Y + System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - Portrait.Height / 2, (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 10.2), (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 7.8));
        }

        public void Draw()
        {
            MouseState m = Mouse.GetState();

            string str = "Vie : " + joueur.champion.Vie + " / " + joueur.champion.VieMax + "\n"
                + "Attaque :" + joueur.champion.Dommages + "\n"
                + "Defense :" + joueur.champion.Defense + "\n";
            spritebatch.Draw(Bas, BasPosition, Color.White);
            spritebatch.Draw(Portrait, PortraitPosition, Color.White);
            spritebatch.DrawString(gamefont, str, new Vector2(BasPosition.X + System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 - gamefont.MeasureString(str).X / 2, BasPosition.Y + Bas.Height / 2), Color.White);
            spritebatch.Draw(Curseur, new Vector2(joueur.camera.Position.X + m.X, joueur.camera.Position.Y + m.Y), Color.White);
        }
    }
}
