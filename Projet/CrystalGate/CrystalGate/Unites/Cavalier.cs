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

        public Cavalier(Vector2 Position, Map map, PackTexture packTexture, int Level = 1)
            : base(Position, map, packTexture, Level)
        {
            // Graphique
            Sprite = packTexture.unites[0];
            Tiles = new Vector2(370 / 5, 835 / 11);

            // Statistiques
            Vie = VieMax = 300;
            Vitesse = 0.2f;
            Vitesse_Attaque = 1f;
            Portee = 2f; // 1 = Corps à corps
            Dommages = 10;
            Puissance = 10;
            Defense = 10;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeVie(Vector2.Zero, packTexture) };

            // Sons
            effetUniteAttaque = new EffetSonore(0);
            effetUniteDeath = new EffetSonore(1);
            statsLevelUpdate();
        }

    }
}
