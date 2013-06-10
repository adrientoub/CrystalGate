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
        private Rectangle champPseudo, boutonLancerLeJeu, boutonRetour;

        private Text lancerJeuT, retourJeuT, pseudoT;

        private Vector2 positionTextePseudo;

        public static string textAsWrited;

        private bool firstTime, firstTimeConnected;
        public bool lancerJeuActive, isServer;

        public static bool isOnlinePlay;

        public override void Initialize()
        {
            textAsWrited = "";
            lancerJeuActive = false;
            firstTime = true;
            firstTimeConnected = true;
            isOnlinePlay = false;
            Reseau.Connexion.InitializeConnexion();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            lancerJeuT = new Text("LaunchGame");
            retourJeuT = new Text("BackToMenu");
            pseudoT = new Text("Pseudo");

            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);

            champPseudo = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 100, boutons.Width, boutons.Height);
            boutonLancerLeJeu = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
            boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);

            positionTextePseudo = new Vector2(champPseudo.Left - spriteFont.MeasureString(pseudoT.get() + " :").X, champPseudo.Center.Y - spriteFont.MeasureString(pseudoT.get() + " :").Y / 2);
        }


        public override void Update(GameTime gameTime)
        {
            if (firstTime)
            {
                isServer = SceneHandler.coopSettingsScene.isServer;
                Reseau.Connexion.Connect();
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
                    isOnlinePlay = true;
                }
                else if (mouseRec.Intersects(boutonRetour))
                {
                    SceneHandler.gameState = GameState.CoopSettings;
                    firstTime = true;
                }
            }
            SaisirTexte(ref textAsWrited);

            if (Reseau.Connexion.isConnected && Reseau.Connexion.selfPlayer != null) // A chaque frame on envoie notre joueur
            {
                Reseau.Connexion.selfPlayer.name = textAsWrited;
                Reseau.Connexion.SendPlayer();
            }

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
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(textAsWrited).X / 2,
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
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(lancerJeuT.get()).X / 2,
                    boutonLancerLeJeu.Top + 10),
                c);

            spriteBatch.DrawString(
                spriteFont,
                retourJeuT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(retourJeuT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);

            spriteBatch.DrawString(spriteFont, pseudoT.get() + " :", positionTextePseudo, Color.Gold);

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
