using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    GameManager gm;

    [SerializeField] AudioClip lobbyBgm;

    [SerializeField] private Transform stageContainerTransform;
    [SerializeField] private StageButton stageBtnModel;

    [SerializeField] private CanvasGroup mainLobby;
    [SerializeField] private CanvasGroup StageList;
    [SerializeField] private CanvasGroup Configs;

    [SerializeField] private Button playBtn;
    [SerializeField] private Button ConfigBtn;

    private CanvasGroup previousCanvas;
    private CanvasGroup currentCanvas;

    private void Awake()
    {
        gm = GameManager.Instance;

        previousCanvas = mainLobby;
        currentCanvas = mainLobby;
        InitCanvasGroup();
    }
    private void Start()
    {
        InitStageList();
        SetButtonListeners();

        SoundManager.Instance.ChangeBgmAndPlay(lobbyBgm);
    }

    #region General
    private void InitCanvasGroup()
    {
        ToggleCanvasGroup(mainLobby, true);

        ToggleCanvasGroup(StageList, false);
        ToggleCanvasGroup(Configs, false);
    }
    private void SetButtonListeners()
    {
        playBtn.onClick.AddListener(() => SwitchCanvasGroup(StageList));
        ConfigBtn.onClick.AddListener(() => SwitchCanvasGroup(Configs));
    }


    public void SwitchCanvasGroup(CanvasGroup nextCanvas)
    {
        ToggleCanvasGroup(currentCanvas, false);
        ToggleCanvasGroup(nextCanvas, true);

        previousCanvas = currentCanvas;
        currentCanvas = nextCanvas;
    }

    private void ToggleCanvasGroup(CanvasGroup canvasGroup, bool isActive)
    {
        canvasGroup.alpha = isActive ? 1f : 0f;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }

    public void ReturnToPrevCanvasGroup()
    {
        SwitchCanvasGroup(previousCanvas);
        currentCanvas = previousCanvas;
    }
    #endregion


    #region StageUI
    private void InitStageList()
    {
        int stageSize = gm.MaxLv;

        for(int i = 0; i < stageSize; i++)
        {
            int levelIndex = i;
            StageButton newBtn = Instantiate(stageBtnModel, stageContainerTransform);

            newBtn.InitBtn(
                GetBtnText(levelIndex + 1),
                () => GameManager.Instance.LoadStage(levelIndex)
                );
        }
    }

    private string GetBtnText(int stageIndex)
    {
        if (stageIndex / 10 > 0)
        {
            return stageIndex.ToString();
        }
        else
        {
            return $"0{stageIndex}";
        }
    }
    #endregion
}
