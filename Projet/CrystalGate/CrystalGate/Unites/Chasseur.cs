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
    public class Chasseur : Unite
    {

        public Chasseur(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Graphique
            Sprite = PackTexture.Chasseur;
            Tiles = new Vector2( 1757 / 5, 3131 / 11);
            Portrait = PackTexture.ChasseurPortrait;
            packAnimation = new PackAnimation();
            ProjectileSprite = PackTexture.projectiles[0];
            Scale = 0.3f;
            // Statistiques
            Vie = VieMax = 350;
            Vitesse = 0.10f;
            Vitesse_Attaque = 0.6f;
            Portee = 15f; // 2 = Corps à corps
            Dommages = 80;
            Puissance = 10;
            Defense = 2;
            IsRanged = true;
            XPUnite = 200;

            spells = new List<Spell> { new Explosion(this), new Soin(this), new Invisibilite(this), new FurieSanguinaire(this), new Polymorphe(this), new Tempete(this) };

            Inventory = new List<Item> { new PotionDeVie(this, Vector2.Zero) };

            // Sons
            effetUniteAttaque = new EffetSonore(PackSon.ArcherAttack);
            effetUniteDeath = new EffetSonore(PackSon.GruntDeath);
            statsLevelUpdate();
        }


    }
}
