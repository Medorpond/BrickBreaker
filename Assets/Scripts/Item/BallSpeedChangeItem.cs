using UnityEngine;

[CreateAssetMenu(fileName = "BallSpeedChangeItem", menuName = "ItemData/BallSpeedChangeItem")]
public class BallSpeedChangeItem : ItemData
{
    public float speedChangeRate = 0.2f;
    public override void ItemEffect(GameObject target)
    {
        if (target.TryGetComponent<Ball>(out var ball))
            ball.ShiftSpeedByRate(speedChangeRate);
    }
}
