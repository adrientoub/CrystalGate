using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalGate.SceneEngine2;
using Microsoft.Xna.Framework;
using CrystalGate.Unites;

namespace CrystalGate
{
    static class PackMap
    {
        static List<Unite>[] Unites = new List<Unite>[10];
        static List<Item>[] Items = new List<Item>[10];
        static List<Wave>[] Waves = new List<Wave>[10];
        static List<Effet>[] Effets = new List<Effet>[10];
        public static Joueur j = null;

        public static void Initialize()
        {
            for (int i = 0; i < Unites.Length; i++)
            {
                Unites[i] = new List<Unite> { };
                Items[i] = new List<Item> { };
                Waves[i] = new List<Wave> { };
                Effets[i] = new List<Effet> { };
            }
            // Creation du joueur et du champion
            j = new Joueur(new Guerrier(new Vector2(0, 9)));
            // Chargement des items et waves du level1
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
            // Pas tres propre
            if (SceneHandler.level == "level1")
                FondSonore.Play(5);
            else
                FondSonore.Play(0);
        }

        public static void LoadLevel(string level)
        {
            // On restitue les infos du packMap dans la Map
            int i = LevelToInt(SceneHandler.level);
            Map.unites = Unites[i];
            // Le champion s'est téléporté, on le rajoute sur la Map
            Map.unites.Add(j.champion);
            Map.joueurs = new List<Joueur> { j };
            Map.items = Items[i];
            Map.waves = Waves[i];
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
            int c = 0;
            foreach (Wave w in Waves[i])
                c += w.unites.Count;
            Map.nombreDeVagues = c;
            return i;
        }

        static void InitializeLevel1()
        {
            Waves[0] = PackWave.PackWaveLevel1();
            Unite Pnj = new Syndra(new Vector2(49, 39));
            Pnj.Dialogue.Clear();
            Pnj.Dialogue.Add(new Text("Dialogue1a"));
            Pnj.Dialogue.Add(new Text("Dialogue1b"));
            Unites[0].Add(Pnj);
            Items[0].Add(new PotionDeVie(null, new Vector2(22, 24)));
            Items[0].Add(new PotionDeVie(null, new Vector2(23, 24)));
            Items[0].Add(new EpeeSolari(j.champion, new Vector2(50, 39)));

            j.champion.Inventory.Add(new GantsDeDevotion(j.champion, Vector2.One));
            j.champion.Inventory.Add(new Epaulieres(j.champion, Vector2.One));
            j.champion.Inventory.Add(new HelmetPurple(j.champion, Vector2.One));
            j.champion.Inventory.Add(new RingLionHead(j.champion, Vector2.One));

        }

        static void InitializeLevel2()
        {
            Unite Pnj = new Syndra(new Vector2(7, 18));
            Pnj.Dialogue.Clear();
            Text a = new Text("Dialogue2a");
            Text b = new Text("Dialogue2b");
            Pnj.Dialogue.Add(a);
            Pnj.Dialogue.Add(b);
            
            Unites[1].Add(Pnj);
            Items[1].Add(new BottesDacier(j.champion, new Vector2(10,15)));
            Waves[1] = PackWave.PackWaveLevel2();
        }
    }
}
