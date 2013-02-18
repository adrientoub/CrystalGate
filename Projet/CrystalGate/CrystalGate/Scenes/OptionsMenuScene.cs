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
        Text fullscreenText, resolutionText, languageText, volumeText;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScene(SceneManager sceneMgr)
            : base(sceneMgr, "Options")
        {
            // Création des options du menu
            _languageMenuItem = new MenuItem(new Text());
            _resolutionMenuItem = new MenuItem(new Text());
            _fullscreenMenuItem = new MenuItem(new Text());
            _volumeMenuItem = new MenuItem(new Text());

            var back = new MenuItem(new Text("Back"));

            fullscreenText = new Text("Fullscreen");
            languageText = new Text("Language");
            volumeText = new Text("Volume");
            resolutionText = new Text("Resolution");

            SetMenuItemText();

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
            _languageMenuItem.Text = new Text(languageText.get() + ": " + _currentLanguage, true);
            _resolutionMenuItem.Text = new Text(resolutionText.get() + ": " + Resolutions[_currentResolution], true);
            _fullscreenMenuItem.Text = new Text(fullscreenText.get() + ": " + (_fullscreen ? "oui" : "non"), true);
            _volumeMenuItem.Text = new Text(volumeText.get() + ": " + _volume, true);
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
