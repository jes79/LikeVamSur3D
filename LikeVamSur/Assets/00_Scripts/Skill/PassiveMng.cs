using UnityEngine;

public class PassiveMng : MonoBehaviour
{
    Session_Mng session;

    private void Start()
    {
        session = MANAGER.SESSION;
    }

    public float Plus(CardDB db, int level)
    {
        return  (db.baseDamage + db.damagePerLevel * (level - 1));
    }

    public void PASSIVE01(CardDB db, int level)
    {
        
        session.magnetRadiusPercentage = Plus(db, level);
    }

    public void PASSIVE02(CardDB db, int level)
    {
        session.DamagePercentage =  Plus(db, level);
    }

    public void PASSIVE03(CardDB db, int level)
    {
        session.expPlusPercentage = Plus(db, level);
    }

    public void PASSIVE04(CardDB db, int level)
    {
        session.CriticalPercentage = Plus(db, level);
    }

    public void PASSIVE05(CardDB db, int level)
    {
        session.CriticalDamage = Plus(db, level);
    }

    public void PASSIVE06(CardDB db, int level)
    {
        session.HPPercentage = Plus(db, level);
    }

    public void SetPassiveCard(CardDB db, int level)
    {
        switch (db.className)
        {
            case "Magnet": PASSIVE01(db, level); break;
            case "ATK":    PASSIVE02(db, level); break;
            case "EXP":    PASSIVE03(db, level); break;
            case "CP":     PASSIVE04(db, level); break;
            case "CD":     PASSIVE05(db, level); break;
            case "HP":     PASSIVE06(db, level); break;
        }
    }
}
