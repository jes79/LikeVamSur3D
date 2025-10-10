using UnityEngine;

public class Database_Mng : MonoBehaviour
{
    public PartDB Monster;
    public PartDB Projectile;

    private void Start()
    {
        Monster = GetDB("Monster");
        //Projectile = GetDB("Projectile");
    }

    private PartDB GetDB(string path)
    {
        return Resources.Load<PartDB>("DB/" + path);
    }
}
