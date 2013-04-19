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
        public Texture2D CurseurRouge { get; set; }
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
        SpriteFont spellfont { get; set; }

        public bool Win;
        public bool Lost;
        public bool CurseurOffensif;

        string tempsDeJeuActuel, compteurDeVague;
        int nombreDeVagues = 8;

        public Joueur joueur { get; set; }

        public int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        public int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        public UI(Joueur joueur, Texture2D barreDesSorts, Texture2D curseur, Texture2D curseurRouge, Texture2D portrait, Texture2D sac, Texture2D blank, SpriteBatch sp, SpriteFont gf, SpriteFont sf)
        {
            this.joueur = joueur;
            Portrait = portrait;
            Sac = sac;
            BarreDesSorts = barreDesSorts;
            Curseur = curseur;
            CurseurRouge = curseurRouge;
            spritebatch = sp;
            gamefont = gf;
            spellfont = sf;
            tempsDeJeuActuel = "0:00";
            compteurDeVague = "0/" + nombreDeVagues.ToString();
            this.blank = blank;

            // evite le crash de debut en initialisant la var des la premiere frame
            SacPosition = new Rectangle((int)joueur.camera.Position.X + width - Sac.Width, (int)joueur.camera.Position.Y + height - Sac.Height, Sac.Width, Sac.Height);
        }

        public void Update()
        {
            int widthFondNoir = 380, heightFondNoir = 250;

            CadrePosition = new Rectangle((int)joueur.camera.Position.X, (int)joueur.camera.Position.Y + height - heightFondNoir, widthFondNoir, heightFondNoir);
            PortraitPosition = new Rectangle(CadrePosition.X + CadrePosition.Width - Portrait.Width, CadrePosition.Y + 50, Portrait.Width, Portrait.Height);
            SacPosition = new Rectangle((int)joueur.camera.Position.X + width - Sac.Width, (int)joueur.camera.Position.Y + height - Sac.Height, Sac.Width, Sac.Height);
            BarreDesSortsPosition = new Rectangle((int)(joueur.camera.Position.X + width / 1.5), (int)(joueur.camera.Position.Y + height - BarreDesSorts.Height), BarreDesSorts.Width, BarreDesSorts.Height);
            if (SceneEngine2.GamePlay.timer.Elapsed.Seconds < 10)
                tempsDeJeuActuel = SceneEngine2.GamePlay.timer.Elapsed.Minutes.ToString() + ":0" + SceneEngine2.GamePlay.timer.Elapsed.Seconds.ToString();
            else
                tempsDeJeuActuel = SceneEngine2.GamePlay.timer.Elapsed.Minutes.ToString() + ":" + SceneEngine2.GamePlay.timer.Elapsed.Seconds.ToString();

            compteurDeVague = Wave.waveNumber.ToString() + "/" + nombreDeVagues.ToString();
        }

        public void Draw()
        {
            int hauteurBarre = 30;

            MouseState m = SceneEngine2.BaseScene.mouse;
            // Affichage des textes
            Text life = new Text("Life"), attack = new Text("Attack"), armor = new Text("Armor"), selectPoint = new Text("SelectPoint"), manaText = new Text("Mana"), levelText = new Text("Level"); // définition des mots traduisibles

            string str = " " + life.get() + " : " + joueur.champion.Vie + " / " + joueur.champion.VieMax + "\n "
                + manaText.get() + " : " + joueur.champion.Mana + " / " + joueur.champion.ManaMax + "\n "
                + attack.get() + " : " + joueur.champion.Dommages + "\n "
                + armor.get() + " : " + joueur.champion.Defense + " / " + joueur.champion.DefenseMagique + "\n "
                + levelText.get() + " : " + joueur.champion.Level + "\n "
                ;
            string str2 = selectPoint.get();

            spritebatch.Draw(BarreDesSorts, BarreDesSortsPosition, null, Color.White, 0, new Vector2(BarreDesSorts.Width / 2, 0), SpriteEffects.None, 1);
            
            // Fond bas gauche noir
            spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X, (int)joueur.camera.Position.Y + height - CadrePosition.Height, CadrePosition.Width, CadrePosition.Height), Color.Black);

            // Portrait
            spritebatch.Draw(Portrait, PortraitPosition, Color.White);

            int margeGaucheVie = 75, margeGaucheMana = 100, longueurBarre = 150;
            // Affichage de la barre de vie
            spritebatch.Draw(blank, new Rectangle(CadrePosition.X + margeGaucheVie, CadrePosition.Y + CadrePosition.Height - (int)gamefont.MeasureString(str).Y, (int)(((float)joueur.champion.Vie / (float)(joueur.champion.VieMax)) * longueurBarre), hauteurBarre), Color.Green);
            // Affichage de la barre de mana
            spritebatch.Draw(blank, new Rectangle(CadrePosition.X + margeGaucheMana, CadrePosition.Y + CadrePosition.Height - (int)gamefont.MeasureString(str).Y + (int)gamefont.MeasureString("char").Y, (int)(((float)joueur.champion.Mana / (float)(joueur.champion.ManaMax)) * longueurBarre), hauteurBarre), new Color(0,0,178));
            // Affichage de la barre d'XP
            int xpToDraw = (int)(((float)joueur.champion.XP / (float)(joueur.champion.Level * 1000)) * CadrePosition.Width);
            if (xpToDraw != 0)
                spritebatch.Draw(blank, new Rectangle(CadrePosition.X, CadrePosition.Y + CadrePosition.Height - hauteurBarre, xpToDraw, hauteurBarre), Color.IndianRed);
            else
                spritebatch.Draw(blank, new Rectangle(CadrePosition.X, CadrePosition.Y + CadrePosition.Height - hauteurBarre, 1, hauteurBarre), Color.IndianRed);
            
            // Affichage de l'aide des sorts
            for (int i = 0; i < joueur.champion.spells.Count; i++)
            {
                if (joueur.SourisHoverCheck(i))
                {
                    int widthCadre = 250;
                    int heightCadre = 100;
                    string nomDuSort = new Text(joueur.champion.spells[i].ToString()).get();
                    // Le cadre noir, le nom du sort, la description
                    spritebatch.Draw(blank, new Rectangle(BarreDesSortsPosition.X - widthCadre / 2, BarreDesSortsPosition.Y - 100, widthCadre, heightCadre), Color.Black);
                    spritebatch.DrawString(spellfont, Outil.Normalize(joueur.champion.spells[i].DescriptionSpell()), new Vector2(BarreDesSortsPosition.X - widthCadre / 2, BarreDesSortsPosition.Y - 120 + 25), Color.White);
                    spritebatch.DrawString(spellfont, nomDuSort, new Vector2(BarreDesSortsPosition.X - spellfont.MeasureString(nomDuSort).X / 2, BarreDesSortsPosition.Y - 120), Color.White);
                }
            }

            // Affichage du texte
            spritebatch.DrawString(gamefont, str, new Vector2(CadrePosition.X, CadrePosition.Y + 25), Color.White);
            string xp = joueur.champion.XP + " / " + joueur.champion.Level * 1000;
            spritebatch.DrawString(gamefont, xp, new Vector2(CadrePosition.X + CadrePosition.Width / 2 - gamefont.MeasureString(xp).X / 2, CadrePosition.Y + CadrePosition.Height - hauteurBarre - 2), Color.White);

            // Nom du personnage
            Text strUnit = new Text(joueur.champion.ToString().Split(new char[1] { '.' })[1]);
            spritebatch.DrawString(gamefont, strUnit.get(), new Vector2(CadrePosition.X + CadrePosition.Width - gamefont.MeasureString(strUnit.get()).X / 2 - Portrait.Width / 2, CadrePosition.Y), Color.White);
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
            Text Victoire = new Text("Win"), Defaite = new Text("Lose");
            // Affichage de la victoire ou de la défaite
            if (Win)
                spritebatch.DrawString(gamefont, Victoire.get(), new Vector2(joueur.camera.Position.X + width / 2 - gamefont.MeasureString(Victoire.get()).X / 2, joueur.camera.Position.Y + height / 2 - gamefont.MeasureString(Defaite.get()).Y / 2), Color.Black);
            if (Lost)
                spritebatch.DrawString(gamefont, Defaite.get(), new Vector2(joueur.camera.Position.X + width / 2 - gamefont.MeasureString(Defaite.get()).X / 2, joueur.camera.Position.Y + height / 2 - gamefont.MeasureString(Defaite.get()).Y / 2), Color.Black);
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
            // Timer
            spritebatch.DrawString(gamefont, tempsDeJeuActuel, new Vector2(joueur.camera.Position.X + width - gamefont.MeasureString(tempsDeJeuActuel).X - 5, joueur.camera.Position.Y + 4), Color.Black);

            // Compteur de vagues
            spritebatch.DrawString(gamefont, compteurDeVague, new Vector2(joueur.camera.Position.X + width - gamefont.MeasureString(compteurDeVague).X - 5, gamefont.MeasureString(tempsDeJeuActuel).Y + joueur.camera.Position.Y + 4), Color.Black);

            // Curseur
            spritebatch.Draw(CurseurOffensif ? CurseurRouge : Curseur, new Vector2(joueur.camera.Position.X + m.X, joueur.camera.Position.Y + m.Y), Color.White);
        }
    }
}
