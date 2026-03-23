using UnityEngine;

public class Freeze_Status : IStatusEffect
{
    public Color originalColor;
    private float freezeStack = 0f;
    private float maxFreezeStack = 1f;
    private float slowRate = 0.8f; //최대 감속
    private float slowDuration = 2.5f;
    private float slowTimer;

    private float frozenDuration = 2f;
    private float elapsed = 0f;

    private bool isFrozen = false;


    private StatusEffect ownerEffect;
    private MONSTER owner;

    public bool IsFinished => isFrozen ? elapsed >= frozenDuration : freezeStack <= 0;

    public void AddStack(float amount)
    {
        freezeStack = Mathf.Clamp01(freezeStack + amount);
    }

    public void Apply(MONSTER target, StatusEffect effect)
    {
        if (owner == null)
        {
            owner = target;
            ownerEffect = effect;
            originalColor = effect.renderer.material.GetColor("_BaseColor");
            effect.renderer.material.SetColor("_BaseColor", new Color(0.5f, 0.8f, 1f, 1f));
        }
        slowTimer = 0f;

        if (!isFrozen && freezeStack >= maxFreezeStack)
        {
            isFrozen = true;
            ownerEffect.FreezeStone.gameObject.SetActive(true);
            elapsed = 0f;
            target.SetSpeedMultiplier(0f);
        }
        else if (isFrozen)
        {
            float slowMutiplier = Mathf.Lerp(0, slowRate, freezeStack);
            target.SetSpeedMultiplier(slowMutiplier);
        }
    }

    public void End(MONSTER target, StatusEffect effect)
    {
        effect.renderer.material.SetColor("_BaseColor", originalColor);
        ownerEffect.FreezeStone.gameObject.SetActive(false);
        target.SetSpeedMultiplier(1f);
    }

    public void Tick(MONSTER target)
    {

        if (isFrozen)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            slowTimer += Time.deltaTime;
            if(slowTimer >= slowDuration)
            {
                freezeStack = 0f;
                target.SetSpeedMultiplier(1f);
            }
            else
            {
                float slowMutiplier = 1 - Mathf.Lerp(0, slowRate, freezeStack);
                target.SetSpeedMultiplier(slowMutiplier);
            }

        }
    }
}
