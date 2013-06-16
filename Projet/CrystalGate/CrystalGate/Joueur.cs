using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using CrystalGate.SceneEngine2;
using System.Timers;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CrystalGate
{
    [Serializable]
    public class Joueur
    {
        public Unite champion;
        public Unite PNJSelected;

        public Camera2D camera;
        public UI Interface;
        bool InWaitingPoint;
        bool InWaitingUnit;
        Spell spell;
        public int id; // spécifie l'identifiant sur le reseau, supérieur à zero sinon on est en local
        bool IsCasting;

        Unite SelectedUnit;
        bool isRoaming;
        int t;

        public Joueur(Unite champ)
        {
            champion = champ;
            // Statistiques
            champion.Drawlife = true;
            champion.isAChamp = true;

            camera = new Camera2D(Vector2.Zero);
            Interface = new UI(this);
        }

        public void Update(List<Unite> unites)
        {
            if (!champion.Mort)
            {
                Interface.mouse = Mouse.GetState();
                Interface.key = Keyboard.GetState();
                // Pour cibler un point pour un sort
                if (Interface.mouse.LeftButton == ButtonState.Pressed && Interface.Oldmouse.LeftButton == ButtonState.Released && InWaitingPoint)
                {
                    champion.pointCible = new Vector2((int)((camera.Position.X + Interface.mouse.X) / 32), (int)((camera.Position.Y + Interface.mouse.Y) / 32));
                    InWaitingPoint = false;
                    Interface.DrawSelectPoint = false;
                    champion.Cast(spell, champion.pointCible, SelectedUnit);
                }
                // Pour cibler une unité pour un sort
                if (Interface.mouse.LeftButton == ButtonState.Pressed && Interface.Oldmouse.LeftButton == ButtonState.Released && InWaitingUnit)
                {
                    Vector2 point = new Vector2((int)((camera.Position.X + Interface.mouse.X) / 32), (int)((camera.Position.Y + Interface.mouse.Y) / 32));
                    
                    foreach (Unite u in Map.unites)
                        if (u != champion && !u.isApnj && Outil.DistancePoints(point, u.PositionTile) <= 46)
                        {
                            SelectedUnit = u;
                        }

                    if (SelectedUnit != null)
                        champion.Cast(spell, champion.pointCible, SelectedUnit);

                    InWaitingUnit = false;
                    Interface.DrawSelectUnit = false;
                }

                // Pour se déplacer
                if (Interface.mouse.RightButton == ButtonState.Pressed && !DonnerOrdreAttaquer() && (SourisHoverInventory() && !Interface.DrawSac || !SourisHoverInventory()) && (SourisHoverEquipement() && !Interface.DrawEquipement || !SourisHoverEquipement()))
                    DonnerOrdreDeplacer();

                if (!Interface.isWriting) // Si on écrit pas on peut utiliser les raccourcis clavier
                {
                    // Pour attaquer un point
                    if (Interface.key.IsKeyDown(Keys.A))
                        DonnerOrdreAttaquerPoint();
                    // Pour arreter les déplacements
                    if (Interface.key.IsKeyDown(Keys.S))
                        DonnerOrdreStop();
                    // Pour se teleporter!
                    if (Interface.key.IsKeyDown(Keys.T) && Interface.Oldkey.IsKeyUp(Keys.T))
                    {
                        if (SceneHandler.level == "level1")
                            SceneHandler.ResetGameplay("level3");
                        else
                            SceneHandler.ResetGameplay("level1");
                    }
                    
                    // Pour afficher/cacher le sac
                    if (Interface.key.IsKeyDown(Keys.B) && Interface.Oldkey.IsKeyUp(Keys.B) || Interface.key.IsKeyDown(Keys.I) && Interface.Oldkey.IsKeyUp(Keys.I))
                        Interface.DrawSac = !Interface.DrawSac;
                    // Pour afficher/cacher le sac
                    if (Interface.key.IsKeyDown(Keys.C) && Interface.Oldkey.IsKeyUp(Keys.C))
                        Interface.DrawEquipement = !Interface.DrawEquipement;
                }

                if (champion.PositionTile == new Vector2(97, 8) || champion.PositionTile == new Vector2(97, 7) || champion.PositionTile == new Vector2(97, 9))
                {
                    if (SceneHandler.level == "level1")
                    {
                        champion.PositionTile = new Vector2(3, 20);
                        champion.ObjectifListe = new List<Noeud> { };
                        camera.Position = new Vector2(0, 200);
                        SceneHandler.ResetGameplay("level2");
                    }
                }
                if (champion.PositionTile == new Vector2(129, 11) || champion.PositionTile == new Vector2(129, 12) || champion.PositionTile == new Vector2(129, 13))
                {
                    if (SceneHandler.level == "level2")
                    {
                        champion.PositionTile = new Vector2(2, 17);
                        champion.ObjectifListe = new List<Noeud> { };
                        camera.Position = new Vector2(0, 200);
                        SceneHandler.ResetGameplay("level3");
                    }
                }


                // Pour lancer un sort
                if (Interface.key.IsKeyDown(Keys.D1) && champion.spells.Count > 0 || Interface.SourisClickSpellCheck(0) && champion.spells.Count > 0)
                {
                    spell = champion.spells[0];
                    if (champion.IsCastable(0))
                    {
                        if (champion.spells[0].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        else
                        {
                            champion.Cast(spell, champion.pointCible, SelectedUnit);
                            IsCasting = true;
                        }
                    }
                }
                if (Interface.key.IsKeyDown(Keys.D2) && champion.spells.Count > 1 || Interface.SourisClickSpellCheck(1) && champion.spells.Count > 1)
                {
                    spell = champion.spells[1];
                    if (champion.IsCastable(1))
                    {
                        if (champion.spells[1].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        else
                        {
                            champion.Cast(spell, champion.pointCible, SelectedUnit);
                            IsCasting = true;
                        }
                    }
                }
                if (Interface.key.IsKeyDown(Keys.D3) && champion.spells.Count > 2 || Interface.SourisClickSpellCheck(2) && champion.spells.Count > 2)
                {
                    spell = champion.spells[2];
                    if (champion.IsCastable(2))
                    {
                        if (champion.spells[2].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        else
                        {
                            champion.Cast(spell, champion.pointCible, SelectedUnit);
                            IsCasting = true;
                        }
                    }
                }
                if (Interface.key.IsKeyDown(Keys.D4) && champion.spells.Count > 3 || Interface.SourisClickSpellCheck(3) && champion.spells.Count > 3)
                {
                    spell = champion.spells[3];
                    if (champion.IsCastable(3))
                    {
                        if (champion.spells[3].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        else
                        {
                            champion.Cast(spell, champion.pointCible, SelectedUnit);
                            IsCasting = true;
                        }
                    }
                }
                if (Interface.key.IsKeyDown(Keys.D5) && champion.spells.Count > 4 || Interface.SourisClickSpellCheck(4) && champion.spells.Count > 4)
                {
                    spell = champion.spells[4];
                    if (champion.IsCastable(4))
                    {
                        if (champion.spells[4].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        if (champion.spells[4].NeedAUnit)
                        {
                            Interface.DrawSelectUnit = true;
                            InWaitingUnit = true;
                        }
                        else
                        {
                            champion.Cast(spell, champion.pointCible, SelectedUnit);
                            IsCasting = true;
                        }
                    }
                }
                if (Interface.key.IsKeyDown(Keys.D6) && champion.spells.Count > 5 || Interface.SourisClickSpellCheck(5) && champion.spells.Count > 5)
                {
                    spell = champion.spells[5];
                    if (champion.IsCastable(5))
                    {
                        if (champion.spells[5].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        if (champion.spells[5].NeedAUnit)
                        {
                            Interface.DrawSelectUnit = true;
                            InWaitingUnit = true;
                        }
                        else
                        {
                            champion.Cast(spell, champion.pointCible, SelectedUnit);
                            IsCasting = true;
                        }
                    }
                }
                
                
                // Fait attaquer l'unité la plus proche
                /*if (isRoaming)
                {
                    float distanceInit = 9000;
                    Unite focus = null;
                    foreach (Unite u in Map.unites)
                    {
                        float distance = Outil.DistanceUnites(champion, u);

                        if (champion != u && !u.isApnj && distance <= distanceInit)
                        {
                            distanceInit = distance;
                            focus = u;
                        }
                    }
                    champion.uniteAttacked = focus;
                }*/
            }
            // Pour verifier si on parle a un pnj
            SpeakToPNJ();
            // Pour déplacer la caméra
            CameraUpdate();
            Interface.Update();
            UpdateReseau();
            CurseurCheck();
            Interface.Oldmouse = Interface.mouse;
            Interface.Oldkey = Interface.key;
            IsCasting = false;

        }

        public void UpdateReseau()
        {
            if ((Client.Started && t >= 10 || IsCasting) && Client.isConnected) // Si on est en reseau et que l'on doit send
            {
                // Envoi
                Client.Send(Serialize(), 42);
                t = 0;
            }
            t++;
        }

        void SendSpell(Player p)
        {
            if (IsCasting)
            {
                Unite u = champion;
                List<Spell> toutLesSpellsPossibles = new List<Spell> { new Explosion(u), new Soin(u), new Invisibilite(u), new FurieSanguinaire(u), new Polymorphe(u), new Tempete(u) };

                foreach (Spell s in toutLesSpellsPossibles)
                    if (s.idSort == spell.idSort)
                    {
                        p.idSortCast = s.idSort;
                        p.pointSortX = champion.pointCible.X;
                        p.pointSortY = champion.pointCible.Y;
                        if(SelectedUnit != null)
                            p.idUniteCibleCast = SelectedUnit.id;
                        break;
                    }
            }
        }

        public byte[] Serialize()
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            // Definition du contenu
            Player p = new Player();
            Joueur Local = Outil.GetJoueur(Client.id);

            // Pathfinding
            if (Local.champion.ObjectifListe.Count > 0)
            {
                p.Mooved = true;
                p.objectifPointX = Local.champion.ObjectifListe[Local.champion.ObjectifListe.Count - 1].Position.X;
                p.objectifPointY = Local.champion.ObjectifListe[Local.champion.ObjectifListe.Count - 1].Position.Y;
            }

            // Unité visé
            p.idUniteAttacked = Local.champion.idUniteAttacked;

                p.LastDeath = (byte)Serveur.LastDead;
            // Unit dernierement morte selon le serveur
            SendSpell(p);

            formatter.Serialize(stream, p);
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public void DonnerOrdreDeplacer()
        {
            if (Interface.mouse.X < CrystalGateGame.graphics.PreferredBackBufferWidth && Interface.mouse.Y < CrystalGateGame.graphics.PreferredBackBufferHeight)
            {
                isRoaming = false;
                Vector2 ObjectifPoint = new Vector2(camera.Position.X + Interface.mouse.X, camera.Position.Y + Interface.mouse.Y) / Map.TailleTiles;
                ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);

                List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, Map.Taille, new List<Unite> { }, Map.unitesStatic, false);
                if (chemin != null)
                    champion.ObjectifListe = chemin;
                champion.uniteAttacked = null;
            }
        }

        public bool DonnerOrdreAttaquer()
        {
            isRoaming = false;
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + Interface.mouse.X, camera.Position.Y + Interface.mouse.Y) / Map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);
            foreach (Unite u in Map.unites)
                if(u != champion && !u.isApnj && Outil.DistancePoints(ObjectifPoint, u .PositionTile) <= 60)
                {
                    champion.uniteAttacked = u;
                    List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, Map.Taille, new List<Unite> { }, Map.unitesStatic, false);
                    if (chemin != null)
                        champion.ObjectifListe = chemin;
                    return true;
                }
            return false;
        }

        public void DonnerOrdreAttaquerPoint()
        {
            //DonnerOrdreDeplacer();
            //isRoaming = true;
        }

        public void DonnerOrdreStop()
        {
            champion.uniteAttacked = null;
            champion.ObjectifListe.Clear();
        }

        public void CameraUpdate()
        {
            int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            int vitesse = 10;
            Vector2 vec = new Vector2();

            // Si on déplace la caméra hors des bords de l'écran
            if (Interface.mouse.X >= width - 1)
                vec.X += vitesse;
            if (Interface.mouse.X <= 1)
                vec.X -= vitesse;
            if (Interface.mouse.Y >= height - 15)
                vec.Y += vitesse;
            if (Interface.mouse.Y <= 1)
                vec.Y -= vitesse;

            // Si on sort de la Map
            if (camera.Position.X > Map.Taille.X * Map.TailleTiles.X - width)
                camera.Position = new Vector2(Map.Taille.X * Map.TailleTiles.X - width, camera.Position.Y);
            if (camera.Position.X < 0)
                camera.Position = new Vector2(0, camera.Position.Y);
            if (camera.Position.Y < 0)
                camera.Position = new Vector2(camera.Position.X, 0);
            if (camera.Position.Y > Map.Taille.Y * Map.TailleTiles.Y - height + Interface.CadrePosition.Height)
                camera.Position = new Vector2(camera.Position.X, Map.Taille.Y * Map.TailleTiles.Y - height + Interface.CadrePosition.Height);
            
            //Update de la position de la caméra et de l'interface
            camera.Position = new Vector2(camera.Position.X, camera.Position.Y) + vec;
        }

        public bool CurseurCheck() // Renvoie vrai si le curseur est sur un méchant :p
        {
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + Interface.mouse.X, camera.Position.Y + Interface.mouse.Y) / Map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);

            foreach (Unite u in Map.unites)
                if (champion != u && Outil.DistancePoints(ObjectifPoint, u.PositionTile) <= 32)
                {
                    Interface.CurseurOffensif = true;
                    return true;
                }

            Interface.CurseurOffensif = false;
            return false;
        }

        public bool SpeakToPNJ() // Renvoie si on parle a un PNJ, et modifie l'UI si c'est le cas
        {
            // Stoque les PNJ pres du joueurs dans une liste
            List<Unite> result = Map.unites.Where(i => Outil.DistanceUnites(i, champion) <= 64 && i.isApnj).ToList();
            PNJSelected = (result.Count > 0) ? result[0] : null; // prend le premier et le met dans la variable PNJSelected
            
            if (PNJSelected != null) // Si on est assez proche du PNJ, on draw le dialogue
                Interface.DrawDialogue = Outil.DistancePoints(champion.PositionTile, PNJSelected.PositionTile) <= 46;
            else
                Interface.DrawDialogue = false;
            return Interface.DrawDialogue;
        }

        public bool SourisHoverInventory()
        {
            return Interface.SacPosition.Intersects(new Rectangle(Interface.mouse.X, Interface.mouse.Y, 1, 1));
        } // Renvoie si la souris est sur l'inventaire

        public bool SourisHoverEquipement()
        {
            return Interface.EquipementPosition.Intersects(new Rectangle(Interface.mouse.X, Interface.mouse.Y, 1, 1));
        } // Renvoie si la souris est sur l'equipement
    }
}
