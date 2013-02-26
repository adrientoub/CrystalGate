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
        public bool InInventory;
        public bool Disabled; // true quand l'objet est utilisé
        public Unite unite; // Unite qui possède l'objet
        public Spell spell; // Spell qui sera appelé quand l'objet sera activé

        public Item(Vector2 position, PackTexture pack)
        {
            Position = position;
            Icone = pack.blank; // definir l'icone ici
        }

        public void Update(List<Unite> unites) // Appelé quand l'objet est sur la map
        {
                foreach (Unite u in unites)
                {
                    if (u.isAChamp && Outil.DistancePoints(Position, u.PositionTile) <= 32 && !InInventory)
                    {
                        if (u.Inventory.Count + 1 <= u.InventoryCapacity)
                        {
                            u.Inventory.Add(this);
                            InInventory = true; // retire l'objet de la carte et l'ajoute a l'inventaire de l'unité proche
                            unite = u;
                            Effet(u);
                        }
                    }
                }
        }

        public virtual void Effet(Unite unite)
        {

        }

        public void Utiliser()
        {
            spell.Begin(unite.pointCible); // Lance le sort de l'objet et le marque comme désactivé , pour qu'il soit retiré de l'inventaire dans le methode InventoryUpdate()
            Disabled = true;
        }
    }
}
