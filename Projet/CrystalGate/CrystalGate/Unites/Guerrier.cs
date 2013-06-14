using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
            [Serializable]
    public class Guerrier : Unite
    {

        public Guerrier(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Graphique
            Sprite = PackTexture.Guerrier;
            Tiles = new Vector2(530 / 5, 930 / 11);

            // Statistiques
            Vie = VieMax = 400;
            Vitesse = 0.10f;
            Vitesse_Attaque = 0.5f;
            Portee = 1f; // 1 = Corps à corps
            Dommages = 50;
            Puissance = 10;
            Defense = 5;
            DefenseMagique = 3;
            XPUnite = 200;

            spells = new List<Spell> { new Explosion(this), new Soin(this), new Invisibilite(this), new FurieSanguinaire(this), new Polymorphe(this), new Tempete(this) };

            // Sons
            effetUniteAttaque = new EffetSonore(PackSon.Epee);
            effetUniteDeath = new EffetSonore(PackSon.GruntDeath);
            statsLevelUpdate();
        }

    }
}
