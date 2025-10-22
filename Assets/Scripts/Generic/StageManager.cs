using System.Collections.Generic;
using UnityEngine;

public class StageManager : BaseManager<StageManager>
{

    private void Start()
    {
        int currentLv = GameManager.Instance.CurrentLevel;
        GameObject stagePrefab = GameManager.Instance.LevelDB.stagePrefabs[currentLv];
        Instantiate(stagePrefab, Vector3.zero, Quaternion.identity);
    }


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
}
