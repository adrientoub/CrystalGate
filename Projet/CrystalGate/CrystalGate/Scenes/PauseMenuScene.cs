using System;
using CrystalGate.Scenes.Core;

namespace CrystalGate.Scenes
{
    /// <summary>
    /// Le menu de pause vient s'afficher devant le jeu
    /// </summary>
    public class PauseMenuScene : AbstractMenuScene
    {
        #region Fields

        private readonly AbstractGameScene _parent;

        #endregion

        #region Initialization

        public PauseMenuScene(SceneManager sceneMgr, AbstractGameScene parent)
            : base(sceneMgr, new Text("Pause").get())
        {
            _parent = parent;
            FondSonore.Pause();

            // Création des options
            var resumeGameMenuItem = new MenuItem(new Text("BackToGame").get());
            var quitGameMenuItem = new MenuItem(new Text("QuitGame").get());
            
            // Gestion des évènements
            resumeGameMenuItem.Selected += OnCancel;
            quitGameMenuItem.Selected += QuitGameMenuItemSelected;

            // Ajout des options du menu
            MenuItems.Add(resumeGameMenuItem);
            MenuItems.Add(quitGameMenuItem);
        }

        #endregion

        #region Handle Input

        private void QuitGameMenuItemSelected(object sender, EventArgs e)
        {
            string message = new Text("QuitLevel").get() + "\n";
            var confirmQuitMessageBox = new MessageBoxScene(SceneManager, message, true);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
            confirmQuitMessageBox.Add();
        }

        private void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e)
        {
            Remove();
            _parent.Remove();
            CrystalGate.FondSonore.Stop();
            LoadingScene.Load(SceneManager, false);
        }

        #endregion
    }
}
