using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwicth : MonoBehaviour
{
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
