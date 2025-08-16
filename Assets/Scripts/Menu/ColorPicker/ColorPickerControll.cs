using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class SliderTextPair
{
    public Slider slider;
    public InputField sliderText;
}
public class ColorPickerControll : MonoBehaviour
{
    public float currenrtHue = 0f;
    public float currentSaturation = 0f;
    public float currentValue = 0f;
    public ColorData colorData;
    [SerializeField] private Image hueImage, saturationImage, outputImage;
    [SerializeField] private Slider hueSlider;
    [SerializeField] private InputField hexText;
    private Texture2D hueTexture, saturationTexture, outputTexture;
    [SerializeField] private MeshRenderer changeThisColor;
    public List<SliderTextPair> sliderTextPairs;

    void Start()
    {
        colorData = Resources.Load<ColorData>("ColorData");
        CreateHueImage();
        CreateSaturationImage();

        if (colorData != null)
        {
            Color.RGBToHSV(colorData.currrentColor, out currenrtHue, out currentSaturation, out currentValue);
        }
        else
        {
            currenrtHue = 0f;
            currentSaturation = 0.8f;
            currentValue = 0.8f;
        }
        CreateOutputImage();
        UpdateOutputImage();
        UpdatePickerPosition();

        foreach (SliderTextPair pair in sliderTextPairs)
        {
            pair.sliderText.text = Mathf.RoundToInt(pair.slider.value).ToString();
            if (pair.slider != null && pair.sliderText != null)
            {
                pair.slider.onValueChanged.AddListener((val) =>
                {
                    pair.sliderText.text = Mathf.RoundToInt(val * 255).ToString();
                    UpdateColorFromRGB();
                });


            }
        }
    }
    private void CreateHueImage()
    {
        hueTexture = new Texture2D(1, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";
        for (int i = 0; i < hueTexture.height; i++)
        {
            Color color = Color.HSVToRGB((float)i / hueTexture.height, 1f, 0.95f);
            hueTexture.SetPixel(0, i, color);
        }
        hueTexture.Apply();
        currenrtHue = 0f;
        hueImage.sprite = Sprite.Create(hueTexture, new Rect(0, 0, hueTexture.width, hueTexture.height), new Vector2(0.5f, 0.5f));
    }

    private void CreateSaturationImage()
    {
        saturationTexture = new Texture2D(16, 16);
        saturationTexture.wrapMode = TextureWrapMode.Clamp;
        saturationTexture.name = "SaturationTexture";
        for (int x = 0; x < saturationTexture.width; x++)
        {
            for (int y = 0; y < saturationTexture.height; y++)
            {
                Color color = Color.HSVToRGB(currenrtHue, (float)x / saturationTexture.width, (float)y / saturationTexture.height);
                saturationTexture.SetPixel(x, y, color);
            }
        }
        saturationTexture.Apply();
        currentSaturation = 0f;
        currentValue = 0f;
        saturationImage.sprite = Sprite.Create(saturationTexture, new Rect(0, 0, saturationTexture.width, saturationTexture.height), new Vector2(0.5f, 0.5f));
    }

    private void CreateOutputImage()
    {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";
        colorData.currrentColor = Color.HSVToRGB(currenrtHue, currentSaturation, currentValue);
        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, colorData.currrentColor);
        }
        outputTexture.Apply();
        outputImage.sprite = Sprite.Create(outputTexture, new Rect(0, 0, outputTexture.width, outputTexture.height), new Vector2(0.5f, 0.5f));
    }

    private void UpdateOutputImage()
    {

        colorData.currrentColor = Color.HSVToRGB(currenrtHue, currentSaturation, currentValue);
        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, colorData.currrentColor);
        }
        outputTexture.Apply();
        hexText.text = ColorUtility.ToHtmlStringRGB(colorData.currrentColor);

    }
    public void SetSV(float saturation, float value)
    {
        currentSaturation = saturation;
        currentValue = value;
        UpdateOutputImage();
        UpdateRGB();
    }
    public void UpdateSVImage()
    {
        currenrtHue = hueSlider.value;
        for (int y = 0; y < saturationTexture.height; y++)
        {
            for (int x = 0; x < saturationTexture.width; x++)
            {
                Color color = Color.HSVToRGB(currenrtHue, (float)x / saturationTexture.width, (float)y / saturationTexture.height);
                saturationTexture.SetPixel(x, y, color);
            }
        }

        saturationTexture.Apply();
        UpdateOutputImage();
        UpdateRGB();
    }
    public void OnTextInput()
    {
        if (hexText.text.Length < 6)
        {
            return;
        }
        Color newColor;
        if (ColorUtility.TryParseHtmlString("#" + hexText.text, out newColor))
        {
            Color.RGBToHSV(newColor, out currenrtHue, out currentSaturation, out currentValue);
        }
        else
        {
            Debug.LogError("Invalid hex color code: " + hexText.text);
            return;

        }
        hueSlider.value = currenrtHue;
        hexText.text = "";
        UpdateOutputImage();
        UpdatePickerPosition();
        UpdateRGB();
    }
    public void UpdatePickerPosition()
    {
        RectTransform svRect = saturationImage.GetComponent<RectTransform>();
        float width = svRect.sizeDelta.x;
        float height = svRect.sizeDelta.y;

        // Обчислюємо локальні координати курсору
        float x = currentSaturation * width - width / 2f;
        float y = currentValue * height - height / 2f;

        // Передаємо їх у SVImageControll
        FindFirstObjectByType<SVImageControll>().SetPickerPosition(new Vector2(x, y));
    }

    private void UpdateRGB()
    {
        Color color = colorData.currrentColor;
        float r = color.r;
        float g = color.g;
        float b = color.b;

        foreach (SliderTextPair pair in sliderTextPairs)
        {
            if (pair.slider != null && pair.sliderText != null)
            {
                string sliderName = pair.slider.name.ToLower();

                if (sliderName.Contains("red"))
                {
                    pair.slider.SetValueWithoutNotify(r);
                    pair.sliderText.text = Mathf.RoundToInt(color.r * 255).ToString();
                }
                else if (sliderName.Contains("g"))
                {
                    pair.slider.SetValueWithoutNotify(g);
                    pair.sliderText.text = Mathf.RoundToInt(color.g * 255).ToString();
                }
                else if (sliderName.Contains("b"))
                {
                    pair.slider.SetValueWithoutNotify(b);
                    pair.sliderText.text = Mathf.RoundToInt(color.b * 255).ToString();
                }
            }
        }
    }
    void UpdateColorFromRGB()
    {
        float r = 0f, g = 0f, b = 0f;
        foreach (SliderTextPair pair in sliderTextPairs)
        {
            if (pair.slider != null && pair.sliderText != null)
            {
                string sliderName = pair.slider.name.ToLower();

                if (sliderName.Contains("red"))
                {
                    r = pair.slider.value;
                }
                else if (sliderName.Contains("g"))
                {
                    g = pair.slider.value;
                }
                else if (sliderName.Contains("b"))
                {
                    b = pair.slider.value;
                }
            }
        }
        Color.RGBToHSV(new Color(r, g, b), out currenrtHue, out currentSaturation, out currentValue);
        UpdateOutputImage();
        UpdatePickerPosition();
        hueSlider.value = currenrtHue;
        hexText.text = "";
    }

}
