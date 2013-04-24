using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CrystalGate.SceneEngine2
{
    class CoopConnexionScene : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private Rectangle champIP, boutonLancerLeJeu, boutonRetour;

        private Text lancerJeuT, retourJeuT;

        private string textAsWrited;

        private bool isServer, lancerJeuActive, firstTime;

        Socket soc, clientSoc;
        SocketAsyncEventArgs socEvent;

        public override void Initialize()
        {
            textAsWrited = "";
            lancerJeuActive = false;
            firstTime = true;
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

        public void ClientConnected(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
            {
                lancerJeuActive = true;
            }
        }

        public void ServerConnected(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
            {
                clientSoc = (Socket)result.AsyncState;
                lancerJeuActive = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (firstTime)
            {
                isServer = SceneHandler.coopSettingsScene.isServer;
                soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socEvent = new SocketAsyncEventArgs();

                if (isServer)
                {
                    AsyncCallback sc = new AsyncCallback(ServerConnected);
                    soc.Bind(new IPEndPoint(IPAddress.Any, 6060));
                    soc.Listen(1);
                    soc.BeginAccept(sc, "serv");
                    socEvent.AcceptSocket = clientSoc;
                }
                else
                {
                    AsyncCallback cc = new AsyncCallback(ClientConnected);
                    soc.BeginConnect(IPAddress.Parse(SceneHandler.coopSettingsScene.textAsWrited), 5050, cc, "clie");
                }
                socEvent.Completed += delegate
                {
                    lancerJeuActive = true;
                };
                firstTime = false;
            }

            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (lancerJeuActive && mouseRec.Intersects(boutonLancerLeJeu))
                {
                    SceneHandler.gameState = GameState.Gameplay;
                    FondSonore.Play();
                    GamePlay.timer.Restart();
                    SceneHandler.gameplayScene.isCoopPlay = true;
                    SceneHandler.gameplayScene.isServer = isServer;
                }
                else if (mouseRec.Intersects(boutonRetour))
                {
                    SceneHandler.gameState = GameState.CoopSettings;
                    firstTime = true;
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

            if (lancerJeuActive)
            {
                if (mouseRec.Intersects(boutonLancerLeJeu))
                    spriteBatch.Draw(boutons, boutonLancerLeJeu, Color.Gray);
                else
                    spriteBatch.Draw(boutons, boutonLancerLeJeu, Color.White);
            }
            else
            {
                spriteBatch.Draw(boutons, boutonLancerLeJeu, Color.Gray);
            }

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

            Color c;
            if (lancerJeuActive)
                c = Color.White;
            else
                c = Color.Gray;

            spriteBatch.DrawString(
                spriteFont,
                lancerJeuT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(lancerJeuT.get()).X / 2,
                    boutonLancerLeJeu.Top + 10),
                c);

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
                bool shiftPressed = pressedKeys.Contains(Keys.LeftShift) || pressedKeys.Contains(Keys.RightShift);
                foreach (Keys key in pressedKeys)
                {
                    if (!prevPressedKeys.Contains(key))
                    {
                        string keyString = key.ToString();

                        if (keyString.Length == 1)
                        {
                            c = keyString[0];
                            if (c >= 'A' && c <= 'Z')
                                if (shiftPressed)
                                    text += c;
                                else
                                    text += (char)(c - 'A' + 'a');
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
