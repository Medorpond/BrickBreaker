using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverResult : MonoBehaviour
{
    private GameManager gm;

    private CanvasGroup group;

    [SerializeField] private Button RetryBtn;
    [SerializeField] private Button ExitBtn;
    [SerializeField] private Button NextStageBtn;

    [SerializeField] private TextMeshProUGUI ResultTMP;
    [SerializeField] private TextMeshProUGUI ScoreTMP;
    [SerializeField] private TextMeshProUGUI LifeTMP;
    [SerializeField] private TextMeshProUGUI ComboTMP;

    [SerializeField] private string clearMessage = "Stage Clear!";
    [SerializeField] private string failMessage = "Fail...";

    [SerializeField] private int totalDigits = 8;

    private void Awake()
    {
        gm = GameManager.Instance;
        group = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        SetListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void TogglePanel(bool isActive)
    {
        group.alpha = isActive ? 1f : 0f;
        group.interactable = isActive;
        group.blocksRaycasts = isActive;
    }

    public void UpdatePanel(bool isSuccess, int score, int life, int combo, bool isNextStageValid)
    {
        if (isSuccess)
        {
            ResultTMP.text = clearMessage;
            NextStageBtn.interactable = true;
        }
        else
        {
            ResultTMP.text = failMessage;
            NextStageBtn.interactable = false;
        }

        if (!isNextStageValid) NextStageBtn.interactable = false;

        ScoreTMP.text = score.ToString($"D{totalDigits}");
        LifeTMP.text = life.ToString($"D{totalDigits}");
        ComboTMP.text = combo.ToString($"D{totalDigits}");
    }

    public void DisplayPanel(bool isSuccess, int score, int life, int combo, bool isNextStageValid)
    {
        UpdatePanel(isSuccess, score, life, combo, isNextStageValid);
        TogglePanel(true);
    }

    private void SetListeners()
    {
        RetryBtn.onClick.AddListener(gm.RetryLevel);
        ExitBtn.onClick.AddListener(gm.LoadLobby);
        NextStageBtn.onClick.AddListener(gm.LoadNextStage);
    }

    private void RemoveListeners()
    {
        RetryBtn.onClick.RemoveListener(gm.RetryLevel);
        ExitBtn.onClick.RemoveListener(gm.LoadLobby);
        NextStageBtn.onClick.RemoveListener(gm.LoadNextStage);
    }
}
