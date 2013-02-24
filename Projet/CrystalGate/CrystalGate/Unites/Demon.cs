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

        public Demon(Vector2 Position, Map map, PackTexture packTexture)
            : base(Position, map, packTexture)
        {
            // Graphique
            Sprite = packTexture.unites[4];
            Tiles = new Vector2( 385 / 5, 840 / 11);
            ProjectileSprite = packTexture.projectiles[2];

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 2.0f;
            Vitesse_Attaque = 1f;
            Portee = 10f; // 2 = Corps à corps
            Dommages = 10;
            Defense = 5;
            IsRanged = true;

            // Sons
            effetUniteAttaque = new EffetSonore(8);
            effetUniteDeath = new EffetSonore(9);
        }


    }
}
