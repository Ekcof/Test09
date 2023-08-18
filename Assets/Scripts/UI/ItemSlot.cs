using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image _mainBody;
    [SerializeField] private Image _logo;
    [SerializeField] private TMPro.TextMeshProUGUI _title;
    [SerializeField] private TMPro.TextMeshProUGUI _price;
    [SerializeField] private TMPro.TextMeshProUGUI _amount;
    [SerializeField] private Button _button;
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private Color _selectedColor;
    private ItemBase _item;

    public Image MainBody => _mainBody;
    public ItemBase Item => _item;

    private void Awake()
    {
        EventsBus.Subscribe<OnSelectSlot>(OnSelectSlot);
    }

    public void Initialize(ItemBase item)
    {
        if (item == null)
        {
            Debug.Log("________Error: item is null");
            return;
        }
        _item = item;
        _logo.sprite = ResourceManager.Instance.GetSpriteByID(item.Id);
        _title.text = item.Name;
        _price.text = $"${item.BasePrice}";
        _amount.text = item.Amount > 1 ? $"x{item.Amount}" : string.Empty;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => EventsBus.Publish(new OnSelectSlot { Slot = this }));
    }

    private void OnSelectSlot(OnSelectSlot data)
    {
        if (data.Slot != this)
        {
            Debug.Log($"_______Set the default color for {_title} slot");
            if (data.Slot != null)
                if (_mainBody != null)
                    _mainBody.color = _unselectedColor;
            return;
        }

        _mainBody.color = _selectedColor;
    }

    public void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
        EventsBus.Unsubscribe<OnSelectSlot>(OnSelectSlot);
    }
}
