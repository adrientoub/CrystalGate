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

        public Grunt(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Graphique
            Sprite = PackTexture.Grunt;
            Tiles = new Vector2( 380 / 5, 620 / 11);
            Portrait = PackTexture.GruntPortrait;

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 0.15f;
            Vitesse_Attaque = 1f;
            Portee = 2f; // 2 = Corps à corps
            Dommages = 20;
            Puissance = 5;
            Defense = 5;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeVie(this, Vector2.Zero) };

            // Sons
            effetUniteAttaque = new EffetSonore(2);
            effetUniteDeath = new EffetSonore(3);
            statsLevelUpdate();
        }


    }
}
