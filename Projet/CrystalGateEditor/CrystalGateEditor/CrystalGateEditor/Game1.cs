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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont sp;
        public static bool isTest = true;
        public static string baseDirectory;

        User user;
        UI ui;

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
        }

        protected override void LoadContent()
        {
            // Créer un SpriteBatch, qui peut être utilisé pour dessiner des textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sp = Content.Load<SpriteFont>("Police");
            user = new User();
            ui = new UI(user, Content.Load<Texture2D>("Palette"), Content.Load<Texture2D>("PaletteHiver"), Content.Load<Texture2D>("PaletteVolcanique"), sp, Content.Load<Texture2D>("writing"));
            ui.mode = UI.Mode.LoadOrCreate;
            if (Game1.isTest)
                baseDirectory = "../../../";
            else
                baseDirectory = "./";
        }

        protected override void Update(GameTime gameTime)
        {
            user.Update();
            ui.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(0, null, null, null, null, null, user.camera.CameraMatrix);
            ui.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
