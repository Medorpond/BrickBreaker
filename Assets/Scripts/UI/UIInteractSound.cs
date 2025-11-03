using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIInteractSound : MonoBehaviour
{
    [SerializeField] AudioClip UISoundClip;

    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(PlayClickSound);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        if (UISoundClip == null) SoundManager.Instance.PlayDefaultUISound();
        else SoundManager.Instance.PlaySfx(UISoundClip);
    }
}
