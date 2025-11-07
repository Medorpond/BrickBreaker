using System.Net.NetworkInformation;
using UnityEngine;

public class EffectModule : BrickModule
{
    private Brick brick;
    [SerializeField] private VFXManager.EffectType effectType;

    private void Awake()
    {
        brick = GetComponent<Brick>();
    }

    private void OnEnable()
    {
        brick.OnHit += OnHit;
    }

    private void OnDisable()
    {
        brick.OnHit -= OnHit;
    }

    public override void OnHit(Ball ball)
    {
        var effect = VFXManager.Instance.GetEffect(effectType, transform.position);
        effect.PlayAnim();
    }
}
