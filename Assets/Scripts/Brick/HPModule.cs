using UnityEngine;

public class HPModule : BrickModule
{
    private Brick brick;

    [SerializeField] private int maxHP;
    private int currentHP;

    private void Awake()
    {
        brick = GetComponent<Brick>();
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
    }

    public override void OnHit(Ball ball)
    {
        // 현재는 Damage 1 고정
        // 추후 게임 설계에 따라 ball.damage 만큼 데미지 입도록 변경

        currentHP--;
        if (currentHP <= 0) brick.Break();
    }
}
