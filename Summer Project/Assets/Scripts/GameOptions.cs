using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameOptions
{
    public static string Version = "v0.1 beta";

    public static float TotalSoundVolume = 1.0f;
    public static float BGMSoundVolume = 1.0f;
    public static float SFXSoundVolume = 1.0f;

    public static readonly Vector2[] ScreenResolutionsOptions = new Vector2[] {
        new Vector2(800, 480),
        new Vector2(1024, 768),
        new Vector2(1280, 720),
        new Vector2(1920, 1080),
        new Vector2(2160, 1080),
        new Vector2(2560, 1440),
        new Vector2(3840, 2160),
    };

    public static int ScreenResolutionOptionIndex = 1;
}
