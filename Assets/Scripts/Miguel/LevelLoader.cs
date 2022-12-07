using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynChronosly(sceneIndex));
    }

    IEnumerator LoadAsynChronosly(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            Debug.Log(operation.isDone);

            yield return null;
        }
    }
}
