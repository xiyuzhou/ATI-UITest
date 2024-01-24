using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnd : MonoBehaviour
{
    //attcached to camera, trigger functions using animation events
    public MainMenuUiManager mainMenuUiManager;

    public void OnAnimationEnd()
    {
        mainMenuUiManager.onCamAnimation = false;
    }
}
