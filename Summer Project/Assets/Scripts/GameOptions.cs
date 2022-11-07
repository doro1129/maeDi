using System;
using UnityEngine;

public static class GameOptions
{
    public static event Action ChangeEvent;

    public static readonly string Version = "v0.1 beta";

    public static float TotalSoundVolume
    {
        get => GameOptions.totalSoundVolume;

        set
        {
            GameOptions.totalSoundVolume = value;
            GameOptions.ChangeEvent();
        }
    }

    public static float BGMSoundVolume
    {
        get => GameOptions.bgmSoundVolume;

        set
        {
            GameOptions.bgmSoundVolume = value;
            GameOptions.ChangeEvent();
        }
    }
    public static float SFXSoundVolume
    {
        get => GameOptions.sfxSoundVolume;

        set
        {
            GameOptions.sfxSoundVolume = value;
            GameOptions.ChangeEvent();
        }
    }

    public static readonly Vector2[] ScreenResolutionsOptions = new Vector2[] {
        new Vector2(800, 480),
        new Vector2(1024, 768),
        new Vector2(1280, 720),
        new Vector2(1920, 1080),
        new Vector2(2160, 1080),
        new Vector2(2560, 1440),
        new Vector2(3840, 2160),
    };

    public static int ScreenResolutionOptionIndex
    {
        get => GameOptions.screenResolutionOptionIndex;

        set
        {
            if (value >= GameOptions.ScreenResolutionsOptions.Length)
            {
                throw new Exception("ScreenResolutionOptionIndex: out of range");
            }

            GameOptions.screenResolutionOptionIndex = value;
            GameOptions.ChangeEvent();
        }
    }

    private static float totalSoundVolume = 1.0f;
    private static float bgmSoundVolume = 1.0f;
    private static float sfxSoundVolume = 1.0f;
    private static int screenResolutionOptionIndex = 0;
}
