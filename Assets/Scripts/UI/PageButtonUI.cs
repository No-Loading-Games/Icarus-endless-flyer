using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageButtonUI : MonoBehaviour
{
    private TutorialManager _tutorialManager;

    private void Start()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
        /*if (_tutorialManager.pageNum == _tutorialManager.currentTabAnimationClipNames.Length || (_tutorialManager.pageNum == 1))
            this.GetComponent<Button>().interactable = false;
*/
    }

    public void NextPage()
    {
        
        _tutorialManager.pageNum++;
        _tutorialManager.UpdatePage();
        
        
    }

    public void PrevPage()
    {
        _tutorialManager.pageNum--;
        _tutorialManager.UpdatePage();
    }

}
