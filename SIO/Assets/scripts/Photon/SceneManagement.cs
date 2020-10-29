using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Image fill;
    public Text progressText;
    public GameObject canvas;


    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartLoading(AsyncOperation async)
    {
        StartCoroutine("ShowLoadingProgress", async);
    }
    public IEnumerator ShowLoadingProgress(AsyncOperation async)
    {
        canvas.SetActive(true);
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            fill.fillAmount = async.progress;
            float taskToPercentage = Mathf.Round(async.progress * 100);
            progressText.text = "Loading : " + taskToPercentage + "%";
            yield return null;
        }
        progressText.text = "Loading Complete";
        canvas.SetActive(false);

    }
}
