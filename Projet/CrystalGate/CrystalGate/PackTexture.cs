using System;
using System.Collections.Generic;
using System.Threading;
using CrystalGate.Scenes.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    public class PackTexture
    {
        public Texture2D blank { get; set; }
        public List<Texture2D> unites { get; set; }
        public List<Texture2D> sorts { get; set; }
        public List<Texture2D> map { get; set; } 

        public PackTexture(Texture2D blank)
        {
            this.blank = blank;
            unites = new List<Texture2D> { };
            sorts = new List<Texture2D> { };
            map = new List<Texture2D> { };
        }
        


    }
}
