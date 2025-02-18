using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup howtoPanel;
    [SerializeField] Transform contents;
    [SerializeField]int index;
    // Start is called before the first frame update
    void Start()
    {
        CloseHowto();
        index = -1;
        Next();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenHowto()
    {
        index = -1;
        Next();
        howtoPanel.alpha = 1;
        howtoPanel.blocksRaycasts = true;
    }
    public void CloseHowto()
    {
        howtoPanel.alpha = 0;
        howtoPanel.blocksRaycasts = false;
    }
    public void Next()
    {
        index++;
        if(index >= contents.childCount)
        {
            index = contents.childCount - 1;
        }
        foreach (Transform item in contents)
        {
            item.gameObject.SetActive(false);
        }
        contents.GetChild(index).gameObject.SetActive(true);
    }
    public void Previous()
    {
        index--;
        if (index <= 0)
        {
            index = 0;
        }
        foreach (Transform item in contents)
        {
            item.gameObject.SetActive(false);
        }
        contents.GetChild(index).gameObject.SetActive(true);
    }
}
