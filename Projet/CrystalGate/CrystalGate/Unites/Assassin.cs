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
    public class Assassin : Unite
    {

        public Assassin(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Physique
            Scale = 0.4f;
            // Graphique
            Sprite = PackTexture.Assassin;
            Tiles = new Vector2(1757 / 5, 1400 / 5);
            Portrait = PackTexture.CavalierPortrait;
            packAnimation = new PackAnimation();
            // Statistiques
            Vie = VieMax = 3000;
            Vitesse = 0.2f;
            Vitesse_Attaque = 1f;
            Portee = 1f; // 1 = Corps à corps
            Dommages = 100;
            Puissance = 100;
            Defense = 10;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeVie(this, Vector2.Zero) };

            // Sons
            effetUniteAttaque = new EffetSonore(PackSon.Epee);
            effetUniteDeath = new EffetSonore(PackSon.CavalierDeath);
            statsLevelUpdate();
        }

        protected override void IA(List<Unite> unitsOnMap)
        {
            base.IA(unitsOnMap);

        }
    }
}
