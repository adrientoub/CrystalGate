﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Troll : Unite
    {

        public Troll(Vector2 Position, Map map, PackTexture packTexture, int Level = 1)
            : base(Position, map, packTexture, Level)
        {
            // Graphique
            Sprite = packTexture.unites[3];
            Tiles = new Vector2( 311 / 5, 620 / 11);
            ProjectileSprite = packTexture.projectiles[1];

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 2.0f;
            Vitesse_Attaque = 1f;
            Portee = 10f; // 2 = Corps à corps
            Dommages = 10;
            Puissance = 10;
            Defense = 5;
            IsRanged = true;
            XPUnite = 200;

            // Sons
            effetUniteAttaque = new EffetSonore(7);
            effetUniteDeath = new EffetSonore(3);
            statsLevelUpdate();
        }


    }
}
