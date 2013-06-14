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
        Random rand = new Random();
        Text description1, description2;

        public Invisibilite(Unite u, bool useMana = true)
            : base(u)
        {
            idSort = 3;
            Cooldown = 5;
            Ticks = 300;
            if (useMana)
                CoutMana = 30;
            else
                CoutMana = 0;
            Tiles = new Vector2(320 / 5, 320 / 5);

            NeedUnPoint = false;
            SpriteBouton = PackTexture.boutons[2];
            SpriteEffect = PackTexture.sorts[0];
            sonSort = new EffetSonore(PackSon.InvisibiliteCible);
            uniteFollowing = new List<Unite>();

            description1 = new Text("DescriptionInvisibilite1");
            description2 = new Text("DescriptionInvisibilite2");
        }

        public override void UpdateSort()
        {
                unite.isInvisible = true;
                if (TickCurrent == 0)
                {
                    foreach (Unite u in Map.unites)
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
                    unite.isInvisible = false;
                }
                /*else
                {
                    if (TickCurrent % 60 == 0)
                    {
                        foreach (Unite u in Map.unites)
                        {
                            if (unite != u && !u.isApnj)
                            {
                                u.ObjectifListe.Add(new Noeud(u.PositionTile + new Vector2(2, 2) * rand.Next(-1, 2), true, 0));
                            }
                        }
                    }

                }*/
        }

        public override string DescriptionSpell()
        {
            return description1.get() + " " + Ticks / 60 + " " + description2.get();
        }
    }
}
