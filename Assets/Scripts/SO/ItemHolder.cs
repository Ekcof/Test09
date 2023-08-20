using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Game Data", menuName = "Assets/ItemStore")]
public class ItemHolder : ScriptableObject
{
    [SerializeField] private Uniform[] _uniforms;
    [SerializeField] private Helmet[] _helmets;
    [SerializeField] private DisposableItem[] _disposables;


    /// <summary>
    /// Create a new Item Object with the respect to it's subclass
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemBase CloneTheItem(string id)
    {
        var uniform = _uniforms.FirstOrDefault(u => u.Id == id);
        if (uniform != null)
        {
            return CloneTheUniform(uniform);
        }

        var helmet = _helmets.FirstOrDefault(h => h.Id == id);
        if (helmet != null)
        {
            return CloneTheHelmet(helmet);
        }

        var disposable = _disposables.FirstOrDefault(d => d.Id == id);
        if (disposable != null)
        {
            return CloneTheDisposableItem(disposable);
        }

        return null;
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

    private DisposableItem CloneTheDisposableItem(DisposableItem oldItem)
    {
        DisposableItem newItem = null;

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
