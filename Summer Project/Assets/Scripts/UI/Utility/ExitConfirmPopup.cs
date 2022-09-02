using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitConfirmPopup : MonoBehaviour
{
    UIView popup = null;

    private void Update()
    {
        if (popup == null && Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape))
        {
            this.OpenPopup();
        }
    }

    public void OpenPopup()
    {
        popup = PopupManager.Instance.OpenPrefab("Confirm Popup");
        popup
            .SetText("Title Text", "Exit")
            .SetText("Contents Text", "Are you sure you want to quit the game?")
            .AddButtonOnClickEvent("Cancel Button", delegate {
                PopupManager.Instance.Close();
            })
            .AddButtonOnClickEvent("OK Button", delegate {
                Application.Quit();
            });
    }
}
