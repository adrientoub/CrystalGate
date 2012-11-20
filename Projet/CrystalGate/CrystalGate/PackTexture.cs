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

        public PackTexture(Texture2D blank)
        {
            this.blank = blank;
        }
        


    }
}
