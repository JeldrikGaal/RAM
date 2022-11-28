using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _pauseMenuContent;
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
    public void Pause()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        _paused = true;
        OnPausedEventHandler(true);
        _onPause.Invoke();
    }
    /// <summary>
    /// Resumes the game
    /// </summary>
    public void UnPause()
    {

        Time.timeScale = 1;
        _settingsMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _paused = false;
        OnPausedEventHandler(false);
        _onUnpause.Invoke();
    }

    /// <summary>
    /// restarting the level (Scene)
    /// </summary>
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void MainMenu() => SceneManager.LoadScene(0);

    /// <summary>
    /// Opens the settings in the pause menu.
    /// </summary>
    public void Settings()
    {
        _settingsMenu.SetActive(true);
        _pauseMenuContent.SetActive(false);
    }
    /// <summary>
    /// Closes the settings on thee pause menu.
    /// </summary>
    public void CloseSettings()
    {
        _settingsMenu.SetActive(false);
        _pauseMenuContent.SetActive(true);
    }
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
        }

        //removes old references
        _inputs.UI.Pause.performed -= Toggle;
        PauseEvent = new System.Action<bool>(Noting);

        static void Noting(bool paused)
        {

        }
    }
    #endregion
}
