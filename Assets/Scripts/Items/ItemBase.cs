using System;
using UnityEngine;


[Serializable]
public class ItemBase
{
    [SerializeField] private protected string _id;
    [SerializeField] private protected string _name;
    [SerializeField] private protected string _description;
    [SerializeField] private protected string _iconId;
    [SerializeField] private protected int _basePrice;
    [SerializeField] private protected int _amount = 1;
    [SerializeField] private protected bool _isStackable;
    public string Id => _id;
    public string Name => _name;
    public string Description => _description;
    public string IconId => _iconId;
    public int BasePrice => _basePrice;
    public int Amount => _amount;
    public bool IsStackable => _isStackable;

    public ItemBase(string id, string name, string description, string iconId, int basePrice, int amount, bool isStackable)
    {
        _id = id;
        _name = name;
        _description = description;
        _iconId = iconId;
        _basePrice = basePrice;
        _amount = amount;
        _isStackable = isStackable;
    }

    public void ChangeAmount(int amount)
    {
        _amount = amount;
        if (_amount < 0)
            amount = 0;
    }

}
