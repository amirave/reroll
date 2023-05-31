
using UnityEngine;

public class ButtonHighlight : MonoBehaviour
{
    public void OnHighlight()
    {
        PauseMenuManager.Instance.OnHighlighted();
    }
}