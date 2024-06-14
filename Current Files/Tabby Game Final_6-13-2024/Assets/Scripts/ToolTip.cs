using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTip : MonoBehaviour
{
    public List<string> messages = new List<string>();

    private void OnMouseEnter()
    {
        // Combine all messages into a single string, separated by new lines
        string combinedMessage = string.Join("\n", messages);
        TooltipManager._instance.SetAndShowToolTip(combinedMessage);
    }

    private void OnMouseExit()
    {
        TooltipManager._instance.HideToolTip();
    }
}
