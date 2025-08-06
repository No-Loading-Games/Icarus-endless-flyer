using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public TutorialTab currentTab;
    public TutorialTab prevTab;
    public int currentTabIdentifier = 0;
    public int prevTabIdentifier = 0;
    public string currentTabTitle = "Movement";
    public string[] currentTabAnimationClipNames = {"Controls 1", "Controls 2" };

    public int pageNum = 1;

    [SerializeField]
    private TextMeshProUGUI _pageTitleTMP;
    [SerializeField]
    private Image _pageContent;
    [SerializeField]
    private TextMeshProUGUI _pageNumTMP;
    [SerializeField]
    private Button _nextPage;
    [SerializeField]
    private Button _prevPage;
    // Start is called before the first frame update

    AsyncOperation loadingOperation;

    GameManager _gameManager;

    private void OnEnable()
    {
        //Load Tutorial in case user plays it

        _gameManager = FindObjectOfType<GameManager>();
        UpdatePage();
    }

    public void UpdatePage()
    {
        //Change sprite to DISABLED when interactable = false
        Animator pageAnimator = _pageContent.GetComponent<Animator>();
        int endPage = currentTabAnimationClipNames.Length;

        CheckTerminalPage(1,_prevPage);
        CheckTerminalPage(endPage, _nextPage);

        pageAnimator.SetLayerWeight(prevTabIdentifier, 0);
        pageAnimator.SetLayerWeight(currentTabIdentifier, 1);

        prevTabIdentifier = currentTabIdentifier;

        pageAnimator.CrossFade(currentTabAnimationClipNames[pageNum - 1], 0.1f);

        _pageTitleTMP.text = currentTabTitle;

        _pageNumTMP.text = "(" + pageNum.ToString() + " " + currentTabAnimationClipNames.Length.ToString() + ")";

    }

    private void CheckTerminalPage(int page, Button button)
    {
        if (pageNum == page)
        {

            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void PlayTutorial()
    {
        loadingOperation.allowSceneActivation = true;
    }

    public void UnloadTutorialScene()
    {
        _gameManager.SetTutorial(false);
        PlayerPrefs.SetInt("replay-tutorial", 0);
        SceneManager.UnloadSceneAsync("TutorialScene");
    }

    public void LoadTutorialScene()
    {
        loadingOperation = SceneManager.LoadSceneAsync("TutorialScene");
        _gameManager.SetTutorial(true);
        PlayerPrefs.SetInt("replay-tutorial", 1);
        loadingOperation.allowSceneActivation = false;
    }
}
