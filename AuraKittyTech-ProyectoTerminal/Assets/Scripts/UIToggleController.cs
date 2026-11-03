using UnityEngine;

public class UIToggleController : MonoBehaviour
{
    public GameObject targetPanel;

    public void TogglePanel()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(!targetPanel.activeSelf);
        }
    }
}
