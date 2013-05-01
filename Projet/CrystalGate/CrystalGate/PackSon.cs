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
    public static class PackSon
    {
        // Attaques
        public static SoundEffect Epee;
        public static SoundEffect GruntAttack;
        public static SoundEffect ArcherAttack;
        public static SoundEffect TrollAttack;
        public static SoundEffect DemonAttack;
        public static SoundEffect GruntDeath;
        public static SoundEffect CavalierDeath;
        public static SoundEffect DemonDeath;
        public static SoundEffect OgreDeath;
        public static SoundEffect SheepDeath;
        // Sorts
        public static SoundEffect Soin;
        public static SoundEffect Explosion;
        public static SoundEffect InvisibiliteCible;
        public static SoundEffect FurieSanguinaireCible;
        public static SoundEffect PolymorphCible;
        // Autres
        public static SoundEffect LevelUp;
        public static SoundEffect Victory, Defeat;


        public static void Initialize(ContentManager content)
        {
            // Sons d'attaque
            Epee = content.Load<SoundEffect>("Sons/sword3"); // Attaque cavalier
            GruntAttack = content.Load<SoundEffect>("Sons/GruntAttack");
            ArcherAttack = content.Load<SoundEffect>("Sons/ArcherAttack");
            TrollAttack = content.Load<SoundEffect>("Sons/AxeMissileLaunch1");
            DemonAttack = content.Load<SoundEffect>("Sons/DemonMissileLaunch1");

            // Sons de mort
            GruntDeath = content.Load<SoundEffect>("Sons/Gruntquimeurt");
            CavalierDeath = content.Load<SoundEffect>("Sons/Cavalierquimeurt");
            DemonDeath = content.Load<SoundEffect>("Sons/DemonDeath1");
            OgreDeath = content.Load<SoundEffect>("Sons/OgreDeath1");
            SheepDeath = content.Load<SoundEffect>("Sons/SheepDeath");
            
            // Sons des sorts.
            Soin = content.Load<SoundEffect>("Sons/soin");
            Explosion = content.Load<SoundEffect>("Sons/explosion");
            InvisibiliteCible = content.Load<SoundEffect>("Sons/InvisibilityTarget");
            FurieSanguinaireCible = content.Load<SoundEffect>("Sons/BloodlustTarget");
            PolymorphCible = content.Load<SoundEffect>("Sons/PolymorphTarget");

            // Autres
            LevelUp = content.Load<SoundEffect>("Sons/LevelUp");
            EffetSonore.InitEffects();
        }
    }
}

