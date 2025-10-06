using UnityEngine;

public class Ball : PoolItem<Ball>
{
    Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }
}
