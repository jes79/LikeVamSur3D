using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public Vector3 centerPoint = Vector3.zero;
    public float spawnRadius = 30.0f;
    public GameObject monsterPrefab;
    public Transform player;
    public float spawnInterval = 1.0f;

    public float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnInterval)
        {
            timer = 0;
            SpawnMosterAtEdge();
        }
    }

    void SpawnMosterAtEdge()
    {
        //Vector3 spawnPos = GetRandomPointOnCircleEdge(centerPoint, spawnRadius);
        Vector3 spawnPos = GetRandomPointOnCircleEdge(player.position, spawnRadius);

        //GameObject monster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        //monster.GetComponent<Monster_Movement>().SetTarget(player);
        //monster.GetComponent<Monster_Movement>().Initialize(player);

        var monster = MANAGER.POOL.Pooling_OBJ("Monster").Get((value) =>
        {
            value.transform.position = spawnPos;
            value.GetComponent<MONSTER>().Initialize(player);
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
