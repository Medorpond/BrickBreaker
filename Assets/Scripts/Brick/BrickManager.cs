using System.Collections;
using UnityEngine;

public class BrickManager : BaseManager<BrickManager>
{
    
    [SerializeField] private float comboTimeWindow;
    private WaitForSeconds comboWindow;

    [SerializeField] private float maxComboMultiplier = 3.0f;
    public int ComboScore { get; private set; } = 0;
    public int ComboCount { get; private set; } = 0;
    private Coroutine ComboWindow = null;

    public int BreakableCount { get; private set; }
    #region Lifecycle
    protected override void Awake()
    {
        base.Awake();
        comboWindow = new WaitForSeconds(comboTimeWindow);
    }
    private void Start()
    {
        CountBreakables();
        Debug.Log($"Brick: {BreakableCount}개");
    }
    #endregion

    public void CountBreakables() => BreakableCount = GetComponentsInChildren<HPModule>().Length;
    public void OnBrickBreak(int score)
    {
        BreakableCount--;

        if (ComboWindow != null) StopCoroutine(ComboWindow);

        
        ComboScore += GetComboScore(score);
        ComboCount++;
        // UI에게 이벤트 발행
        Debug.Log($"{ComboCount}Combo! Point: {ComboScore}");

        if(BreakableCount <= 0)
        {
            ReportScore();
            // GM.EndGame(isSuccess = true) 호출
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
        StageManager.Instance.AddScore(ComboScore);
        //TESTONLY
        ComboScore = 0;
        ComboCount = 0;
    }
}
