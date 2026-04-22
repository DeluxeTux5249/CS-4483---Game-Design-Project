using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static void LoadScene(string sceneToLoad)
    {
        // Load the target scene
        if (PlayerPrefs.HasKey("LastPlayerScene"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("LastPlayerScene"));

        } else
        {
            SceneManager.LoadScene(sceneToLoad);
        }

            
    }

    public static void QuitGame() { 
        Application.Quit();
    }

    public static void clearSaveData()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void UnloadScene(string sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }

}
