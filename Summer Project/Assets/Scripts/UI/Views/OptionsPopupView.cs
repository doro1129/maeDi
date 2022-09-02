using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsPopupView : UIView
{
    private void Awake()
    {
        this.SetSliderValue("Total Volume Slider", GameOptions.TotalSoundVolume);
        this.SetSliderValue("BGM Volume Slider", GameOptions.BGMSoundVolume);
        this.SetSliderValue("SFX Volume Slider", GameOptions.SFXSoundVolume);

        List<TMP_Dropdown.OptionData> options = new();

        foreach (var option in GameOptions.ScreenResolutionsOptions)
        {
            var optionData = new TMP_Dropdown.OptionData($"{option.x} x {option.y}");
            options.Add(optionData);
        }

        this.SetDropdownOptions("Screen Resolution Dropdown", options);
        this.SetDropdownValue("Screen Resolution Dropdown", GameOptions.ScreenResolutionOptionIndex);

        this.SetText("Current Version Text", GameOptions.Version);
    }

    public void Close()
    {
        PopupManager.Instance.Close();
    }
}
