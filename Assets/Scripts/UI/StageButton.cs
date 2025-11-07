using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitBtn(string text, Action OnClickAction)
    {
        buttonText.text = text;
        button.onClick.AddListener(() => OnClickAction());
        button.onClick.AddListener(SoundManager.Instance.PlayDefaultUISound);
    }
}
