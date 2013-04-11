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

        public Grunt(Vector2 Position, Map map, PackTexture packTexture, int Level = 1)
            : base(Position, map, packTexture, Level)
        {
            // Graphique
            Sprite = packTexture.unites[1];
            Tiles = new Vector2( 380 / 5, 620 / 11);

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 0.15f;
            Vitesse_Attaque = 1f;
            Portee = 2f; // 2 = Corps à corps
            Dommages = 20;
            Puissance = 5;
            Defense = 5;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeVie(Vector2.Zero, packTexture) };

            // Sons
            effetUniteAttaque = new EffetSonore(2);
            effetUniteDeath = new EffetSonore(3);
            statsLevelUpdate();
        }


    }
}
