using System.Collections.Generic;
using UnityEngine;

public class EntityActionCollector : MonoBehaviour
{
    [SerializeField] private Transform parentTransformWithActions;

    private List<EntityAction> allAction = new List<EntityAction>();

    private void Awake()
    {
        for (int i = 0; i < parentTransformWithActions.childCount; i++)
        {
            if (parentTransformWithActions.GetChild(i).gameObject.activeSelf == true)
            {
                EntityAction action = parentTransformWithActions.GetChild(i).GetComponent<EntityAction>();
                if (action != null)
                { 
                    allAction.Add(action);
                }
            }
        }
    }

    public T GetAction<T>() where T : EntityAction
    {

        for (int i = 0; i < allAction.Count; i++)
        {
            if (allAction[i] is T)
                return (T)allAction[i];
        }

        return null;
    }

    public List<T> GetActionList<T>() where T : EntityAction
    {
        List<T> actions = new List<T>();

        for (int i = 0; i < allAction.Count; i++)
        {
            if (allAction[i] is T)
                actions.Add((T)allAction[i]);
        }

        return actions;
    }
}
