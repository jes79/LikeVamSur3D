using UnityEngine;

public class Item : MonoBehaviour
{
    public string ItemId;
    private IFactory<Item> factory;
    private IItemEffect itemEffect;

    public void Initialize(string id)
    {
        ItemId = id;
        if(factory == null)
        {
            factory = new GenericPartFactory<Item>(MANAGER.DB.Item);
        }

        factory.Build(this, id);

        itemEffect = GetComponentInChildren<IItemEffect>();
        itemEffect.Initialize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemEffect.OnPickUp(other.gameObject); //1인게임이라 owner가 필요없긴 함..
            MANAGER.POOL.m_pool_Dictionary["Item"].Return(this.gameObject);
        }
    }

}
