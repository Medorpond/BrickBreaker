using UnityEngine;

public class Deadzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Kill(collision);
    }


    private void Kill(Collider2D collision)
    {
        if (collision.TryGetComponent<IRetrievable>(out var poolItem))
        {
            poolItem.Retrieve();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
