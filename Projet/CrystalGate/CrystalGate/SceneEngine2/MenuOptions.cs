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
    class Resolution
    {
        public int width;
        public int height;
        public Resolution(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    class MenuOptions : BaseScene
    {
        private enum Language
        {
            English,
            Français,
        }
        public static bool isFullscreen;

        private ContentManager content;

        private Texture2D volume;
        private Texture2D volumeVide;

        private Rectangle mouseRec;

        public static bool isPauseOption;
        public static bool endFirstClic;

        private Rectangle boutonPleinEcran, boutonLangue, boutonRetour, boutonCredits, volumeEffects, volumeFondSonore, boutonResolution;
        private Vector2 positionTexteVolumeEffects, positionTexteVolumeFondSonore, positionTexteFullscreen, positionTexteLangue, positionTexteResolution;

        private Text pleinEcranT, langueT, retourT, noT, yesT, effetSonoreT, fondSonoreT, creditsT, resolutionT;
        private string resolution;

        private static Language _currentLanguage = Language.Français;
        private List<Resolution> possibleResolutions;
        private int _currentRes = 0;

        string fullscreenText;

        public override void Initialize()
        {
            fullscreenText = isFullscreen ? noT.get() : yesT.get();
            isPauseOption = false;
            endFirstClic = false;
            possibleResolutions = new List<Resolution>
            {
                new Resolution(1920, 1080),
                new Resolution(1600, 900),
                new Resolution(1280, 1024),
                new Resolution(1280, 720),
                new Resolution(1024, 768),
                new Resolution(800, 600),
                new Resolution(640, 480)
            };
            for (int i = 0; i < possibleResolutions.Count; i++)
            {
                if (possibleResolutions[i].width == CrystalGateGame.graphics.GraphicsDevice.Viewport.Width && possibleResolutions[i].height == CrystalGateGame.graphics.GraphicsDevice.Viewport.Height)
                    _currentRes = i;
            }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            volume = content.Load<Texture2D>("Menu/Volume");
            volumeVide = content.Load<Texture2D>("Menu/VolumeVide");
            curseur = content.Load<Texture2D>("Curseur");

            pleinEcranT = new Text("Fullscreen");
            langueT = new Text("Language");
            retourT = new Text("Back");
            noT = new Text("no");
            yesT = new Text("yes");
            effetSonoreT = new Text("VolumeEffects");
            fondSonoreT = new Text("Volume");
            resolutionT = new Text("Resolution");
            creditsT = new Text("Credits");

            UpdatePositions();
        }

        public void UpdatePositions()
        {
            boutonPleinEcran = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 300, boutons.Width, boutons.Height);
            boutonLangue = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 200, boutons.Width, boutons.Height);
            volumeEffects = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 100, volume.Width, volume.Height);
            volumeFondSonore = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2,
                CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, volume.Width, volume.Height);
            boutonResolution = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);
            boutonCredits = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 200, boutons.Width, boutons.Height);
            boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 300, boutons.Width, boutons.Height);

            positionTexteVolumeEffects = new Vector2(volumeEffects.Left - spriteFont.MeasureString(effetSonoreT.get() + " :").X, volumeEffects.Center.Y - spriteFont.MeasureString(effetSonoreT.get() + " :").Y / 2);
            positionTexteVolumeFondSonore = new Vector2(volumeFondSonore.Left - spriteFont.MeasureString(fondSonoreT.get() + " :").X, volumeFondSonore.Center.Y - spriteFont.MeasureString(fondSonoreT.get() + " :").Y / 2);
            positionTexteFullscreen = new Vector2(boutonPleinEcran.Left - spriteFont.MeasureString(pleinEcranT.get() + " :").X, boutonPleinEcran.Center.Y - spriteFont.MeasureString(pleinEcranT.get() + " :").Y / 2);
            positionTexteLangue = new Vector2(boutonLangue.Left - spriteFont.MeasureString(langueT.get() + " :").X, boutonLangue.Center.Y - spriteFont.MeasureString(langueT.get() + " :").Y / 2);
            positionTexteResolution = new Vector2(boutonResolution.Left - spriteFont.MeasureString(fondSonoreT.get() + " :").X, boutonResolution.Center.Y - spriteFont.MeasureString(fondSonoreT.get() + " :").Y / 2);

            resolution = CrystalGateGame.graphics.GraphicsDevice.Viewport.Width + "x" + CrystalGateGame.graphics.GraphicsDevice.Viewport.Height;
        }

        public override void Update(GameTime gameTime)
        {
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            if (endFirstClic && mouse.LeftButton == ButtonState.Pressed)
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
                    else if (mouseRec.Intersects(boutonResolution))
                    {
                        _currentRes++;
                        if (_currentRes == possibleResolutions.Count)
                            _currentRes = 0;
                        CrystalGateGame.graphics.PreferredBackBufferWidth = possibleResolutions[_currentRes].width;
                        CrystalGateGame.graphics.PreferredBackBufferHeight = possibleResolutions[_currentRes].height;
                        CrystalGateGame.graphics.ApplyChanges();
                        SceneHandler.UpdatePositions();
                    }
                    else if (mouseRec.Intersects(boutonCredits))
                    {
                        SceneHandler.gameState = GameState.Credits;
                        SceneHandler.creditsScene.ecranPrecedent = GameState.Setting;
                        SceneHandler.creditsScene.Reinit();
                    }
                    else if (mouseRec.Intersects(boutonRetour))
                    {
                        if (isPauseOption)
                            SceneHandler.gameState = GameState.Pause;
                        else
                            SceneHandler.gameState = GameState.MainMenu;
                        endFirstClic = false;
                    }
                }
            }
            else if (!endFirstClic && mouse.LeftButton == ButtonState.Released)
            {
                endFirstClic = true;
            }

            if (isPauseOption && keyboardState.IsKeyDown(Keys.Escape))
            {
                SceneHandler.gameState = GameState.Gameplay;
            }
            fullscreenText = (isFullscreen ? noT.get() : yesT.get());

            positionTexteVolumeEffects = new Vector2(volumeEffects.Left - spriteFont.MeasureString(effetSonoreT.get() + " :").X, volumeEffects.Center.Y - spriteFont.MeasureString(effetSonoreT.get() + " :").Y / 2);
            positionTexteVolumeFondSonore = new Vector2(volumeFondSonore.Left - spriteFont.MeasureString(fondSonoreT.get() + " :").X, volumeFondSonore.Center.Y - spriteFont.MeasureString(fondSonoreT.get() + " :").Y / 2);
            positionTexteFullscreen = new Vector2(boutonPleinEcran.Left - spriteFont.MeasureString(pleinEcranT.get() + " :").X, boutonPleinEcran.Center.Y - spriteFont.MeasureString(pleinEcranT.get() + " :").Y / 2);
            positionTexteLangue = new Vector2(boutonLangue.Left - spriteFont.MeasureString(langueT.get() + " :").X, boutonLangue.Center.Y - spriteFont.MeasureString(langueT.get() + " :").Y / 2);
            positionTexteResolution = new Vector2(boutonResolution.Left - spriteFont.MeasureString(resolutionT.get() + " :").X, boutonResolution.Center.Y - spriteFont.MeasureString(resolutionT.get() + " :").Y / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.Begin();
            if (isPauseOption)
                spriteBatch.Draw(blank, fullscene, new Color(0, 0, 0, 127));
            else
                spriteBatch.Draw(background, fullscene, Color.White);

            if (mouseRec.Intersects(boutonPleinEcran))
                spriteBatch.Draw(boutons, boutonPleinEcran, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonPleinEcran, Color.White);
            if (mouseRec.Intersects(boutonLangue))
                spriteBatch.Draw(boutons, boutonLangue, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonLangue, Color.White);
            if (mouseRec.Intersects(boutonResolution))
                spriteBatch.Draw(boutons, boutonResolution, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonResolution, Color.White);
            if (mouseRec.Intersects(boutonCredits))
                spriteBatch.Draw(boutons, boutonCredits, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonCredits, Color.White);
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
                _currentLanguage.ToString(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(_currentLanguage.ToString()).X / 2,
                    boutonLangue.Top + 10),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                creditsT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(creditsT.get()).X / 2,
                    boutonCredits.Top + 10),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                retourT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(retourT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                resolution,
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(resolution).X / 2,
                    boutonResolution.Top + 10),
                Color.White);

            spriteBatch.DrawString(spriteFont, effetSonoreT.get() + " :", positionTexteVolumeEffects, Color.Gold);
            spriteBatch.DrawString(spriteFont, fondSonoreT.get() + " :", positionTexteVolumeFondSonore, Color.Gold);
            spriteBatch.DrawString(spriteFont, pleinEcranT.get() + " :", positionTexteFullscreen, Color.Gold);
            spriteBatch.DrawString(spriteFont, resolutionT.get() + " :", positionTexteResolution, Color.Gold);
            spriteBatch.DrawString(spriteFont, langueT.get() + " :", positionTexteLangue, Color.Gold);

            spriteBatch.Draw(curseur, new Vector2(mouse.X, mouse.Y), Color.White);

            spriteBatch.End();
        }
    }
}
