using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    [Header("ScoreUI")]
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI comboCountUI;
    [SerializeField] private TextMeshProUGUI comboScoreUI;
    [Space(10f)]

    [Header("LifeUI")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite brokenHeartSprite;
    [SerializeField] private List<Image> lifeUI;
    [SerializeField] private TextMeshProUGUI extraLifeUI;

    private int maxHeartImage;

    #region Lifecycle
    private void Awake()
    {
        maxHeartImage = lifeUI.Count;
    }
    private void OnEnable()
    {
        StageManager.Instance.OnScoreChange += UpdateScoreUI;
        StageManager.Instance.OnBallLost += UpdateLifeUI;
        StageManager.Instance.OnCombo += UpdateComboStatus;
    }

    private void OnDisable()
    {
        StageManager.Instance.OnScoreChange -= UpdateScoreUI;
        StageManager.Instance.OnBallLost -= UpdateLifeUI;
        StageManager.Instance.OnCombo -= UpdateComboStatus;
    }
    #endregion

    public void UpdateScoreUI(int score) => scoreUI.text = $"Score: {ModifyIntToString(score)}";
    public void UpdateLifeUI(int life)
    {
        extraLifeUI.text = $"+{life - maxHeartImage}";

        if (life > maxHeartImage)
        {
            foreach (Image heartImage in lifeUI) heartImage.sprite = fullHeartSprite;
        }
        else
        {
            for (int i = 0; i < maxHeartImage; i++)
            {
                if (i < life)
                {
                    lifeUI[i].sprite = fullHeartSprite;
                }
                else
                {
                    lifeUI[i].sprite = brokenHeartSprite;
                }
            }
        }
    }
    public void UpdateComboStatus(int comboCount, int comboScore)
    {
        comboCountUI.text = $"Combo: x{ModifyIntToString(comboCount, 6)}!";
        comboScoreUI.text = $"{ModifyIntToString(comboScore)}";
    }

    private string ModifyIntToString(int num, int blankSize = 8)
    {
        int length = 0;
        int temp = num;
        while(temp > 0)
        {
            length++;
            temp /= 10;
        }
        blankSize -= length;
        if (blankSize <= 0) return num.ToString();

        string result = "";
        for(int i = 0; i < blankSize; i++)
        {
            result += '0';
        }
        return result += num.ToString();
    }
}
