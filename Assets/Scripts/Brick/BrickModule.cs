using UnityEngine;

[RequireComponent(typeof(Brick))]
public abstract class BrickModule : MonoBehaviour
{
    public virtual void InitModule() { }
    public virtual void OnHit(Ball ball) { }
    public virtual void OnBreak() { }
}
