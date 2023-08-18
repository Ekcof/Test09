using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Game Data", menuName = "Assets/ItemStore")]
public class ItemHolder : ScriptableObject
{
    [SerializeField] private Uniform[] _uniforms;
    [SerializeField] private Helmet[] _helmets;
    [SerializeField] private DisposableItem[] _disposables;


    /// <summary>
    /// Make a new Item Object with the respect to it's subclass
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemBase CloneTheItem(string id)
    {
        ItemBase item = null;

        switch (id)
        {
            case var uniformId when _uniforms.Any(u => u.Id == uniformId):
                item = _uniforms.FirstOrDefault(u => u.Id == uniformId);
                break;

            case var helmetId when _helmets.Any(h => h.Id == helmetId):
                item = _helmets.FirstOrDefault(h => h.Id == helmetId);
                break;

            case var disposableId when _disposables.Any(d => d.Id == disposableId):
                item = _disposables.FirstOrDefault(d => d.Id == disposableId);
                break;
            default: Debug.Log($"________No item with Id {id} in item holder"); break;
        }

        return item;
    }

    private Uniform CloneTheUniform(Uniform oldItem)
    {
        Uniform newItem = null;
        if (oldItem != null)
        {
            newItem = new Uniform(
                oldItem.Id,
                oldItem.Name,
                oldItem.Description,
                oldItem.IconId,
                oldItem.BasePrice,
                oldItem.Amount,
                oldItem.IsStackable,
                oldItem.Animator,
                oldItem.ArmourPoints);
        }
        return newItem;
    }

    private Helmet CloneTheHelmet(Helmet oldItem)
    {
        Helmet newItem = null;
        if (oldItem != null)
        {
            newItem = new Helmet(
                oldItem.Id,
                oldItem.Name,
                oldItem.Description,
                oldItem.IconId,
                oldItem.BasePrice,
                oldItem.Amount,
                oldItem.IsStackable,
                oldItem.Animator,
                oldItem.ArmourPoints);
        }
        return newItem;
    }

    private DisposableItem CloneTheDisposableItem(string id)
    {
        DisposableItem newItem = null;

        DisposableItem oldItem = _disposables.FirstOrDefault(s => s.Id == id);
        if (oldItem != null)
        {
            newItem = new DisposableItem(
                oldItem.Id,
                oldItem.Name,
                oldItem.Description,
                oldItem.IconId,
                oldItem.BasePrice,
                oldItem.Amount,
                oldItem.IsStackable,
                oldItem.Effect,
                oldItem.Value);
        }
        return newItem;
    }
}
