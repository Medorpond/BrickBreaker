using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : BaseManager<StageManager>
{
    GameManager gm;

    [Header("Sounds")]
    [SerializeField] AudioClip pauseSound;
    [SerializeField] AudioClip resumeSound;
    [SerializeField] AudioClip gameOverSfx;
    [SerializeField] AudioClip gameClearSfx;
    [SerializeField] AudioClip gameClearBgm;

    [Header("Player Config")]
    [SerializeField, Min(0)] private int life = 3;

    [Header("Combo Config")]
    [SerializeField] private float comboTimeWindow;
    private WaitForSeconds comboWindow;

    [SerializeField] private float maxComboMultiplier = 3.0f;
    public int ComboScore { get; private set; } = 0;
    public int ComboCount { get; private set; } = 0;
    private Coroutine ComboWindow = null;

    [HideInInspector] public int totalScore;
    [HideInInspector] public int maxCombo = 0;

    public event Action<int> OnScoreChange;
    public event Action<int> OnBallLost;
    public event Action<int, int> OnCombo;

    private bool isGamePaused = false;
    public event Action OnPause;
    public event Action OnResume;
    public event Action<bool, int, int, int, bool> OnGameOver;

    #region LifeCycle
    protected override void Awake()
    {
        base.Awake();
        gm = GameManager.Instance;
        comboWindow = new WaitForSeconds(comboTimeWindow);
    }

    private void Start()
    {
        int currentLv = gm.CurrentLevel;
        LevelData levelData = gm.LevelDB.LevelDatas[currentLv];
        SoundManager.Instance.ChangeBgmAndPlay(levelData.LevelBgm);
        Instantiate(levelData.LevelPrefab, Vector3.zero, Quaternion.identity);

        OnScoreChange?.Invoke(0);
        OnCombo?.Invoke(0, 0);
        OnBallLost?.Invoke(life);
    }

    private void Update()
    {
        GetUserPauseInput();
    }
    #endregion




    public void AddScore(int score){
        totalScore += score;
        OnScoreChange?.Invoke(totalScore);
    }

    public void GameOver(bool isSuccess)
    {
        Time.timeScale = 0f;
        if (isSuccess)
        {
            SoundManager.Instance.ChangeBgmAndPlay(gameClearBgm);
            SoundManager.Instance.PlaySfx(gameClearSfx);
        }
        else
        {
            SoundManager.Instance.PlaySfx(gameOverSfx);
            SoundManager.Instance.StopBgm();
        }

        OnGameOver?.Invoke(isSuccess, totalScore, life, maxCombo, GameManager.Instance.IsNextStageValid);
    }

    private void GetUserPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void OnBrickBreak(int score, int remainingBrick)
    {
        if (ComboWindow != null) StopCoroutine(ComboWindow);

        ComboScore += GetComboScore(score);
        ComboCount++;
        // UI에게 이벤트 발행
        OnCombo?.Invoke(ComboCount, ComboScore);

        if (remainingBrick  <= 0)
        {
            ReportScore();
            GameOver(true);
        }
        else
        {
            ComboWindow = StartCoroutine(WaitComboTime());
        }
    }


    IEnumerator WaitComboTime()
    {
        yield return comboWindow;
        ReportScore();
        ComboWindow = null;
    }

    int GetComboScore(int score)
    {
        float multiplier = 1f + ComboCount / 10f;
        multiplier = Mathf.Min(multiplier, maxComboMultiplier);
        return Mathf.RoundToInt(score * multiplier);
    }

    void ReportScore()
    {
        AddScore(ComboScore);
        maxCombo = Mathf.Max(maxCombo, ComboCount);
        ComboScore = 0;
        ComboCount = 0;
        OnCombo(0, 0);
    }

    public void OnAllBallLost()
    {
        life--;
        if (life > 0) OnBallLost.Invoke(life);
        else GameOver(false);
    }

    public void TogglePause()
    {
        if (isGamePaused) ResumeGame();
        else PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        SoundManager.Instance.PlaySfx(pauseSound);
        OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        OnResume?.Invoke();
        SoundManager.Instance.PlaySfx(resumeSound);
    }
}
