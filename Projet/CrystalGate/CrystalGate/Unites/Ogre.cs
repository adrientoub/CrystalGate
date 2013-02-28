using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Ogre : Unite
    {

        public Ogre(Vector2 Position, Map map, PackTexture packTexture, int Level = 1)
            : base(Position, map, packTexture, Level)
        {
            // Graphique
            Sprite = packTexture.unites[5];
            Tiles = new Vector2(370 / 5, 800 / 11);
            ProjectileSprite = packTexture.projectiles[2];

            // Statistiques
            Vie = VieMax = 200;
            Vitesse = 2.0f;
            Vitesse_Attaque = 1f;
            Portee = 10f; // 2 = Corps à corps
            Dommages = 20;
            Puissance = 20;
            Defense = 5;
            IsRanged = true;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeMana(Vector2.Zero, packTexture) };

            // Sons
            effetUniteAttaque = new EffetSonore(8);
            effetUniteDeath = new EffetSonore(3);
            statsLevelUpdate();
        }


    }
}
