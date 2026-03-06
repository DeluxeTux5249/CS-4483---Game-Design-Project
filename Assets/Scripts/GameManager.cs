using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static void LoadScene(string sceneToLoad)
    {
        // Load the target scene
        SceneManager.LoadScene(sceneToLoad);
    }

    public static void QuitGame() { 
        Application.Quit();
    }

}
