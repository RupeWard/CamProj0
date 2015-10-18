using UnityEngine;
using System.Collections;

public class SceneControllerTest : MonoBehaviour
{
    public Transform logPanelFront;
    public Transform logPanelBack;

    public RectTransform logPanelRT;

    public void Start()
    {
        logPanelRT.SetParent(logPanelBack);
    }

    public void OnLogPanelToggle()
    {
        if (logPanelRT.parent == logPanelBack)
        {
            logPanelRT.SetParent(logPanelFront);
        }
        else
        {
            logPanelRT.SetParent(logPanelBack);
        }
    }
}
