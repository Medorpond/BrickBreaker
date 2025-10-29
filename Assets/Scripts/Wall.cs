using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] AudioClip hitSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ball>(out var _))
        {
            SoundManager.Instance.PlaySfx(hitSound);
        }
    }
}
