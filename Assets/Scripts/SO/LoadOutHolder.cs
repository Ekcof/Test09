using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadOutHolder", menuName = "Assets/LoadOutHolder")]
public class LoadOutHolder : ScriptableObject
{
    [SerializeField] private LoadOut[] _loadouts;

    public LoadOut GetLoadOutById(string id)
    {
        var loadout = _loadouts.FirstOrDefault(u => u.Id == id);

        return loadout;
    }
}
