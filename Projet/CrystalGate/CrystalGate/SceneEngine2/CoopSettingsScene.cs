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
    class CoopSettingsScene : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private Rectangle boutonServeurOuClient, champIP, boutonConnexion, boutonRetour;

        private Text serveurT, clientT, lancerJeuT, retourJeuT, ipT, modeT, hostT;

        public string textAsWrited;

        public static IPAddress ip;

        private Vector2 positionTexteIP, positionTexteMode;

        public bool isServer, validIpAddress;

        public override void Initialize()
        {
            textAsWrited = "";
            validIpAddress = false;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            lancerJeuT = new Text("LaunchConnexion");
            retourJeuT = new Text("BackToMenu");
            serveurT = new Text("Server");
            clientT = new Text("Client");
            ipT = new Text("IP");
            modeT = new Text("Mode");
            hostT = new Text("Host");

            ip = IPAddress.Parse("127.0.0.1");

            UpdatePositions();

            positionTexteIP = new Vector2(champIP.Left - spriteFont.MeasureString(ipT.get() + " :").X, champIP.Center.Y - spriteFont.MeasureString(ipT.get() + " :").Y / 2);
            positionTexteMode = new Vector2(boutonServeurOuClient.Left - spriteFont.MeasureString(modeT.get() + " :").X, boutonServeurOuClient.Center.Y - spriteFont.MeasureString(modeT.get() + " :").Y / 2);
        }

        public void UpdatePositions()
        {
            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);

            champIP = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 150, boutons.Width, boutons.Height);
            boutonServeurOuClient = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 50, boutons.Width, boutons.Height);
            boutonConnexion = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 50, boutons.Width, boutons.Height);
            boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 150, boutons.Width, boutons.Height);
        }

        public override void Update(GameTime gameTime)
        {
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            validIpAddress = isServer || IPAddress.TryParse(textAsWrited, out ip);

            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (mouseRec.Intersects(boutonConnexion) && validIpAddress)
                {
                    if (isServer)
                        Serveur.Host();
                    else
                        Client.Connect();

                    SceneHandler.gameState = GameState.CoopConnexion;
                }
                else if (mouseRec.Intersects(boutonServeurOuClient))
                    isServer = !isServer;
                else if (mouseRec.Intersects(boutonRetour))
                {
                    SceneHandler.gameState = GameState.MainMenu;
                    Serveur.Shutdown();
                }
            }
            if (!isServer)
                SaisirTexte(ref textAsWrited);

            positionTexteIP = new Vector2(champIP.Left - spriteFont.MeasureString(ipT.get() + " :").X, champIP.Center.Y - spriteFont.MeasureString(ipT.get() + " :").Y / 2);
            positionTexteMode = new Vector2(boutonServeurOuClient.Left - spriteFont.MeasureString(modeT.get() + " :").X, boutonServeurOuClient.Center.Y - spriteFont.MeasureString(modeT.get() + " :").Y / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, fullscene, Color.White);

            if (mouseRec.Intersects(boutonServeurOuClient))
                spriteBatch.Draw(boutons, boutonServeurOuClient, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonServeurOuClient, Color.White);

            if (!isServer)
            {
                if (mouseRec.Intersects(champIP))
                    spriteBatch.Draw(boutons, champIP, Color.Gray);
                else
                    spriteBatch.Draw(boutons, champIP, Color.White);

                spriteBatch.DrawString(
                    spriteFont,
                    textAsWrited,
                    new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(textAsWrited).X / 2,
                        champIP.Top + 10),
                    Color.White);

                spriteBatch.DrawString(spriteFont, ipT.get() + " :", positionTexteIP, Color.Gold);
            }

            Color c;
            if (validIpAddress)
                c = Color.White;
            else
                c = Color.Gray;

            if (mouseRec.Intersects(boutonConnexion))
                spriteBatch.Draw(boutons, boutonConnexion, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonConnexion, c);

            if (mouseRec.Intersects(boutonRetour))
                spriteBatch.Draw(boutons, boutonRetour, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonRetour, Color.White);

            spriteBatch.DrawString(
                spriteFont,
                isServer ? serveurT.get() : clientT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(isServer ? serveurT.get() : clientT.get()).X / 2,
                    boutonServeurOuClient.Top + 10),
                Color.White);

            spriteBatch.DrawString(
                spriteFont,
                (isServer) ? hostT.get() : lancerJeuT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString((isServer) ? hostT.get() : lancerJeuT.get()).X / 2,
                    boutonConnexion.Top + 10),
                c);

            spriteBatch.DrawString(
                spriteFont,
                retourJeuT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(retourJeuT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);

            spriteBatch.DrawString(spriteFont, modeT.get() + " :", positionTexteMode, Color.Gold);

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
