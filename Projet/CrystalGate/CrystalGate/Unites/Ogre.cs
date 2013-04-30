using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Ogre : Unite
    {

        public Ogre(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Graphique
            Sprite = PackTexture.Ogre;
            Tiles = new Vector2(370 / 5, 800 / 11);
            ProjectileSprite = PackTexture.projectiles[2];

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 0.15f;
            Vitesse_Attaque = 1f;
            Portee = 10f; // 2 = Corps à corps
            Dommages = 20;
            Puissance = 20;
            Defense = 5;
            IsRanged = true;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeMana(this, Vector2.Zero) };

            // Sons
            effetUniteAttaque = new EffetSonore(PackSon.DemonAttack);
            effetUniteDeath = new EffetSonore(PackSon.OgreDeath);
            statsLevelUpdate();
        }


    }
}
