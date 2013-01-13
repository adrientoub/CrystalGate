﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Mur : Batiment
    {

        public Mur(Vector2 Position, Map map, SpriteBatch spriteBatch, PackTexture packTexture)
            : base(Position, map, spriteBatch, packTexture)
        {
            // Graphique par défaut
            PositionSprite = new Vector2(16, 0);
        }
    }
}
