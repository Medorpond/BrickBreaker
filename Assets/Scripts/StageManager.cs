using UnityEngine;

public class StageManager : BaseManager<StageManager>
{
    //TESTONLY
    [HideInInspector] public int totalScore;

    public void AddScore(int score){
        totalScore += score; Debug.Log($"Score: {totalScore}");
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
    //TESTONLY
}
