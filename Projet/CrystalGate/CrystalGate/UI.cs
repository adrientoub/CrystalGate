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
        public Texture2D Cadre { get; set; }
        public Texture2D BarreDesSorts { get; set; }
        public Texture2D Curseur { get; set; }
        public Texture2D Portrait { get; set; }

        public Rectangle CadrePosition { get; set; }
        public Rectangle BarreDesSortsPosition { get; set; }
        public Rectangle PortraitPosition { get; set; }

        public bool DrawSelectPoint;
        public Joueur joueur { get; set; }
        SpriteBatch spritebatch { get; set; }
        SpriteFont gamefont { get; set; }

        int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        public UI(Joueur joueur, Texture2D cadre, Texture2D barreDesSorts, Texture2D curseur, Texture2D portrait, SpriteBatch sp, SpriteFont gf)
        {
            Cadre = cadre;
            this.joueur = joueur;
            Portrait = portrait;
            BarreDesSorts = barreDesSorts;
            Curseur = curseur;
            spritebatch = sp;
            gamefont = gf;
        }

        public void Update()
        {
            CadrePosition = new Rectangle((int)joueur.camera.Position.X, (int)joueur.camera.Position.Y + height - Cadre.Height, Cadre.Width, Cadre.Height);
            PortraitPosition = new Rectangle(CadrePosition.X, CadrePosition.Y, Portrait.Width / 2, Portrait.Height / 2);
            BarreDesSortsPosition = new Rectangle((int)(joueur.camera.Position.X + width / 2), (int)(joueur.camera.Position.Y + height - BarreDesSorts.Height), BarreDesSorts.Width, BarreDesSorts.Height);
        }

        public void Draw()
        {
            MouseState m = Mouse.GetState();

            string str = "Vie : " + joueur.champion.Vie + " / " + joueur.champion.VieMax + "\n"
                + "Attaque :" + joueur.champion.Dommages + "\n"
                + "Defense :" + joueur.champion.Defense + "\n";
            string str2 = "Selectionnez un point";

            //spritebatch.Draw(Cadre, CadrePosition, Color.White);
            //spritebatch.Draw(Portrait, PortraitPosition, Color.White);
            spritebatch.Draw(BarreDesSorts, BarreDesSortsPosition, null, Color.White, 0, new Vector2(BarreDesSorts.Width / 2, 0), SpriteEffects.None, 1); 
            
            spritebatch.DrawString(gamefont, str, new Vector2(CadrePosition.X, CadrePosition.Y + CadrePosition.Height / 2), Color.White);
            if(DrawSelectPoint)
                spritebatch.DrawString(gamefont, str2, new Vector2(BarreDesSortsPosition.X - gamefont.MeasureString(str2).X / 2, BarreDesSortsPosition.Y - BarreDesSorts.Height), Color.White);

            foreach (Spell s in joueur.champion.spells)
            {
                spritebatch.Draw(s.SpriteBouton, new Vector2(BarreDesSortsPosition.X - 128, BarreDesSortsPosition.Y + 8), Color.White);
            }
            spritebatch.Draw(Curseur, new Vector2(joueur.camera.Position.X + m.X, joueur.camera.Position.Y + m.Y), Color.White);
        }
    }
}
