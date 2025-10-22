using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreUI;

    public void UpdateScoreUI(int score) => scoreUI.text = $"Score: {score}";
}
