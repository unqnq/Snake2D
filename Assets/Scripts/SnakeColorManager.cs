using UnityEngine;

public static class SnakeColorManager
{
    private const string COLOR_R = "SnakeColor_R";
    private const string COLOR_G = "SnakeColor_G";
    private const string COLOR_B = "SnakeColor_B";

    public static void SaveColor(Color color)
    {
        PlayerPrefs.SetFloat(COLOR_R, color.r);
        PlayerPrefs.SetFloat(COLOR_G, color.g);
        PlayerPrefs.SetFloat(COLOR_B, color.b);
        PlayerPrefs.Save();
    }

    public static Color LoadColor(Color defaultColor)
    {
        if (!PlayerPrefs.HasKey(COLOR_R) ||
            !PlayerPrefs.HasKey(COLOR_G) ||
            !PlayerPrefs.HasKey(COLOR_B))
        {
            return Color.red;
        }
        float r = PlayerPrefs.GetFloat(COLOR_R, defaultColor.r);
        float g = PlayerPrefs.GetFloat(COLOR_G, defaultColor.g);
        float b = PlayerPrefs.GetFloat(COLOR_B, defaultColor.b);
        return new Color(r, g, b);
    }
}

