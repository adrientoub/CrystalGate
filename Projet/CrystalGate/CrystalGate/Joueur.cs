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
        public UI Interface { get; set; }
        bool InWaitingPoint;
        Vector2 point;
        int spell;

        bool isRoaming;

        public Joueur(Unite champ)
        {
            champion = champ;

            // Statistiques
            champion.Drawlife = true;
            champion.isAChamp = true;

            camera = new Camera2D(Vector2.One);
        }

        public void Update(List<Objet> unites)
        {
            if (!champion.Mort)
            {

                mouse = Mouse.GetState();
                key = Keyboard.GetState();
                // Pour cibler un point pour un sort
                if (mouse.LeftButton == ButtonState.Pressed && InWaitingPoint)
                {
                    point = new Vector2((int)((camera.Position.X + mouse.X) / 32), (int)((camera.Position.Y + mouse.Y) / 32));
                    InWaitingPoint = false;
                    Interface.DrawSelectPoint = false;
                    champion.Cast(spell, point);
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
                //int i = 0;
                if (key.IsKeyDown(Keys.D1)/* || mouse.X + camera.Position.X >= Interface.BarreDesSortsPosition.X - 128 + i * 32 + 3 && mouse.X + camera.Position.X <= Interface.BarreDesSortsPosition.X - 128 + i * 32 + 3 + 32 && mouse.Y + camera.Position.Y >= Interface.BarreDesSortsPosition.Y + 8 && mouse.Y + camera.Position.Y <= Interface.BarreDesSortsPosition.Y + 8 + 32 && mouse.LeftButton == ButtonState.Pressed && Oldmouse.LeftButton == ButtonState.Released*/)
                {
                    spell = 0;
                    if (champion.Map.gametime.TotalGameTime.TotalMilliseconds - champion.spells[spell].LastCast > champion.spells[spell].Cooldown * 1000 && champion.spells[spell].NeedUnPoint)
                    {
                        Interface.DrawSelectPoint = true;
                        InWaitingPoint = true;
                    }
                }
                if (key.IsKeyDown(Keys.D2))
                {
                    spell = 1;
                    if (champion.Map.gametime.TotalGameTime.TotalMilliseconds - champion.spells[spell].LastCast > champion.spells[spell].Cooldown * 1000)
                    {
                        champion.Cast(spell, point);
                    }
                }
                // Pour Update et Draw les sorts
                foreach (Spell s in champion.spells)
                    if (s.ToDraw)
                        s.Update(point);

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
            CheckWinandLose();
            Oldmouse = mouse;
        }

        public void DonnerOrdreDeplacer()
        {
            isRoaming = false;
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + mouse.X, camera.Position.Y + mouse.Y) / champion.Map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);
                
            List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, champion.Map.Taille, new List<Objet> { }, champion.Map.unitesStatic, false);
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
                if(u != champion && u.PositionTile == ObjectifPoint)
                {
                    //champion.Attaquer(u);
                    champion.uniteAttacked = u;
                    List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, champion.Map.Taille, new List<Objet> { }, champion.Map.unitesStatic, false);
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
            if (camera.Position.Y > champion.Map.Taille.Y * champion.Map.TailleTiles.Y - height + Interface.BarreDesSorts.Height)
                camera.Position = new Vector2(camera.Position.X, champion.Map.Taille.Y * champion.Map.TailleTiles.Y - height + Interface.BarreDesSorts.Height);
            
            //Update de la position de la caméra et de l'interface
            camera.Position = new Vector2(camera.Position.X, camera.Position.Y) + vec;
            Interface.Update();
        }

        public void CheckWinandLose()
        {
            if (champion.PositionTile == new Vector2(8, 25) || champion.PositionTile == new Vector2(8, 26))
            {
                Interface.Win = true;
            }
            if (champion.Vie == 0)
                Interface.Lost = true;

        }
    }
}
