using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalGate.SceneEngine2;
using Microsoft.Xna.Framework;
using CrystalGate.Unites;
using FarseerPhysics.Dynamics;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;

namespace CrystalGate
{
    static class PackMap
    {
        static List<Unite>[] Unites = new List<Unite>[10];
        static List<Item>[] Items = new List<Item>[10];
        public static List<Wave>[] Waves = new List<Wave>[10];
        static List<Effet>[] Effets = new List<Effet>[10];
        // Represente les tiles
        static Vector2[][,] Cellules = new Vector2[10][,];
        // Represente les noeuds non franchissables
        static Noeud[][,] prohibedTiles = new Noeud[10][,];
        // Represente la palette de tuile du niveau et le type de terrain
        static Texture2D[] Palettes = new Texture2D[10];
        static Map.TypeDeTerrain[] typeDeTerrain = new Map.TypeDeTerrain[10];
        static World[] Worlds = new World[10];
        // Represente la taille de chaque carte
        static Vector2[] Taille = new Vector2[10];
        public static int NbDeJoueur = 3;

        public static List<Joueur> joueurs = new List<Joueur> { };

        public static void Initialize()
        {
            for (int i = 0; i < Unites.Length; i++)
            {
                Unites[i] = new List<Unite> { };
                Items[i] = new List<Item> { };
                Waves[i] = new List<Wave> { };
                Effets[i] = new List<Effet> { };
                Worlds[i] = new World(Vector2.Zero);
            }
            
            // Chargement des textures : Pack de texture (Contient toutes les sprites des unites et des sorts)
            PackTexture.Initialize(SceneHandler.content);

            // Chargement sons
            PackSon.Initialize(SceneHandler.content);
            FondSonore.Load(SceneHandler.content);

            // On initialise la carte avec les tuiles
            OuvrirMap("level1");
            OuvrirMap("level2");

            // Creation du joueur et du champion
            Map.world = Worlds[0]; // pas très propre mais marche

            // Ici on chargera les infos du champion via un fichier texte, en attendant...
            
            // Chargement des items et waves
            InitLevels();
        }

        public static void InitLevels()
        {
            for (int i = 0; i < Unites.Length; i++)
            {
                Unites[i] = new List<Unite> { };
                Items[i] = new List<Item> { };
                Waves[i] = new List<Wave> { };
                Effets[i] = new List<Effet> { };
                Worlds[i] = new World(Vector2.Zero);
            }
            InitializeLevel1();
            InitializeLevel2();
        }

        public static void Sauvegarder()
        {
            // On stock les infos de la Map dans le PackMap
            int i = LevelToInt(SceneHandler.level);
            Unites[i] = Map.unites;
            // Le champion se téléporte, on le supprime donc de la Map
            Unites[i].RemoveAll(p => p.isAChamp);
            Items[i] = Map.items;
            Waves[i] = Map.waves;
            Effets[i] = Map.effets;
            // Pas tres propre, le changement de musique
            if (SceneHandler.level == "level1")
                FondSonore.Play(5);
            else
                FondSonore.Play(0);
        }

        public static void LoadLevel(string level)
        {
            // On restitue les infos du packMap dans la Map
            int i = LevelToInt(level);
            Map.unites = Unites[i];
            // Le champion s'est téléporté, on le rajoute sur la Map
            foreach (Joueur j in joueurs)
            {
                Map.unites.Add(j.champion);
                Map.unites[Map.unites.Count - 1].id = (byte)Map.unites.Count;
            }

            Map.items = Items[i];
            Map.waves = Waves[i];
            Map.effets = Effets[i];
            Map.Cellules = Cellules[i];
            Map.unitesStatic = prohibedTiles[i];
            Map.Sprite = Palettes[i];
            Map.typeDeTerrain = typeDeTerrain[i];
            Map.Taille = Taille[i];
            Map.world = Worlds[i];
            // On reset le compteur
            ResetCompteur();
        }

        public static void LoadPlayers()// charge le joueur et le met sur la map
        {
            joueurs.Clear();
            if (SceneHandler.gameplayScene.isCoopPlay)
            {
                for (int i = 0; i < Client.joueursConnectes.Count; i++)
                {
                            if (Client.joueursConnectes[i].championChoisi == 0)
                                joueurs.Add(new Joueur(new Guerrier(new Vector2(0, 9))));
                            else
                                joueurs.Add(new Joueur(new Voleur(new Vector2(0, 11))));

                            joueurs[joueurs.Count - 1].id = joueurs.Count;
                }
            }

            else
            {
                joueurs.Add(new Joueur(new Voleur(new Vector2(0, 9))));
                // On spécifie le joueur local
            }

        }

        public static void OuvrirMap(string MapName)
        {
            int id = LevelToInt(MapName); // numero de la carte dans le tableau
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

            switch (int.Parse(file.ReadLine()))
            {
                case 1: typeDeTerrain[id] = Map.TypeDeTerrain.Herbe;
                    Palettes[id] = PackTexture.SummerTiles;
                    break;
                case 2: typeDeTerrain[id] = Map.TypeDeTerrain.Desert;
                    Palettes[id] = PackTexture.SummerTiles;
                    break;
                case 3: typeDeTerrain[id] = Map.TypeDeTerrain.Hiver;
                    Palettes[id] = PackTexture.WinterTiles;
                    break;
                case 4: typeDeTerrain[id] = Map.TypeDeTerrain.Volcanique;
                    Palettes[id] = PackTexture.VolcanicTiles;
                    break;
                default: typeDeTerrain[id] = Map.TypeDeTerrain.Herbe;
                    Palettes[id] = PackTexture.SummerTiles;
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
            // On initialise la taille de notre map
            Cellules[LevelToInt(MapName)] = new Vector2[longueur, (int)hauteur];
            prohibedTiles[LevelToInt(MapName)] = new Noeud[longueur, (int)hauteur];
            Taille[LevelToInt(MapName)] = new Vector2(longueur, hauteur);
            
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
                    Cellules[id][i, j] = new Vector2(x, y);
                    // Si c'est une tile infranchissable
                    if (ProhibedTiles(MapName).Contains(new Vector2(x, y)))
                    {
                        // On ajoute l'obstacle au monde physique
                        // LES UNITES PARTAGENT LE MEME WORLD OMFG
                        Body bodyTemp = BodyFactory.CreateRectangle(Worlds[id], ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(32), 100f);
                        bodyTemp.Position = ConvertUnits.ToSimUnits(new Vector2(i, j) * Map.TailleTiles + new Vector2(16, 16));
                        prohibedTiles[id][i, j] = new Noeud(new Vector2(i, j), false, 1);
                    }
                }
                j++;
            }
            file.Close();

        }

        public static List<Vector2> ProhibedTiles(string Mapname)
        {
            List<Vector2> pT = new List<Vector2> { };

            if (typeDeTerrain[LevelToInt(Mapname)] == Map.TypeDeTerrain.Herbe)
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

        public static List<Vector2> ProhibedTiles()
        {
            return ProhibedTiles(SceneHandler.level);
        }

        static int LevelToInt(string level)
        {
            int i = 0;
            switch (level)
            {
                case "level1": i = 0;
                    break;
                case "level2": i = 1;
                    break;
                default: throw new Exception("Niveau incorrect");
            }
            return i;
        }

        static void ResetCompteur()
        {
            Map.nombreDeVaguesPop = 0;
            int nb = 0;
            foreach (Wave w in PackMap.Waves[PackMap.LevelToInt(SceneHandler.level)])
                foreach (List<Unite> u in w.unites)
                    nb++;

            Map.nombreDeVagues = nb;
        }

        static void InitializeLevel1()
        {
            Waves[0] = PackWave.PackWaveLevel1();
            Unite Pnj = new Syndra(new Vector2(49, 39));
            Pnj.Dialogue.Clear();
            Pnj.Dialogue.Add(new Text("Dialogue1a"));
            Pnj.Dialogue.Add(new Text("Dialogue1b"));

            Unites[0] = new List<Unite> { Pnj };
            Items[0] = new List<Item>{
                new PotionDeVie(null, new Vector2(22, 24)),
                new PotionDeVie(null, new Vector2(23, 24)),
                new EpeeSolari(null, new Vector2(50, 39))}; // bug, j.champion
            foreach (Joueur j in joueurs)
            {
                j.champion.Inventory = new List<Item>{
                new GantsDeDevotion(j.champion, Vector2.One),
                new Epaulieres(j.champion, Vector2.One),
                new HelmetPurple(j.champion, Vector2.One),
                new RingLionHead(j.champion, Vector2.One)};
            }
        }

        static void InitializeLevel2()
        {
            Unite Pnj = new Syndra(new Vector2(7, 18));
            Text a = new Text("Dialogue2a");
            Text b = new Text("Dialogue2b");
            Pnj.Dialogue.Clear();
            Pnj.Dialogue.Add(a);
            Pnj.Dialogue.Add(b);

            Unites[1] = new List<Unite> { Pnj };
            Items[1] = new List<Item>{new BottesDacier(null, new Vector2(10,15))}; // bug, j.champion
            Waves[1] = PackWave.PackWaveLevel2();
        }
    }
}
