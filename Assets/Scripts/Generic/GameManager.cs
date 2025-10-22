using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private void Update()
    {
        //TESTONLY
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadStage(0);
        }
        //TESTONLY
    }

    [field: SerializeField] public LevelDatabase LevelDB { get; private set; }
    public int CurrentLevel { get; private set; }

    public void LoadStage(int stageId)
    {
        CurrentLevel = stageId;
        ChangeScene(1);
    }
    public void ChangeScene(int sceneId) => SceneManager.LoadScene(sceneId);
}
