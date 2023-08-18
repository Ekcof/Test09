using System;
using UnityEngine;

[Serializable]
public class DisposableItem : ItemBase
{
    [SerializeField] private DisposableEffect _effect;
    [SerializeField] private int _value;
    public DisposableEffect Effect => _effect;
    public int Value => _value;

    public DisposableItem(string id, string name, string description, string iconId, int basePrice, int amount, bool isStackable, DisposableEffect effect, int value) : base(id, name, description, iconId, basePrice, amount, isStackable)
    {
        _effect = effect;
        _value = value;
    }
}

//Possible effects for disposable item
public enum DisposableEffect
{
    None,
    Feed,
    Drink,
    Heal
}