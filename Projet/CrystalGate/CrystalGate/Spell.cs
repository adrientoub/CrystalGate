using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Spell
    {
        public string Nom { get; set; }
        public Texture2D SpriteBouton { get; set; }
        public Unite unite { get; set; }
        public float Cooldown { get; set; }

        public float LastCast { get; set; }

        public Spell(Unite u)
        {
            unite = u;
            Cooldown = 1;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteBouton, new Vector2(100, 100), Color.White);
        }
    }
}
