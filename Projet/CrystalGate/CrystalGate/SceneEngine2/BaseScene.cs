using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate.SceneEngine2
{
    public abstract class BaseScene
    {
        public static MouseState mouse, oldMouse;
        public static KeyboardState keyboardState, oldKeyboardState;

        public static SpriteFont spriteFont;

        public static Texture2D boutons, background, curseur;

        public abstract void Initialize();

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
