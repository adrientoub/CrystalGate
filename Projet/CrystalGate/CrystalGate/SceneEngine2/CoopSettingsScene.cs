using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CrystalGate.SceneEngine2
{
    class CoopSettingsScene : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private Rectangle champIP, boutonLancerLeJeu, boutonRetour;

        private Text lancerJeuT, retourJeuT;

        private string textAsWrited;

        public override void Initialize()
        {
            textAsWrited = "";
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            lancerJeuT = new Text("LaunchGame");
            retourJeuT = new Text("BackToMenu");

            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
            champIP = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 100, boutons.Width, boutons.Height);
            boutonLancerLeJeu = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
            boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);
        }

        public override void Update(GameTime gameTime)
        {
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (mouseRec.Intersects(boutonLancerLeJeu))
                {
                    SceneHandler.gameState = GameState.Gameplay;
                    FondSonore.Play();
                    GamePlay.timer.Restart();
                }
                else if (mouseRec.Intersects(boutonRetour))
                {
                    SceneHandler.gameState = GameState.MainMenu;
                }
            }
            SaisirTexte(ref textAsWrited);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, fullscene, Color.White);

            if (mouseRec.Intersects(champIP))
                spriteBatch.Draw(boutons, champIP, Color.Gray);
            else
                spriteBatch.Draw(boutons, champIP, Color.White);

            if (mouseRec.Intersects(boutonLancerLeJeu))
                spriteBatch.Draw(boutons, boutonLancerLeJeu, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonLancerLeJeu, Color.White);
            if (mouseRec.Intersects(boutonRetour))
                spriteBatch.Draw(boutons, boutonRetour, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonRetour, Color.White);

            spriteBatch.DrawString(
                spriteFont,
                textAsWrited,
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(textAsWrited).X / 2,
                    champIP.Top + 10),
                Color.White);

            spriteBatch.DrawString(
                spriteFont,
                lancerJeuT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(lancerJeuT.get()).X / 2,
                    boutonLancerLeJeu.Top + 10),
                Color.White);

            spriteBatch.DrawString(
                spriteFont,
                retourJeuT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(retourJeuT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);

            spriteBatch.Draw(curseur, new Vector2(mouse.X, mouse.Y), Color.White);

            spriteBatch.End();
        }

        public void SaisirTexte(ref string text)
        {
            Keys[] pressedKeys = keyboardState.GetPressedKeys();
            Keys[] prevPressedKeys = oldKeyboardState.GetPressedKeys();

            char c;

            if (text.Length < 15)
            {
                foreach (Keys key in pressedKeys)
                {
                    if (!prevPressedKeys.Contains(key))
                    {
                        string keyString = key.ToString();

                        if (key == Keys.OemPeriod)
                        {
                            text += '.';
                        }
                        else if (keyString.Length == 7)
                        {
                            c = keyString[6];
                            if (c >= '0' && c <= '9')
                                text += c;
                            else if (c == 'l')
                                text += '.';
                        }
                        else if (keyString.Length == 2)
                        {
                            c = keyString[1];
                            if (c >= '0' && c <= '9')
                                text += c;
                        }
                    }
                }
            }

            // Delete
            if (keyboardState.IsKeyDown(Keys.Back) && oldKeyboardState.IsKeyUp(Keys.Back))
            {
                oldKeyboardState = keyboardState;
                prevPressedKeys = pressedKeys;
                string text2 = "";
                for (int i = 0; i < text.Length - 1; i++)
                    text2 += text[i];

                text = text2;
            }
        }
    }
}
