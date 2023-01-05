using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// загрузка
/// </summary>
public class Loading : MonoBehaviour
{
    public int sceneID; // идентификатор сцены

    public Image indicator; // индикатор загрузки
    public TMP_Text progress; // прогресс загрузки


    private void Start()
    {
        StartCoroutine(LoadScene());
    }


    /// <summary>
    /// загрузить сцену
    /// </summary>
    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            indicator.fillAmount = progress;
            this.progress.text = string.Format("{0:0}%", progress * 100);
            yield return null;
        }
    }
}
