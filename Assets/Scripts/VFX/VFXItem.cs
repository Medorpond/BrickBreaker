using UnityEngine;

public class VFXItem : PoolItem<VFXItem> 
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnim(Quaternion? rotation = null)
    {
        transform.rotation = rotation ?? Quaternion.identity;

        animator.Rebind();
        animator.Update(0f);
    }
}