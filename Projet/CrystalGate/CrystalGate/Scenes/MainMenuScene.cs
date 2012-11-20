using System;
using CrystalGate.Scenes.Core;

namespace CrystalGate.Scenes
{
    /// <summary>
    /// Le menu principal est la premi�re chose affich�e lors du lancement du binaire
    /// </summary>
    public class MainMenuScene : AbstractMenuScene
    {
        #region Initialization

        /// <summary>
        /// Le constructeur remplit le menu
        /// </summary>
        public MainMenuScene(SceneManager sceneMgr)
            : base(sceneMgr, "Menu principal")
        {
            // Cr�ation des options
            var playGameMenuItem = new MenuItem("Lancer le jeu");
            var optionsMenuItem = new MenuItem("Options");
            var exitMenuItem = new MenuItem("Quitter");

            // Gestion des �v�nements
            playGameMenuItem.Selected += PlayGameMenuItemSelected;
            optionsMenuItem.Selected += OptionsMenuItemSelected;
            exitMenuItem.Selected += OnCancel;

            // Ajout des options du menu
            MenuItems.Add(playGameMenuItem);
            MenuItems.Add(optionsMenuItem);
            MenuItems.Add(exitMenuItem);
        }

        #endregion

        #region Handle Input

        private void PlayGameMenuItemSelected(object sender, EventArgs e)
        {
            LoadingScene.Load(SceneManager, true, new GameplayScene(SceneManager));
        }

        private void OptionsMenuItemSelected(object sender, EventArgs e)
        {
            new OptionsMenuScene(SceneManager).Add();
        }

        protected override void OnCancel()
        {
            const string message = "Etes vous sur de vouloir quitter le sample?\n";
            var confirmExitMessageBox = new MessageBoxScene(SceneManager, message, true);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            confirmExitMessageBox.Add();
        }

        private void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            SceneManager.Game.Exit();
        }

        #endregion
    }
}
