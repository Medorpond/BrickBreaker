using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    GameManager gm;
    SoundManager soundManager;

    private CanvasGroup group;

    [SerializeField] private Button RetryBtn;
    [SerializeField] private Button GiveUpBtn;
    [SerializeField] private Button CloseBtn;

    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private TMP_Dropdown ScreenMode;

    [SerializeField] AudioClip optionOpenSound;
    [SerializeField] AudioClip optionCloseSound;

    private void Awake()
    {
        gm = GameManager.Instance;
        soundManager = SoundManager.Instance;
        group = GetComponent<CanvasGroup>();
        SetData();
    }
    private void OnEnable()
    {
        SetListeners();
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
        RemoveListeners();
    }

    public void TogglePanel(bool isActive)
    {
        group.alpha = isActive ? 1f : 0f;
        group.interactable = isActive;
        group.blocksRaycasts = isActive;

        if (isActive) soundManager.Muffle();
        else soundManager.DeMuffle();
    }

    private void SetData()
    {
        MasterSlider.value = PlayerPrefs.GetFloat(PrefKeys.MasterVolume, 1f);
        BGMSlider.value = PlayerPrefs.GetFloat(PrefKeys.BGMVolume, 1f);
        SFXSlider.value = PlayerPrefs.GetFloat(PrefKeys.SFXVolume, 1f);

        ScreenMode.value = PlayerPrefs.GetInt(PrefKeys.ScreenMode, 0);
    }

    private void SetListeners()
    {
        RetryBtn.onClick.AddListener(gm.RetryLevel);
        GiveUpBtn.onClick.AddListener(gm.LoadLobby);
        CloseBtn.onClick.AddListener(StageManager.Instance.ResumeGame);

        MasterSlider.onValueChanged.AddListener(OnMasterVolChange);
        BGMSlider.onValueChanged.AddListener(OnBGMVolChange);
        SFXSlider.onValueChanged.AddListener(OnVFXVolChange);

        ScreenMode.onValueChanged.AddListener(OnScreenModeChange);
    }
    private void RemoveListeners()
    {
        RetryBtn.onClick.RemoveAllListeners();
        GiveUpBtn.onClick.RemoveAllListeners();
        CloseBtn.onClick.RemoveAllListeners();

        MasterSlider.onValueChanged.RemoveListener(OnMasterVolChange);
        BGMSlider.onValueChanged.RemoveListener(OnBGMVolChange);
        SFXSlider.onValueChanged.RemoveListener(OnVFXVolChange);

        ScreenMode.onValueChanged.RemoveListener(OnScreenModeChange);
    }

    private void OnMasterVolChange(float volume)
    {
        soundManager.SetMasterVolume(volume);
    }

    private void OnBGMVolChange(float volume)
    {
        soundManager.SetBgmVolume(volume);
    }

    private void OnVFXVolChange(float volume)
    {
        soundManager.SetSfxVolume(volume);
    }

    private void OnScreenModeChange(int modeIndex)
    {
        FullScreenMode mode = modeIndex switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.MaximizedWindow,
            3 => FullScreenMode.Windowed,
            _ => FullScreenMode.Windowed,
        };

        gm.SetScreenMode(mode);
        PlayerPrefs.SetInt(PrefKeys.ScreenMode, modeIndex);
    }
}
