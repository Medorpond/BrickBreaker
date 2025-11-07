using UnityEngine;

public class ItemModule : BrickModule
{
    private Brick brick;

    [SerializeField, Range(0, 1)] private float dropRate;

    private void Awake()
    {
        brick = GetComponent<Brick>();
    }

    private void OnEnable()
    {
        brick.OnBreak += OnBreak;
    }

    private void OnDisable()
    {
        brick.OnBreak -= OnBreak;
    }

    public override void OnBreak()
    {
        if (Random.value <= dropRate) ItemManager.Instance.TrySpawnRandomItem(transform.position);
    }
}
