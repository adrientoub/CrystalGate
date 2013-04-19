using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using AForge;
using System.IO;

namespace CrystalGate.SceneEngine2
{
    public enum GameState
    {
        MainMenu,
        Setting, 
        Gameplay,
        Pause
    }

    class SceneHandler
    {
        public static GameState gameState;
        public static SpriteBatch spriteBatch;
        public static ContentManager content;

        public static GamePlay gameplayScene;
        public static MainMenu mainmenuScene;
        public static MenuOptions menuoptionScene;
        public static PauseScene pauseScene;

        public SceneHandler()
        {
            gameState = GameState.MainMenu;
            
            gameplayScene = new GamePlay();
            mainmenuScene = new MainMenu();
            menuoptionScene = new MenuOptions();
            pauseScene = new PauseScene();
        }

        public void Initialize()
        {
            mainmenuScene.Initialize();
            menuoptionScene.Initialize();
            gameplayScene.Initialize();
            pauseScene.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            BaseScene.mouse = Mouse.GetState();
            BaseScene.keyboardState = Keyboard.GetState();
            switch (gameState)
            {
                case GameState.MainMenu:
                    mainmenuScene.Update(gameTime);
                    break;
                case GameState.Setting:
                    menuoptionScene.Update(gameTime);
                    break;
                case GameState.Gameplay:
                    gameplayScene.Update(gameTime);
                    break;
                case GameState.Pause:
                    pauseScene.Update(gameTime);
                    break;
            }
            BaseScene.oldMouse = BaseScene.mouse;
            BaseScene.oldKeyboardState = BaseScene.keyboardState;
        }

        public void Load()
        {
            mainmenuScene.LoadContent();
            menuoptionScene.LoadContent();
            pauseScene.LoadContent();
            gameplayScene.LoadContent();
        }

        public void Draw()
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    mainmenuScene.Draw(spriteBatch);
                    break;
                case GameState.Setting:
                    if (MenuOptions.isPauseOption)
                        gameplayScene.Draw(spriteBatch);
                    menuoptionScene.Draw(spriteBatch);
                    break;
                case GameState.Gameplay:
                    gameplayScene.Draw(spriteBatch);
                    break;
                case GameState.Pause:
                    gameplayScene.Draw(spriteBatch);
                    pauseScene.Draw(spriteBatch);
                    break;
            }
        }

        public static void ResetGameplay()
        {
            gameplayScene = new GamePlay();
            gameplayScene.Initialize();
            gameplayScene.LoadContent();
        }
    }
}
