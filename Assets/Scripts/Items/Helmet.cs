using System;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public class Helmet : ItemBase
{
    [SerializeField] private AnimatorController _animator;
    [SerializeField] private int _armourPoints;
    public int ArmourPoints => _armourPoints;
    public AnimatorController Animator => _animator;

    public Helmet(string id, string name, string description, string iconId, int basePrice, int amount, bool isStackable, AnimatorController animator, int armourPoints)
        : base(id, name, description, iconId, basePrice, amount, isStackable)
    {
        _animator = animator;
        _armourPoints = armourPoints;
    }
}
