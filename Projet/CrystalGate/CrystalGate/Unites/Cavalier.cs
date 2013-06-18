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

        public Cavalier(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Graphique
            Sprite = PackTexture.Cavalier;
            Tiles = new Vector2(370 / 5, 835 / 11);
            Portrait = PackTexture.CavalierPortrait;

            // Statistiques
            Vie = VieMax = 300;
            Vitesse = 0.15f;
            Vitesse_Attaque = 1f;
            Portee = 1f; // 1 = Corps à corps
            Dommages = 10;
            Puissance = 1;
            Defense = 10;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeVie(this, Vector2.Zero) };

            // Sons
            effetUniteAttaque = new EffetSonore(PackSon.Epee);
            effetUniteDeath = new EffetSonore(PackSon.CavalierDeath);
            statsLevelUpdate();
        }

    }
}
