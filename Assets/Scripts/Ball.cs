using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rbody.linearVelocity = new Vector2( 3f, 3f );
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 효과 처리
    }
}
