using CrystalGate.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;

namespace CrystalGate
{
    /// <summary>
    /// Ceci est un sample montrant comment g�rer diff�rents �tats de jeu avec transitions.
    /// D�mo de menus, sc�ne de chargement, sc�ne de jeu et sc�ne de pause. Cette classe est
    /// extr�mement simple, tout se passe dans le gestionnaire de sc�nes: le SceneManager.
    /// </summary>
    public class CrystalGateGame : Game
    {
        public static GraphicsDeviceManager graphics;
        public static bool isTest = false;
        public static bool exit;
        SceneEngine2.SceneHandler scene;

        public CrystalGateGame()
        {
            Content.RootDirectory = "Content";

            // Initialisation du GraphicsDeviceManager
            // pour obtenir une fen�tre de dimensions 800*480
            graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
            };
            if (!isTest)
                graphics.IsFullScreen = true;

            SceneEngine2.MenuOptions.isFullscreen = !graphics.IsFullScreen;
        }

        protected override void Initialize()
        {
            GameText.initGameText();
            scene = new SceneEngine2.SceneHandler();
            SceneEngine2.SceneHandler.content = Content;
            Unite.rand = new Random();
            exit = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SceneEngine2.SceneHandler.spriteBatch = new SpriteBatch(GraphicsDevice);
            scene.Load();
            scene.Initialize();
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
        }

        protected override void UnloadContent()
        {
            foreach (Thread t in Serveur.threads)
                t.Abort();
            // TODO: Unload any non ContentManager content here
        }

        public static void Main()
        {
            // Point d'entr�e
            using (var game = new CrystalGateGame())
                game.Run();
        }
    }
}
