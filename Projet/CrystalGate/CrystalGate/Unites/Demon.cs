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

        public Demon(Vector2 Position, Map map, PackTexture packTexture, int Level = 1)
            : base(Position, map, packTexture, Level)
        {
            // Graphique
            Sprite = packTexture.unites[4];
            Tiles = new Vector2( 385 / 5, 840 / 11);
            ProjectileSprite = packTexture.projectiles[2];

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
            Inventory = new List<Item> { new PotionDeMana(Vector2.Zero, packTexture) };

            // Sons
            effetUniteAttaque = new EffetSonore(8);
            effetUniteDeath = new EffetSonore(9);
            statsLevelUpdate();
        }


    }
}
