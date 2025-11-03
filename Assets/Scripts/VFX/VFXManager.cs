using UnityEngine;

public class VFXManager : BaseManager<VFXManager>
{
    [SerializeField] private VFXPool BreakEffectPoolModel;
    [SerializeField] private VFXPool BouncEffectPoolModel;

    private VFXPool BreakEffectPool;
    private VFXPool BouncEffectPool;

    #region LifeCycle
    protected override void Awake()
    {
        dontDestroyOnLoad = false;
        base.Awake();
        InitPool();
    }
    #endregion

    void InitPool()
    {
        BreakEffectPool = Instantiate(BreakEffectPoolModel, transform);
        BouncEffectPool = Instantiate(BouncEffectPoolModel, transform);
    }

    public VFXItem GetEffect(EffectType type, Vector3 initPos)
    {
        VFXItem effect = type switch
        {
            EffectType.Break => BreakEffectPool.GetItem(initPos),
            EffectType.Bounce => BouncEffectPool.GetItem(initPos),

            _ => throw new System.Exception("No Such EffectType Exist")
        };

        return effect;
    }


    public enum EffectType
    {
        Break,
        Bounce
    }
}
