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
    class ChampionSelection : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private List<Rectangle> portraits;
        private List<Texture2D> imagesPortraits;
        private List<Color> couleurAffichagePortrait;
        public int personnageSelectionne;
        private int nbFrames;
        bool lancerLeJeuActive;
        public bool Error;

        private Text lancerJeuT, retourJeuT;

        private Texture2D fondLobby;
        private Rectangle[] positionJoueur;
        private Rectangle[] positionsImages;

        private Rectangle boutonLancerLeJeu, boutonRetour;

        public override void Initialize()
        {
            personnageSelectionne = -1;
            lancerLeJeuActive = false;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            fondLobby = content.Load<Texture2D>("Menu/Lobby");

            lancerJeuT = new Text("LaunchGame");
            retourJeuT = new Text("BackToMenu");

            imagesPortraits = new List<Texture2D>();
            imagesPortraits.Add(content.Load<Texture2D>("UI/guerrierIcone"));
            imagesPortraits.Add(content.Load<Texture2D>("UI/voleurIcone"));
            portraits = new List<Rectangle>();
            couleurAffichagePortrait = new List<Color>();
            for (int i = 0; i < imagesPortraits.Count; i++)
            {
                portraits.Add(new Rectangle(CrystalGateGame.graphics.PreferredBackBufferWidth / 2 + (i - imagesPortraits.Count / 2) * (imagesPortraits[i].Bounds.Width + 30), CrystalGateGame.graphics.PreferredBackBufferHeight / 2 - 200,
                imagesPortraits[i].Bounds.Width, imagesPortraits[i].Bounds.Height));
                couleurAffichagePortrait.Add(Color.White);
            }
            positionsImages = new Rectangle[4];
            positionJoueur = new Rectangle[4];
            nbFrames = 0;

            UpdatePositions();
        }

        public void UpdatePositions()
        {
            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.PreferredBackBufferWidth, CrystalGateGame.graphics.PreferredBackBufferHeight);

            if (CoopConnexionScene.isOnlinePlay)
            {
                positionJoueur[0] = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 150, fondLobby.Width, fondLobby.Height);
                positionJoueur[1] = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 50, fondLobby.Width, fondLobby.Height);
                positionJoueur[2] = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 50, fondLobby.Width, fondLobby.Height);
                positionJoueur[3] = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 150, fondLobby.Width, fondLobby.Height);

                boutonLancerLeJeu = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - boutons.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
                boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - boutons.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);

                for (int i = 0; i < positionsImages.Length; i++)
                {
                    positionsImages[i] = new Rectangle(positionJoueur[i].X + 10, positionJoueur[i].Y + 10, 60, 60);
                }
            }
            else
            {
                boutonLancerLeJeu = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
                boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);
            }

            for (int i = 0; i < imagesPortraits.Count; i++)
            {
                portraits[i] = new Rectangle(CrystalGateGame.graphics.PreferredBackBufferWidth / 2 + (i - imagesPortraits.Count / 2) * (imagesPortraits[i].Bounds.Width + 30), CrystalGateGame.graphics.PreferredBackBufferHeight / 2 - 200,
                imagesPortraits[i].Bounds.Width, imagesPortraits[i].Bounds.Height);
            }
        }

        public override void Update(GameTime gameTime)
        {
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            UpdatePositions();
            nbFrames++;
            for (int i = 0; i < portraits.Count; i++)
            {
                if (personnageSelectionne == i)
                {
                    couleurAffichagePortrait[i] = Color.Cyan;
                }
                else if (mouseRec.Intersects(portraits[i]))
                {
                    couleurAffichagePortrait[i] = Color.Gray;
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                    {
                        personnageSelectionne = i;
                    }
                }
                else
                {
                    couleurAffichagePortrait[i] = Color.White;
                }
            }

            if (CoopConnexionScene.isOnlinePlay)
            {
                if (Client.isConnected)
                {
                    Client.ownPlayer.championChoisi = personnageSelectionne;
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
                }

                if (Serveur.IsRunning) // Si on est le serveur
                {
                    bool everybodyHaveChose = true;
                    for (int i = 0; i < Client.joueursConnectes.Count; i++)
                    {
                        everybodyHaveChose &= (Client.joueursConnectes[i].championChoisi != 1);
                    }
                    if (everybodyHaveChose)
                        lancerLeJeuActive = true;
                }
                else // Le client ne peut pas lancer le jeu
                {
                    lancerLeJeuActive = false;
                }
            }
            else // Si on joue en local il faut avoir sélectionné un perso avant de lancer le jeu
                if (personnageSelectionne == -1)
                    lancerLeJeuActive = false;
                else
                    lancerLeJeuActive = true;

            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (mouseRec.Intersects(boutonLancerLeJeu) && personnageSelectionne != -1)
                {
                    // C'est parti!
                    if (Serveur.IsRunning)
                        Serveur.Send( BitConverter.GetBytes(3));
                    else
                        if (!Client.isConnected)
                        {
                            SceneHandler.ResetGameplay();
                            SceneHandler.gameState = GameState.Gameplay;
                            FondSonore.Play();
                            GamePlay.timer.Restart();
                        }
                }
                else if (mouseRec.Intersects(boutonRetour))
                    if (CoopConnexionScene.isOnlinePlay)
                        SceneHandler.gameState = GameState.CoopConnexion;
                    else
                        SceneHandler.gameState = GameState.MainMenu;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, fullscene, Color.White);

            for (int i = 0; i < imagesPortraits.Count; i++)
            {
                spriteBatch.Draw(imagesPortraits[i], portraits[i], couleurAffichagePortrait[i]);
            }

            if (mouseRec.Intersects(boutonLancerLeJeu) || !lancerLeJeuActive)
                spriteBatch.Draw(boutons, boutonLancerLeJeu, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonLancerLeJeu, Color.White);

            if (mouseRec.Intersects(boutonRetour))
                spriteBatch.Draw(boutons, boutonRetour, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonRetour, Color.White);

            spriteBatch.DrawString(
                spriteFont,
                lancerJeuT.get(),
                new Vector2(boutonLancerLeJeu.Center.X - spriteFont.MeasureString(lancerJeuT.get()).X / 2,
                    boutonLancerLeJeu.Top + 10),
                Color.White);

            spriteBatch.DrawString(
                spriteFont,
                retourJeuT.get(),
                new Vector2(boutonRetour.Center.X - spriteFont.MeasureString(retourJeuT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);

            for (int i = 0; i < positionJoueur.Length; i++)
            {
                if (mouseRec.Intersects(positionJoueur[i]))
                    spriteBatch.Draw(fondLobby, positionJoueur[i], Color.Gray);
                else
                    spriteBatch.Draw(fondLobby, positionJoueur[i], Color.White);
            }

            for (int i = 0; i < Client.joueursConnectes.Count; i++)
            {
                if (Client.joueursConnectes[i].championChoisi != -1)
                {
                    spriteBatch.Draw(imagesPortraits[Client.joueursConnectes[i].championChoisi], positionsImages[i], Color.White);
                }
                spriteBatch.DrawString(spriteFont, SceneHandler.coopConnexionScene.pseudoJoueurs[i], new Vector2(positionJoueur[i].Center.X - spriteFont.MeasureString(SceneHandler.coopConnexionScene.pseudoJoueurs[i]).X / 2, positionJoueur[i].Top + 10), Color.White);
            }

            if (Error)
                spriteBatch.DrawString(spriteFont, "Le client a recontré un problème veuillez reesayer", Vector2.Zero, Color.White);

            spriteBatch.Draw(curseur, new Vector2(mouse.X, mouse.Y), Color.White);

            spriteBatch.End();
        }
    }
}
