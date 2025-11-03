using UnityEngine;

[CreateAssetMenu(fileName = "AddLife", menuName = "ItemData/AddLife")]
public class AddLife : ItemData
{
    public override void ItemEffect(GameObject target)
    {
        if(target.TryGetComponent<StageManager>(out var manager))
        {
            ;
        }
    }
}
