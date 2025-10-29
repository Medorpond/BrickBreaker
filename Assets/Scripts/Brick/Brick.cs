using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private AudioClip hitSound;

    #region Events
    public event Action<Ball> OnHit;
    public event Action OnBreak;
    #endregion

    #region Lifecycle
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            OnHit?.Invoke(ball);
            SoundManager.Instance.PlaySfx(hitSound);
        }
    }

    public void Break()
    {
        OnBreak?.Invoke();
        BrickManager.Instance.OnBrickBreak(score);
        Destroy(gameObject);
    }
}
