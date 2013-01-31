using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

namespace CrystalGate
{
    class Outil
    {
        public static bool Collision(Objet box1, Objet box2)
        {
            if ((box2.body.Position.X >= box1.body.Position.X + ConvertUnits.ToSimUnits(32))      // trop à droite
             || (box2.body.Position.X + ConvertUnits.ToSimUnits(32) <= box1.body.Position.X) // trop à gauche
             || (box2.body.Position.Y >= box1.body.Position.Y + ConvertUnits.ToSimUnits(32)) // trop en bas
             || (box2.body.Position.Y + ConvertUnits.ToSimUnits(32) <= box1.body.Position.Y))  // trop en haut
                return false;
            else
                return true;
        }

        public static float AngleUnites(Objet unite1, Objet unite2)
        {
            return (float)Math.Atan2(unite1.body.Position.Y - unite2.body.Position.Y, unite1.body.Position.X - unite2.body.Position.X);
        }

        public static float DistanceUnites(Objet unite1, Objet unite2)
        {
            return (float)Math.Sqrt(Math.Pow(ConvertUnits.ToDisplayUnits(unite1.body.Position - unite2.body.Position).X, 2) + Math.Pow(ConvertUnits.ToDisplayUnits(unite1.body.Position - unite2.body.Position).Y, 2));
        }
        public static float DistancePoints(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt( Math.Pow(32 * (point1.X - point2.X), 2) + Math.Pow(32 * (point1.Y - point2.Y), 2));
        }

        public static void RemoveDeadBodies(List<Unite> unites)
        {
            for (int i = 0; i < unites.Count; i++)
                if (unites[i].Mort)
                    unites.RemoveAt(i);
        }

        public static List<Unite> ObjetToUnits(List<Objet> objets)
        {
            List<Unite> u = new List<Unite> { };
            foreach (Objet o in objets)
                u.Add((Unite)o);

            return u;
        }

        public static void OuvrirMap(string MapName, ref Map map, PackTexture pack)
        {
            // Read the file and display it line by line.
            string mapString = "../../../Maps/" + MapName + ".txt";
            string line;
            int longueur = 0;
            int hauteur = 0;
            StreamReader file = new StreamReader(@mapString);
            // On établit la longueur et la hauteur
            while ((line = file.ReadLine()) != null)
            {
                char[] splitchar = { '|' };

                if (line != null)
                    longueur = line.Split(splitchar).Length - 1;
                hauteur++;
            }
            // Creation de la carte
            map = new Map(pack.map[0], new Vector2(longueur, hauteur), new Vector2(32, 32));
            // Reset
            file = new StreamReader(@mapString);
            int j = 0;
            while ((line = file.ReadLine()) != null)
            {
                char[] splitchar = { '|' };
                string[] tiles = line.Split(splitchar);

                for (int i = 0; i < longueur; i++)
                {
                    char[] splitchar2 = { ',' };
                    int x = int.Parse((tiles[i].Split(splitchar2))[0]);
                    int y = int.Parse((tiles[i].Split(splitchar2))[1]);
                    map.Cellules[i, j] = new Vector2(x, y);
                    // Si c'est une tile infranchissable
                    if (Outil.ProhibedTiles().Contains(new Vector2(x, y)))
                    {
                        // On ajoute l'obstacle au monde physique
                        Body bodyTemp = BodyFactory.CreateRectangle(map.world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(32), 100f);
                        bodyTemp.Position = ConvertUnits.ToSimUnits(new Vector2(i, j) * map.TailleTiles + new Vector2(16, 16));
                        map.unitesStatic[i, j] = new Noeud(new Vector2(i, j), false, 1);
                    }
                }
                j++;
            }
            file.Close();

        }

        public static List<Vector2> ProhibedTiles()
        {
            List<Vector2> pT = new List<Vector2> { };
            for (int j = 0; j < 9; j++)
                for (int i = 0; i < 19; i++)
                    pT.Add(new Vector2(i, j));

            for (int i = 0; i < 9; i++)
                pT.Add(new Vector2(i, 9));

            for (int i = 16; i < 19; i++)
                pT.Add(new Vector2(i, 10));

            for (int i = 0; i < 19; i++)
                pT.Add(new Vector2(i, 11));

            for (int i = 0; i < 10; i++)
                pT.Add(new Vector2(i, 12));

            for (int i = 15; i < 19; i++)
                pT.Add(new Vector2(i, 15));

            for (int i = 0; i < 19; i++)
                pT.Add(new Vector2(i, 16));

            for (int i = 0; i < 11; i++)
                pT.Add(new Vector2(i, 17));
            
            return pT;
        }

        public static void LoadSprites(ref PackTexture pack, ContentManager content)
        {
            pack = new PackTexture(content.Load<Texture2D>("blank"));
            pack.unites = new List<Texture2D> { content.Load<Texture2D>("knight"), content.Load<Texture2D>("grunt") };
            pack.sorts.Add(content.Load<Texture2D>("Spells/Explosion"));
            pack.sorts.Add(content.Load<Texture2D>("Spells/Soin"));
            pack.boutons = new List<Texture2D> { content.Load<Texture2D>("Boutons/Explosion"), content.Load<Texture2D>("Boutons/Soin") };
            pack.map.Add(content.Load<Texture2D>("summertiles"));
        }

        public static void LoadSounds(List<SoundEffect> listeSound,  ContentManager content)
        {
            // Les sons.
            listeSound.Add(content.Load<SoundEffect>("Sons/sword3")); // Attaque cavalier
            listeSound.Add(content.Load<SoundEffect>("Sons/Cavalierquimeurt"));
            listeSound.Add(content.Load<SoundEffect>("Sons/GruntAttack"));
            listeSound.Add(content.Load<SoundEffect>("Sons/Gruntquimeurt"));
            // Sons des sorts.
            listeSound.Add(content.Load<SoundEffect>("Sons/soin"));
            listeSound.Add(content.Load<SoundEffect>("Sons/explosion"));
            listeSound.Add(content.Load<SoundEffect>("Sons/ArcherAttack"));

            EffetSonore.InitEffects();
        }
    }
}
