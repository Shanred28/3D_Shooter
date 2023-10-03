using System.Collections.Generic;
using UnityEngine;

public class ContextActionInputControl : MonoBehaviour
{

    [SerializeField] private EntityActionCollector targetActionCollector;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            List<EntityContextAction> actionsList = targetActionCollector.GetActionList<EntityContextAction>();

            for (int i = 0; i < actionsList.Count; i++)
            {
                //characterMovement.PreapreAction(actionsList[i]);
                actionsList[i].StartAction();
                actionsList[i].EndAction();
            }     
        }
    }
}
