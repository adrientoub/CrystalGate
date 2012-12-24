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
        public UI Interface { get; set; }

        public Joueur(Unite champ)
        {
            champion = champ;

            // Graphique
            champion.Sprite = champ.packTexture.unites[1];
            champion.Tiles = new Vector2(380 / 5, 600 / 11);

            // Statistiques
            champion.Vie = champ.VieMax = 100;
            champion.Vitesse = 2.0f;
            champion.Portee = 2f; // 2 = Corps à corps
            champion.Dommages = 20;
            champion.Drawlife = true;
            champion.isAChamp = true;

            camera = new Camera2D(Vector2.One);
        }

        public void Update(List<Objet> unites)
        {
            mouse = Mouse.GetState();
            // Pour se déplacer
            if (mouse.RightButton == ButtonState.Pressed)
                DonnerOrdreDeplacer(champion, champion.Map);
            // Pour déplacer la caméra
            CameraCheck();
        }

        public void DonnerOrdreDeplacer(Unite unite, Map map)
        {
            Vector2 ObjectifPoint = new Vector2(camera.Position.X + mouse.X, camera.Position.Y + mouse.Y) / map.TailleTiles;
            ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);
                
            List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, unite.Map.Taille, new List<Objet> { }, map.unitesStatic, false);
                if (chemin != null)
                    unite.ObjectifListe = chemin;
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
            if (camera.Position.Y > champion.Map.Taille.Y * champion.Map.TailleTiles.Y - height + Interface.Bas.Height)
                camera.Position = new Vector2(camera.Position.X, champion.Map.Taille.Y * champion.Map.TailleTiles.Y - height + Interface.Bas.Height);
            
            //Update de la position de la caméra et de l'interface
            camera.Position = new Vector2(camera.Position.X, camera.Position.Y) + vec;
            Interface.BasPosition = new Rectangle((int)(camera.Position.X), (int)(height - Interface.Bas.Height + camera.Position.Y), (int)Interface.BasPosition.Width, (int)Interface.BasPosition.Height);
        }
    }
}
