using CrystalGate.Inputs;
using CrystalGate.Scenes;
using CrystalGate.Scenes.Core;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    /// <summary>
    /// Ceci est un sample montrant comment gérer différents états de jeu avec transitions.
    /// Démo de menus, scène de chargement, scène de jeu et scène de pause. Cette classe est
    /// extrèmement simple, tout se passe dans le gestionnaire de scènes: le SceneManager.
    /// </summary>
    public class CrystalGateGame : Game
    {
        public CrystalGateGame()
        {
            Content.RootDirectory = "Content";

            // Initialisation du GraphicsDeviceManager
            // pour obtenir une fenêtre de dimensions 800*480
            new GraphicsDeviceManager(this) { PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height , IsFullScreen = true};
            IsMouseVisible = true;
            
            // Création du gestionnaire de scènes
            var sceneMgr = new SceneManager(this);

            // Mise à jour automatique de Win... des entrées utilisateur
            // et du gestionnaire de scènes
            Components.Add(new InputState(this));
            Components.Add(sceneMgr);

            // Activation des premières scènes
            new BackgroundScene(sceneMgr).Add();
            new MainMenuScene(sceneMgr).Add();
        }

        public static void Main()
        {
            // Point d'entrée
            using (var game = new CrystalGateGame())
                game.Run();
        }
    }
}
