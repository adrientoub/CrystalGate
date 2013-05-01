using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    public static class PackTexture
    {
        public static Texture2D blank;
        public static Texture2D tresor;
        public static List<Texture2D> sorts;
        public static List<Texture2D> boutons;
        public static List<Texture2D> projectiles;
        // Tiles
        public static Texture2D SummerTiles;
        public static Texture2D WinterTiles;
        public static Texture2D VolcanicTiles;
        // Polices
        public static SpriteFont gamefont;
        public static SpriteFont spellfont;
        // UI
        public static Texture2D Dialogue;
        public static Texture2D BarreDesSorts;
        public static Texture2D Curseur;
        public static Texture2D CurseurRouge;
        public static Texture2D Portrait;
        public static Texture2D Sac;
        public static Texture2D Equipement;
        // Portraits
        public static Texture2D CavalierPortrait;
        public static Texture2D GruntPortrait;
        public static Texture2D ArcherPortrait;
        // Unites
        public static Texture2D Archer;
        public static Texture2D Cavalier;
        public static Texture2D Demon;
        public static Texture2D Grunt;
        public static Texture2D Guerrier;
        public static Texture2D Ogre;
        public static Texture2D Troll;
        public static Texture2D Odin;
        public static Texture2D Assassin;
        // Autres
        public static Texture2D Critters;


        public static void Initialize(ContentManager content)
        {
            sorts = new List<Texture2D> { };
            boutons = new List<Texture2D> { };
            projectiles = new List<Texture2D> { };

            blank = content.Load<Texture2D>("blank");
            tresor = content.Load<Texture2D>("tresor");
           
            sorts.Add(content.Load<Texture2D>("Spells/Explosion"));
            sorts.Add(content.Load<Texture2D>("Spells/Soin"));
            sorts.Add(content.Load<Texture2D>("Spells/ManaRegen"));
            boutons = new List<Texture2D> { content.Load<Texture2D>("Boutons/Explosion"), content.Load<Texture2D>("Boutons/Soin"), content.Load<Texture2D>("Boutons/Invisibility"), content.Load<Texture2D>("Boutons/PotionDeVie"), content.Load<Texture2D>("Boutons/PotionMana"), content.Load<Texture2D>("Boutons/BloodLust"), content.Load<Texture2D>("Boutons/EpeeSolari"), content.Load<Texture2D>("Boutons/BottesDacier"), content.Load<Texture2D>("Boutons/Epaulieres"), content.Load<Texture2D>("Boutons/GantsDeDevotion"), content.Load<Texture2D>("Boutons/HelmutPurple"), content.Load<Texture2D>("Boutons/RingLionHead"), content.Load<Texture2D>("Boutons/Polymorph") };
            projectiles = new List<Texture2D> { content.Load<Texture2D>("Projectiles/arrow"), content.Load<Texture2D>("Projectiles/axe"), content.Load<Texture2D>("Projectiles/fireball") };
            
            // Fonts
            gamefont = content.Load<SpriteFont>("Polices/gameFont");
            spellfont = content.Load<SpriteFont>("Polices/SpellFont");

            // Tiles
            SummerTiles = content.Load<Texture2D>("PaletteEte");
            WinterTiles = content.Load<Texture2D>("PaletteHiver");
            VolcanicTiles = content.Load<Texture2D>("PaletteVolcanique");

            // UI
            Dialogue = content.Load<Texture2D>("UI/DialogueFond");
            BarreDesSorts = content.Load<Texture2D>("UI/barre des sorts");
            Curseur = content.Load<Texture2D>("curseur");
            CurseurRouge = content.Load<Texture2D>("UI/curseurRouge");
            Portrait = content.Load<Texture2D>("UI/GuerrierIcone");
            Sac = content.Load<Texture2D>("UI/inventaire");
            Equipement = content.Load<Texture2D>("UI/Equipement");

            // Portraits
            CavalierPortrait = content.Load<Texture2D>("UI/knightIcone");
            GruntPortrait = content.Load<Texture2D>("UI/gruntIcone");
            ArcherPortrait = content.Load<Texture2D>("UI/archerIcone");

            // Unites
            Archer = content.Load<Texture2D>("Unites/archer");
            Cavalier = content.Load<Texture2D>("Unites/knight");
            Demon = content.Load<Texture2D>("Unites/demon");
            Grunt = content.Load<Texture2D>("Unites/grunt");
            Guerrier = content.Load<Texture2D>("Unites/champion");
            Ogre = content.Load<Texture2D>("Unites/ogre");
            Troll = content.Load<Texture2D>("Unites/troll");
            Critters = content.Load<Texture2D>("Unites/critters");
            Odin = content.Load<Texture2D>("Unites/Odin");
            Assassin = content.Load<Texture2D>("Unites/Assassin");
        }
    }
}
