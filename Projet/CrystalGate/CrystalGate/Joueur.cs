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
        public Unite champion { get; set; }

        public Camera2D camera { get; set; }

        MouseState mouse { get; set; }
        MouseState Oldmouse { get; set; }
        KeyboardState key { get; set; }
        KeyboardState Oldkey { get; set; }
        public UI Interface { get; set; }
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
                mouse = Mouse.GetState();
                key = Keyboard.GetState();
                // Pour cibler un point pour un sort
                if (mouse.LeftButton == ButtonState.Pressed && Oldmouse.LeftButton == ButtonState.Released && InWaitingPoint)
                {
                    champion.pointCible = new Vector2((int)((camera.Position.X + mouse.X) / 32), (int)((camera.Position.Y + mouse.Y) / 32));
                    InWaitingPoint = false;
                    Interface.DrawSelectPoint = false;
                    champion.Cast(spell, champion.pointCible);
                }
                // Pour se déplacer
                if (mouse.RightButton == ButtonState.Pressed && !DonnerOrdreAttaquer())
                    DonnerOrdreDeplacer();
                // Pour attaquer un point
                if (key.IsKeyDown(Keys.A))
                    DonnerOrdreAttaquerPoint();
                // Pour arreter les déplacements
                if (key.IsKeyDown(Keys.S))
                    DonnerOrdreStop();
                // Pour lancer un sort
                if (key.IsKeyDown(Keys.D1) && champion.spells.Count > 0 || SourisCheck(0) && champion.spells.Count > 0)
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
                if (key.IsKeyDown(Keys.D2) && champion.spells.Count > 1 || SourisCheck(1) && champion.spells.Count > 1)
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
                if (key.IsKeyDown(Keys.D3) && champion.spells.Count > 2 || SourisCheck(2) && champion.spells.Count > 2)
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
                if (key.IsKeyDown(Keys.D4) && champion.spells.Count > 3 || SourisCheck(3) && champion.spells.Count > 3)
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
                if (key.IsKeyDown(Keys.B) && Oldkey.IsKeyUp(Keys.B) || key.IsKeyDown(Keys.I) && Oldkey.IsKeyUp(Keys.I))
                    Interface.DrawSac = !Interface.DrawSac;
                // Pour utiliser les objets
                if (mouse.X + camera.Position.X >= Interface.SacPosition.X && mouse.Y + camera.Position.Y >= Interface.SacPosition.Y)
                    if (mouse.LeftButton == ButtonState.Pressed && Oldmouse.LeftButton == ButtonState.Released)
                    {
                        int marge = 7;
                        Vector2 position = new Vector2(mouse.X - (Interface.width - Interface.Sac.Width), mouse.Y - (Interface.height- Interface.Sac.Height));
                        int indice = (int)position.X / (32 + marge) + (int)Interface.TailleSac.X * ((int)position.Y / (32 + marge));
                        if (champion.Inventory.Count > indice)
                            champion.Inventory[indice].Utiliser();
                    }
                
                // Fait attaquer l'unité la plus proche
                if (isRoaming)
                {
                    float distanceInit = 9000;
                    Unite focus = null;
                    foreach (Unite u in champion.Map.unites)
                    {
                        float distance = Outil.DistanceUnites(champion, u);

                        if (champion != u && distance <= distanceInit)
                        {
                            distanceInit = distance;
                            focus = u;
                        }
                    }
                    champion.uniteAttacked = focus;
                }
            }

            // Pour déplacer la caméra
            CameraCheck();
            CurseurCheck();
            CheckWinandLose();
            Oldmouse = mouse;
            Oldkey = key;
        }

        public void DonnerOrdreDeplacer()
        {
            isRoaming = false;
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + mouse.X, camera.Position.Y + mouse.Y) / champion.Map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);
                
            List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, champion.Map.Taille, new List<Unite> { }, champion.Map.unitesStatic, false);
                if (chemin != null)
                    champion.ObjectifListe = chemin;
                champion.uniteAttacked = null;
        }

        public bool DonnerOrdreAttaquer()
        {
            isRoaming = false;
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + mouse.X, camera.Position.Y + mouse.Y) / champion.Map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);
            foreach(Unite u in champion.Map.unites)
                if(u != champion && Outil.DistancePoints(ObjectifPoint, u .PositionTile) <= 64)
                {
                    if (true)
                    {
                        //champion.Attaquer(u);
                        champion.uniteAttacked = u;
                        List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, champion.Map.Taille, new List<Unite> { }, champion.Map.unitesStatic, false);
                        if (chemin != null)
                            champion.ObjectifListe = chemin;
                        return true;
                    }
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

        public void CameraCheck()
        {
            int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            int vitesse = 10;
            Vector2 vec = new Vector2();

            // Si on déplace la caméra hors des bords de l'écran
            if (mouse.X >= width - 1)
                vec.X += vitesse;
            if (mouse.X <= 1)
                vec.X -= vitesse;
            if (mouse.Y >= height - 15)
                vec.Y += vitesse;
            if (mouse.Y <= 1)
                vec.Y -= vitesse;

            // Si on sort de la map
            if (camera.Position.X > champion.Map.Taille.X * champion.Map.TailleTiles.X - width)
                camera.Position = new Vector2(champion.Map.Taille.X * champion.Map.TailleTiles.X - width, camera.Position.Y);
            if (camera.Position.X < 0)
                camera.Position = new Vector2(0, camera.Position.Y);
            if (camera.Position.Y < 0)
                camera.Position = new Vector2(camera.Position.X, 0);
            if (camera.Position.Y > champion.Map.Taille.Y * champion.Map.TailleTiles.Y - height + Interface.CadrePosition.Height)
                camera.Position = new Vector2(camera.Position.X, champion.Map.Taille.Y * champion.Map.TailleTiles.Y - height + Interface.CadrePosition.Height);
            
            //Update de la position de la caméra et de l'interface
            camera.Position = new Vector2(camera.Position.X, camera.Position.Y) + vec;
            Interface.Update();
        }

        public bool CurseurCheck()
        {
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + mouse.X, camera.Position.Y + mouse.Y) / champion.Map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);

            foreach (Unite u in champion.Map.unites)
                if (Outil.DistancePoints(ObjectifPoint, u.PositionTile) <= 32)
                {
                    Interface.CurseurOffensif = true;
                    return true;
                }

            Interface.CurseurOffensif = false;
            return false;
        }

        public bool SourisCheck(int i)
        {
            int largeurBoutonSort = 32;
            return mouse.X + camera.Position.X >= Interface.BarreDesSortsPosition.X - 130 + i * largeurBoutonSort && mouse.X + camera.Position.X <= Interface.BarreDesSortsPosition.X - 130 + i * largeurBoutonSort + largeurBoutonSort && mouse.Y + camera.Position.Y >= Interface.BarreDesSortsPosition.Y + 8 && mouse.Y + camera.Position.Y <= Interface.BarreDesSortsPosition.Y + 8 + largeurBoutonSort && mouse.LeftButton == ButtonState.Pressed && Oldmouse.LeftButton == ButtonState.Released;
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
