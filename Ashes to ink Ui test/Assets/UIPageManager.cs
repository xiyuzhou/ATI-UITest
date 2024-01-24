using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPageManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject HowToPlay;
    public GameObject Settings;
    public GameObject TheMakingOf;
    public GameObject Play;
    public GameObject NewOrLoad;
    public GameObject ExitToDesktop;
    [HideInInspector] public GameObject[] screens;
    [Header("HowToPlay")]
    public GameObject[] HowToPlayPages;

    public GameObject[] SettingsPages;
    public GameObject[] TheMakingOfPages;
    public GameObject[] NewOrLoadPages;

    void Start()
    {
        InitializeScreensArray();
    }

    private void InitializeScreensArray()
    {
        screens = new GameObject[]
        {
            HowToPlay,
            Settings,
            TheMakingOf,
            Play,
            NewOrLoad,
            ExitToDesktop
        };
    }

    public void ShowScreens(MenuSelection selection)
    {
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
        int index = ReturnIndexOfSelection(selection);
        screens[index].SetActive(true);
    }

    public int ReturnIndexOfSelection(MenuSelection selection)
    {
        return (int)selection;
    }

}
