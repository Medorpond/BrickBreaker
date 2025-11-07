using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ItemShell : PoolItem<ItemShell>
{
    [HideInInspector] public ItemData data;
    [SerializeField] private float dropSpeed;
    [SerializeField] AudioClip itemGetSound;

    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    

    private void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + dropSpeed * Time.fixedDeltaTime * Vector2.down);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (data && collision.TryGetComponent<PlayerController>(out var _))
        {
            ItemManager.Instance.InvokeEvent(data);
            SoundManager.Instance.PlaySfx(itemGetSound);
            data = null;
            Retrieve();
        }
    }

    public void SetData(ItemData _data)
    {
        data = _data;
        spriteRenderer.sprite = _data.ItemSprite;
    }
}
