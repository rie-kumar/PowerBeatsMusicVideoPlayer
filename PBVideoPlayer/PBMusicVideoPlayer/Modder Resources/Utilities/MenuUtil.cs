﻿using powerbeatsvr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PBMusicVideoPlayer
{
    public class MenuUtil : Singleton<MenuUtil>
    {
        public readonly string MenuSceneName = "Menu";
        public readonly string FrameworkSceneName = "Framework";
        public readonly string MedievelEnvironmentSceneName = "Medieval Environment";
        public readonly string SETTINGS_SONG_EXTERNAL_NAME = "song_external_name";
        public readonly string SETTINGS_SONG_FILE_DIR = "song_file_dir";

        public Controller LeftController;
        public Controller RightController;

        public event OnSceneLoaded EnvironmentSceneLoadedEvent;
        public event OnSceneLoaded FrameworkSceneLoadedEvent;
        public event OnSceneLoaded MenuSceneLoadedEvent;
        public event OnMenuReady MenuReadyEvent;
        public event OnPauseMenuReady PauseMenuReadyEvent;
        public event OnSongSelected SongSelectedEvent;
        public event OnGamePaused GamePausedEvent;
        public event MenuSelectButton.OnClick StartGameEvent;
        public event MenuSelectButton.OnClick GContinueEvent;
        public event MenuSelectButton.OnClick GToMainMenu;
        public event OnNavigationHappened NavigationHappenedEvent;

        public delegate void OnSongSelected(int index, string songPath, string songName);
        public delegate void OnMenuReady(Menu menu);
        public delegate void OnPauseMenuReady(PauseMenu menu);
        public delegate void OnSceneLoaded(Scene scene);
        public delegate void OnNavigationHappened(string dir);
        public delegate void OnGamePaused(bool isPaused);

        private Menu menu;
        private PauseMenu pauseMenu;
        private FileListManager manager;
        private bool inGame = false;
        private bool isPaused = false;

        public void OnLoad()
        {
            StartCoroutine(FindMenu());

            SceneManager.sceneLoaded += SceneLoaded;
        }

        public bool TryGetSelectedSongIndexCustom(out int selectedSongIndexCustom)
        {
            selectedSongIndexCustom = -1;

            if (manager != null && PlayerPrefs.HasKey(SETTINGS_SONG_EXTERNAL_NAME))
            {
                selectedSongIndexCustom = manager.GetIndexOfEntry(PlayerPrefs.GetString(SETTINGS_SONG_EXTERNAL_NAME));
            }

            return selectedSongIndexCustom != -1;
        }

        public bool TryGetSelectedSongNameCustom(out string selectedSongNameCustom)
        {
            selectedSongNameCustom = string.Empty;

            if (manager != null && PlayerPrefs.HasKey(SETTINGS_SONG_EXTERNAL_NAME))
            {
                selectedSongNameCustom = PlayerPrefs.GetString(SETTINGS_SONG_EXTERNAL_NAME);
            }

            return string.IsNullOrEmpty(selectedSongNameCustom);
        }

        public string GetSongFileDir()
        {
            string songFileDir = string.Empty;

            if (manager != null && PlayerPrefs.HasKey(SETTINGS_SONG_FILE_DIR))
            {
                songFileDir = PlayerPrefs.GetString(SETTINGS_SONG_FILE_DIR);
            }

            return songFileDir;
        }

        private void Start()
        {
            foreach (Controller controller in UnityEngine.Object.FindObjectsOfType<Controller>())
            {
                if (controller.handSide.Equals(LLManager.HandSide.LEFT))
                {
                    LeftController = controller;
                }
                if (controller.handSide.Equals(LLManager.HandSide.RIGHT))
                {
                    RightController = controller;
                }
            }

            LeftController.systemButtonPressed += SystemButtonPressed;
            LeftController.appButtonChanged += AppMenuButtonChanged;
        }

        private void AppMenuButtonChanged(bool isPressed)
        {
            if(inGame && isPressed)
            {
                if (!isPaused)
                {
                    FindPauseMenu();
                }
                else
                {
                    ExitPauseMenu();
                }
            }
        }

        private void SystemButtonPressed(bool pausePressed)
        {
            if(inGame && pausePressed)
            {
                FindPauseMenu();
            }
        }

        private void SceneLoaded(Scene loaded, LoadSceneMode loadMode)
        {
            Logger.Instance.Log($"Scene Loaded: {loaded.name}", Logger.LogSeverity.DEBUG);

            if (loaded.name == MenuSceneName)
            {
                inGame = false;
                MenuSceneLoadedEvent?.Invoke(loaded);

                StartCoroutine(FindMenu());
            }
            else if (loaded.name == FrameworkSceneName)
            {
                FrameworkSceneLoadedEvent?.Invoke(loaded);
            }
            else
            {
                inGame = true;
                EnvironmentSceneLoadedEvent?.Invoke(loaded);
            }
        }

        private void SelectionButtonSelect(MenuSelectButton button)
        {
            MenuSelectButton.Action action = button.GetAction();

            Logger.Instance.Log(action.ToString(), Logger.LogSeverity.DEBUG);

            switch (action)
            {
                case MenuSelectButton.Action.StartGame:
                    StartGameEvent?.Invoke(button);
                    ExitMenu();
                    break;
                case MenuSelectButton.Action.Exit:
                    break;
                case MenuSelectButton.Action.Settings:
                    break;
                case MenuSelectButton.Action.Scores:
                    break;
                case MenuSelectButton.Action.Editor:
                    break;
                case MenuSelectButton.Action.NextSongs:
                    break;
                case MenuSelectButton.Action.PreviousSongs:
                    break;
                case MenuSelectButton.Action.WelcomeClose:
                    break;
                case MenuSelectButton.Action.JoinCommunity:
                    break;
                case MenuSelectButton.Action.NeedHelp:
                    break;
                case MenuSelectButton.Action.ReportBug:
                    break;
                case MenuSelectButton.Action.Credits:
                    break;
                case MenuSelectButton.Action.DecreaseQuality:
                    break;
                case MenuSelectButton.Action.IncreaseQuality:
                    break;
                case MenuSelectButton.Action.ScrollToTop:
                    break;
                case MenuSelectButton.Action.ScrollToPlayer:
                    break;
                case MenuSelectButton.Action.EMenuPosLeft:
                    break;
                case MenuSelectButton.Action.EMenuPosRight:
                    break;
                case MenuSelectButton.Action.EBeatStepLeft:
                    break;
                case MenuSelectButton.Action.EBeatStepRight:
                    break;
                case MenuSelectButton.Action.EUndo:
                    break;
                case MenuSelectButton.Action.ERedo:
                    break;
                case MenuSelectButton.Action.ERevert:
                    break;
                case MenuSelectButton.Action.ESave:
                    break;
                case MenuSelectButton.Action.EExit:
                    break;
                case MenuSelectButton.Action.EBallNormal:
                    break;
                case MenuSelectButton.Action.EBallPower:
                    break;
                case MenuSelectButton.Action.EBallObstacle:
                    break;
                case MenuSelectButton.Action.EWall1:
                    break;
                case MenuSelectButton.Action.EWall2:
                    break;
                case MenuSelectButton.Action.EWall3:
                    break;
                case MenuSelectButton.Action.EWall4:
                    break;
                case MenuSelectButton.Action.EWall5:
                    break;
                case MenuSelectButton.Action.EWall6:
                    break;
                case MenuSelectButton.Action.EWall7:
                    break;
                case MenuSelectButton.Action.EWall8:
                    break;
                case MenuSelectButton.Action.EStreamAStartMid:
                    break;
                case MenuSelectButton.Action.EStreamAEnd:
                    break;
                case MenuSelectButton.Action.EStreamBStartMid:
                    break;
                case MenuSelectButton.Action.EStreamBEnd:
                    break;
                case MenuSelectButton.Action.EOffsetLeft:
                    break;
                case MenuSelectButton.Action.EOffsetRight:
                    break;
                case MenuSelectButton.Action.ECopySetStart:
                    break;
                case MenuSelectButton.Action.ECopySetEnd:
                    break;
                case MenuSelectButton.Action.ECopyMerge:
                    break;
                case MenuSelectButton.Action.ECopyOverwrite:
                    break;
                case MenuSelectButton.Action.ERippleInsert:
                    break;
                case MenuSelectButton.Action.ERippleDelete:
                    break;
                case MenuSelectButton.Action.ESmoothA:
                    break;
                case MenuSelectButton.Action.ESmoothB:
                    break;
                case MenuSelectButton.Action.EExitConfirm:
                    break;
                case MenuSelectButton.Action.ERevertConfirm:
                    break;
                case MenuSelectButton.Action.ECancelConfirmDialog:
                    break;
                case MenuSelectButton.Action.GenderLeft:
                    break;
                case MenuSelectButton.Action.GenderRight:
                    break;
                case MenuSelectButton.Action.DecreaseAge:
                    break;
                case MenuSelectButton.Action.IncreaseAge:
                    break;
                case MenuSelectButton.Action.DecreaseWeight:
                    break;
                case MenuSelectButton.Action.IncreaseWeight:
                    break;
                case MenuSelectButton.Action.ResetCalories:
                    break;
                case MenuSelectButton.Action.HeartrateMonitorSearch:
                    break;
                case MenuSelectButton.Action.HeartrateMonitorConnect:
                    break;
                case MenuSelectButton.Action.HeartrateMonitorToggleLeft:
                    break;
                case MenuSelectButton.Action.HeartrateMonitorToggleRight:
                    break;
                case MenuSelectButton.Action.GuideToCustomSongs:
                    break;
                case MenuSelectButton.Action.HeartRateMonitorInfo:
                    break;
                case MenuSelectButton.Action.HitSoundLeft:
                    break;
                case MenuSelectButton.Action.HitSoundRight:
                    break;
                case MenuSelectButton.Action.HeartRateMonitorDisconnect:
                    break;
                case MenuSelectButton.Action.ECopyToClipboardRequest:
                    break;
                case MenuSelectButton.Action.ECopyToPatternRequest:
                    break;
                case MenuSelectButton.Action.EPasteFromPatternRequested:
                    break;
                case MenuSelectButton.Action.EPasteFromClipboardRequest:
                    break;
                case MenuSelectButton.Action.ECloseSingleButtonDialog:
                    break;
                case MenuSelectButton.Action.ECopyToPatternConfirm:
                    break;
                case MenuSelectButton.Action.EPasteFromPatternConfirm:
                    break;
                case MenuSelectButton.Action.EKeyboardKey:
                    break;
                case MenuSelectButton.Action.ECancelPatternDialog:
                    break;
                case MenuSelectButton.Action.ECopyToPatternConfirmConfirm:
                    break;
                case MenuSelectButton.Action.EDeletePattern:
                    break;
                case MenuSelectButton.Action.EDeletePatternConfirm:
                    break;
                case MenuSelectButton.Action.DebugSelectExternal:
                    break;
                case MenuSelectButton.Action.EDetectBPMAndOffset:
                    break;
                case MenuSelectButton.Action.EBPMLeft:
                    break;
                case MenuSelectButton.Action.EBPMRight:
                    break;
                case MenuSelectButton.Action.EInvertOffset:
                    break;
                case MenuSelectButton.Action.EGenerate:
                    break;
                case MenuSelectButton.Action.EConfirmGenerate:
                    break;
                case MenuSelectButton.Action.GContinue:
                    GContinueEvent?.Invoke(button);
                    ExitPauseMenu();
                    break;
                case MenuSelectButton.Action.GToMainMenu:
                    GToMainMenu?.Invoke(button);
                    ExitPauseMenu();
                    break;
                case MenuSelectButton.Action.GBPMHalf:
                    break;
                case MenuSelectButton.Action.GBPMDec:
                    break;
                case MenuSelectButton.Action.GBPMInc:
                    break;
                case MenuSelectButton.Action.GBPMDouble:
                    break;
                case MenuSelectButton.Action.GOffsetDec:
                    break;
                case MenuSelectButton.Action.GOffsetInc:
                    break;
                case MenuSelectButton.Action.GOffsetToggle:
                    break;
                case MenuSelectButton.Action.GReset:
                    break;
                case MenuSelectButton.Action.GPreview:
                    break;
                case MenuSelectButton.Action.GCancel:
                    break;
                case MenuSelectButton.Action.GPlay:
                    break;
                case MenuSelectButton.Action.GInfoDialogOK:
                    break;
                case MenuSelectButton.Action.GSettingsBallsDown:
                    break;
                case MenuSelectButton.Action.GSettingsBallsUp:
                    break;
                case MenuSelectButton.Action.GSettingsWallsDown:
                    break;
                case MenuSelectButton.Action.GSettingsWallsUp:
                    break;
                case MenuSelectButton.Action.GSettingsStreamsDown:
                    break;
                case MenuSelectButton.Action.GSettingsStreamsUp:
                    break;
                case MenuSelectButton.Action.GSettingsSwingsDown:
                    break;
                case MenuSelectButton.Action.GSettingsSwingsUp:
                    break;
                case MenuSelectButton.Action.GSettingsJumpsDown:
                    break;
                case MenuSelectButton.Action.GSettingsJumpsUp:
                    break;
                case MenuSelectButton.Action.GSettingsSquatsDown:
                    break;
                case MenuSelectButton.Action.GSettingsSquatsUp:
                    break;
                case MenuSelectButton.Action.TakeScreenShot:
                    break;
                case MenuSelectButton.Action.SongAdjust:
                    break;
                case MenuSelectButton.Action.GSettingsReset:
                    break;
                default:
                    break;
            }
        }

        private void ExitMenu()
        {
            foreach (var selectButton in menu.GetComponentsInChildren<MenuSelectButton>())
            {
                selectButton.onClick -= SelectionButtonSelect;
            }

            if (manager != null)
            {
                manager.SongClicked -= SongSelectected;
            }
        }

        private void ExitPauseMenu()
        {
            isPaused = false;

            foreach (var selectButton in pauseMenu.GetComponentsInChildren<MenuSelectButton>())
            {
                selectButton.onClick -= SelectionButtonSelect;
            }

            if(inGame)
            {
                GamePausedEvent?.Invoke(isPaused);
            }
        }

        private IEnumerator FindMenu()
        {
            Logger.Instance.Log("Finding Menu", Logger.LogSeverity.DEBUG);

            while (menu == null)
            {
                menu = FindObjectOfType<Menu>();
                yield return null;
            }

            foreach (var button in menu.GetComponentsInChildren<MenuSelectButton>())
            { 
                button.onClick += SelectionButtonSelect;
            }

            MenuReadyEvent?.Invoke(menu);

            Logger.Instance.Log(menu.transform.position.ToString(), Logger.LogSeverity.DEBUG);
            StartCoroutine(FindManager());
        }

        private IEnumerator FindPauseMenu()
        {
            Logger.Instance.Log("Finding Pause Menu", Logger.LogSeverity.DEBUG);

            while (pauseMenu == null)
            {
                pauseMenu = FindObjectOfType<PauseMenu>();
                yield return null;
            }

            foreach (var button in menu.GetComponentsInChildren<MenuSelectButton>())
            {
                button.onClick += SelectionButtonSelect;
            }

            isPaused = true;
            PauseMenuReadyEvent?.Invoke(pauseMenu);
            GamePausedEvent?.Invoke(isPaused);
        }

        private IEnumerator FindManager()
        {
            while (manager == null)
            {
                manager = FindObjectOfType<FileListManager>();
                yield return null;
            }

            manager.SongClicked += SongSelectected;
            manager.NavigationHappened += NavigationHappened;
        }

        private void NavigationHappened(string dir)
        {
            NavigationHappenedEvent?.Invoke(dir);
        }

        private void SongSelectected(int songIndex, string songFullPath)
        {
            SongSelectedEvent?.Invoke(songIndex, songFullPath, Path.GetFileNameWithoutExtension(songFullPath));
        }
    }
}
