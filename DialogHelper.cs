using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHelper : MonoBehaviour
{
    GameObject[] npc;

    private void Start()
    {
        npc = GameObject.FindGameObjectsWithTag("NPC");
    }

    public void ResponseChosen(int val)
    {
        GetInteractedWithDialogHandler().ChosenReplyContinue(val);
    }

    DialogueMe GetInteractedWithDialogHandler()
    {
        foreach (var O in npc)
        {
            var h = O.GetComponent<DialogueMe>();
            if (h != null && h.ongoingDialogue == true) 
                return h;
        }
        Debug.Log("Couldn't find matching DialogueHandler component");
        return null;
    }
}
