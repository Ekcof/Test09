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
}
