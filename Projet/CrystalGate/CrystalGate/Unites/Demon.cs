using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Demon : Unite
    {

        public Demon(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Graphique
            Sprite = PackTexture.Demon;
            Tiles = new Vector2( 385 / 5, 840 / 11);
            ProjectileSprite = PackTexture.projectiles[2];

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 0.15f;
            Vitesse_Attaque = 1f;
            Portee = 10f; // 2 = Corps à corps
            Dommages = 10;
            Puissance = 20;
            Defense = 5;
            IsRanged = true;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeMana(this, Vector2.Zero) };

            // Sons
            effetUniteAttaque = new EffetSonore(8);
            effetUniteDeath = new EffetSonore(9);
            statsLevelUpdate();
        }


    }
}
