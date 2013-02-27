﻿using System;
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

        public Archer(Vector2 Position, Map map, PackTexture packTexture)
            : base(Position, map, packTexture)
        {
            // Graphique
            Sprite = packTexture.unites[2];
            Tiles = new Vector2( 295 / 5, 660 / 9);
            packAnimation.isArcher = true;
            ProjectileSprite = packTexture.projectiles[0];

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 2.0f;
            Vitesse_Attaque = 1f;
            Portee = 15f; // 2 = Corps à corps
            Dommages = 10;
            Defense = 5;
            IsRanged = true;
            XPUnite = 200;

            // Sons
            effetUniteAttaque = new EffetSonore(6);
            effetUniteDeath = new EffetSonore(3);
        }


    }
}
