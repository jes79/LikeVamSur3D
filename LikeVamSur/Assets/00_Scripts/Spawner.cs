using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public Vector3 centerPoint = Vector3.zero;
    public float spawnRadius = 30.0f;
    public GameObject monsterPrefab;
    public Transform player;
    private float spawnInterval;

    public float timer;


    private void Start()
    {
        spawnInterval = MANAGER.DB.levelDesign.MonsterSpawnRate;
        MANAGER.SESSION.onBossTime += SpawnBossMonstser;
    }

    private void OnDestroy()
    {
        MANAGER.SESSION.onBossTime -= SpawnBossMonstser;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnInterval)
        {
            timer = 0;
            SpawnMosterAtEdge();
        }
    }
    void SpawnBossMonstser()
    {
        SpawnMosterAtEdge("Skeleton_Boss");
    }

    void SpawnMosterAtEdge(string id = "")
    {
        //Vector3 spawnPos = GetRandomPointOnCircleEdge(centerPoint, spawnRadius);
        Vector3 spawnPos = GetRandomPointOnCircleEdge(player.position, spawnRadius);

        //GameObject monster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        //monster.GetComponent<Monster_Movement>().SetTarget(player);
        //monster.GetComponent<Monster_Movement>().Initialize(player);

        var monster = MANAGER.POOL.Pooling_OBJ("Monster").Get((value) =>
        {
            value.transform.position = spawnPos;
            value.GetComponent<MONSTER>().Initialize(player, string.IsNullOrEmpty(id) ? "Skeleton_01" : id);
        });
    }

    Vector3 GetRandomPointOnCircleEdge(Vector3 center, float radius)
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2f);
        float x = Mathf.Cos(angle)*radius; 
        float z = Mathf.Sin(angle)*radius;

        return new Vector3(center.x +x , center.y, center.z+ z);
    }

 
}
