using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuView : UIView
{
    public void OnGameStartButtonClick()
    {
        Navigation.Push("World Menu Canvas");
    }

    public void OnCollectionsButtonClick()
    {
        Navigation.Push("Collections Canvas");
    }

    public void OnOptionsButtonClick()
    {
        PopupManager.Instance.OpenPrefab("Options Popup");
    }
}
