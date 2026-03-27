using UnityEngine;

public class Health_Effect : MonoBehaviour, IItemEffect
{
    public void OnPickUp(GameObject owner)
    {
        float hpCount = MANAGER.SESSION.MaxHP * 0.3f;
        MANAGER.SESSION.HP += hpCount;

        var damageFont = MANAGER.POOL.Pooling_OBJ("DamageFont").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initialize(
            Base_Canvas.instance.HOLDERLAYER,
            transform.position,
            ((int)hpCount).ToString(),
            Color.green,
            false
            );
        });
    }
}
