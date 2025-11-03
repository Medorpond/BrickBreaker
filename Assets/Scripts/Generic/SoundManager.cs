using UnityEngine;

public class SoundManager : BaseManager<SoundManager>
{
    [Header("AudioSources")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Params")]
    [SerializeField] float muffleRate = 0.4f;

    [Header("AudioClips")]
    [SerializeField] AudioClip DefaultUISound;

    public float MasterVol { get; private set; } = 1f;
    public float BgmVol { get; private set; } = 1f;
    public float SfxVol { get; private set; } = 1f;

    private float muffleMultiplier = 1f;

    #region Lifecycle
    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
        InitSoundManager();
    }

    #endregion

    public void Muffle()
    {
        muffleMultiplier = muffleRate;
        UpdateAllVolumes();
    }

    public void DeMuffle()
    {
        muffleMultiplier = 1f;
        UpdateAllVolumes();
    }

    public void InitSoundManager()
    {
        if (!bgmSource || !sfxSource) Debug.LogError($"{name}: AudioSource Not Set in Inspector");

        MasterVol = PlayerPrefs.GetFloat(PrefKeys.MasterVolume, 1f);
        BgmVol = PlayerPrefs.GetFloat(PrefKeys.BGMVolume, 1f);
        SfxVol = PlayerPrefs.GetFloat(PrefKeys.SFXVolume, 1f);

        UpdateAllVolumes();
    }

    public void SetMasterVolume(float vol)
    {
        MasterVol = vol;
        UpdateAllVolumes();
        PlayerPrefs.SetFloat(PrefKeys.MasterVolume, vol);
    }

    public void SetBgmVolume(float vol)
    {
        BgmVol = vol;
        UpdateBgmVolume();
        PlayerPrefs.SetFloat(PrefKeys.BGMVolume, vol);
    }
    public void SetSfxVolume(float vol)
    {
        SfxVol = vol;
        UpdateSfxVolume();
        PlayerPrefs.SetFloat(PrefKeys.SFXVolume, vol);
    }

    private void UpdateAllVolumes()
    {
        UpdateBgmVolume();
        UpdateSfxVolume();
    }

    private void UpdateBgmVolume() => bgmSource.volume = BgmVol * MasterVol * muffleMultiplier;
    private void UpdateSfxVolume() => sfxSource.volume = SfxVol * MasterVol * muffleMultiplier;

    public void PlaySfx(AudioClip sfx, float volScale = 1f)
    {
        if (!sfx) return;
        sfxSource.PlayOneShot(sfx, volScale);
    }
    public void ChangeBgmAndPlay(AudioClip bgm)
    {
        bgmSource.clip = bgm;
        bgmSource.Play();
    }

    public void ResumeBgm()
    {
        if (bgmSource.clip == null) return;
        bgmSource.Play();
    }
    public void StopBgm() => bgmSource.Stop();

    public void PlayDefaultUISound() => PlaySfx(DefaultUISound);

}
