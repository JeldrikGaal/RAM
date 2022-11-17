using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _settingsMenu;
    [SerializeField] UnityEvent _onPause;
    [SerializeField] UnityEvent _onUnpause;
    private static bool _paused = false;

    #region most of Input
    RammyInputActions _inputs;
    private void Start()
    {
        _inputs = new RammyInputActions();
        _inputs.UI.Pause.Enable();
        _inputs.UI.Pause.performed += Toggle;

    }
    // Intened for pausing and unpausing with escape
    
    /// <summary>
    /// toggles between paused and unpaused.
    /// </summary>
    private void Toggle(CallbackContext context)
    {
        if (_paused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }


    #endregion


    #region Menu Interaction

    /// <summary>
    /// Pauses the game.
    /// </summary>
    private void Pause()
    {
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        _paused = true;
        OnPausedEventHandler(true);
    }
    /// <summary>
    /// Resumes the game
    /// </summary>
    public void UnPause()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _paused = false;
        OnPausedEventHandler(false);
    }

    /// <summary>
    /// restarting the level (Scene)
    /// </summary>
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void MainMenu() => SceneManager.LoadScene(0);

    /// <summary>
    /// Opens the settings
    /// </summary>
    /*public void Settings()
    {
        _settingsMenu.SetActive(true);
    }*/

    #endregion

    /// <summary>
    /// Event for pausing the game, the argument is true when pausing.
    /// </summary>
    public static event System.Action<bool> PauseEvent;

    
    /// <summary>
    /// To check if the game is paused.
    /// </summary>
    /// <returns></returns>
    public static bool IsPaused() => _paused;

    #region private Misc

    //simple handler for the pause event
    private void OnPausedEventHandler(bool paused) => PauseEvent?.Invoke(paused);

    // should pause the game if the player tabbs out to another application
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Pause();
        }
    }
    // Should unpause the game when exiting.
    private void OnDisable()
    {
        if (_paused)
        {
            UnPause();
            print("test");
        }
        //removes old references
        _inputs.UI.Pause.performed -= Toggle;
        
    }
    #endregion
}
