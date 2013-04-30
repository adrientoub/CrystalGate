using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CrystalGate.Animations;

namespace CrystalGate
{
    public class Odin : Unite
    {

        public Odin(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Graphique
            Sprite = PackTexture.Odin;
            Tiles = new Vector2(195 / 3, 81);
            Portrait = PackTexture.CavalierPortrait;
            packAnimation = new AnimationOdin();
            // Statistiques
            Vie = VieMax = 300;
            Vitesse = 0.2f;
            Vitesse_Attaque = 1f;
            Portee = 2f; // 1 = Corps à corps
            Dommages = 10;
            Puissance = 10;
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
