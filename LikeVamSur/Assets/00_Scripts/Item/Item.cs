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
    }

}
