using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI lifeUI;

    private void OnEnable()
    {
        StageManager.Instance.OnScoreChange += UpdateScoreUI;
        StageManager.Instance.OnBallLost += UpdateLifeUI;
    }

    private void OnDisable()
    {
        StageManager.Instance.OnScoreChange -= UpdateScoreUI;
        StageManager.Instance.OnBallLost -= UpdateLifeUI;
    }

    public void UpdateScoreUI(int score) => scoreUI.text = $"Score: {score}";
    public void UpdateLifeUI(int life) => lifeUI.text = $"Life: {life}";
}
