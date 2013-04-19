using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalGate.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CrystalGate.SceneEngine2
{
    class MenuOptions : BaseScene
    {
        private enum Language
        {
            English,
            Français,
        }
        public static bool isFullscreen;

        private ContentManager content;

        private Texture2D background;
        private Texture2D boutons;
        private Texture2D curseur;
        private Texture2D volume;
        private Texture2D volumeVide;

        private SpriteFont spriteFont;

        private Rectangle mouseRec;
        private KeyboardState keyboardState;

        private Rectangle boutonPleinEcran, boutonLangue, boutonRetour, volumeEffects, volumeFondSonore;

        private Text pleinEcranT, langueT, retourT, noT, yesT;

        private static Language _currentLanguage = Language.Français;
        string fullscreenText, langueText;

        public void Initialize()
        {
            fullscreenText = pleinEcranT.get() + " : " + (isFullscreen ? noT.get() : yesT.get());
            langueText = langueT.get() + " : " + _currentLanguage.ToString();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            background = content.Load<Texture2D>("background");
            boutons = content.Load<Texture2D>("Menu/Boutons");
            volume = content.Load<Texture2D>("Menu/Volume");
            volumeVide = content.Load<Texture2D>("Menu/VolumeVide");   
            curseur = content.Load<Texture2D>("Curseur");         

            pleinEcranT = new Text("Fullscreen");
            langueT = new Text("Language");
            retourT = new Text("Back");
            noT = new Text("no");
            yesT = new Text("yes");

            spriteFont = content.Load<SpriteFont>("Polices/sceneengine2font");

            boutonPleinEcran = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 200, boutons.Width, boutons.Height);
            boutonLangue = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 100, boutons.Width, boutons.Height);
            volumeEffects = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, volume.Width, volume.Height);
            volumeFondSonore = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, volume.Width, volume.Height);
            boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 200, boutons.Width, boutons.Height);
        }

        public override void Update(GameTime gameTime)
        {
            // Handle mouse and keyboard to use the menu.
            keyboardState = InputState.CurrentKeyboardState;
            mouse = Mouse.GetState();
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (mouseRec.Intersects(volumeEffects))
                {
                    EffetSonore.volume = (float)(mouse.X - volumeEffects.X) / volumeEffects.Width;
                }
                else if (mouseRec.Intersects(volumeFondSonore))
                {
                    FondSonore.volume = (float)(mouse.X - volumeFondSonore.X) / volumeEffects.Width;
                    FondSonore.UpdateVolume();
                }
                else if (oldMouse.LeftButton == ButtonState.Released)
                {
                    if (mouseRec.Intersects(boutonPleinEcran))
                    {
                        isFullscreen = !isFullscreen;
                        CrystalGate.CrystalGateGame.graphics.ToggleFullScreen();
                    }
                    else if (mouseRec.Intersects(boutonLangue))
                    {
                        _currentLanguage++;

                        if (_currentLanguage > Language.Français)
                            _currentLanguage = 0;

                        if (_currentLanguage == Language.Français)
                        {
                            GameText.langue = "french";
                        }
                        else if (_currentLanguage == Language.English)
                        {
                            GameText.langue = "english";
                        }
                        GameText.initGameText();
                    }
                    else if (mouseRec.Intersects(boutonRetour))
                    {
                        SceneHandler.gameState = GameState.MainMenu;
                        // TODO : Retourner au jeu si options du jeu
                    }
                }
            }
            fullscreenText = pleinEcranT.get() + " : " + (isFullscreen ? noT.get() : yesT.get());
            langueText = langueT.get() + " : " + _currentLanguage.ToString();
            oldMouse = mouse;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.Begin();
            spriteBatch.Draw(background, fullscene, Color.White);

            if (mouseRec.Intersects(boutonPleinEcran))
                spriteBatch.Draw(boutons, boutonPleinEcran, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonPleinEcran, Color.White);
            if (mouseRec.Intersects(boutonLangue))
                spriteBatch.Draw(boutons, boutonLangue, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonLangue, Color.White);
            if (mouseRec.Intersects(boutonRetour))
                spriteBatch.Draw(boutons, boutonRetour, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonRetour, Color.White);

            spriteBatch.Draw(volumeVide, volumeEffects, Color.White);
            spriteBatch.Draw(volume, new Rectangle(volumeEffects.X, volumeEffects.Y + volumeEffects.Height - (int)(EffetSonore.volume * volumeEffects.Height),
                (int)(EffetSonore.volume * volumeEffects.Width), (int)(EffetSonore.volume * volumeEffects.Height)),
                Color.White
                );

            spriteBatch.Draw(volumeVide, volumeFondSonore, Color.White);
            spriteBatch.Draw(volume, new Rectangle(volumeFondSonore.X, volumeFondSonore.Y + volumeFondSonore.Height - (int)(FondSonore.volume * volumeFondSonore.Height),
                (int)(FondSonore.volume * volumeFondSonore.Width), (int)(FondSonore.volume * volumeFondSonore.Height)),
                Color.White
                );

            spriteBatch.DrawString(
                spriteFont,
                fullscreenText,
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(fullscreenText).X / 2,
                    boutonPleinEcran.Top + 10),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                langueText,
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(langueText).X / 2,
                    boutonLangue.Top + 10),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                retourT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(retourT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);

            spriteBatch.Draw(curseur, new Vector2(mouse.X, mouse.Y), Color.White);

            spriteBatch.End();
        }
    }
}
