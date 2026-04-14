using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool isGameRunning = true; // in the event of state needing be checked;
    PlayerInput playerInput;
    [SerializeField] GameObject pauseScreen;

    InputAction pauseAction;
    InputAction resumeGame;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.Log("Couldn't find a player");
        }

        /* Debug: These may be able to dynamically assign to the player pause/resume functionallity

        // These technically work, but cause a lot of errors that aren't safe. 
        pauseAction = playerInput.actions.FindAction("PauseGame");
        pauseAction.performed += ctx => PauseGame();

        resumeGame = playerInput.actions.FindAction("UnpauseGame");
        resumeGame.performed += ctx => ResumeGame();
         */

    }

    /*
    private void OnDestroy()
    {
        pauseAction.performed -= ctx => PauseGame();    
        resumeGame.performed -= ctx => ResumeGame();
    }
     
     */

    // stops game world from running
    void StopWorldSimulation()
    {
        isGameRunning = false;
        Time.timeScale = 0;
        playerInput.SwitchCurrentActionMap("UI");
    }

    // stops game world from running
    void StartWorldSimulation()
    {
        isGameRunning = true;
        Time.timeScale = 1;
        playerInput.SwitchCurrentActionMap("Player");
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
