using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayer : UnitBase
{
    private readonly List<ItemContainer> _containersList = new();
    public int ContainersNum => _containersList.Count;
    private Trader _currentTrader;
    public Trader Trader => _currentTrader;

    private protected override void Awake()
    {
        EventsBus.Subscribe<OnEnterItemPickableZone>(OnEnterItemPickableZone);
        EventsBus.Subscribe<OnLeaveItemPickableZone>(OnLeaveItemPickableZone);
        EventsBus.Subscribe<OnEnterTraderZone>(OnEnterTraderZone);
        EventsBus.Subscribe<OnLeaveTraderZone>(OnLeaveTraderZone);
        base.Awake();
    }

    private protected override void OnDestroy()
    {
        EventsBus.Unsubscribe<OnEnterItemPickableZone>(OnEnterItemPickableZone);
        EventsBus.Unsubscribe<OnLeaveItemPickableZone>(OnLeaveItemPickableZone);
        EventsBus.Unsubscribe<OnEnterTraderZone>(OnEnterTraderZone);
        EventsBus.Unsubscribe<OnLeaveTraderZone>(OnLeaveTraderZone);
        base.OnDestroy();
    }

    private void OnEnterItemPickableZone(OnEnterItemPickableZone data)
    {
        if (data.Player != gameObject)
            return;
        _containersList.Add(data.Container);
        EventsBus.Publish<OnTogglePickableZone>(new OnTogglePickableZone { IsCanPick = true });
    }

    private void OnLeaveItemPickableZone(OnLeaveItemPickableZone data)
    {
        if (data.Player != gameObject)
            return;
        _containersList.Remove(data.Container);
        EventsBus.Publish<OnTogglePickableZone>(new OnTogglePickableZone { Player = this, IsCanPick = (_containersList.Count > 0) });
    }

    public bool TryPickUpItem()
    {
        if (_containersList.Count == 0)
            return false;

        var container = _containersList[0];
        if (container.Item == null && string.IsNullOrEmpty(container.Item.Id))
            return false;
        EventsBus.Publish<OnAddItem>(new OnAddItem { Item = container.Item, Obtainer = this});
        Destroy(container.gameObject);
        return true;
    }

    private protected override void AddMoney(int money)
    {
        base.AddMoney(money);
        EventsBus.Publish(new OnChangeMoneyAmount { Money = _money });
    }

    private void OnEnterTraderZone(OnEnterTraderZone data)
    {
        if (data.Player != this)
            return;

        _currentTrader =  data.Trader;
    }

    private void OnLeaveTraderZone(OnLeaveTraderZone data)
    {
        if (data.Player != this)
            return;

        _currentTrader = null;
    }

}
