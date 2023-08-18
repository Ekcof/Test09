using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _spriteAtlas;
    [SerializeField] private ItemHolder _itemHolder;
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

        item.ChangeAmount(amount);
        return item;

    }

}
