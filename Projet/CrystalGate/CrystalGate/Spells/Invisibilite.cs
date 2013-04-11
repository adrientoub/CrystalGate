using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class Invisibilite : Spell
    {
        List<Unite> uniteFollowing;

        public Invisibilite(Unite u, bool useMana = true)
            : base(u)
        {
            Cooldown = 5;
            Ticks = 300;
            if (useMana)
                CoutMana = 30;
            else
                CoutMana = 0;
            Tiles = new Vector2(320 / 5, 320 / 5);

            NeedUnPoint = false;
            SpriteBouton = unite.packTexture.boutons[2];
            SpriteEffect = unite.packTexture.sorts[0];
            sonSort = new EffetSonore(10);
            uniteFollowing = new List<Unite>();
        }

        public override void Update()
        {
            if (TickCurrent < Ticks)
            {
                if (TickCurrent == 0)
                {
                    foreach (Unite u in unite.Map.unites)
                    {
                        if (u != unite && u.uniteAttacked == unite)
                        {
                            u.uniteAttacked = null;
                            uniteFollowing.Add(u);
                        }
                    }
                    unite.color = new Color(255, 255, 255, 0);
                }
                else if (TickCurrent == Ticks - 1)
                {
                    unite.color = Color.White;
                    foreach (Unite u in uniteFollowing)
                    {
                        u.uniteAttacked = unite;
                    }
                    uniteFollowing = new List<Unite>();
                }
                /*else
                {
                    if (TickCurrent % 60 == 0)
                    {
                        foreach (Unite u in unite.Map.unites)
                        {
                            if (unite != u)
                            {
                                Random rand = new Random();
                                u.ObjectifListe.Add(new Noeud(u.PositionTile + new Vector2(1, 1) * rand.Next(-1, 2), true, 0));
                            }
                        }
                    }

                }*/
                TickCurrent++;
            }
        }

        public override string DescriptionSpell()
        {
            return "Rend le héros invisible pendant " + Ticks / 60 + " secondes, l'immunisant contre toutes attaques directes. Si le héros attaque ou utilise une competence, il redevient visible.";
        }
    }
}
