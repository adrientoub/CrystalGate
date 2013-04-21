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
        public Texture2D BarreDesSorts;
        public Texture2D Curseur;
        public Texture2D CurseurRouge;
        public Texture2D Portrait;
        public Texture2D Sac;
        public Texture2D Equipement;
        public Texture2D blank;
        
        // Position des images sur l'ecran
        public Rectangle CadrePosition;
        public Rectangle BarreDesSortsPosition;
        public Rectangle PortraitPosition;
        public Rectangle SacPosition;
        public Rectangle EquipementPosition;

        // Position des pièces d'équipement
        const int decalage = 44;
        public Vector2 CasquePosition = new Vector2(35,25); // 42
        public Vector2 EpaulieresPosition = new Vector2(35, 25 + decalage * 1);
        public Vector2 GantsPosition = new Vector2(35, 25 + decalage * 2);
        public Vector2 PlastronPosition = new Vector2(35, 25 + decalage * 3);
        public Vector2 AnneauPosition = new Vector2(35, 25 + decalage * 4);
        public Vector2 BottesPosition = new Vector2(35, 25 + decalage * 5);
        public Vector2 ArmePosition = new Vector2(356, 115);

        public bool DrawSelectPoint;
        public bool DrawSac;
        public bool DrawEquipement;
        public Vector2 TailleSac = new Vector2(8, 8);
        SpriteBatch spritebatch;
        SpriteFont gamefont;
        SpriteFont spellfont;

        public bool Win;
        public bool Lost;
        public bool CurseurOffensif;
        public bool IsDrag;
        public Item ItemSelected;

        public MouseState mouse;
        public MouseState Oldmouse;
        public KeyboardState key;
        public KeyboardState Oldkey;

        string tempsDeJeuActuel, compteurDeVague;
        int nombreDeVagues = 8;

        public Joueur joueur;

        public int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        public int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        public UI(Joueur joueur, Texture2D barreDesSorts, Texture2D curseur, Texture2D curseurRouge, Texture2D portrait, Texture2D sac, Texture2D equipement, Texture2D blank, SpriteBatch sp, SpriteFont gf, SpriteFont sf)
        {
            this.joueur = joueur;
            Portrait = portrait;
            Sac = sac;
            Equipement = equipement;
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

            CadrePosition = new Rectangle(0,height - heightFondNoir, widthFondNoir, heightFondNoir);
            PortraitPosition = new Rectangle(CadrePosition.X + CadrePosition.Width - Portrait.Width, CadrePosition.Y + 50, Portrait.Width, Portrait.Height);
            SacPosition = new Rectangle(width - Sac.Width, height - Sac.Height, Sac.Width, Sac.Height);
            EquipementPosition = new Rectangle(width / 2 - Equipement.Width / 2, height / 2 - Equipement.Height / 2, Equipement.Width, Equipement.Height);
            BarreDesSortsPosition = new Rectangle(width / 2, height - BarreDesSorts.Height, BarreDesSorts.Width, BarreDesSorts.Height);

            UtiliserInventaire();
            // Timer et vagues
            if (SceneEngine2.GamePlay.timer.Elapsed.Seconds < 10)
                tempsDeJeuActuel = SceneEngine2.GamePlay.timer.Elapsed.Minutes.ToString() + ":0" + SceneEngine2.GamePlay.timer.Elapsed.Seconds.ToString();
            else
                tempsDeJeuActuel = SceneEngine2.GamePlay.timer.Elapsed.Minutes.ToString() + ":" + SceneEngine2.GamePlay.timer.Elapsed.Seconds.ToString();

            compteurDeVague = Wave.waveNumber.ToString() + "/" + nombreDeVagues.ToString();

        }

        public bool EquipementClick()
        {
            return EquipementPosition.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1));
        }

        public void UtiliserInventaire() // Fonction qui s'occupe de l'utilisation de l'inventaire
        {
            // Si on clique sur le cadre de l'inventaire
            if (mouse.X >= SacPosition.X && mouse.Y >= SacPosition.Y)
            {
                int marge = 7;
                Vector2 position = new Vector2(mouse.X - (width - Sac.Width), mouse.Y - (height - Sac.Height));
                int indice = (int)position.X / (32 + marge) + (int)TailleSac.X * ((int)position.Y / (32 + marge));
                if (mouse.RightButton == ButtonState.Pressed) // Si c'est pour utiliser l'objet
                {
                    if (Oldmouse.RightButton == ButtonState.Released)
                        if (joueur.champion.Inventory.Count > indice)
                            if (joueur.champion.Inventory[indice] is Stuff) // Si on équipe
                            {
                                ((Stuff)joueur.champion.Inventory[indice]).Equiper();
                            }
                            else // Si on utilise un objet
                                joueur.champion.Inventory[indice].Utiliser();
                }
                if (mouse.LeftButton == ButtonState.Pressed && Oldmouse.LeftButton == ButtonState.Released) // Si c'est pour le Drag&Drop
                {
                    if (joueur.champion.Inventory.Count > indice)
                    {
                        IsDrag = true; // On passe en mode GlisserDeposer
                        ItemSelected = joueur.champion.Inventory[indice];
                    }
                }
            }
            if (IsDrag && mouse.LeftButton == ButtonState.Released) // Si on veut déposer sur l'inventaire
            {
                if (DrawEquipement) // Si l'inventaire est activé
                {
                    int t = 40;
                    // Casque
                    if (new Rectangle(EquipementPosition.X + (int)CasquePosition.X, EquipementPosition.Y + (int)CasquePosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && ItemSelected.type == Type.Casque)
                        ((Stuff)ItemSelected).Equiper();
                    // Epaulieres
                    if (new Rectangle(EquipementPosition.X + (int)EpaulieresPosition.X, EquipementPosition.Y + (int)EpaulieresPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && ItemSelected.type == Type.Epaulieres)
                        ((Stuff)ItemSelected).Equiper();
                    // Gants
                    if (new Rectangle(EquipementPosition.X + (int)GantsPosition.X, EquipementPosition.Y + (int)GantsPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && ItemSelected.type == Type.Gants)
                        ((Stuff)ItemSelected).Equiper();
                    // Plastron
                    if (new Rectangle(EquipementPosition.X + (int)PlastronPosition.X, EquipementPosition.Y + (int)PlastronPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && ItemSelected.type == Type.Plastron)
                        ((Stuff)ItemSelected).Equiper();
                    // Anneau
                    if (new Rectangle(EquipementPosition.X + (int)AnneauPosition.X, EquipementPosition.Y + (int)AnneauPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && ItemSelected.type == Type.Anneau)
                        ((Stuff)ItemSelected).Equiper();
                    // Bottes
                    if (new Rectangle(EquipementPosition.X + (int)BottesPosition.X, EquipementPosition.Y + (int)BottesPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && ItemSelected.type == Type.Bottes)
                        ((Stuff)ItemSelected).Equiper();
                    // Arme
                    if (new Rectangle(EquipementPosition.X + (int)ArmePosition.X, EquipementPosition.Y + (int)ArmePosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && ItemSelected.type == Type.Arme)
                        ((Stuff)ItemSelected).Equiper();
                }
                IsDrag = false;
                ItemSelected = null;
            }
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
            // Affichage de la barre des sorts
            spritebatch.Draw(BarreDesSorts, new Rectangle(BarreDesSortsPosition.X + (int)joueur.camera.Position.X, BarreDesSortsPosition.Y + (int)joueur.camera.Position.Y, BarreDesSortsPosition.Width, BarreDesSortsPosition.Height), null, Color.White, 0, new Vector2(BarreDesSorts.Width / 2, 0), SpriteEffects.None, 1);

            // Affichage de l'équipement
            if (DrawEquipement)
            {
                spritebatch.Draw(Equipement, new Vector2(EquipementPosition.X, EquipementPosition.Y) + joueur.camera.Position, Color.White);
                foreach (Item i in joueur.champion.Stuff)
                {
                    Vector2 marge = new Vector2(7, 5);
                    Vector2 P = new Vector2(EquipementPosition.X, EquipementPosition.Y) + joueur.camera.Position + marge;
                    switch (i.type)
                    {
                        case Type.Casque: P += CasquePosition;
                            break;
                        case Type.Epaulieres: P += EpaulieresPosition;
                            break;
                        case Type.Gants: P += GantsPosition;
                            break;
                        case Type.Plastron: P += PlastronPosition;
                            break;
                        case Type.Anneau: P += AnneauPosition;
                            break;
                        case Type.Bottes: P += BottesPosition;
                            break;
                        case Type.Arme: P += ArmePosition;
                            break;
                    }
                    spritebatch.Draw(i.Icone, P, Color.White);
                }
            }
            if (IsDrag)
                spritebatch.Draw(ItemSelected.Icone, new Vector2(mouse.X, mouse.Y) + joueur.camera.Position, Color.White);

            // Fond bas gauche noir
            spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X, (int)joueur.camera.Position.Y + height - CadrePosition.Height, CadrePosition.Width, CadrePosition.Height), Color.Black);

            // Portrait
            spritebatch.Draw(Portrait, new Vector2(PortraitPosition.X, PortraitPosition.Y) + joueur.camera.Position, Color.White);

            int margeGaucheVie = 75, margeGaucheMana = 100, longueurBarre = 150;
            // Affichage de la barre de vie
            spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X + CadrePosition.X + margeGaucheVie, (int)joueur.camera.Position.Y + CadrePosition.Y + CadrePosition.Height - (int)gamefont.MeasureString(str).Y, (int)(((float)joueur.champion.Vie / (float)(joueur.champion.VieMax)) * longueurBarre), hauteurBarre), Color.Green);
            // Affichage de la barre de mana
            spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X + CadrePosition.X + margeGaucheMana, (int)joueur.camera.Position.Y + CadrePosition.Y + CadrePosition.Height - (int)gamefont.MeasureString(str).Y + (int)gamefont.MeasureString("char").Y, (int)(((float)joueur.champion.Mana / (float)(joueur.champion.ManaMax)) * longueurBarre), hauteurBarre), new Color(0, 0, 178));
            // Affichage de la barre d'XP
            int xpToDraw = (int)(((float)joueur.champion.XP / (float)(joueur.champion.Level * 1000)) * CadrePosition.Width);
            if (xpToDraw != 0)
                spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X + CadrePosition.X, (int)joueur.camera.Position.Y + CadrePosition.Y + CadrePosition.Height - hauteurBarre, xpToDraw, hauteurBarre), Color.IndianRed);
            else
                spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X + CadrePosition.X, (int)joueur.camera.Position.Y + CadrePosition.Y + CadrePosition.Height - hauteurBarre, 1, hauteurBarre), Color.IndianRed);
            
            // Affichage de l'aide des sorts
            for (int i = 0; i < joueur.champion.spells.Count; i++)
            {
                if (joueur.SourisHoverCheck(i))
                {
                    int widthCadre = 250;
                    int heightCadre = 100;
                    string nomDuSort = new Text(joueur.champion.spells[i].ToString()).get();
                    // Le cadre noir, le nom du sort, la description
                    spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X + BarreDesSortsPosition.X - widthCadre / 2, (int)joueur.camera.Position.Y + BarreDesSortsPosition.Y - 100, widthCadre, heightCadre), Color.Black);
                    spritebatch.DrawString(spellfont, Outil.Normalize(joueur.champion.spells[i].DescriptionSpell()), new Vector2((int)joueur.camera.Position.X + BarreDesSortsPosition.X - widthCadre / 2, (int)joueur.camera.Position.Y + BarreDesSortsPosition.Y - 120 + 25), Color.White);
                    spritebatch.DrawString(spellfont, nomDuSort, new Vector2((int)joueur.camera.Position.X + BarreDesSortsPosition.X - spellfont.MeasureString(nomDuSort).X / 2, (int)joueur.camera.Position.Y + BarreDesSortsPosition.Y - 120), Color.White);
                }
            }

            // Affichage du texte
            spritebatch.DrawString(gamefont, str, new Vector2(CadrePosition.X, CadrePosition.Y + 25) + joueur.camera.Position, Color.White);
            string xp = joueur.champion.XP + " / " + joueur.champion.Level * 1000;
            spritebatch.DrawString(gamefont, xp, new Vector2(CadrePosition.X + CadrePosition.Width / 2 - gamefont.MeasureString(xp).X / 2, CadrePosition.Y + CadrePosition.Height - hauteurBarre - 2) + joueur.camera.Position, Color.White);

            // Nom du personnage
            Text strUnit = new Text(joueur.champion.ToString().Split(new char[1] { '.' })[1]);
            spritebatch.DrawString(gamefont, strUnit.get(), new Vector2(CadrePosition.X + CadrePosition.Width - gamefont.MeasureString(strUnit.get()).X / 2 - Portrait.Width / 2, CadrePosition.Y) + joueur.camera.Position, Color.White);
            // Sac
            if (DrawSac)
            {
                spritebatch.Draw(Sac, new Vector2(SacPosition.X, SacPosition.Y) + joueur.camera.Position, Color.White);
                // Affichage des items
                int x = 0;
                int j = 0;
                Vector2 marge = new Vector2(8, 8); // marge de l'inventaire
                foreach (Item it in joueur.champion.Inventory)
                {
                    if (!it.Activated)
                        spritebatch.Draw(it.Icone, new Vector2(SacPosition.X, SacPosition.Y) + joueur.camera.Position + marge + new Vector2((32 + 7) * x, (32 + 7) * j), Color.White);
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
            for(int i = 0; i < joueur.champion.spells.Count; i++)
            {
                if (joueur.champion.Map.gametime != null)
                {
                    Color color;
                    if (joueur.champion.IsCastable(i))
                        color = Color.White;
                    else
                        color = Color.Red;
                    spritebatch.Draw(joueur.champion.spells[i].SpriteBouton, new Vector2(BarreDesSortsPosition.X - 130 + i * (32 + 5), BarreDesSortsPosition.Y + 8) + joueur.camera.Position, color);
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
