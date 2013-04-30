using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace CrystalGateEditor.SceneEngine2
{
    public enum GameState
    {
        MainMenu,
        Editor,
        EditorSettings,
        Setting,
        Pause,
    }

    public class SceneHandler
    {
        public static GameState gameState;
        public static SpriteBatch spriteBatch;
        public static ContentManager content;

        public static Editor editorScene;
        public static MainMenu mainMenuScene;
        public static EditorSettings editorSettingsScene;
        public static MenuOptions menuOptionsScene;

        public SceneHandler()
        {
            gameState = GameState.MainMenu;

            editorScene = new Editor();
            mainMenuScene = new MainMenu();
            menuOptionsScene = new MenuOptions();
            editorSettingsScene = new EditorSettings();
        }

        public void Initialize()
        {
            mainMenuScene.Initialize();
            menuOptionsScene.Initialize();
            editorScene.Initialize();
            editorSettingsScene.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            BaseScene.mouse = Mouse.GetState();
            BaseScene.keyboardState = Keyboard.GetState();
            switch (gameState)
            {
                case GameState.Editor:
                    editorScene.Update(gameTime);
                    break;
                case GameState.MainMenu:
                    mainMenuScene.Update(gameTime);
                    break;
                case GameState.Setting:
                    menuOptionsScene.Update(gameTime);
                    break;
                case GameState.EditorSettings:
                    editorSettingsScene.Update(gameTime);
                    break;
            }
            BaseScene.oldMouse = BaseScene.mouse;
            BaseScene.oldKeyboardState = BaseScene.keyboardState;
        }

        public void Load()
        {
            mainMenuScene.LoadContent();
            menuOptionsScene.LoadContent();
            editorScene.LoadContent();
            editorSettingsScene.LoadContent();
        }

        public void Draw()
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    mainMenuScene.Draw(spriteBatch);
                    break;
                case GameState.Setting:
                    menuOptionsScene.Draw(spriteBatch);
                    break;
                case GameState.Editor:
                    editorScene.Draw(spriteBatch);
                    break;
                case GameState.EditorSettings:
                    editorSettingsScene.Draw(spriteBatch);
                    break;
            }
        }

        public void ReinitilizeEditor()
        {
            editorSettingsScene.Initialize();
        }
    }
}
