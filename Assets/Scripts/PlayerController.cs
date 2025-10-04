using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;

    [Header("Paddle Configs")]
    [SerializeField, Min(0)] private float lerpSpeed = 20f;
    [SerializeField, Min(0)] private float widthRatio = 1f;
    [SerializeField, Min(0)] private float moveLimit;

    //====================================================
    #region Lifecycle
    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    public void Move()
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentPos = transform.position;
        targetPos.x = Mathf.Clamp(targetPos.x, -moveLimit, moveLimit);
        targetPos.y = currentPos.y;
        transform.position = Vector2.Lerp(currentPos, targetPos, lerpSpeed* Time.fixedDeltaTime);
    }

    
}
