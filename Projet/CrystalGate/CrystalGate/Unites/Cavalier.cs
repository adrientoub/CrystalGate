using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Cavalier : Unite
    {

        public Cavalier(Vector2 Position, Map map, PackTexture packTexture)
            : base(Position, map, packTexture)
        {
            // Graphique
            Sprite = packTexture.unites[0];
            Tiles = new Vector2(521 / 5, 935 / 11);

            // Statistiques
            Vie = VieMax = 300;
            Vitesse = 2.5f;
            Vitesse_Attaque = 1f;
            Portee = 2f; // 1 = Corps à corps
            Dommages = 80;
            Defense = 10;

            // Sons
            effetUniteAttaque = new EffetSonore(0);
            effetUniteDeath = new EffetSonore(1);
        }

    }
}
