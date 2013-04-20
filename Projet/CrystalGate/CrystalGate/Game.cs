using CrystalGate.Inputs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    /// <summary>
    /// Ceci est un sample montrant comment gérer différents états de jeu avec transitions.
    /// Démo de menus, scène de chargement, scène de jeu et scène de pause. Cette classe est
    /// extrèmement simple, tout se passe dans le gestionnaire de scènes: le SceneManager.
    /// </summary>
    public class CrystalGateGame : Game
    {
        public static GraphicsDeviceManager graphics;
        public static bool isTest = true;
        public static bool exit;
        SceneEngine2.SceneHandler scene;

        public CrystalGateGame()
        {
            Content.RootDirectory = "Content";

            // Initialisation du GraphicsDeviceManager
            // pour obtenir une fenêtre de dimensions 800*480
            graphics = new GraphicsDeviceManager(this) { 
                PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
            };
            if (!isTest)
                graphics.IsFullScreen = true;

        }

        protected override void Initialize()
        {
            GameText.initGameText();
            scene = new SceneEngine2.SceneHandler();
            SceneEngine2.SceneHandler.content = Content;
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
            // TODO: Unload any non ContentManager content here
        }

        public static void Main()
        {
            // Point d'entrée
            using (var game = new CrystalGateGame())
                game.Run();
        }
    }
}
