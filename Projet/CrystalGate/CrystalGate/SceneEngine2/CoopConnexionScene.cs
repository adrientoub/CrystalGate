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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CrystalGate.SceneEngine2
{
    class CoopConnexionScene : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private Rectangle champPseudo, boutonLancerLeJeu, boutonRetour;

        private Text lancerJeuT, retourJeuT, pseudoT;
        private Text waitClientT, waitLaunchT, tryToConnectServT;

        private Vector2 positionTextePseudo;
        private Texture2D fondLobby;
        private Rectangle positionJ1, positionJ2, positionJ3, positionJ4;

        private string currentMessage;
        public string[] pseudoJoueurs;
        private int nbFrames;
        private string suspension;

        public static string textAsWrited;

        public bool lancerJeuActive, isServer, Error;

        public static bool isOnlinePlay;

        public override void Initialize()
        {
            textAsWrited = "";
            isOnlinePlay = false;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            lancerJeuT = new Text("SelectChampion");
            retourJeuT = new Text("BackToMenu");
            pseudoT = new Text("Pseudo");
            waitClientT = new Text("WaitClient");
            waitLaunchT = new Text("WaitLaunch");
            tryToConnectServT = new Text("TryToConnectServ");

            fondLobby = content.Load<Texture2D>("Menu/Lobby");

            nbFrames = 0;

            currentMessage = isServer ? waitClientT.get() : tryToConnectServT.get();
            pseudoJoueurs = new string[] {
                "","","",""
            };

            UpdatePositions();

            positionTextePseudo = new Vector2(champPseudo.Left - spriteFont.MeasureString(pseudoT.get() + " :").X, champPseudo.Center.Y - spriteFont.MeasureString(pseudoT.get() + " :").Y / 2);
        }

        public void UpdatePositions()
        {
            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);

            champPseudo = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - boutons.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 100, boutons.Width, boutons.Height);
            boutonLancerLeJeu = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - boutons.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
            boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - boutons.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);

            positionJ1 = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 150, fondLobby.Width, fondLobby.Height);
            positionJ2 = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 50, fondLobby.Width, fondLobby.Height);
            positionJ3 = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 50, fondLobby.Width, fondLobby.Height);
            positionJ4 = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 150, fondLobby.Width, fondLobby.Height);
        }

        public override void Update(GameTime gameTime)
        {
            isServer = SceneHandler.coopSettingsScene.isServer;
            lancerJeuActive = Serveur.clients.Count >= 2 || Client.isConnected && !isServer;

            nbFrames++;
            if (nbFrames == 1)
            {
                suspension = ".";
            }
            else if (nbFrames == 60)
            {
                suspension = "..";
            }
            else if (nbFrames == 120)
            {
                suspension = "...";
            }
            else if (nbFrames == 180)
            {
                nbFrames = 0;
            }

            if (Client.isConnected)
            {
                currentMessage = waitLaunchT.get();
                Client.ownPlayer.name = textAsWrited;
                if (nbFrames % 15 == 0)
                {
                    // On s'envoit
                    MemoryStream stream = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, Client.ownPlayer);
                    byte[] buffer = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(buffer, 0, buffer.Length);

                    // Envoi
                    Client.Send(buffer, 1);
                }

                // Mise à jour des pseudos
                for (int i = 0; i < Client.joueursConnectes.Count && i < pseudoJoueurs.Length; i++)
                {
                    pseudoJoueurs[i] = Client.joueursConnectes[i].name;
                }
            }
            else
            {
                pseudoJoueurs[0] = textAsWrited;
            }

            mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);
            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (lancerJeuActive && mouseRec.Intersects(boutonLancerLeJeu))
                {
                    isOnlinePlay = true;
                    SceneHandler.gameState = GameState.ChampionSelection;
                }
                else if (mouseRec.Intersects(boutonRetour))
                    SceneHandler.gameState = GameState.CoopSettings;
            }
            SaisirTexte(ref textAsWrited);

            positionTextePseudo = new Vector2(champPseudo.Left - spriteFont.MeasureString(pseudoT.get() + " :").X, champPseudo.Center.Y - spriteFont.MeasureString(pseudoT.get() + " :").Y / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, fullscene, Color.White);

            if (mouseRec.Intersects(champPseudo))
                spriteBatch.Draw(boutons, champPseudo, Color.Gray);
            else
                spriteBatch.Draw(boutons, champPseudo, Color.White);

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
                new Vector2(champPseudo.Center.X - spriteFont.MeasureString(textAsWrited).X / 2,
                    champPseudo.Top + 10),
                Color.White);

            Color c;
            if (lancerJeuActive)
                c = Color.White;
            else
                c = Color.Gray;

            spriteBatch.DrawString(
                spriteFont,
                lancerJeuT.get(),
                new Vector2(boutonLancerLeJeu.Center.X - spriteFont.MeasureString(lancerJeuT.get()).X / 2,
                    boutonLancerLeJeu.Top + 10),
                c);

            spriteBatch.DrawString(
                spriteFont,
                retourJeuT.get(),
                new Vector2(boutonRetour.Center.X - spriteFont.MeasureString(retourJeuT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);

            spriteBatch.DrawString(spriteFont, pseudoT.get() + " :", positionTextePseudo, Color.Gold);

            // Affichage du lobby :
            spriteBatch.DrawString(spriteFont, currentMessage + suspension, new Vector2(positionJ1.Center.X - spriteFont.MeasureString(currentMessage).X / 2, positionJ1.Top - 80), Color.White);
            if (mouseRec.Intersects(positionJ1))
                spriteBatch.Draw(fondLobby, positionJ1, Color.Gray);
            else
                spriteBatch.Draw(fondLobby, positionJ1, Color.White);
            if (mouseRec.Intersects(positionJ2))
                spriteBatch.Draw(fondLobby, positionJ2, Color.Gray);
            else
                spriteBatch.Draw(fondLobby, positionJ2, Color.White);
            if (mouseRec.Intersects(positionJ3))
                spriteBatch.Draw(fondLobby, positionJ3, Color.Gray);
            else
                spriteBatch.Draw(fondLobby, positionJ3, Color.White);
            if (mouseRec.Intersects(positionJ4))
                spriteBatch.Draw(fondLobby, positionJ4, Color.Gray);
            else
                spriteBatch.Draw(fondLobby, positionJ4, Color.White);
            spriteBatch.DrawString(spriteFont, pseudoJoueurs[0], new Vector2(positionJ1.Center.X - spriteFont.MeasureString(pseudoJoueurs[0]).X / 2, positionJ1.Top + 10), Color.White);
            spriteBatch.DrawString(spriteFont, pseudoJoueurs[1], new Vector2(positionJ2.Center.X - spriteFont.MeasureString(pseudoJoueurs[1]).X / 2, positionJ2.Top + 10), Color.White);
            spriteBatch.DrawString(spriteFont, pseudoJoueurs[2], new Vector2(positionJ3.Center.X - spriteFont.MeasureString(pseudoJoueurs[2]).X / 2, positionJ3.Top + 10), Color.White);
            spriteBatch.DrawString(spriteFont, pseudoJoueurs[3], new Vector2(positionJ4.Center.X - spriteFont.MeasureString(pseudoJoueurs[3]).X / 2, positionJ4.Top + 10), Color.White);
            // Fin d'affichage du lobby
            if(Error)
                spriteBatch.DrawString(spriteFont, "Le client a recontré un problème veuillez reesayer", Vector2.Zero, Color.White);
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
