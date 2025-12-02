using UnityEngine;

[System.Serializable]
public class SelectCard
{
    public CardDB db;
    public int Level;
}

public enum CardState
{
    Active,
    Passive
}


[CreateAssetMenu(fileName = "Scriptable", menuName = "DB/Card", order = int.MinValue)]
public class CardDB : ScriptableObject
{
    public string id;
    public string className;

    [Space(20f)]
    [TextArea(1, 10)]
    public string description;
    public CardState state;

    
    [Space(20f)]
    [Header("스킬 데이터")]
//  public float DamagePercentage;
    public float baseCooldown;
    public float baseDamage;
    public float cooldownPerLevel;
    public float damagePerLevel;
}
