using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    StageManager sm;

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
        sm = StageManager.Instance;
        maxHeartImage = lifeUI.Count;
    }

    private void Start()
    {
        UpdateScoreUI(0);
        UpdateComboStatus(0, 0);
    }
    private void OnEnable()
    {
        sm.OnScoreChange += UpdateScoreUI;
        sm.OnLifeChange += UpdateLifeUI;
        sm.OnCombo += UpdateComboStatus;
    }

    private void OnDisable()
    {
        sm.OnScoreChange -= UpdateScoreUI;
        sm.OnLifeChange -= UpdateLifeUI;
        sm.OnCombo -= UpdateComboStatus;
    }
    #endregion

    public void UpdateScoreUI(int score) => scoreUI.text = $"{ModifyIntToString(score)}";
    public void UpdateLifeUI(int life)
    {
        if (life > maxHeartImage)
        {
            extraLifeUI.text = $"+{life - maxHeartImage}";

            foreach (Image heartImage in lifeUI) heartImage.sprite = fullHeartSprite;
        }
        else
        {
            extraLifeUI.text = $"+{0}";

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
        comboCountUI.text = $"x{ModifyIntToString(comboCount, 4)}!";
        comboScoreUI.text = $"{ModifyIntToString(comboScore)}";
    }

    private string ModifyIntToString(int num, int blankSize = 6)
    { 

        int length = 0;
        int temp = num;
        while(temp > 0)
        {
            length++;
            temp /= 10;
        }
        if (num == 0) length++;

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
