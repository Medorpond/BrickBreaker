using UnityEngine;

[CreateAssetMenu(fileName = "PaddleWidthChangeItem", menuName = "ItemData/PaddleWidthChangeItem")]
public class PaddleWidthChangeItem : ItemData
{
    public int widthLevelDiff;
    public override void ItemEffect(GameObject target)
    {
        if (target.TryGetComponent<PlayerController>(out var paddle))
            paddle.ChangeWidthLevelByDiff(widthLevelDiff);
    }
}