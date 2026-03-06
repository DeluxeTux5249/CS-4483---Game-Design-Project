using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string sceneToLoad;          // Name of the scene this door goes to
    public Vector3 spawnPosition;       // Where the player will appear in the target scene

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Save spawn position for next scene
            PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);

            // Load the target scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        // Load the target scene
        SceneManager.LoadScene(sceneToLoad);
    }
}