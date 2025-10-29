using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
public abstract class ItemData: ScriptableObject
{
    [field: SerializeField] public ItemTarget TargetType { get; protected set; }
    public abstract void ItemEffect(GameObject target);
}
