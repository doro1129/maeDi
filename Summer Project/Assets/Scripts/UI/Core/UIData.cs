using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public static class UIData
{
    public static UIView SetText(this UIView targetView, string objectName, string value)
    {
        var texts = targetView.GetComponentsInChildren<TMP_Text>();

        foreach (var text in texts)
        {
            if (text.name == objectName)
            {
                text.text = value;
            }
        }

        return targetView;
    }

    public static UIView SetSliderValue(this UIView targetView, string objectName, float value)
    {
        var sliders = targetView.GetComponentsInChildren<Slider>();

        foreach (var slider in sliders)
        {
            if (slider.name == objectName)
            {
                slider.value = value;
            }
        }

        return targetView;
    }

    public static UIView SetDropdownValue(this UIView targetView, string objectName, int index)
    {
        var dropdowns = targetView.GetComponentsInChildren<TMP_Dropdown>();

        foreach (var dropdown in dropdowns)
        {
            if (dropdown.name == objectName)
            {
                dropdown.value = index;
            }
        }

        return targetView;
    }

    public static UIView SetDropdownOptions(this UIView targetView, string objectName, List<TMP_Dropdown.OptionData> options)
    {
        var dropdowns = targetView.GetComponentsInChildren<TMP_Dropdown>();

        foreach (var dropdown in dropdowns)
        {
            if (dropdown.name == objectName)
            {
                dropdown.options = options;
            }
        }

        return targetView;
    }

    public static UIView AddButtonOnClickEvent(this UIView targetView, string objectName, UnityEngine.Events.UnityAction action)
    {
        var buttons = targetView.GetComponentsInChildren<Button>();

        foreach (var button in buttons)
        {
            if (button.name == objectName)
            {
                button.onClick.AddListener(action);
            }
        }

        return targetView;
    }
}
