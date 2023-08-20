using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _spriteAtlas;
    [SerializeField] private ItemHolder _itemHolder;
    [SerializeField] private LoadOutHolder _loadoutHolder;
    [SerializeField] private GameObject _droppedItemPrefab;

    public GameObject DroppedItemPrefab => _droppedItemPrefab;

    private static ResourceManager _instance;


    public static ResourceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ResourceManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Sprite GetSpriteByID(string id)
    {
        return _spriteAtlas.GetSprite(id);
    }

    public ItemBase CreateNewItem(string id, int amount = 1)
    {
        var item = _itemHolder.CloneTheItem(id);
        if (item == null)
        {
            Debug.Log("______Failed to create new item");
            return null;
        }

        item.SetAmount(item.IsStackable ? amount : 1);
        return item;

    }

    public (List<ItemBase>, Helmet, Uniform) GetLoadOut(string id)
    {
        var loadout = _loadoutHolder.GetLoadOutById(id);
        if (loadout == null)
            return (null, null, null);

        var items = new List<ItemBase>();
        foreach (var item in loadout.ItemDatas)
        {
            if (item != null && !string.IsNullOrEmpty(item.Id) && item.Amount > 0)
            {
                items.Add(CreateNewItem(item.Id, item.Amount));
            }
        }

        var helmet = CreateNewItem(loadout.HelmetId);
        var uniform = CreateNewItem(loadout.UniformId);

        return (items, (helmet is Helmet newHelmet ? newHelmet : null),(uniform is Uniform newUniform ? newUniform : null));
    }

}
