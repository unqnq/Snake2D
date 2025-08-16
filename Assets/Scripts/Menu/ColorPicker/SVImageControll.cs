using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SVImageControll : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] Image pickerImage;
    private Image SVImage;
    private ColorPickerControll colorPickerControll;
    private RectTransform rectTransform, pickerTransform;

    void Awake()
    {
        SVImage = GetComponent<Image>();
        colorPickerControll = FindFirstObjectByType<ColorPickerControll>();
        rectTransform = GetComponent<RectTransform>();
        pickerTransform = pickerImage.GetComponent<RectTransform>();
    }

    void UpdateColor(PointerEventData eventData)
    {
        Vector3 pos = rectTransform.InverseTransformPoint(eventData.position);
        float deltaX = rectTransform.sizeDelta.x*0.5f;
        float deltaY = rectTransform.sizeDelta.y*0.5f;
        if (pos.x < -deltaX) 
        {
            pos.x = -deltaX;
        }
        else if (pos.x > deltaX) 
        {
            pos.x = deltaX; 
        }
        if (pos.y < -deltaY) 
        {
            pos.y = -deltaY; 
        }
        else if (pos.y > deltaY) 
        {
            pos.y = deltaY; 
        }

        float x = pos.x + deltaX;
        float y = pos.y + deltaY;

        float xNormalized = x / rectTransform.sizeDelta.x;
        float yNormalized = y / rectTransform.sizeDelta.y;
        
        pickerTransform.localPosition = pos;
        pickerImage.color = Color.HSVToRGB(0,0, 1-yNormalized);

        colorPickerControll.SetSV(xNormalized, yNormalized);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }
    public void SetPickerPosition(Vector2 localPos)
    {
        // Обмеження (запобігає виходу за межі)
        float halfX = rectTransform.sizeDelta.x / 2f;
        float halfY = rectTransform.sizeDelta.y / 2f;

        localPos.x = Mathf.Clamp(localPos.x, -halfX, halfX);
        localPos.y = Mathf.Clamp(localPos.y, -halfY, halfY);

        pickerTransform.localPosition = localPos;
    }

}
