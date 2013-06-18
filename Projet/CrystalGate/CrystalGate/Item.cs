using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
            [Serializable]
    public class Item
    {
        public Texture2D Icone;
        public Vector2 Position;
        public bool InInventory;
        public bool Activated; // true quand l'objet est utilisé
        public Unite unite; // Unite qui possède l'objet
        public Spell spell; // Spell qui sera appelé quand l'objet sera activé
        public Type type;
        public int id;

        public Item(Unite unite, Vector2 position)
        {
            this.unite = unite;
            Position = position;
            Icone = PackTexture.blank; // definir l'icone ici
            type = Type.Consommable;
        }

        public void Update(List<Unite> unites) // Appelé quand l'objet est sur la Map
        {
            foreach (Unite u in unites)
            {
                if (u.isAChamp && Outil.DistancePoints(Position, u.PositionTile) <= 32 && !InInventory)
                {
                    if (u.Inventory.Count + 1 <= u.InventoryCapacity)
                    {
                        unite = u;
                        u.Inventory.Add(this);
                        InInventory = true; // retire l'objet de la carte et l'ajoute a l'inventaire de l'unité proche
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
            spell.Begin(unite.pointCible, null); // Lance le sort de l'objet et le marque comme activé , pour qu'il soit retiré de l'inventaire dans le methode InventoryUpdate()
            Activated = true;
        }
    }

    public enum Type
    {
        Casque,
        Epaulieres,
        Gants,
        Plastron,
        Anneau,
        Bottes,
        Arme,
        Consommable
    }
}
