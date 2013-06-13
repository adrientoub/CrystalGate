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
        private int personnageSelectionne;
        
        private Text lancerJeuT, retourJeuT;

        private Texture2D fondLobby;
        private Rectangle positionJ1, positionJ2, positionJ3, positionJ4;

        private Rectangle boutonLancerLeJeu, boutonRetour;

        public override void Initialize()
        {
            personnageSelectionne = -1;
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
            imagesPortraits.Add(content.Load<Texture2D>("UI/assassinIcone"));
            portraits = new List<Rectangle>();
            couleurAffichagePortrait = new List<Color>();
            for (int i = 0; i < imagesPortraits.Count; i++)
            {
                portraits.Add(new Rectangle(CrystalGateGame.graphics.PreferredBackBufferWidth / 2 + (i - imagesPortraits.Count / 2) * (imagesPortraits[i].Bounds.Width + 30), CrystalGateGame.graphics.PreferredBackBufferHeight / 2 - 200,
                imagesPortraits[i].Bounds.Width, imagesPortraits[i].Bounds.Height));
                couleurAffichagePortrait.Add(Color.White);
            }

            UpdatePositions();
        }

        public void UpdatePositions()
        {
            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.PreferredBackBufferWidth, CrystalGateGame.graphics.PreferredBackBufferHeight);

            positionJ1 = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 150, fondLobby.Width, fondLobby.Height);
            positionJ2 = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 50, fondLobby.Width, fondLobby.Height);
            positionJ3 = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 50, fondLobby.Width, fondLobby.Height);
            positionJ4 = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - fondLobby.Width) / 2 + fondLobby.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 150, fondLobby.Width, fondLobby.Height);

            boutonLancerLeJeu = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - boutons.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
            boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - boutons.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);
        }

        public override void Update(GameTime gameTime)
        {
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
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

            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (mouseRec.Intersects(boutonLancerLeJeu))
                {
                    SceneHandler.ResetGameplay();
                    SceneHandler.gameState = GameState.Gameplay;
                    FondSonore.Play();
                    GamePlay.timer.Restart();
                }
                else if (mouseRec.Intersects(boutonRetour))
                    SceneHandler.gameState = GameState.CoopConnexion;
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
            spriteBatch.DrawString(spriteFont, SceneHandler.coopConnexionScene.pseudoJoueurs[0], new Vector2(positionJ1.Center.X - spriteFont.MeasureString(SceneHandler.coopConnexionScene.pseudoJoueurs[0]).X / 2, positionJ1.Top + 10), Color.White);
            spriteBatch.DrawString(spriteFont, SceneHandler.coopConnexionScene.pseudoJoueurs[1], new Vector2(positionJ2.Center.X - spriteFont.MeasureString(SceneHandler.coopConnexionScene.pseudoJoueurs[1]).X / 2, positionJ2.Top + 10), Color.White);
            spriteBatch.DrawString(spriteFont, SceneHandler.coopConnexionScene.pseudoJoueurs[2], new Vector2(positionJ3.Center.X - spriteFont.MeasureString(SceneHandler.coopConnexionScene.pseudoJoueurs[2]).X / 2, positionJ3.Top + 10), Color.White);
            spriteBatch.DrawString(spriteFont, SceneHandler.coopConnexionScene.pseudoJoueurs[3], new Vector2(positionJ4.Center.X - spriteFont.MeasureString(SceneHandler.coopConnexionScene.pseudoJoueurs[3]).X / 2, positionJ4.Top + 10), Color.White);

            spriteBatch.Draw(curseur, new Vector2(mouse.X, mouse.Y), Color.White);

            spriteBatch.End();
        }
    }
}
