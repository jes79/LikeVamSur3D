using UnityEngine;

public class Burn_Status : IStatusEffect
{

    private float totalDamage => MANAGER.SESSION.Damage*0.5f;
    private float duration = 4f;
    private float elapsed = 0.04f;
    private float tickInterval = 1f;
    private float tickTimer = 0f;


    public bool IsFinished => elapsed >= duration;

    public void Apply(MONSTER target, StatusEffect effect)
    {
        effect.Burn.gameObject.SetActive(true);
        //초기 발동 이펙트 구현
    }

    public void End(MONSTER monster, StatusEffect effect)
    {
        effect.Burn.gameObject.SetActive(false);
    }

    public void Tick(MONSTER target)
    {
        elapsed += Time.deltaTime;
        tickTimer += Time.deltaTime;
        if(tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            target.GetDamage(totalDamage);
        }
    }
}
