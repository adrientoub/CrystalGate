using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Grunt : Unite
    {

        public Grunt(Vector2 Position, Map map, PackTexture packTexture)
            : base(Position, map, packTexture)
        {
            // Graphique
            Sprite = packTexture.unites[1];
            Tiles = new Vector2( 380 / 5, 620 / 11);

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 2.0f;
            Vitesse_Attaque = 1f;
            Portee = 2f; // 2 = Corps à corps
            Dommages = 20;
            Defense = 5;
            XPUnite = 200;

            // Sons
            effetUniteAttaque = new EffetSonore(2);
            effetUniteDeath = new EffetSonore(3);
        }


    }
}
