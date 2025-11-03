using System.Collections.Generic;
using UnityEngine;

public class HPModule : BrickModule
{
    private Brick brick;

    [SerializeField, Range(1, 4)] private int maxHP;
    [SerializeField] private List<Sprite> HPSprite;

    private SpriteRenderer spriteRenderer;
    private int currentHP;


    private void Awake()
    {
        brick = GetComponent<Brick>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        brick.OnHit += OnHit;
    }
    private void Start()
    {
        InitModule();
    }

    private void OnDisable()
    {
        brick.OnHit -= OnHit;
    }


    public override void InitModule()
    {
        currentHP = maxHP;
        UpdateSprite();
    }

    public override void OnHit(Ball ball)
    {
        currentHP--;
        
        if (currentHP <= 0) brick.Break();
        else UpdateSprite();
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = HPSprite[currentHP - 1];
    }
}
