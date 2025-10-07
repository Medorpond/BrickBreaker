using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Contact!");
        if(collision.TryGetComponent<Ball>(out var ball))
        {
            Debug.Log("It's a Ball!");
            if (BallManager.Instance) BallManager.Instance.RetrieveBall(ball);
            else Destroy(ball.gameObject);
        }
    }
}
