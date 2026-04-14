using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool isGameRunning = true; // in the event of state needing be checked;
    PlayerInput playerInput;
    [SerializeField] GameObject pauseScreen;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.Log("Couldn't find a player");
        }

        // TODO: see if possible to add event for pause instead of manually adding to each scene
        // otherwise, deal with it
    }

    // stops game world from running
    void StopWorldSimulation()
    {
        isGameRunning = false;
        Time.timeScale = 0;
        playerInput.DeactivateInput();
    }

    // stops game world from running
    void StartWorldSimulation()
    {
        isGameRunning = true;
        Time.timeScale = 1;
        playerInput.ActivateInput();
    }

    public void PauseGame()
    {
        StopWorldSimulation();
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        StartWorldSimulation();
        pauseScreen.SetActive(false);
    }

    public void SaveGame()
    {
        Debug.Log("Logic for Save TDB");
        // logic tbd
    }

    public void QuitGameToMenu()
    {
        SceneManager.LoadScene("Title");
    }

}
