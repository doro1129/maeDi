using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvas scaler setter for screen resolution option
/// </summary>
public class SetterCanvasScaler : MonoBehaviour
{
    /// <summary>
    /// Target canvas scaler
    /// </summary>
    public CanvasScaler TargetCanvas;

    private void Awake()
    {
        GameOptions.ChangeEvent += SetResolution;

        this.SetResolution();
    }

    private void OnDestroy()
    {
        GameOptions.ChangeEvent -= SetResolution;
    }

    /// <summary>
    /// Resolution setter
    /// </summary>
    private void SetResolution()
    {
        var index = GameOptions.ScreenResolutionOptionIndex;
        this.TargetCanvas.referenceResolution = GameOptions.ScreenResolutionsOptions[index];
    }
}
