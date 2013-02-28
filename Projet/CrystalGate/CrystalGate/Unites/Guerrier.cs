﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Guerrier : Unite
    {

        public Guerrier(Vector2 Position, Map map, PackTexture packTexture, int Level = 1)
            : base(Position, map, packTexture, Level)
        {
            // Graphique
            Sprite = packTexture.unites[6];
            Tiles = new Vector2(530 / 5, 930 / 11);

            // Statistiques
            Vie = VieMax = 500;
            Vitesse = 2.0f;
            Vitesse_Attaque = 0.5f;
            Portee = 2f; // 1 = Corps à corps
            Dommages = 70;
            Puissance = 50;
            Defense = 5;
            DefenseMagique = 3;
            XPUnite = 200;

            // Sons
            effetUniteAttaque = new EffetSonore(0);
            effetUniteDeath = new EffetSonore(1);
            statsLevelUpdate();
        }

    }
}
