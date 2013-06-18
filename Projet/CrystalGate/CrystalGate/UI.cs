using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using CrystalGate.SceneEngine2;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        const int decalage = 43;
        public Vector2 CasquePosition = new Vector2(35, 25); // 42
        public Vector2 EpaulieresPosition = new Vector2(35, 25 + decalage * 1);
        public Vector2 GantsPosition = new Vector2(35, 25 + decalage * 2);
        public Vector2 PlastronPosition = new Vector2(35, 25 + decalage * 3);
        public Vector2 AnneauPosition = new Vector2(35, 25 + decalage * 4);
        public Vector2 BottesPosition = new Vector2(35, 25 + decalage * 5);
        public Vector2 ArmePosition = new Vector2(356, 115);

        public bool DrawSelectPoint, DrawSelectUnit, DrawSac, DrawEquipement, DrawDialogue, OldDrawDialogue;
        public bool DrawUI = true;

        public Vector2 TailleSac = new Vector2(8, 8);
        SpriteBatch spritebatch;
        SpriteFont gamefont;
        SpriteFont spellfont;

        public bool Win,Lost,CurseurOffensif,IsDrag;
        public Item ItemSelected;

        public bool isWriting = false;
        string message = "";
        List<Text> dialogue = new List<Text> { };

        int MaxChar;
        public static bool Error;

        public static string messageRecu = "";
        public Vector2 positionChat;

        int widthFondNoir, heightFondNoir;

        public MouseState mouse;
        public MouseState Oldmouse;
        public KeyboardState key;
        public KeyboardState Oldkey;

        string tempsDeJeuActuel, compteurDeVague;

        public Joueur joueur;

        public int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        public int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        public UI(Joueur joueur)
        {
            this.joueur = joueur;
            Portrait = (joueur.champion is Voleur) ? PackTexture.VoleurPortrait : PackTexture.GuerrierPortrait;
            Sac = PackTexture.Sac;
            Equipement = PackTexture.Equipement;
            BarreDesSorts = PackTexture.BarreDesSorts;
            Curseur = PackTexture.Curseur;
            CurseurRouge = PackTexture.CurseurRouge;
            this.blank = PackTexture.blank;
            spritebatch = SceneHandler.spriteBatch;
            gamefont = PackTexture.gamefont;
            spellfont = PackTexture.spellfont;
            tempsDeJeuActuel = "0:00";
            compteurDeVague = Map.nombreDeVaguesPop.ToString() + Map.nombreDeVagues.ToString();

            widthFondNoir = 380;
            heightFondNoir = 250;

            CadrePosition = new Rectangle(0, height - heightFondNoir, widthFondNoir, heightFondNoir);
            PortraitPosition = new Rectangle(CadrePosition.X + CadrePosition.Width - Portrait.Width, CadrePosition.Y + 50, Portrait.Width, Portrait.Height);
            SacPosition = new Rectangle(width - Sac.Width, height - Sac.Height, Sac.Width, Sac.Height);
            EquipementPosition = new Rectangle(width / 2 - Equipement.Width / 2, height / 2 - Equipement.Height / 2, Equipement.Width, Equipement.Height);
            BarreDesSortsPosition = new Rectangle(width / 2, height - BarreDesSorts.Height, BarreDesSorts.Width, BarreDesSorts.Height);
        }

        public void Update()
        {
            CadrePosition = new Rectangle(0, height - heightFondNoir, widthFondNoir, heightFondNoir);
            PortraitPosition = new Rectangle(CadrePosition.X + CadrePosition.Width - Portrait.Width, CadrePosition.Y + 50, Portrait.Width, Portrait.Height);
            SacPosition = new Rectangle(width - Sac.Width, height - Sac.Height, Sac.Width, Sac.Height);
            EquipementPosition = new Rectangle(width / 2 - Equipement.Width / 2, height / 2 - Equipement.Height / 2, Equipement.Width, Equipement.Height);
            BarreDesSortsPosition = new Rectangle(width / 2, height - BarreDesSorts.Height, BarreDesSorts.Width, BarreDesSorts.Height);

            UtiliserInventaire();
            DesequiperUpdate();
            // Timer et vagues
            if (SceneEngine2.GamePlay.timer.Elapsed.Seconds < 10)
                tempsDeJeuActuel = SceneEngine2.GamePlay.timer.Elapsed.Minutes.ToString() + ":0" + SceneEngine2.GamePlay.timer.Elapsed.Seconds.ToString();
            else
                tempsDeJeuActuel = SceneEngine2.GamePlay.timer.Elapsed.Minutes.ToString() + ":" + SceneEngine2.GamePlay.timer.Elapsed.Seconds.ToString();

            compteurDeVague = Map.nombreDeVaguesPop.ToString() + "/" + Map.nombreDeVagues.ToString();

            if (SceneEngine2.BaseScene.keyboardState.IsKeyDown(Keys.Enter) &&
                SceneEngine2.BaseScene.oldKeyboardState.IsKeyUp(Keys.Enter) &&
                SceneEngine2.CoopConnexionScene.isOnlinePlay)
            {
                if (isWriting && message != "")
                {
                    Message envoi = new Message(EffetSonore.time.Elapsed, Client.ownPlayer.name + " : " + message);

                    // On s'envoit
                    MemoryStream stream = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, envoi);
                    byte[] buffer = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(buffer, 0, buffer.Length);

                    // Envoi
                    Client.Send(buffer, 2);
                    message = "";
                    isWriting = false;
                }
                else
                    isWriting = !isWriting;
            }
            if (!SceneEngine2.CoopConnexionScene.isOnlinePlay)
                isWriting = false;

            if (isWriting)
                SaisirTexte(ref message);

            if (dialogue.Count > 0) // pour passer les dialogues
            {
                if (key.IsKeyDown(Keys.Enter) && Oldkey.IsKeyUp(Keys.Enter)) // Si on est a la fin de la replique, on passe a la suivante.
                {
                    dialogue.RemoveAt(0);
                    MaxChar = 0;
                }
            }

            messageRecu = "";
            for (int i = 0; i < Client.discution.Count; i++)
			{
                if (Client.discution[i].dateEnvoi + new TimeSpan(0, 0, 10) < EffetSonore.time.Elapsed)
                {
                    Client.discution.RemoveAt(i);
                    i--;
                }
                else
                {
                    messageRecu += "\n" + Client.discution[i].message;
                }
			}
            positionChat = new Vector2(widthFondNoir, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height - heightFondNoir - gamefont.MeasureString(messageRecu).Y - 50);

            if (Win)
            {
                FondSonore.Stop();
                SceneEngine2.SceneHandler.gameState = SceneEngine2.GameState.Victory;
            }
            else if (Lost)
            {
                FondSonore.Stop();
                SceneEngine2.SceneHandler.gameState = SceneEngine2.GameState.Defeat;
            }
        }

        #region fonctionsSupl
        public void UtiliserInventaire() // Fonction qui s'occupe de l'utilisation de l'inventaire
        {
            // Si on clique sur le cadre de l'inventaire
            if (mouse.X >= SacPosition.X && mouse.Y >= SacPosition.Y && DrawSac)
            {
                int marge = 7;
                Vector2 position = new Vector2(mouse.X - (width - Sac.Width), mouse.Y - (height - Sac.Height));
                int indice = (int)position.X / (32 + marge) + (int)TailleSac.X * ((int)position.Y / (32 + marge));
                if (mouse.RightButton == ButtonState.Pressed) // Si c'est pour utiliser l'objet
                {
                    if (Oldmouse.RightButton == ButtonState.Released)
                        if (joueur.champion.Inventory.Count > indice)
                        {
                            if (joueur.champion.Inventory[indice] is Stuff) // Si on équipe
                            {
                                if (SceneHandler.gameplayScene.isCoopPlay)
                                {
                                    // pour le reseau
                                    joueur.lastStuffUsed = indice;
                                }
                                else
                                    ((Stuff)joueur.champion.Inventory[indice]).Equiper();
                            }
                            else
                            {
                                // Si on utilise un objet
                                if (SceneHandler.gameplayScene.isCoopPlay)
                                {
                                    // pour le reseau
                                    joueur.lastItemUsed = indice;
                                }
                                else
                                    joueur.champion.Inventory[indice].Utiliser();
                            }

                        }
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

        public void DesequiperUpdate()
        {
            if (DrawEquipement && EquipementPosition.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && mouse.RightButton == ButtonState.Pressed)
                if (SourisHoverStuff())
                    ((Stuff)ItemSelected).Desequiper();
        } // Fonction qui s'occupe du déséquipement

        public bool SourisHoverStuff() // Renvoie vrai si la souris est sur une piece de stuff de l'equipement
        {
            int t = 40;
            if (new Rectangle(EquipementPosition.X + (int)ArmePosition.X, EquipementPosition.Y + (int)ArmePosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && joueur.champion.Stuff.Where(i => i.type == Type.Arme).Count() > 0)
                ItemSelected = joueur.champion.Stuff.Where(i => i.type == Type.Arme).ToList()[0];
            else if (new Rectangle(EquipementPosition.X + (int)CasquePosition.X, EquipementPosition.Y + (int)CasquePosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && joueur.champion.Stuff.Where(i => i.type == Type.Casque).Count() > 0)
                ItemSelected = joueur.champion.Stuff.Where(i => i.type == Type.Casque).ToList()[0];
            else if (new Rectangle(EquipementPosition.X + (int)AnneauPosition.X, EquipementPosition.Y + (int)AnneauPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && joueur.champion.Stuff.Where(i => i.type == Type.Anneau).Count() > 0)
                ItemSelected = joueur.champion.Stuff.Where(i => i.type == Type.Anneau).ToList()[0];
            else if (new Rectangle(EquipementPosition.X + (int)EpaulieresPosition.X, EquipementPosition.Y + (int)EpaulieresPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && joueur.champion.Stuff.Where(i => i.type == Type.Epaulieres).Count() > 0)
                ItemSelected = joueur.champion.Stuff.Where(i => i.type == Type.Epaulieres).ToList()[0];
            else if (new Rectangle(EquipementPosition.X + (int)GantsPosition.X, EquipementPosition.Y + (int)GantsPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && joueur.champion.Stuff.Where(i => i.type == Type.Gants).Count() > 0)
                ItemSelected = joueur.champion.Stuff.Where(i => i.type == Type.Gants).ToList()[0];
            else if (new Rectangle(EquipementPosition.X + (int)PlastronPosition.X, EquipementPosition.Y + (int)PlastronPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && joueur.champion.Stuff.Where(i => i.type == Type.Plastron).Count() > 0)
                ItemSelected = joueur.champion.Stuff.Where(i => i.type == Type.Plastron).ToList()[0];
            else if (new Rectangle(EquipementPosition.X + (int)BottesPosition.X, EquipementPosition.Y + (int)BottesPosition.Y, t, t).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && joueur.champion.Stuff.Where(i => i.type == Type.Bottes).Count() > 0)
                ItemSelected = joueur.champion.Stuff.Where(i => i.type == Type.Bottes).ToList()[0];
            else

                return false;

            return true;
        }

        public bool SourisHoverSpellCheck(int i) // Renvoie vrai si le joueur a la souris sur le bouton i
        {
            int largeurBoutonSort = 32;
            return mouse.X >= BarreDesSortsPosition.X - 130 + i * largeurBoutonSort && mouse.X <= BarreDesSortsPosition.X - 130 + i * largeurBoutonSort + largeurBoutonSort && mouse.Y >= BarreDesSortsPosition.Y + 8 && mouse.Y <= BarreDesSortsPosition.Y + 8 + largeurBoutonSort;
        }

        public bool SourisClickSpellCheck(int i) // Renvoie vrai si le joueur clique sur le bouton i
        {
            int largeurBoutonSort = 32;
            return mouse.X >= BarreDesSortsPosition.X - 130 + i * largeurBoutonSort && mouse.X <= BarreDesSortsPosition.X - 130 + i * largeurBoutonSort + largeurBoutonSort && mouse.Y >= BarreDesSortsPosition.Y + 8 && mouse.Y <= BarreDesSortsPosition.Y + 8 + largeurBoutonSort && mouse.LeftButton == ButtonState.Pressed && Oldmouse.LeftButton == ButtonState.Released;
        }

        public void SaisirTexte(ref string text)
        {
            Keys[] pressedKeys = SceneEngine2.BaseScene.keyboardState.GetPressedKeys();
            Keys[] prevPressedKeys = SceneEngine2.BaseScene.oldKeyboardState.GetPressedKeys();

            char c;

            bool shiftPressed = pressedKeys.Contains(Keys.LeftShift) || pressedKeys.Contains(Keys.RightShift);
            foreach (Keys key in pressedKeys)
            {
                if (!prevPressedKeys.Contains(key))
                {
                    string keyString = key.ToString();

                    if (keyString.Length == 1)
                    {
                        c = keyString[0];
                        if (c >= 'A' && c <= 'Z')
                            if (shiftPressed)
                                text += c;
                            else
                                text += (char)(c - 'A' + 'a');
                    }
                    else if (keyString == "Space")
                    {
                        text += " ";
                    }
                }
            }

            // Delete
            if (SceneEngine2.BaseScene.keyboardState.IsKeyDown(Keys.Back) && SceneEngine2.BaseScene.oldKeyboardState.IsKeyUp(Keys.Back))
            {
                SceneEngine2.BaseScene.oldKeyboardState = SceneEngine2.BaseScene.keyboardState;
                prevPressedKeys = pressedKeys;
                string text2 = "";
                for (int i = 0; i < text.Length - 1; i++)
                    text2 += text[i];

                text = text2;
            }
        }
        #endregion fonctionsSupl

        #region draw
        public void Draw()
        {
            int hauteurBarre = 30;

            MouseState m = SceneEngine2.BaseScene.mouse;
            
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
            // Si on fait glisser déposer
            if (IsDrag)
                spritebatch.Draw(ItemSelected.Icone, new Vector2(mouse.X, mouse.Y) + joueur.camera.Position, Color.White);

            // Textes
            Text life = new Text("Life"), attack = new Text("Attack"), armor = new Text("Armor"), selectPoint = new Text("SelectPoint"), selectUnit = new Text("SelectUnit"), manaText = new Text("Mana"), levelText = new Text("Level"); // définition des mots traduisibles

            // Affichage du dialogue avec les PNJ
            if (DrawDialogue)
            {
                if (DrawDialogue != OldDrawDialogue) // Si on va pres du pnj ( et que l'on ne l'etait pas avant)
                {
                    MaxChar = 0; // On reset tout
                    dialogue.Clear();
                    foreach (Text t in joueur.PNJSelected.Dialogue)
                        dialogue.Add(t);
                }

                if (dialogue.Count() > 0) // Si le dialogue est en cours on le draw
                {
                    string strDiag = dialogue[0].get();
                    string strDraw = "";
                    int HeightBDialogue = 200;
                    int tailleCadre = 150;
                    int margeCadre = 100;
                    spritebatch.Draw(PackTexture.Dialogue, new Rectangle((int)joueur.camera.Position.X + margeCadre, (int)joueur.camera.Position.Y + height - HeightBDialogue, width - 2 * margeCadre, HeightBDialogue), Color.White);
                    spritebatch.Draw(Portrait, new Rectangle((int)joueur.camera.Position.X + margeCadre, (int)joueur.camera.Position.Y + height - tailleCadre, tailleCadre, tailleCadre), Color.White);
                    spritebatch.Draw(joueur.PNJSelected.Portrait, new Rectangle((int)joueur.camera.Position.X + width - tailleCadre - margeCadre, (int)joueur.camera.Position.Y + height - tailleCadre, tailleCadre, tailleCadre), Color.White);
                    Text l1 = new Text(joueur.champion.ToString().Split(new char[1] { '.' })[1]);
                    string l2 = joueur.PNJSelected.ToString().Split(new char[1] { '.' })[2];
                    spritebatch.DrawString(gamefont, l1.get(), new Vector2( 3 * margeCadre / 2 - gamefont.MeasureString(l1.get()).X / 2, height - HeightBDialogue) + joueur.camera.Position, Color.BurlyWood);
                    spritebatch.DrawString(gamefont, l2, new Vector2(width - 3 * margeCadre / 2 - gamefont.MeasureString(l2).X / 2, height - HeightBDialogue) + joueur.camera.Position, Color.BurlyWood);

                    for (int i = 0; i < MaxChar && i < strDiag.Count(); i++)
                        strDraw += strDiag[i];


                    MaxChar++;
                    spritebatch.DrawString(gamefont, Outil.Normalize((width - 2 * (margeCadre + tailleCadre)) / 13, strDraw), new Vector2(margeCadre + tailleCadre, height - HeightBDialogue / 2 - tailleCadre / 2) + joueur.camera.Position, Color.BurlyWood);
                }
            }
            if (dialogue.Count() > 0) // Si le dialogue est en cours on cache l'UI
                DrawUI = !DrawDialogue;
            else // Sinon si c'est fini on l'affiche
                DrawUI = true;

            if (DrawUI)
            {
                string str = " " + life.get() + " : " + joueur.champion.Vie + " / " + joueur.champion.VieMax + "\n "
                    + manaText.get() + " : " + joueur.champion.Mana + " / " + joueur.champion.ManaMax + "\n "
                    + attack.get() + " : " + joueur.champion.Dommages + "\n "
                    + armor.get() + " : " + joueur.champion.Defense + " / " + joueur.champion.DefenseMagique + "\n "
                    + levelText.get() + " : " + joueur.champion.Level + "\n ";

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

                // Affichage du texte
                // Nom du personnage
                Text strUnit = new Text(joueur.champion.ToString().Split(new char[1] { '.' })[1]);
                spritebatch.DrawString(gamefont, strUnit.get(), new Vector2(CadrePosition.X + CadrePosition.Width - gamefont.MeasureString(strUnit.get()).X / 2 - Portrait.Width / 2, CadrePosition.Y) + joueur.camera.Position, Color.White);

                spritebatch.DrawString(gamefont, str, new Vector2(CadrePosition.X, CadrePosition.Y + 25) + joueur.camera.Position, Color.White);
                string xp = joueur.champion.XP.ToString() + " / " + joueur.champion.Level * 1000;
                spritebatch.DrawString(gamefont, xp, new Vector2(CadrePosition.X + CadrePosition.Width / 2 - gamefont.MeasureString(xp).X / 2, CadrePosition.Y + CadrePosition.Height - hauteurBarre - 2) + joueur.camera.Position, Color.White);

                // Affichage de la barre des sorts
                spritebatch.Draw(BarreDesSorts, new Rectangle(BarreDesSortsPosition.X + (int)joueur.camera.Position.X, BarreDesSortsPosition.Y + (int)joueur.camera.Position.Y, BarreDesSortsPosition.Width, BarreDesSortsPosition.Height), null, Color.White, 0, new Vector2(BarreDesSorts.Width / 2, 0), SpriteEffects.None, 1);
                // Affichage des spells
                for (int i = 0; i < joueur.champion.spells.Count; i++)
                {
                    if (Map.gametime != null)
                    {
                        Color color;
                        if (joueur.champion.IsCastable(i))
                            color = Color.White;
                        else
                            color = Color.Red;
                        spritebatch.Draw(joueur.champion.spells[i].SpriteBouton, new Vector2(BarreDesSortsPosition.X - 130 + i * (32 + 3), BarreDesSortsPosition.Y + 8) + joueur.camera.Position, color);
                    }

                }
                // Affichage de l'aide des sorts
                for (int i = 0; i < joueur.champion.spells.Count; i++)
                {
                    if (SourisHoverSpellCheck(i))
                    {
                        int widthCadre = 250;
                        int heightCadre = 100;
                        string nomDuSort = new Text(joueur.champion.spells[i].ToString()).get();
                        // Le cadre noir, le nom du sort, la description
                        spritebatch.Draw(blank, new Rectangle((int)joueur.camera.Position.X + BarreDesSortsPosition.X - widthCadre / 2, (int)joueur.camera.Position.Y + BarreDesSortsPosition.Y - 100, widthCadre, heightCadre), Color.Black);
                        spritebatch.DrawString(spellfont, Outil.Normalize(30, joueur.champion.spells[i].DescriptionSpell()), new Vector2((int)joueur.camera.Position.X + BarreDesSortsPosition.X - widthCadre / 2, (int)joueur.camera.Position.Y + BarreDesSortsPosition.Y - 120 + 25), Color.White);
                        spritebatch.DrawString(spellfont, nomDuSort, new Vector2((int)joueur.camera.Position.X + BarreDesSortsPosition.X - spellfont.MeasureString(nomDuSort).X / 2, (int)joueur.camera.Position.Y + BarreDesSortsPosition.Y - 120), Color.White);
                    }
                }
            }

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

            string str2 = selectPoint.get();
            string str3 = selectUnit.get();
            // Selections d'unités et de points
            if (DrawSelectPoint)
                spritebatch.DrawString(gamefont, str2, new Vector2(BarreDesSortsPosition.X - gamefont.MeasureString(str2).X / 2 , BarreDesSortsPosition.Y - BarreDesSorts.Height) + joueur.camera.Position, Color.White);
            if (DrawSelectUnit)
                spritebatch.DrawString(gamefont, str3, new Vector2(BarreDesSortsPosition.X - gamefont.MeasureString(str2).X / 2, BarreDesSortsPosition.Y - BarreDesSorts.Height) + joueur.camera.Position, Color.White);

            if (isWriting)
            {
                spritebatch.Draw(blank,
                    new Rectangle((int)joueur.camera.Position.X + widthFondNoir + 10, (int)joueur.camera.Position.Y + CrystalGateGame.graphics.GraphicsDevice.Viewport.Height - heightFondNoir, 
                        CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - widthFondNoir * 2 - 20, 30),
                    new Color(0, 0, 0, 127));
                spritebatch.DrawString(gamefont, message, joueur.camera.Position + new Vector2(widthFondNoir, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height - heightFondNoir), Color.White);
            }

            if (messageRecu != "")
            {
                spritebatch.DrawString(gamefont, messageRecu, joueur.camera.Position +
                    positionChat, Color.White);
            }

            // Timer
            spritebatch.DrawString(gamefont, tempsDeJeuActuel, new Vector2(joueur.camera.Position.X + width - gamefont.MeasureString(tempsDeJeuActuel).X - 5, joueur.camera.Position.Y + 4), Color.Black);

            // Compteur de vagues
            spritebatch.DrawString(gamefont, compteurDeVague, new Vector2(joueur.camera.Position.X + width - gamefont.MeasureString(compteurDeVague).X - 5, gamefont.MeasureString(tempsDeJeuActuel).Y + joueur.camera.Position.Y + 4), Color.Black);

            // Erreur reseau
            if(Error)
                spritebatch.DrawString(gamefont, new Text("Error1").get(), joueur.camera.Position, Color.White);
            // Curseur
            spritebatch.Draw(CurseurOffensif ? CurseurRouge : Curseur, new Vector2(joueur.camera.Position.X + m.X, joueur.camera.Position.Y + m.Y), Color.White);

            OldDrawDialogue = DrawDialogue;
        }
        #endregion draw
    }
}
