using UnityEngine;
using UnityEngine.UI;

public class StageGeneral : MonoBehaviour
{
    StageManager sm;

    [SerializeField] private Options OptionsUI;
    [SerializeField] private GameOverResult GameOverUI;
    [SerializeField] private Button OptionBtn;

    private void Awake()
    {
        sm = StageManager.Instance;
    }
    private void Start()
    {
        GameOverUI.TogglePanel(false);
        OptionsUI.TogglePanel(false);
    }
    private void OnEnable()
    {
        SetListeners();
    }
    private void OnDisable()
    {
        RemoveListers();
    }

    #region General
    private void SetListeners()
    {
        sm.OnPause += ShowOptionsPanel;
        sm.OnResume += HideOptionsPanel;
        sm.OnGameOver += OnGameOver;
        OptionBtn.onClick.AddListener(sm.TogglePause);
    }
    private void RemoveListers()
    {
        sm.OnPause -= ShowOptionsPanel;
        sm.OnResume -= HideOptionsPanel;
        sm.OnGameOver -= OnGameOver;
        OptionBtn.onClick.RemoveListener(sm.TogglePause);
    }

    public void ShowOptionsPanel() => OptionsUI.TogglePanel(true);
    public void HideOptionsPanel() => OptionsUI.TogglePanel(false);

    private void OnGameOver(bool isSuccess, int score, int life, int combo, bool isNextStageValid)
    {
        GameOverUI.DisplayPanel(isSuccess, score, life, combo, isNextStageValid);
    }
    #endregion
}
