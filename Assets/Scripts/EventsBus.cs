using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the events for interaction among classes
/// </summary>
public static class EventsBus
{
    private static readonly Dictionary<Type, List<Action<object>>> eventSubscriptions = new Dictionary<Type, List<Action<object>>>();

    public static void Subscribe<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);

        if (!eventSubscriptions.ContainsKey(eventType))
        {
            eventSubscriptions[eventType] = new List<Action<object>>();
        }

        eventSubscriptions[eventType].Add(obj => eventHandler((T)obj));
    }

    public static void Unsubscribe<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);

        if (eventSubscriptions.TryGetValue(eventType, out var handlers))
        {
            handlers.RemoveAll(obj => obj.Equals(eventHandler));
        }
    }

    public static void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);

        if (eventSubscriptions.TryGetValue(eventType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                handler.Invoke(eventData);
            }
        }
        else
        {
            Debug.LogWarning($"Event {eventType.Name} is not subscribed to in the EventBus.");
        }
    }
}

public class OnStartDialogue
{
    public UnitBase NPC;
}

public class OnEndDialogue
{
    public UnitBase NPC;
}

public class OnStartTrade
{
    public UnitBase Merchant;
}

public class OnGetPayment
{
    public UnitBase Payer;
    public int Money;
    public UnitBase Payee;
}

public class OnOpenWindow
{
    public UnitBase Player;
    public UnitBase Counterpart;
    public BaseWindow Window;
}

public class OnSelectSlot
{
    public ItemSlot Slot;
    public UnitBase Owner;
}

public class OnDropItem
{
    public UnitBase Owner;
    public ItemBase Item;
    public bool IsDroppedDown;
}

public class OnAddItem
{
    public UnitBase Obtainer;
    public ItemBase Item;
}

public class OnTradeItem
{
    public UnitBase Buyer;
    public UnitBase Seller;
    public ItemBase Item;
    public int Price;
    public bool IsSellingAll;
}

public class OnEnterItemPickableZone
{
    public GameObject Player;
    public ItemContainer Container;
}

public class OnLeaveItemPickableZone
{
    public GameObject Player;
    public ItemContainer Container;
}