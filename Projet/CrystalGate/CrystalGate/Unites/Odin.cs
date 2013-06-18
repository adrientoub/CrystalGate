using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CrystalGate.Animations;
using System.Diagnostics;

namespace CrystalGate
{
    public class Odin : Unite
    {
        Stopwatch timer = new Stopwatch();
        bool isAtRange;

        public Odin(Vector2 Position, int Level = 1)
            : base(Position, Level)
        {
            // Physique
            largeurPhysique = 5;
            CreateBody(largeurPhysique, Position);
            // Graphique
            Sprite = PackTexture.Odin;
            Tiles = new Vector2(160, 160);
            Portrait = PackTexture.CavalierPortrait;
            packAnimation = new AnimationOdin();
            // Statistiques
            Vie = VieMax = 3500;
            Vitesse = 0.20f;
            Vitesse_Attaque = 1f;
            Portee = 1f; // 1 = Corps à corps
            Dommages = 45;
            Puissance = 100;
            Defense = 10;
            XPUnite = 200;
            Inventory = new List<Item> { new PotionDeVie(this, Vector2.Zero) };

            spells = new List<Spell> { new NovaDeFeu(this), new Bump(this)} ;

            // Sons
            effetUniteAttaque = new EffetSonore(PackSon.Epee);
            effetUniteDeath = new EffetSonore(PackSon.CavalierDeath);
            statsLevelUpdate();
        }

        protected override void IA(List<Unite> unitsOnMap)
        {
            if (!isAtRange)
                if (Outil.DistanceUnites(Outil.GetJoueur(Client.id).champion, this) <= 600)
                    isAtRange = true;
            if (isAtRange)
            {
                if (!timer.IsRunning)
                    timer.Start();

                if (timer.Elapsed.Seconds < 1)
                    color = Color.Pink;

                if (timer.Elapsed.Seconds > 1 && timer.Elapsed.Seconds < 5)
                {

                    if (IsCastable(0))
                    {
                        for (int i = 0; i < spellsUpdate.Count; i++)
                            if (spellsUpdate[i].idSort == 45)
                                spellsUpdate.RemoveAt(i);
                        ((NovaDeFeu)spells[0]).Positions.Clear();
                        Cast(spells[0], Vector2.Zero, null);
                        ObjectifListe.Clear();
                        color = Color.White;

                    }
                    uniteAttacked = null;
                }
                else
                {
                    spellsUpdate.Clear();
                    uniteAttacked = Outil.GetJoueur(Client.id).champion;
                    if (timer.Elapsed.Seconds > 9)
                        timer.Reset();
                }
            }

        }

        protected override void TestMort(List<Effet> effets)
        {
            if(Vie < 0)
                Map.OnaWin = true;
            base.TestMort(effets);
        }
    }
}
