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
        // Images de l'UI
        public Texture2D BarreDesSorts { get; set; } 
        public Texture2D Curseur { get; set; }
        public Texture2D Portrait { get; set; }
        public Texture2D Sac { get; set; }
        public Texture2D blank{ get; set; }
        // Position des images sur l'ecran
        public Rectangle CadrePosition { get; set; }
        public Rectangle BarreDesSortsPosition { get; set; }
        public Rectangle PortraitPosition { get; set; }
        public Rectangle SacPosition { get; set; }

        public bool DrawSelectPoint;
        public bool DrawSac;
        public Vector2 TailleSac = new Vector2(8, 8);
        SpriteBatch spritebatch { get; set; }
        SpriteFont gamefont { get; set; }

        public bool Win;
        public bool Lost;

        public Joueur joueur { get; set; }

        public int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        public int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        public UI(Joueur joueur, Texture2D barreDesSorts, Texture2D curseur, Texture2D portrait, Texture2D sac, Texture2D blank, SpriteBatch sp, SpriteFont gf)
        {
            this.joueur = joueur;
            Portrait = portrait;
            Sac = sac;
            BarreDesSorts = barreDesSorts;
            Curseur = curseur;
            spritebatch = sp;
            gamefont = gf;
            this.blank = blank;
        }

        public void Update()
        {
            int widthFondNoir = 350, heightFondNoir = 180;

            CadrePosition = new Rectangle((int)joueur.camera.Position.X, (int)joueur.camera.Position.Y + height - heightFondNoir, widthFondNoir, heightFondNoir);
            PortraitPosition = new Rectangle(CadrePosition.X + CadrePosition.Width - Portrait.Width, CadrePosition.Y + 50, Portrait.Width, Portrait.Height);
            SacPosition = new Rectangle((int)joueur.camera.Position.X + width - Sac.Width, (int)joueur.camera.Position.Y + height - Sac.Height, Sac.Width, Sac.Height);
            BarreDesSortsPosition = new Rectangle((int)(joueur.camera.Position.X + width / 1.5), (int)(joueur.camera.Position.Y + height - BarreDesSorts.Height), BarreDesSorts.Width, BarreDesSorts.Height);
        }

        public void Draw()
        {
            MouseState m = Mouse.GetState();
            Text life = new Text("Life"), attack = new Text("Attack"), armor = new Text("Armor"), selectPoint = new Text("SelectPoint"), manaText = new Text("Mana"); // définition des mots traduisibles

            string str = life.get() + " : " + joueur.champion.Vie + " / " + joueur.champion.VieMax + "\n"
                + manaText.get() + " : " + joueur.champion.Mana + " / " + joueur.champion.ManaMax + "\n"
                + attack.get() + " : " + joueur.champion.Dommages + "\n"
                + armor.get() + " : " + joueur.champion.Defense + "\n";
            string str2 = selectPoint.get();

            spritebatch.Draw(BarreDesSorts, BarreDesSortsPosition, null, Color.White, 0, new Vector2(BarreDesSorts.Width / 2, 0), SpriteEffects.None, 1);
            
            // Fond bas gauche noir
            spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X, (int)joueur.camera.Position.Y + height - CadrePosition.Height, CadrePosition.Width, CadrePosition.Height), Color.Black);
            
            // Portrait et strings
            spritebatch.Draw(Portrait, PortraitPosition, Color.White);
            spritebatch.DrawString(gamefont, str, new Vector2(CadrePosition.X, CadrePosition.Y + 25), Color.White);
            string strUnit = joueur.champion.ToString().Split(new char[1] { '.' })[1];
            spritebatch.DrawString(gamefont, strUnit, new Vector2(CadrePosition.X + CadrePosition.Width - gamefont.MeasureString(strUnit).X / 2 - Portrait.Width / 2, CadrePosition.Y), Color.White);
            // Sac
            if (DrawSac)
            {
                spritebatch.Draw(Sac, SacPosition, Color.White);
                // Affichage des items
                int x = 0;
                int j = 0;
                Vector2 marge = new Vector2(8, 8); // marge de l'inventaire
                foreach (Item it in joueur.champion.Inventory)
                {
                    if(!it.Disabled)
                        spritebatch.Draw(it.Icone, new Vector2(SacPosition.X, SacPosition.Y) + marge + new Vector2((32 + 7) * x, (32 + 7) * j), Color.White);
                    x++;
                    if (x >= TailleSac.X)
                    {
                        x = 0;
                        j++;
                    }
                }
            }
            // Affichage de la victoire ou de la défaite
            if(Win)
                spritebatch.DrawString(gamefont, "Victoire!", new Vector2(joueur.camera.Position.X + width / 2 - gamefont.MeasureString("Victoire").X / 2, joueur.camera.Position.Y + height / 2 - gamefont.MeasureString("Victoire").Y / 2), Color.Black);
            if (Lost)
                spritebatch.DrawString(gamefont, "Defaite!", new Vector2(joueur.camera.Position.X + width / 2 - gamefont.MeasureString("Defaite").X / 2, joueur.camera.Position.Y + height / 2 - gamefont.MeasureString("Defaite").Y / 2), Color.Black);
            if(DrawSelectPoint)
                spritebatch.DrawString(gamefont, str2, new Vector2(BarreDesSortsPosition.X - gamefont.MeasureString(str2).X / 2, BarreDesSortsPosition.Y - BarreDesSorts.Height), Color.White);
            
            // Affichage des spells
            for(int i = 0; i < joueur.champion.spells.Count;i++)
            {
                if (joueur.champion.Map.gametime != null)
                {
                    Color color;
                    if (joueur.champion.IsCastable(i))
                        color = Color.White;
                    else
                        color = Color.Red;
                    spritebatch.Draw(joueur.champion.spells[i].SpriteBouton, new Vector2(BarreDesSortsPosition.X - 130 + i * (32 + 5), BarreDesSortsPosition.Y + 8), color);
                }

            }
            spritebatch.Draw(Curseur, new Vector2(joueur.camera.Position.X + m.X, joueur.camera.Position.Y + m.Y), Color.White);
        }
    }
}
