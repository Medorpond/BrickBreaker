using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
public abstract class ItemData: ScriptableObject
{
    [field: SerializeField] public ItemTarget TargetType { get; protected set; }
    [field: SerializeField] public Sprite ItemSprite { get; protected set; }
    public abstract void ItemEffect(GameObject target);
}
