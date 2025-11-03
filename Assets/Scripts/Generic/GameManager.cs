using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    #region Lifecycle
    #endregion

    [field: SerializeField] public LevelDatabase LevelDB { get; private set; }
    public int MaxLv { get; private set; }
    public int CurrentLevel { get; private set; }
    public bool IsNextStageValid { get; private set; } = true;

    protected override void Awake()
    {
        base.Awake();
        MaxLv = LevelDB.LevelDatas.Count - 1;
    }

    public void LoadStage(int stageId)
    {
        CurrentLevel = stageId;
        if (CurrentLevel == MaxLv) IsNextStageValid = false;
        else IsNextStageValid = true;
        LoadScene(1);
    }
    public void LoadNextStage()
    {
        if (CurrentLevel > MaxLv) LoadLobby();
        else LoadStage(++CurrentLevel);
    }
    public void RetryLevel() => LoadScene(1);
    public void LoadLobby() => LoadScene(0);

    public void SetScreenMode(FullScreenMode mode) => Screen.fullScreenMode = mode;

    private void LoadScene(int sceneId) 
    { 
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
    }

    public void ExitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
