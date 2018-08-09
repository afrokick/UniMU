using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : BaseState
{
    [Inject]
    public MainModel MainModel { get; private set; }
    [Inject]
    public ICoroutineExecuter CoroutineExecuter { get; private set; }
    [Inject]
    public SoundService SoundService { get; private set; }
    [Inject]
    public IStorageService StorageService { get; private set; }
    [Inject]
    public LocalizationService LocalizationService { get; private set; }

    //[Inject]
    //public GameWnd GameScreen { get; private set; }

    //[Inject]
    //public OpenThemesMenuSignal OpenThemesMenuSignal { get; private set; }

    private float _duration;

    private int _lastCellId = -1;

    public override void Load()
    {
        base.Load();

        //GameManager.CustomDataFiller = Generator.Generate;

        //_duration = Time.realtimeSinceStartup;

        //GameScreen.CellClicked += OnCellClicked;
        //GameScreen.CellUpped += OnCellUpped;
        //GameScreen.MenuClicked += OnMenuClicked;
        //GameScreen.SettingsClicked += OnSettingsClicked;
        //GameScreen.TipClicked += OnTipClicked;

        //CoinsBarView.AddCoinsClicked += OnAddCoinsClicked;

        //GameManager.LifeEnded += OnLifeEnded;
        //GameManager.WordFinished += OnWordFinished;

        //Tip1ChangedSignal.AddListener(OnTip1CountChanged);
        //CoinsChangedSignal.AddListener(OnCoinsCountChanged);

        //if (!MainModel.IsDailyGame)
        //{
        //    StartLevel();
        //}
        //else
        //{
        //    StartDaily();
        //}

        //if (!GameManager.TryLoad() || GameManager.IsWin)
        //{
        //    GameManager.Restart();

        //    if (!MainModel.IsDailyGame)
        //        this.TrackWordCreated(MainModel.DictData.Title, MainModel.CurrentLevel);
        //}
        //else if (GameManager.Lifes == 0)
        //{
        //    OpenLoseSignal.Dispatch();
        //}

        //if (!MainModel.IsDailyGame)
        //    this.TrackLevelOpened(MainModel.DictData.Title, MainModel.CurrentLevel);

        //OnTip1CountChanged(MainModel.Tip1Count);
        //OnCoinsCountChanged(MainModel.Coins);

        //SoundService.Stop(SoundService.Sounds.MenuTheme);

        //if (!SoundService.IsPlaying(SoundService.Sounds.GameTheme))
        //{
        //    SoundService.Play(SoundService.Sounds.GameTheme, true);
        //}

        //if (!MainModel.TutorialShowed && GameManager.Lifes > 0)
        //{
        //    OpenTutorialSignal.Dispatch();
        //}

        //GameScreen.Show();
        //CoinsBarView.Show();
    }

    public override void Unload()
    {
        base.Unload();

        //GameScreen.CellClicked -= OnCellClicked;
        //GameScreen.CellUpped -= OnCellUpped;
        //GameScreen.MenuClicked -= OnMenuClicked;
        //GameScreen.SettingsClicked -= OnSettingsClicked;
        //GameScreen.TipClicked -= OnTipClicked;

        //GameManager.LifeEnded -= OnLifeEnded;
        //GameManager.WordFinished -= OnWordFinished;

        //CoinsBarView.AddCoinsClicked -= OnAddCoinsClicked;

        //Tip1ChangedSignal.RemoveListener(OnTip1CountChanged);
        //CoinsChangedSignal.RemoveListener(OnCoinsCountChanged);

        //GameScreen.Hide();
        //CoinsBarView.Hide();

        //SoundService.Stop(SoundService.Sounds.GameTheme);

        //if (!MainModel.IsDailyGame)
        //this.TrackLevelDuration(MainModel.DictData.Title, MainModel.CurrentLevel, Time.realtimeSinceStartup - _duration);
    }

    //private void StartLevel()
    //{
    //    int phase;
    //    var finished = MainModel.GetFinish(MainModel.GetLvlId);
    //    if (finished)
    //    {
    //        phase = MainModel.PhasesCount - 1;
    //    }
    //    else
    //    {
    //        phase = MainModel.CurrentProgress;
    //        phase = Mathf.Min(phase, MainModel.PhasesCount - 1);
    //    }
    //    GameManager.SetData(GameScreen,
    //                        MainModel.Data.Phases[phase].Width,
    //                        MainModel.Data.Phases[phase].Height,
    //                        MainModel.Data.Phases[phase].WordLength,
    //                        MainModel.Data.Phases[phase].Mode);

    //    GameScreen.SetTitle(MainModel.DictData.Title);

    //    GameScreen.SetRemainWords(MainModel.PhasesCount, Mathf.Max(0, MainModel.PhasesCount - MainModel.CurrentProgress));

    //    if (!MainModel.GetLevelOpened(MainModel.GetLvlId))
    //    {
    //        MainModel.SetLevelOpened(MainModel.GetLvlId);

    //        this.TrackLevelOpened(MainModel.DictData.Title, MainModel.CurrentLevel);
    //    }
    //}

    //private void StartDaily()
    //{
    //    int phase;
    //    var finished = MainModel.DailyProgress > 4;
    //    if (finished)
    //    {
    //        phase = 4;
    //    }
    //    else
    //    {
    //        phase = MainModel.DailyProgress;
    //    }

    //    string word = MainModel.DailyDict.AllWords[MainModel.DailyProgress];
    //    if (word.Length <= 4)
    //    {
    //        GameManager.SetData(GameScreen, 4, 4, word.Length, LevelDifficulty.Easy);
    //    }
    //    else if (word.Length == 5)
    //    {
    //        GameManager.SetData(GameScreen, 5, UnityEngine.Random.Range(4, 5), word.Length, LevelDifficulty.Easy);
    //    }
    //    else
    //    {
    //        GameManager.SetData(GameScreen, 6, UnityEngine.Random.Range(5, 6), word.Length, LevelDifficulty.Easy);
    //    }

    //    GameScreen.SetTitle(MainModel.DailyDict.Title);

    //    GameScreen.SetRemainWords(5, Mathf.Max(0, 5 - phase));
    //}

    //protected override void OnHardwareBackPress()
    //{
    //    if (StateMachine.LastState == this)
    //    {
    //        OnMenuClicked();
    //    }
    //}

    //private void OnTip1CountChanged(int count)
    //{
    //    var text = string.Empty;

    //    if (count > 99)
    //        text = "99+";
    //    else if (count == 0)
    //        text = "+";
    //    else
    //        text = count + "";

    //    GameScreen.SetTipCount(text);
    //}

    //private void OnCoinsCountChanged(int count)
    //{
    //    CoinsBarView.SetCoins(count.ToString());
    //}

    //private void OnAddCoinsClicked()
    //{
    //    this.TrackCoinsClicked();

    //    OpenBuyCoinsSignal.Dispatch();
    //}

    //private void OnCellClicked(Cell cell)
    //{
    //    Debug.Log("ONCell clicked " + cell.Id);

    //    if (_lastCellId >= 0)
    //    {
    //        var id = cell.Id;

    //        if (id == _lastCellId + 1 ||
    //           id == _lastCellId - 1 ||
    //           id == _lastCellId + GameManager.Width ||
    //           id == _lastCellId - GameManager.Width)
    //        {

    //        }
    //        else
    //        {
    //            Debug.Log("cell is invalid");
    //            return;
    //        }
    //    }

    //    if (GameManager.SelectedCharacters.Count == GameManager.Word.Length) return;

    //    var status = cell.Finished;

    //    if (status) return;

    //    _lastCellId = cell.Id;

    //    GameManager.OnCellClick(cell);

    //    SoundService.Play(SoundService.Sounds.Click);
    //}

    //private void OnCellUpped(Cell cell)
    //{
    //    if (GameManager.IsWin)
    //    {
    //        OnWordFinished();
    //    }
    //    else
    //    {
    //        if (GameManager.SelectedCharacters.Count == GameManager.Word.Length)
    //            SoundService.Play(SoundService.Sounds.ClickError);

    //        GameManager.ClearCells();

    //        GameManager.SelectedCharacters.Clear();
    //    }

    //    _lastCellId = -1;
    //}

    //private void OnLifeEnded()
    //{
    //    UnityEngine.EventSystems.EventSystem.current.currentInputModule.DeactivateModule();

    //    OpenLoseSignal.Dispatch();

    //    if (!MainModel.IsDailyGame)
    //        this.TrackLifeEnded(MainModel.DictData.Title, MainModel.CurrentLevel, Time.realtimeSinceStartup - _duration);

    //    SoundService.Play(SoundService.Sounds.WordLose);
    //}

    //private void OnWordFinished()
    //{
    //    //_blockedClicks = true;

    //    UnityEngine.EventSystems.EventSystem.current.currentInputModule.DeactivateModule();

    //    CoroutineExecuter.Execute(WaitAndShow(0.5f));
    //}

    //private IEnumerator WaitAndShow(float waitingTime)
    //{
    //    var oldProgress = MainModel.CurrentProgress;

    //    var newProgress = Mathf.Min(oldProgress + 1, MainModel.PhasesCount);

    //    var finished = MainModel.IsDailyGame ? newProgress >= 5 : MainModel.GetFinish(MainModel.GetLvlId);

    //    MainModel.SetLevelProgress(MainModel.GetLvlId, newProgress);

    //    if (!MainModel.IsDailyGame && MainModel.CurrentLevel == 2 && MainModel.LearnProgress < 5)
    //    {
    //        MainModel.LearnProgress++;
    //    }

    //    if (!MainModel.IsDailyGame && newProgress >= MainModel.PhasesCount && !finished)
    //    {
    //        MainModel.SetFinish(MainModel.GetLvlId);

    //        this.TrackLevelDone(MainModel.DictData.Title, MainModel.CurrentLevel, Time.realtimeSinceStartup - _duration);
    //        this.TrackLevelUnlocked(MainModel.DictData.Title, MainModel.CurrentLevel + 1);
    //    }

    //    if (!MainModel.IsDailyGame)
    //        this.TrackWordFinished(MainModel.DictData.Title, MainModel.CurrentLevel, Time.realtimeSinceStartup - _duration);

    //    StorageService.Save();

    //    yield return new WaitForSeconds(waitingTime);

    //    //last word - show new level opened popup!
    //    if (oldProgress < newProgress && newProgress == MainModel.PhasesCount)
    //    {
    //        GameScreen.SetRemainWords(MainModel.PhasesCount, Mathf.Max(0, MainModel.PhasesCount - MainModel.CurrentProgress));

    //        SoundService.Play(SoundService.Sounds.LevelFinished);

    //        if (!MainModel.IsDailyGame)
    //        {
    //            this.TrackLevelFinished(MainModel.DictData.Title, MainModel.CurrentLevel, Time.realtimeSinceStartup - _duration);

    //            var pack = LevelService.GetDictionary(LocalizationService.LangId,
    //                MainModel.SelectedDictionaryId);

    //            if (MainModel.SelectedLevelId == (pack.Levels.Count - 1) && !finished)
    //            {
    //                this.TrackFinishedThemes(GetFinishedThemes());

    //                this.TrackFinishedTheme(MainModel.DictData.Title);

    //                MainModel.SetLastCompletedTheme(LocalizationService.LangId, MainModel.DictData.Id);

    //                OpenThemesMenuSignal.Dispatch();

    //                yield break;
    //            }
    //            else
    //            {
    //                OpenWinSignal.Dispatch();

    //                yield return new WaitForSeconds(1.0f);

    //                GameScreen.SetRemainWords(MainModel.PhasesCount, Mathf.Max(0, MainModel.PhasesCount - MainModel.CurrentProgress));

    //                yield return new WaitForSeconds(0.5f);

    //                GameManager.Restart();

    //                if (!MainModel.IsDailyGame)
    //                    this.TrackWordCreated(MainModel.DictData.Title, MainModel.CurrentLevel);
    //            }
    //        }
    //        else
    //        {
    //            OpenDailyFinishSignal.Dispatch();

    //            yield break;
    //        }
    //    }
    //    else
    //    {
    //        int phase = newProgress;

    //        phase = Mathf.Min(phase, MainModel.PhasesCount - 1);

    //        if (MainModel.IsDailyGame)
    //        {
    //            MainModel.DailyProgress = newProgress;
    //            string word = MainModel.DailyDict.AllWords[MainModel.DailyProgress];
    //            if (word.Length <= 4)
    //            {
    //                GameManager.SetData(GameScreen, 4, 4, word.Length, LevelDifficulty.Easy);
    //            }
    //            else if (word.Length == 5)
    //            {
    //                GameManager.SetData(GameScreen, 5, UnityEngine.Random.Range(4, 5), word.Length, LevelDifficulty.Easy);
    //            }
    //            else
    //            {
    //                GameManager.SetData(GameScreen, 6, UnityEngine.Random.Range(5, 6), word.Length, LevelDifficulty.Easy);
    //            }
    //        }
    //        else
    //        {
    //            GameManager.SetData(GameScreen,
    //                                MainModel.Data.Phases[phase].Width,
    //                                MainModel.Data.Phases[phase].Height,
    //                                MainModel.Data.Phases[phase].WordLength,
    //                                MainModel.Data.Phases[phase].Mode);
    //        }

    //        OpenWordFinishedSignal.Dispatch();

    //        yield break;
    //    }
    //}

    //private string[] GetFinishedThemes()
    //{
    //    var list = new List<string>(32);

    //    var pack = LevelService.GetPack(LocalizationService.LangId);

    //    foreach (var pair in pack.Dictionaries)
    //    {
    //        var dictId = pair.Key;
    //        var dict = WordsDictionary.GetDictionary(LocalizationService.LangId, dictId);

    //        var levelPack = LevelService.GetDictionary(LocalizationService.LangId, MainModel.SelectedDictionaryId);

    //        var allLevelFinished = true;

    //        for (int i = 0; i < levelPack.Levels.Count; i++)
    //        {
    //            var lvlId = LocalizationService.LangId + dictId + i;

    //            allLevelFinished &= MainModel.GetFinish(lvlId);
    //        }

    //        if (allLevelFinished)
    //        {
    //            list.Add(dict.Title);
    //        }
    //    }

    //    return list.ToArray();
    //}

    //private void OnMenuClicked()
    //{
    //    if (!MainModel.IsDailyGame)
    //        this.TrackLevelClose(MainModel.DictData.Title, MainModel.CurrentLevel, Time.realtimeSinceStartup - _duration);

    //    OpenThemesMenuSignal.Dispatch();
    //}

    //private void OnSettingsClicked()
    //{
    //    OpenSettingsSignal.Dispatch();
    //}

    //private void OnTipClicked()
    //{
    //    this.TrackTipClicked();

    //    UseTipSignal.Dispatch();
    //}
}