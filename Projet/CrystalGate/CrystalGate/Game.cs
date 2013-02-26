using CrystalGate.Inputs;
using CrystalGate.Scenes;
using CrystalGate.Scenes.Core;
using Microsoft.Xna.Framework;

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
        public CrystalGateGame()
        {
            Content.RootDirectory = "Content";

            // Initialisation du GraphicsDeviceManager
            // pour obtenir une fen�tre de dimensions 800*480
            graphics = new GraphicsDeviceManager(this) { PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height , /*IsFullScreen = true*/};
            Scenes.OptionsMenuScene._fullscreen = !graphics.IsFullScreen;

            // Cr�ation du gestionnaire de sc�nes
            var sceneMgr = new SceneManager(this);

            // Mise � jour automatique de Win... des entr�es utilisateur
            // et du gestionnaire de sc�nes
            Components.Add(new InputState(this));
            Components.Add(sceneMgr);

            // Activation des premi�res sc�nes
            new BackgroundScene(sceneMgr).Add();
            new MainMenuScene(sceneMgr).Add();
        }

        public static void Main()
        {
            // Point d'entr�e
            using (var game = new CrystalGateGame())
                game.Run();
        }
    }
}
