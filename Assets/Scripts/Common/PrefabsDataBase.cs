using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu]
public class PrefabsDataBase : ScriptableObject
{
    public Entity _playerPref;
    public List<Entity> AllPrefabs;

    public GameObject CreateEntityFromId(long id)
    {
        foreach (var entity in AllPrefabs)
        {
            if((entity is ISerializableEntity) == false) continue; 

            if ((entity as ISerializableEntity).EntityId == id)
            { 
              return  Instantiate(entity.gameObject);
            }
        }
        return null;
    }
    public bool IsPlayerId(long id)
    {
        return id == (_playerPref as ISerializableEntity).EntityId;
    }

    public GameObject CreatePlayer()
    {
        return Instantiate(_playerPref.gameObject);
    }
}
