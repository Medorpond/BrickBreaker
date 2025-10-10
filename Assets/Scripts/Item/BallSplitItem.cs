using UnityEngine;

[CreateAssetMenu(fileName = "BallSplitItem", menuName = "ItemData/BallSplitItem")]
public class BallSplitItem : ItemData
{
    public int splitQuantity;
    public override void ItemEffect(GameObject target)
    {
        if(target.TryGetComponent<Ball>(out var ball))
        ball.Split(splitQuantity);
    }
}
