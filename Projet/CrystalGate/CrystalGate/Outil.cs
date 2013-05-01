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
            return (float)Math.Sqrt(Math.Pow(32 * (point1.X - point2.X), 2) + Math.Pow(32 * (point1.Y - point2.Y), 2));
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

        public static void OuvrirMap(string MapName)
        {
            // Read the file and display it line by line.
            string MapString;
            if (CrystalGateGame.isTest)
                MapString = "../../../Maps/" + MapName + ".txt";
            else
                MapString = "Maps/" + MapName + ".txt";

            string line;
            int longueur = 0;
            int hauteur = 0;
            StreamReader file = new StreamReader(@MapString);
            // On lit le header pour determiner la palette et le type de terrain
            Texture2D TileSelected;
            switch (int.Parse(file.ReadLine()))
            {
                case 1: Map.typeDeTerrain = Map.TypeDeTerrain.Herbe;
                    TileSelected = PackTexture.SummerTiles;
                    break;
                case 2: Map.typeDeTerrain = Map.TypeDeTerrain.Desert;
                    TileSelected = PackTexture.SummerTiles;
                    break;
                case 3: Map.typeDeTerrain = Map.TypeDeTerrain.Hiver;
                    TileSelected = PackTexture.WinterTiles;
                    break;
                case 4: Map.typeDeTerrain = Map.TypeDeTerrain.Volcanique;
                    TileSelected = PackTexture.VolcanicTiles;
                    break;
                default: Map.typeDeTerrain = Map.TypeDeTerrain.Herbe;
                    TileSelected = PackTexture.SummerTiles;
                    break;
            }
            // On établit la longueur et la hauteur
            while ((line = file.ReadLine()) != null)
            {
                char[] splitchar = { '|' };

                if (line != null)
                    longueur = line.Split(splitchar).Length - 1;
                hauteur++;
            }
            // Creation de la carte
            Map.Initialize(TileSelected, new Vector2(longueur, hauteur), new Vector2(32, 32));
            // Reset
            file = new StreamReader(@MapString);
            file.ReadLine(); // On saute le header
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
                    Map.Cellules[i, j] = new Vector2(x, y);
                    // Si c'est une tile infranchissable
                    if (Outil.ProhibedTiles().Contains(new Vector2(x, y)))
                    {
                        // On ajoute l'obstacle au monde physique
                        Body bodyTemp = BodyFactory.CreateRectangle(Map.world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(32), 100f);
                        bodyTemp.Position = ConvertUnits.ToSimUnits(new Vector2(i, j) * Map.TailleTiles + new Vector2(16, 16));
                        Map.unitesStatic[i, j] = new Noeud(new Vector2(i, j), false, 1);
                    }
                }
                j++;
            }
            file.Close();

        }

        public static List<Vector2> ProhibedTiles()
        {
            List<Vector2> pT = new List<Vector2> { };

            if (Map.typeDeTerrain == Map.TypeDeTerrain.Herbe)
            {
                for (int j = 0; j < 9; j++)
                    for (int i = 0; i < 19; i++)
                        pT.Add(new Vector2(i, j));
                pT.Remove(new Vector2(12, 6));

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
            }
            else
            {
                for (int j = 0; j < 9; j++)
                    for (int i = 0; i < 19; i++)
                        pT.Add(new Vector2(i, j));
                pT.Remove(new Vector2(12, 6));
                

                // Eau
                for (int j = 15; j < 16; j++)
                    for (int i = 0; i < 19; i++)
                        pT.Add(new Vector2(i, j));
            }

            return pT;
        }

        public static string Normalize(int maxParLigne, string description)
        {
            string rendu = "";

            int i = 0;
            string[] descriptionWords = description.Split(new char[] { ' ' });

            for (int j = 0; j < descriptionWords.Length; j++)
            {
                if (descriptionWords[j].Length + i <= maxParLigne)
                {
                    rendu += descriptionWords[j] + " ";
                    i += descriptionWords[j].Length + 1;
                }
                else if (descriptionWords[j].Length <= maxParLigne)
                {
                    rendu += "\n" + descriptionWords[j] + " ";
                    i = descriptionWords[j].Length + 1;
                }
                else
                {
                    rendu += "\n";
                    i = 0;
                    foreach (char c in descriptionWords[j])
                    {
                        if (i < maxParLigne)
                        {
                            rendu += c;
                            i++;
                        }
                        else
                        {
                            rendu += "\n" + c.ToString();
                            i = 0;
                        }
                    }
                }
            }
            return rendu;
        }

        enum Direction
        {
            Haut,
            Bas,
            Gauche,
            Droite,
            HautDroite,
            BasDroite,
            HautGauche,
            BasGauche
        }
    }
}
