using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Archer : Unite
    {

        public Archer(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Graphique
            Sprite = PackTexture.unites[2];
            Tiles = new Vector2( 295 / 5, 660 / 9);
            //PackAnimation.isArcher = true; // ca va faire buger tout le monde!
            ProjectileSprite = PackTexture.projectiles[0];

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 0.2f;
            Vitesse_Attaque = 1f;
            Portee = 15f; // 2 = Corps à corps
            Dommages = 10;
            Puissance = 10;
            Defense = 5;
            IsRanged = true;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeVie(this, Vector2.Zero) };

            // Sons
            effetUniteAttaque = new EffetSonore(6);
            effetUniteDeath = new EffetSonore(3);
            statsLevelUpdate();
        }


    }
}
