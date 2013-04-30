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
    public class EditorSettings : BaseScene
    {
        enum Phase
        {
            LoadOrCreate,
            NouvelleMap,
            ChargerMap,
            Draw,
            Nom,
            TailleX,
            TailleY
        }

        private ContentManager content;

        private Phase phase;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private Rectangle bouton1, bouton2, bouton3, bouton4;

        private Text serveurT, clientT, lancerJeuT, retourJeuT, ipT, modeT;

        public string textAsWrited;

        private Vector2 positionDescriptionBouton1, positionDescriptionBouton2, positionDescriptionBouton3, positionDescriptionBouton4;

        private string texteDescriptionBouton1, texteDescriptionBouton2, texteDescriptionBouton3, texteDescriptionBouton4;
        private string texteBouton1, texteBouton2, texteBouton3, texteBouton4;

        private string nomMap, longueur, largeur;

        public bool CreateNew, bouton3Lock;

        public override void Initialize()
        {
            textAsWrited = "";
            CreateNew = true;
            bouton3Lock = false;
            phase = Phase.LoadOrCreate;
            texteBouton1 = "";
            texteBouton2 = "";
            texteBouton3 = "";
            texteBouton4 = "";

            nomMap = "";
            longueur = "";
            largeur = "";

            texteDescriptionBouton1 = "";
            texteDescriptionBouton2 = "";
            texteDescriptionBouton3 = "";
            texteDescriptionBouton4 = "";
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

            fullscene = new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height);

            bouton1 = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 200, boutons.Width, boutons.Height);
            bouton2 = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 100, boutons.Width, boutons.Height);
            bouton3 = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
            bouton4 = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);

            positionDescriptionBouton1 = new Vector2(bouton1.Left - spriteFont.MeasureString(texteDescriptionBouton1).X, bouton1.Center.Y - spriteFont.MeasureString(texteDescriptionBouton1).Y / 2);
            positionDescriptionBouton2 = new Vector2(bouton2.Left - spriteFont.MeasureString(texteDescriptionBouton2).X, bouton2.Center.Y - spriteFont.MeasureString(texteDescriptionBouton2).Y / 2);
            positionDescriptionBouton3 = new Vector2(bouton3.Left - spriteFont.MeasureString(texteDescriptionBouton3).X, bouton2.Center.Y - spriteFont.MeasureString(texteDescriptionBouton3).Y / 2);
            positionDescriptionBouton4 = new Vector2(bouton4.Left - spriteFont.MeasureString(texteDescriptionBouton4).X, bouton2.Center.Y - spriteFont.MeasureString(texteDescriptionBouton4).Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            switch (phase)
            {
                case Phase.LoadOrCreate:
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                    {
                        if (mouseRec.Intersects(bouton2))
                        {
                            CreateNew = !CreateNew;
                        }
                        else if (mouseRec.Intersects(bouton3) && !bouton3Lock)
                        {
                            phase = CreateNew ? Phase.TailleX : Phase.ChargerMap;
                        }
                        else if (mouseRec.Intersects(bouton4))
                        {
                            SceneHandler.gameState = GameState.MainMenu;
                        }
                    }

                    SceneHandler.editorScene.user.SaisirTexte(ref nomMap, false);
                    if (nomMap == "")
                    {
                        texteBouton1 = " ";
                        bouton3Lock = true;
                    }
                    else
                    {
                        bouton3Lock = false;
                        texteBouton1 = nomMap;
                    }

                    if (CreateNew)
                    {
                        texteBouton2 = "Nouvelle carte";
                        texteBouton3 = "Créer";
                    }
                    else
                    {
                        texteBouton2 = "Charger carte";
                        texteBouton3 = "Charger";
                    }
                    texteBouton4 = "Retour";
                    break;
                case Phase.ChargerMap:
                    if (SceneHandler.editorScene.ui.OuvrirMap(nomMap))
                    {
                        SceneHandler.gameState = GameState.Editor;
                    }
                    else
                    {
                        texteDescriptionBouton1 = "Nom incorrect";
                        phase = Phase.LoadOrCreate;
                    }
                    break;
                case Phase.TailleX:
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                    {
                        if (mouseRec.Intersects(bouton2))
                        {
                            SceneHandler.editorScene.ui.textureStart++;
                            if (SceneHandler.editorScene.ui.textureStart > UI.TextureStart.Hiver)
                                SceneHandler.editorScene.ui.textureStart = 0;
                        }
                        else if (mouseRec.Intersects(bouton3) && !bouton3Lock)
                        {
                            phase = Phase.TailleY;
                        }
                        else if (mouseRec.Intersects(bouton4))
                        {
                            phase = Phase.LoadOrCreate;
                        }
                    }

                    SceneHandler.editorScene.user.SaisirTexte(ref longueur, true);
                    if (longueur == "")
                    {
                        texteBouton1 = " ";
                        bouton3Lock = true;
                    }
                    else
                    {
                        texteBouton1 = longueur;
                        bouton3Lock = false;
                    }
                    texteDescriptionBouton1 = "Longueur";
                    texteBouton2 = SceneHandler.editorScene.ui.textureStart.ToString();;
                    texteBouton3 = "Valider";
                    texteBouton4 = "Retour";
                    break;
                case Phase.TailleY:
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                    {
                        if (mouseRec.Intersects(bouton2))
                        {
                            SceneHandler.editorScene.ui.textureStart++;
                            if (SceneHandler.editorScene.ui.textureStart > UI.TextureStart.Hiver)
                                SceneHandler.editorScene.ui.textureStart = 0;
                        }
                        else if (mouseRec.Intersects(bouton3) && !bouton3Lock)
                        {
                            phase = Phase.NouvelleMap;
                        }
                        else if (mouseRec.Intersects(bouton4))
                        {
                            phase = Phase.TailleX;
                        }
                    }

                    SceneHandler.editorScene.user.SaisirTexte(ref largeur, true);
                    if (largeur == "")
                    {
                        texteBouton1 = " ";
                        bouton3Lock = true;
                    }
                    else
                    {
                        texteBouton1 = largeur;
                        bouton3Lock = false;
                    }
                    texteDescriptionBouton1 = "Largeur";
                    texteBouton2 = SceneHandler.editorScene.ui.textureStart.ToString(); ;
                    texteBouton3 = "Valider";
                    texteBouton4 = "Retour";
                    break;
                case Phase.NouvelleMap:
                    SceneHandler.editorScene.ui.Map = new Vector2[int.Parse(longueur), int.Parse(largeur)];
                    SceneHandler.editorScene.ui.MapName = nomMap;
                    SceneHandler.editorScene.ui.Initialiser();
                    SceneHandler.gameState = GameState.Editor;
                    break;
            }

            positionDescriptionBouton1 = new Vector2(bouton1.Left - spriteFont.MeasureString(texteDescriptionBouton1).X, bouton1.Center.Y - spriteFont.MeasureString(texteDescriptionBouton1).Y / 2);
            positionDescriptionBouton2 = new Vector2(bouton2.Left - spriteFont.MeasureString(texteDescriptionBouton2).X, bouton2.Center.Y - spriteFont.MeasureString(texteDescriptionBouton2).Y / 2);
            positionDescriptionBouton3 = new Vector2(bouton3.Left - spriteFont.MeasureString(texteDescriptionBouton3).X, bouton2.Center.Y - spriteFont.MeasureString(texteDescriptionBouton3).Y / 2);
            positionDescriptionBouton4 = new Vector2(bouton4.Left - spriteFont.MeasureString(texteDescriptionBouton4).X, bouton2.Center.Y - spriteFont.MeasureString(texteDescriptionBouton4).Y / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, fullscene, Color.White);

            if (texteBouton1 != "")
            {
                if (mouseRec.Intersects(bouton1))
                    spriteBatch.Draw(boutons, bouton1, Color.Gray);
                else
                    spriteBatch.Draw(boutons, bouton1, Color.White);

                spriteBatch.DrawString(
                    spriteFont,
                    texteBouton1,
                    new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(texteBouton1).X / 2,
                        bouton1.Top + 10),
                    Color.White);

                spriteBatch.DrawString(spriteFont, texteDescriptionBouton1, positionDescriptionBouton1, Color.Blue);
            }

            if (texteBouton2 != "")
            {
                if (mouseRec.Intersects(bouton2))
                    spriteBatch.Draw(boutons, bouton2, Color.Gray);
                else
                    spriteBatch.Draw(boutons, bouton2, Color.White);

                spriteBatch.DrawString(
                    spriteFont,
                    texteBouton2,
                    new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(texteBouton2).X / 2,
                        bouton2.Top + 10),
                    Color.White);

                spriteBatch.DrawString(spriteFont, texteDescriptionBouton2, positionDescriptionBouton2, Color.Blue);
            }

            if (texteBouton3 != "")
            {
                Color c;
                if (bouton3Lock)
                    c = Color.Gray;
                else
                    c = Color.White;

                if (mouseRec.Intersects(bouton3))
                    spriteBatch.Draw(boutons, bouton3, Color.Gray);
                else
                    spriteBatch.Draw(boutons, bouton3, c);

                spriteBatch.DrawString(
                    spriteFont,
                    texteBouton3,
                    new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(texteBouton3).X / 2,
                        bouton3.Top + 10),
                    c);

                spriteBatch.DrawString(spriteFont, texteDescriptionBouton2, positionDescriptionBouton2, Color.Blue);
            }

            if (texteBouton4 != "")
            {
                if (mouseRec.Intersects(bouton4))
                    spriteBatch.Draw(boutons, bouton4, Color.Gray);
                else
                    spriteBatch.Draw(boutons, bouton4, Color.White);

                spriteBatch.DrawString(
                    spriteFont,
                    retourJeuT.get(),
                    new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(retourJeuT.get()).X / 2,
                        bouton4.Top + 10),
                    Color.White);

                spriteBatch.DrawString(spriteFont, texteDescriptionBouton2, positionDescriptionBouton2, Color.Blue);
            }

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
