using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _CTAText;

    private bool[] _tutorialIsDone = { false, true };


    AsyncOperation loadingOperation;
    // Start is called before the first frame update
    void Start()
    {
        _CTAText.text = "";
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        //CHANGE the scene to load depending if we need tutorial or not
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneToLoad());

        Debug.Log("Loading Scene: " + SceneToLoad());
        
        if (loadingOperation == null)
        {
            Debug.LogError("Failed to load scene: " + SceneToLoad());
            yield break;
        }

        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            Debug.Log("PROGRESS  " + loadingOperation.progress);

            if (loadingOperation.progress >= 0.9f)
            {
                Debug.Log("DONE");
                _CTAText.text = "Press Anywhere\nto Continue";

                if(Input.touches.Length > 0)
                {
                    if (Input.touches[0].tapCount == 1 && Input.touches[0].phase == TouchPhase.Ended)
                    {
                        loadingOperation.allowSceneActivation = true;
                    }
                   
                }
            }

            yield return null;
        }
    }

    private string SceneToLoad()
    {
        if (!PlayerPrefs.HasKey("tutorial"))
        {
            return "Scenes/TutorialScene";
        }
        else
        {
            return "Scenes/GameScene";
        }
    }


}
