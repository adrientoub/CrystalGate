using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CrystalGateEditor.SceneEngine2
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

        private Texture2D blank;

        private Rectangle mouseRec;

        public static bool isPauseOption;
        public static bool endFirstClic;

        private Rectangle boutonPleinEcran, boutonLangue, boutonRetour;
        private Vector2 positionTexteFullscreen, positionTexteLangue;

        private Text pleinEcranT, langueT, retourT, noT, yesT;

        private static Language _currentLanguage = Language.Français;
        string fullscreenText;

        public override void Initialize()
        {
            fullscreenText = isFullscreen ? noT.get() : yesT.get();
            isPauseOption = false;
            endFirstClic = false;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            blank = content.Load<Texture2D>("blank");

            pleinEcranT = new Text("Fullscreen");
            langueT = new Text("Language");
            retourT = new Text("Back");
            noT = new Text("no");
            yesT = new Text("yes");

            boutonPleinEcran = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 100, boutons.Width, boutons.Height);
            boutonLangue = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
            boutonRetour = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);

            positionTexteFullscreen = new Vector2(boutonPleinEcran.Left - spriteFont.MeasureString(pleinEcranT.get()).X, boutonPleinEcran.Center.Y - spriteFont.MeasureString(pleinEcranT.get()).Y / 2);
            positionTexteLangue = new Vector2(boutonLangue.Left - spriteFont.MeasureString(langueT.get()).X, boutonLangue.Center.Y - spriteFont.MeasureString(langueT.get()).Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            if (endFirstClic && mouse.LeftButton == ButtonState.Pressed)
            {
                if (oldMouse.LeftButton == ButtonState.Released)
                {
                    if (mouseRec.Intersects(boutonPleinEcran))
                    {
                        isFullscreen = !isFullscreen;
                        Game1.graphics.ToggleFullScreen();
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
            positionTexteFullscreen = new Vector2(boutonPleinEcran.Left - spriteFont.MeasureString(pleinEcranT.get()).X, boutonPleinEcran.Center.Y - spriteFont.MeasureString(pleinEcranT.get()).Y / 2);
            positionTexteLangue = new Vector2(boutonLangue.Left - spriteFont.MeasureString(langueT.get()).X, boutonLangue.Center.Y - spriteFont.MeasureString(langueT.get()).Y / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var fullscene = new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height);
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
            if (mouseRec.Intersects(boutonRetour))
                spriteBatch.Draw(boutons, boutonRetour, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonRetour, Color.White);

            spriteBatch.DrawString(
                spriteFont,
                fullscreenText,
                new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(fullscreenText).X / 2,
                    boutonPleinEcran.Top + 10),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                _currentLanguage.ToString(),
                new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(_currentLanguage.ToString()).X / 2,
                    boutonLangue.Top + 10),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                retourT.get(),
                new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(retourT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);

            spriteBatch.DrawString(spriteFont, pleinEcranT.get(), positionTexteFullscreen, Color.Blue);
            spriteBatch.DrawString(spriteFont, langueT.get(), positionTexteLangue, Color.Blue);

            spriteBatch.End();
        }
    }
}
