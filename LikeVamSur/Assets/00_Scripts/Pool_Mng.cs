using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public interface IPool
{
    Transform parentTransform { get; set; }
    Queue<GameObject> pool { get; set; }
    GameObject Get(Action<GameObject> action = null);

    void Return(GameObject obj, Action<GameObject> action = null);

}

public class Object_Pool : IPool
{
    public Transform parentTransform { get ; set ; }
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();
    //Queue -> FiFO -> 선입선출
    //Dequeue -> 먼저 들어온 오브젝트를 내보낸다.
    //EnQueue -> 오브젝트를 Queue 내부에 집어넣는다.

    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);

        if(action != null)
        {
            action?.Invoke(obj);
        }
        return obj; 
    }

    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);
        obj.transform.parent = parentTransform;
        obj.SetActive(false);
        if(action != null)
        {
            action?.Invoke(obj);
        }
    }
}
public class Pool_Mng : MonoBehaviour
{
    public Dictionary<string,IPool> m_pool_Dictionary = new Dictionary<string, IPool> ();

    Transform base_Obj = null;

    private void Start()
    {
        base_Obj = this.transform;   
    }


    public IPool Pooling_OBJ(string path)
    {
        if (m_pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }
        
        if(m_pool_Dictionary[path].pool.Count <= 0)
        {
            Add_Queue(path);
        }

        return m_pool_Dictionary[path];

    }


    GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject(path + "##POOL");
        //obj.transform.parent = base_Obj;
        obj.transform.SetParent(base_Obj);
        Object_Pool T_Pool = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Pool);
        T_Pool.parentTransform = obj.transform;
        return obj;
    }


    void Add_Queue(string path)
    {
        var obj = Instantiate(Resources.Load<GameObject>("POOL/" + path));

        m_pool_Dictionary[path].Return(obj);
    }
}
