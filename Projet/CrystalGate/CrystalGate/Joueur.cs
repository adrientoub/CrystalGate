using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Joueur
    {
        public Unite champion;
        public Unite PNJSelected;

        public Camera2D camera;
        public UI Interface;
        bool InWaitingPoint;
        int spell;

        bool isRoaming;

        public Joueur(Unite champ)
        {
            champion = champ;

            // Statistiques
            champion.Drawlife = true;
            champion.isAChamp = true;

            camera = new Camera2D(Vector2.Zero);
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
                    champion.Cast(spell, champion.pointCible);
                }
                // Pour se déplacer
                if (Interface.mouse.RightButton == ButtonState.Pressed && !DonnerOrdreAttaquer() && (OnInventory() && !Interface.DrawSac || !OnInventory()) && (OnEquipement() && !Interface.DrawEquipement || !OnEquipement()))
                    DonnerOrdreDeplacer();
                // Pour attaquer un point
                if (Interface.key.IsKeyDown(Keys.A))
                    DonnerOrdreAttaquerPoint();
                // Pour arreter les déplacements
                if (Interface.key.IsKeyDown(Keys.S))
                    DonnerOrdreStop();
                // Pour lancer un sort
                if (Interface.key.IsKeyDown(Keys.D1) && champion.spells.Count > 0 || ClickCheck(0) && champion.spells.Count > 0)
                {
                    spell = 0;
                    if (champion.IsCastable(0))
                    {
                        if (champion.spells[0].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        else
                            champion.Cast(spell, champion.pointCible);
                    }
                }
                if (Interface.key.IsKeyDown(Keys.D2) && champion.spells.Count > 1 || ClickCheck(1) && champion.spells.Count > 1)
                {
                    spell = 1;
                    if (champion.IsCastable(1))
                    {
                        if (champion.spells[1].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        else
                            champion.Cast(spell, champion.pointCible);
                    }
                }
                if (Interface.key.IsKeyDown(Keys.D3) && champion.spells.Count > 2 || ClickCheck(2) && champion.spells.Count > 2)
                {
                    spell = 2;
                    if (champion.IsCastable(2))
                    {
                        if (champion.spells[2].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        else
                            champion.Cast(spell, champion.pointCible);
                    }
                }
                if (Interface.key.IsKeyDown(Keys.D4) && champion.spells.Count > 3 || ClickCheck(3) && champion.spells.Count > 3)
                {
                    spell = 3;
                    if (champion.IsCastable(3))
                    {
                        if (champion.spells[3].NeedUnPoint)
                        {
                            Interface.DrawSelectPoint = true;
                            InWaitingPoint = true;
                        }
                        else
                            champion.Cast(spell, champion.pointCible);
                    }
                }
                // Pour afficher/cacher le sac
                if (Interface.key.IsKeyDown(Keys.B) && Interface.Oldkey.IsKeyUp(Keys.B) || Interface.key.IsKeyDown(Keys.I) && Interface.Oldkey.IsKeyUp(Keys.I))
                    Interface.DrawSac = !Interface.DrawSac;
                // Pour afficher/cacher le sac
                if (Interface.key.IsKeyDown(Keys.C) && Interface.Oldkey.IsKeyUp(Keys.C))
                    Interface.DrawEquipement = !Interface.DrawEquipement;
                
                
                // Fait attaquer l'unité la plus proche
                if (isRoaming)
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
                }
            }
            // Pour verifier si on parle a un pnj
            SpeakToPNJ();
            // Pour déplacer la caméra
            CameraUpdate();
            Interface.Update();
            CurseurCheck();
            CheckWinandLose();
            Interface.Oldmouse = Interface.mouse;
            Interface.Oldkey = Interface.key;
        }

        public void DonnerOrdreDeplacer()
        {
            isRoaming = false;
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + Interface.mouse.X, camera.Position.Y + Interface.mouse.Y) / Map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);

            List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, Map.Taille, new List<Unite> { }, Map.unitesStatic, false);
                if (chemin != null)
                    champion.ObjectifListe = chemin;
                champion.uniteAttacked = null;
        }

        public bool DonnerOrdreAttaquer()
        {
            isRoaming = false;
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + Interface.mouse.X, camera.Position.Y + Interface.mouse.Y) / Map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);
            foreach (Unite u in Map.unites)
                if(u != champion && !u.isApnj && Outil.DistancePoints(ObjectifPoint, u .PositionTile) <= 64)
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
            isRoaming = true;
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

        public bool OnInventory()
        {
            return Interface.SacPosition.Intersects(new Rectangle(Interface.mouse.X, Interface.mouse.Y, 1, 1));
        }

        public bool OnEquipement()
        {
            return Interface.EquipementPosition.Intersects(new Rectangle(Interface.mouse.X, Interface.mouse.Y, 1, 1));
        }

        public bool ClickCheck(int i) // Renvoie vrai si le joueur clique sur le bouton i
        {
            int largeurBoutonSort = 32;
            return Interface.mouse.X >= Interface.BarreDesSortsPosition.X - 130 + i * largeurBoutonSort && Interface.mouse.X <= Interface.BarreDesSortsPosition.X - 130 + i * largeurBoutonSort + largeurBoutonSort && Interface.mouse.Y >= Interface.BarreDesSortsPosition.Y + 8 && Interface.mouse.Y <= Interface.BarreDesSortsPosition.Y + 8 + largeurBoutonSort && Interface.mouse.LeftButton == ButtonState.Pressed && Interface.Oldmouse.LeftButton == ButtonState.Released;
        }

        public bool SourisHoverCheck(int i) // Renvoie vrai si le joueur a la souris sur le bouton i
        {
            int largeurBoutonSort = 32;
            return Interface.mouse.X >= Interface.BarreDesSortsPosition.X - 130 + i * largeurBoutonSort && Interface.mouse.X <= Interface.BarreDesSortsPosition.X - 130 + i * largeurBoutonSort + largeurBoutonSort && Interface.mouse.Y >= Interface.BarreDesSortsPosition.Y + 8 && Interface.mouse.Y <= Interface.BarreDesSortsPosition.Y + 8 + largeurBoutonSort;
        }

        public void CheckWinandLose()
        {
            if (champion.PositionTile == new Vector2(8, 25) || champion.PositionTile == new Vector2(8, 26))
                Interface.Win = true;
            if (champion.Vie == 0)
                Interface.Lost = true;

        }
    }
}
