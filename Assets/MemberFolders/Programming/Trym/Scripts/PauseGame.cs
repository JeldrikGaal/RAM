using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] UnityEvent _onPause;
    [SerializeField] UnityEvent _onUnpause;
    private static bool _paused;

    RammyInputActions _inputs;
    /// <summary>
    /// Event for pausing the game, the argument is true when pausing.
    /// </summary>
    public static System.Action<bool> PausingEvent;
    /// <summary>
    /// toggles between paused and unpaused.
    /// </summary>
    private void Toggle()
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
    /// <summary>
    /// Pauses the game.
    /// </summary>
    private void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        _paused = true;
        OnPausedEventHandler(true);
    }
    /// <summary>
    /// Unpauses the game
    /// </summary>
    public void UnPause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _paused = false;
        OnPausedEventHandler(false);
    }
    /// <summary>
    /// To check if the game is paused.
    /// </summary>
    /// <returns></returns>
    public static bool IsPaused() => _paused;
    private void Start()
    {
        _inputs = new RammyInputActions();
        _inputs.UI.Pause.Enable();
        _inputs.UI.Pause.performed += (CallbackContext context) => Toggle();
        
    }
    private void OnPausedEventHandler(bool paused) => PausingEvent?.Invoke(paused);
}
