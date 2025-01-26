using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnable : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene(2);
    }
}
