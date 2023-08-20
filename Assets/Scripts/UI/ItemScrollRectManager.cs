using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class ItemScrollRectManager : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _slotPrefab;

    private float _slotSizeDeltaY;

    private readonly List<ItemSlot> _slots = new List<ItemSlot>();

    private void Awake()
    {
        _slotSizeDeltaY = ((RectTransform)_slotPrefab.transform).sizeDelta.y;
    }

    public void FillScrollRect(UnitBase owner)
    {
        if (_slots != null && _slots.Count > 0)
        ClearInventory();

        if (owner == null)
            return;

        var items = owner.Items;


        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null || string.IsNullOrEmpty(items[i].Id))
                continue;

            GameObject slotGO = Instantiate(_slotPrefab, _content);
            ItemSlot slot = slotGO.GetComponent<ItemSlot>();
            slot.Initialize(items[i], owner);

            // Position slots based on the current index
            slotGO.transform.localPosition = new Vector3(
                slotGO.transform.localPosition.x,
                -_slotSizeDeltaY * i,
                slotGO.transform.localPosition.z
            );

            _slots.Add(slot); // Add the new slot to the list
        }

        _content.sizeDelta = new Vector2(_content.sizeDelta.x, _slotSizeDeltaY * items.Count / 2);
        DOTween.Kill(_content);
        _scrollRect.DOVerticalNormalizedPos(1f, 0.3f);
    }

    public void ClearInventory()
    {
        foreach (var slot in _slots)
        {
            Destroy(slot.gameObject);
        }

        _slots.Clear();
    }
}