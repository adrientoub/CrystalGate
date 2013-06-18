using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CrystalGateEditor
{
    /// <summary>
    /// Type principal pour votre jeu
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static bool isTest = false;
        public static SceneEngine2.SceneHandler scene;

        public static bool exit;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            if (!isTest)
                graphics.IsFullScreen = true;
            this.IsMouseVisible = true;
            int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            Content.RootDirectory = "Content";

            scene = new SceneEngine2.SceneHandler();
            SceneEngine2.SceneHandler.content = Content;
            exit = false;

            SceneEngine2.MenuOptions.isFullscreen = !graphics.IsFullScreen;

            GameText.initGameText();
        }

        protected override void LoadContent()
        {
            // Créer un SpriteBatch, qui peut être utilisé pour dessiner des textures.
            SceneEngine2.SceneHandler.spriteBatch = new SpriteBatch(GraphicsDevice);
            scene.Initialize();
            scene.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            scene.Update(gameTime);
            if (exit)
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            scene.Draw();
            base.Draw(gameTime);
        }
    }
}
