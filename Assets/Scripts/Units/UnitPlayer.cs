using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayer : UnitBase
{
    private readonly List<ItemContainer> _itemsList = new();

    private protected override void Awake()
    {
        EventsBus.Subscribe<OnEnterItemPickableZone>(OnEnterItemPickableZone);
        EventsBus.Subscribe<OnLeaveItemPickableZone>(OnLeaveItemPickableZone);
        base.Awake();
    }

    private protected override void OnDestroy()
    {
        EventsBus.Unsubscribe<OnEnterItemPickableZone>(OnEnterItemPickableZone);
        EventsBus.Unsubscribe<OnLeaveItemPickableZone>(OnLeaveItemPickableZone);
        base.OnDestroy();
    }

    private void OnEnterItemPickableZone(OnEnterItemPickableZone data)
    {
        if (data.Player != gameObject)
            return;

        _itemsList.Add(data.Container);
    }

    private void OnLeaveItemPickableZone(OnLeaveItemPickableZone data)
    {
        if (data.Player != gameObject)
            return;

        _itemsList.Remove(data.Container);
    }

    public void TryPickUpItem()
    {
        if (_itemsList.Count == 0)
            return;

        var container = _itemsList[0];
        if (container.Item == null && string.IsNullOrEmpty(container.Item.Id))
            return;
        EventsBus.Publish<OnAddItem>(new OnAddItem { Item = container.Item, Obtainer = this});
        Destroy(container.gameObject);
    }
}
