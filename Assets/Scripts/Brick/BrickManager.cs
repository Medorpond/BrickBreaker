using System.Collections;
using UnityEngine;

public class BrickManager : BaseManager<BrickManager>
{
    
    

    public int BreakableCount { get; private set; }
    #region Lifecycle
    
    private void Start()
    {
        CountBreakables();
        Debug.Log($"Brick: {BreakableCount}°³");
    }
    #endregion

    public void CountBreakables() => BreakableCount = GetComponentsInChildren<HPModule>().Length;
    public void OnBrickBreak(int score)
    {
        BreakableCount--;
        StageManager.Instance.OnBrickBreak(score, BreakableCount);
    }
}
