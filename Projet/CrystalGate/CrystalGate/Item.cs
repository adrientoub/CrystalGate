using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Item
    {
        public Texture2D Icone;
        public Vector2 Position;
        public bool Disabled;

        public Item(Vector2 position, PackTexture pack)
        {
            Position = position;
            Icone = pack.boutons[0];
        }

        public void Update(List<Unite> unites) // Appelé quand l'objet est sur la map
        {
                foreach (Unite u in unites)
                {
                    if (Outil.DistancePoints(Position, u.PositionTile) <= 32)
                    {
                        u.Inventory.Add(this);
                        Disabled = true;
                    }
                }
        }
    }
}
