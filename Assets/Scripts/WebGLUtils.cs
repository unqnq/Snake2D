using System.Runtime.InteropServices;

public static class WebGLUtils
{
    [DllImport("__Internal")]
    private static extern int IsMobileBrowser();

    public static bool IsMobile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsMobileBrowser() == 1;
#else
        return false;
#endif
    }
}

