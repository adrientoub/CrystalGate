﻿using System;
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
        Defeat,
        Credits,
        ChampionSelection
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
        public static Credits creditsScene;
        public static ChampionSelection championSelectionScene;

        public static string level = "level1";

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
            creditsScene = new Credits();
            championSelectionScene = new ChampionSelection();
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
            creditsScene.Initialize();
            championSelectionScene.Initialize();
        }

        public static void UpdatePositions()
        {
            mainmenuScene.UpdatePositions();
            menuoptionScene.UpdatePositions();
            pauseScene.UpdatePositions();
            defeatScene.UpdatePositions();
            victoryScene.UpdatePositions();
            creditsScene.UpdatePositions();
            coopConnexionScene.UpdatePositions();
            coopSettingsScene.UpdatePositions();
            championSelectionScene.UpdatePositions();
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
                case GameState.Credits:
                    creditsScene.Update(gameTime);
                    break;
                case GameState.ChampionSelection:
                    championSelectionScene.Update(gameTime);
                    break;
            }
            BaseScene.oldMouse = BaseScene.mouse;
            BaseScene.oldKeyboardState = BaseScene.keyboardState;
        }

        public void Load()
        {
            PackMap.Initialize();
            splashScreenScene.LoadContent();
            mainmenuScene.LoadContent();
            menuoptionScene.LoadContent();
            coopSettingsScene.LoadContent();
            coopConnexionScene.LoadContent();
            pauseScene.LoadContent();
            gameplayScene.LoadContent();
            victoryScene.LoadContent();
            defeatScene.LoadContent();
            creditsScene.LoadContent();
            championSelectionScene.LoadContent();
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
                case GameState.Credits:
                    creditsScene.Draw(spriteBatch);
                    break;
                case GameState.ChampionSelection:
                    championSelectionScene.Draw(spriteBatch);
                    break;
            }
        }

        public static void ResetGameplay()
        {
            // Reinitialise les levels ET le joueur (il faudra ajouter un truc qui charge les infos du joueur a partir d'un fichier texte
            level = "level1";
            PackMap.LoadPlayers();
            PackMap.InitLevels();
            gameplayScene = new GamePlay();
            gameplayScene.Initialize();
            gameplayScene.LoadContent();
            defeatScene.firstTime = true;
            victoryScene.firstTime = true;
        }

        public static void ResetGameplay(string map) // Si on change de map
        {
            // Oldmap
            PackMap.Sauvegarder();
            level = map;
            // Nouvelle map
            gameplayScene = new GamePlay();
            gameplayScene.Initialize();
            gameplayScene.LoadContent();
            defeatScene.firstTime = true;
            victoryScene.firstTime = true;
        }
    }
}
