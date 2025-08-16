using UnityEngine;

public class ColorPickerUI : MonoBehaviour
{
    public GameObject panelPickColor;

    void Start()
    {
        panelPickColor = GameObject.Find("ColorPickerPanel");
        panelPickColor.SetActive(false);
    }
    public void PickColor()
    {
        panelPickColor.SetActive(true);
    }

    public void saveColor()
    {
        panelPickColor.SetActive(false);
    }
}
