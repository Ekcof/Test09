using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _itemImage;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _id;
    [SerializeField] private Collider2D _collider;
    private ItemBase _item;
    //TODO: to make an array of items instead of single item

    public ItemBase Item => _item;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(_id))
            Initialize();
    }

    private void Initialize(string id = "", int amount = 1)
    {
        if (string.IsNullOrEmpty(_id) && !string.IsNullOrEmpty(id))
            _id = id;
        
        _item = ResourceManager.Instance.CreateNewItem(_id, amount);

        // If there is no preset animation or image it sets the image for item from sprite atlas
        if (_itemImage != null && _itemImage.sprite == null && _animator == null)
            _itemImage.sprite = ResourceManager.Instance.GetSpriteByID(_item.IconId);
    }

    public void SetItem(ItemBase item)
    {
        _item = item;
        if (_itemImage.sprite == null && _animator == null)
            _itemImage.sprite = ResourceManager.Instance.GetSpriteByID(_item.IconId);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventsBus.Publish(new OnEnterItemPickableZone { Player = collision.gameObject, Container = this });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventsBus.Publish(new OnLeaveItemPickableZone { Player = collision.gameObject, Container = this });
        }
    }

}
