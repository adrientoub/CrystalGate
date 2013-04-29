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
        SplashScreen,
        MainMenu,
        CoopSettings,
        CoopConnexion,
        Setting,
        Gameplay,
        Pause,
        Victory,
        Defeat
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
        public static VictoryScene victoryScene;
        public static DefeatScene defeatScene;
        public static CoopSettingsScene coopSettingsScene;
        public static CoopConnexionScene coopConnexionScene;
        public static SplashScreenScene splashScreenScene;

        public SceneHandler()
        {
            gameState = GameState.SplashScreen;

            gameplayScene = new GamePlay();
            mainmenuScene = new MainMenu();
            menuoptionScene = new MenuOptions();
            pauseScene = new PauseScene();
            victoryScene = new VictoryScene();
            defeatScene = new DefeatScene();
            coopSettingsScene = new CoopSettingsScene();
            coopConnexionScene = new CoopConnexionScene();
            splashScreenScene = new SplashScreenScene();
        }

        public void Initialize()
        {
            mainmenuScene.Initialize();
            menuoptionScene.Initialize();
            gameplayScene.Initialize();
            pauseScene.Initialize();
            victoryScene.Initialize();
            defeatScene.Initialize();
            coopSettingsScene.Initialize();
            coopConnexionScene.Initialize();
            splashScreenScene.Initialize();
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
                case GameState.CoopSettings:
                    coopSettingsScene.Update(gameTime);
                    break;
                case GameState.CoopConnexion:
                    coopConnexionScene.Update(gameTime);
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
                case GameState.Victory:
                    victoryScene.Update(gameTime);
                    break;
                case GameState.Defeat:
                    defeatScene.Update(gameTime);
                    break;
                case GameState.SplashScreen:
                    splashScreenScene.Update(gameTime);
                    break;
            }
            BaseScene.oldMouse = BaseScene.mouse;
            BaseScene.oldKeyboardState = BaseScene.keyboardState;
        }

        public void Load()
        {
            mainmenuScene.LoadContent();
            menuoptionScene.LoadContent();
            coopSettingsScene.LoadContent();
            coopConnexionScene.LoadContent();
            pauseScene.LoadContent();
            gameplayScene.LoadContent();
            victoryScene.LoadContent();
            defeatScene.LoadContent();
            splashScreenScene.LoadContent();
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
                case GameState.CoopSettings:
                    coopSettingsScene.Draw(spriteBatch);
                    break;
                case GameState.CoopConnexion:
                    coopConnexionScene.Draw(spriteBatch);
                    break;
                case GameState.Gameplay:
                    gameplayScene.Draw(spriteBatch);
                    break;
                case GameState.Pause:
                    gameplayScene.Draw(spriteBatch);
                    pauseScene.Draw(spriteBatch);
                    break;
                case GameState.Victory:
                    gameplayScene.Draw(spriteBatch);
                    victoryScene.Draw(spriteBatch);
                    break;
                case GameState.Defeat:
                    gameplayScene.Draw(spriteBatch);
                    defeatScene.Draw(spriteBatch);
                    break;
                case GameState.SplashScreen:
                    splashScreenScene.Draw(spriteBatch);
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
