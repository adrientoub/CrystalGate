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
        private readonly MenuItem _volumeEffectsMenuItem;

        private enum Language
        {
            English,
            Fran�ais,
        }

        private static Language _currentLanguage = Language.Fran�ais;
        private static readonly string[] Resolutions = { "480x800", "800x600", "1024x768", "1280x1024", "1680x1050", "1920x1080" };
        private static int _currentResolution;
        public static bool _fullscreen;
        private static int _volume = (int)(FondSonore.volume * 100), _volumeEffects = (int)(EffetSonore.volume * 100);
        Text fullscreenText, resolutionText, languageText, volumeText, volumeEffectsText, yesText, noText;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScene(SceneManager sceneMgr)
            : base(sceneMgr, "Options")
        {
            // Cr�ation des options du menu
            _languageMenuItem = new MenuItem(new Text());
            _resolutionMenuItem = new MenuItem(new Text());
            _fullscreenMenuItem = new MenuItem(new Text());
            _volumeMenuItem = new MenuItem(new Text());
            _volumeEffectsMenuItem = new MenuItem(new Text());

            var back = new MenuItem(new Text("Back"));

            fullscreenText = new Text("Fullscreen");
            languageText = new Text("Language");
            volumeText = new Text("Volume");
            volumeEffectsText = new Text("VolumeEffects");
            resolutionText = new Text("Resolution");
            noText = new Text("no");
            yesText = new Text("yes");

            SetMenuItemText();

            // Gestion des �v�nements
            _languageMenuItem.Selected += LanguageMenuItemSelected;
            _resolutionMenuItem.Selected += ResolutionMenuItemSelected;
            _fullscreenMenuItem.Selected += FullscreenMenuItemSelected;
            _volumeMenuItem.Selected += VolumeMenuItemSelected;
            _volumeEffectsMenuItem.Selected += VolumeEffectsMenuItemSelected;
            back.Selected += OnCancel;
            
            // Ajout des options au menu
            MenuItems.Add(_languageMenuItem);
            //MenuItems.Add(_resolutionMenuItem);
            MenuItems.Add(_fullscreenMenuItem);
            MenuItems.Add(_volumeMenuItem);
            MenuItems.Add(_volumeEffectsMenuItem);
            MenuItems.Add(back);
        }

        /// <summary>
        /// Mise � jour des valeurs du menu
        /// </summary>
        private void SetMenuItemText()
        {
            _languageMenuItem.Text = new Text(languageText.get() + ": " + _currentLanguage, true);
            _resolutionMenuItem.Text = new Text(resolutionText.get() + ": " + Resolutions[_currentResolution], true);
            _fullscreenMenuItem.Text = new Text(fullscreenText.get() + ": " + (_fullscreen ? noText.get() : yesText.get()), true);
            _volumeMenuItem.Text = new Text(volumeText.get() + ": " + _volume, true);
            _volumeEffectsMenuItem.Text = new Text(volumeEffectsText.get() + ": " + _volumeEffects, true);
        }

        #endregion

        #region Handle Input

        private void LanguageMenuItemSelected(object sender, EventArgs e)
        {
            _currentLanguage++;

            if (_currentLanguage > Language.Fran�ais)
                _currentLanguage = 0;

            if (_currentLanguage == Language.Fran�ais)
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

        private void VolumeEffectsMenuItemSelected(object sender, EventArgs e)
        {
            _volumeEffects++;
            EffetSonore.volume = _volumeEffects * 0.01f;
            SetMenuItemText();
        }

        #endregion
    }
}
