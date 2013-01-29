using System;
using CrystalGate.Scenes.Core;

namespace CrystalGate.Scenes
{
    /// <summary>
    /// Le menu principal est la première chose affichée lors du lancement du binaire
    /// </summary>
    public class MainMenuScene : AbstractMenuScene
    {
        #region Initialization

        /// <summary>
        /// Le constructeur remplit le menu
        /// </summary>
        public MainMenuScene(SceneManager sceneMgr)
            : base(sceneMgr, "Crystal Gate")
        {
            GameText.initGameText();
            // Création des options
            Text launchGame = new Text("LaunchGame");
            Text quitGame = new Text("Quit");
            var playGameMenuItem = new MenuItem(launchGame.get());
            var optionsMenuItem = new MenuItem("Options");
            var exitMenuItem = new MenuItem(quitGame.get());

            // Gestion des évènements
            playGameMenuItem.Selected += PlayGameMenuItemSelected;
            optionsMenuItem.Selected += OptionsMenuItemSelected;
            exitMenuItem.Selected += OnCancel;

            // Ajout des options du menu
            MenuItems.Add(playGameMenuItem);
            //MenuItems.Add(optionsMenuItem);
            MenuItems.Add(exitMenuItem);
        }

        #endregion

        #region Handle Input

        private void PlayGameMenuItemSelected(object sender, EventArgs e)
        {
            LoadingScene.Load(SceneManager, true, new GameplayScene(SceneManager));
            CrystalGate.FondSonore.Play();
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
