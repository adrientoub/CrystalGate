using System;
using CrystalGate.Scenes.Core;

namespace CrystalGate.Scenes
{
    /// <summary>
    /// Un exemple de menu d'options
    /// </summary>
    public class OptionsMenuScene : AbstractMenuScene
    {
        #region Fields

        private readonly MenuItem _languageMenuItem;
        private readonly MenuItem _resolutionMenuItem;
        private readonly MenuItem _fullscreenMenuItem;
        private readonly MenuItem _volumeMenuItem;

        private enum Language
        {
            English,
            Francais,
        }

        private static Language _currentLanguage = Language.Francais;
        private static readonly string[] Resolutions = { "480x800", "800x600", "1024x768", "1280x1024", "1680x1050", "1920x1080" };
        private static int _currentResolution;
        private static bool _fullscreen;
        private static int _volume = 50;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScene(SceneManager sceneMgr)
            : base(sceneMgr, "Options")
        {
            // Création des options du menu
            _languageMenuItem = new MenuItem(string.Empty);
            _resolutionMenuItem = new MenuItem(string.Empty);
            _fullscreenMenuItem = new MenuItem(string.Empty);
            _volumeMenuItem = new MenuItem(string.Empty);
            SetMenuItemText();
            var back = new MenuItem("Retour");

            // Gestion des évènements
            _languageMenuItem.Selected += LanguageMenuItemSelected;
            _resolutionMenuItem.Selected += ResolutionMenuItemSelected;
            _fullscreenMenuItem.Selected += FullscreenMenuItemSelected;
            _volumeMenuItem.Selected += VolumeMenuItemSelected;
            back.Selected += OnCancel;
            
            // Ajout des options au menu
            MenuItems.Add(_languageMenuItem);
            //MenuItems.Add(_resolutionMenuItem);
            MenuItems.Add(_fullscreenMenuItem);
            MenuItems.Add(_volumeMenuItem);
            MenuItems.Add(back);
        }

        /// <summary>
        /// Mise à jour des valeurs du menu
        /// </summary>
        private void SetMenuItemText()
        {
            _languageMenuItem.Text = "Langue: " + _currentLanguage;
            _resolutionMenuItem.Text = "Resolution: " + Resolutions[_currentResolution];
            _fullscreenMenuItem.Text = "Plein ecran: " + (_fullscreen ? "oui" : "non");
            _volumeMenuItem.Text = "Volume: " + _volume;
        }

        #endregion

        #region Handle Input

        private void LanguageMenuItemSelected(object sender, EventArgs e)
        {
            _currentLanguage++;

            if (_currentLanguage > Language.Francais)
                _currentLanguage = 0;

            if (_currentLanguage == Language.Francais)
            {
                GameText.langue = "french";
            }
            else if (_currentLanguage == Language.English)
            {
                GameText.langue = "english";
            }
            GameText.initGameText();

            SetMenuItemText();
        }

        private void ResolutionMenuItemSelected(object sender, EventArgs e)
        {
            _currentResolution = (_currentResolution + 1) % Resolutions.Length;
            SetMenuItemText();
        }

        private void FullscreenMenuItemSelected(object sender, EventArgs e)
        {
            _fullscreen = !_fullscreen;
            SetMenuItemText();
            CrystalGate.CrystalGateGame.graphics.ToggleFullScreen();
        }

        private void VolumeMenuItemSelected(object sender, EventArgs e)
        {
            _volume++;
            FondSonore.volume = _volume * 0.01f;
            SetMenuItemText();
        }

        #endregion
    }
}
