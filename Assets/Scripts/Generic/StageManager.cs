using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : BaseManager<StageManager>
{
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

    public event Action<int> OnScoreChange;
    public event Action<int> OnBallLost;
    public event Action<int, int> OnCombo;

    #region LifeCycle
    protected override void Awake()
    {
        base.Awake();
        comboWindow = new WaitForSeconds(comboTimeWindow);
    }

    private void Start()
    {
        int currentLv = GameManager.Instance.CurrentLevel;
        GameObject stagePrefab = GameManager.Instance.LevelDB.stagePrefabs[currentLv];
        Instantiate(stagePrefab, Vector3.zero, Quaternion.identity);

        OnScoreChange?.Invoke(0);
        OnCombo?.Invoke(0, 0);
        OnBallLost?.Invoke(life);
    }
    #endregion


    

    public void AddScore(int score){
        totalScore += score;
        OnScoreChange?.Invoke(totalScore);
    }

    public void GameOver(bool isSuccess)
    {
        Debug.Log($"Game Over");
        if (isSuccess)
        {
            Debug.Log("You Win!");
        }
        else
        {
            Debug.Log("You Lose...");
            Time.timeScale = 0;
        }
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
            // StageManager.EndGame(isSuccess = true) 호출
            Debug.Log("GameOver!");
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
        // Get GameManager and Report Score;
        //TESTONLY
        AddScore(ComboScore);
        //TESTONLY
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
}
