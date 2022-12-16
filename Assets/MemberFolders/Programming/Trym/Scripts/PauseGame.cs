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
    [SerializeField] GameObject _ingameUi;
    [SerializeField] GameObject _journal;
    [SerializeField] UnityEvent _onPause;
    [SerializeField] UnityEvent _onUnpause;
    
    private static bool _paused = false;

    [HideInInspector] public bool AllowPause;

    #region most of Input
    RammyInputActions _inputs;
    private void Start()
    {
        AllowPause = true;
        _inputs = new RammyInputActions();
        _inputs.UI.Pause.Enable();
        _inputs.UI.Pause.performed += Toggle;

    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Toggle2();
        }
    }
    // Intened for pausing and unpausing with escape

    /// <summary>
    /// toggles between paused and unpaused.
    /// </summary>
    private void Toggle(CallbackContext context)
    {
        Debug.LogError(AllowPause);
        if (!AllowPause) return;
        if (_paused)
        {
            UnPause();
            if (_journal.activeSelf == true)
            {
                _journal.GetComponent<Journal>().CloseTheBook();
            }
        }
        else
        {
            Pause();

        }

    }

    private void Toggle2()
    {
        Debug.LogError("1");
        Debug.LogError(AllowPause);
        if (!AllowPause) return;
        if (_paused)
        {
            Debug.LogError("1.1");
            UnPause();
            if (_journal.activeSelf == true)
            {
                _journal.GetComponent<Journal>().CloseTheBook();
            }
            Debug.LogError("2");
        }
        else
        {
            Debug.LogError("3");
            Pause();

        }
        Debug.LogError("4");
    }

    #endregion
    /// <summary>
    /// Use this to (de)activate a gameObject.
    /// </summary>
    /// <param name="panel"></param>
    public void OpenClosePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf!);
    }

    #region Menu Interaction

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void Pause()
    {
        Debug.LogError("5");
        Cursor.visible = true;
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        _pauseMenuContent.GetComponent<Animator>().SetTrigger("OpenMenu");
        _settingsMenu.SetActive(true);
        //_settingsMenu.GetComponent<Animator>().SetTrigger("CloseOptionsPanel");
        _paused = true;
        if (_ingameUi) _ingameUi.SetActive(false);
        OnPausedEventHandler(true);
        _onPause.Invoke();
        Debug.LogError("6");
    }
    /// <summary>
    /// Resumes the game
    /// </summary>
    public void UnPause()
    {
        Debug.LogError("7");
        //Cursor.visible = false;
        _settingsMenu.SetActive(false);
        Time.timeScale = 1;
        _pauseMenuContent.GetComponent<Animator>().SetTrigger("CloseMenu");
        _paused = false;
        if (_ingameUi) _ingameUi.SetActive(true);
        OnPausedEventHandler(false);
        _onUnpause.Invoke();
        Debug.LogError("8");

    }

    /// <summary>
    /// restarting the level (Scene)
    /// </summary>
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void MainMenu() => SceneManager.LoadScene(0);

    /// <summary>
    /// Opens the settings in the pause menu.
    /// </summary>
    public void OpenSettings(GameObject forAnimator)
    {
        forAnimator.GetComponent<Animator>().SetTrigger("OpenOptionsPanel");
        _pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Closes the settings on thee pause menu.
    /// </summary>
    public void CloseSettings(GameObject forAnimator)
    {
        forAnimator.GetComponent<Animator>().SetTrigger("CloseOptionsPanel");
        _pauseMenu.SetActive(true);
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

	#region Jorn_Stuff
    public void EnablePause()
	{
        AllowPause = true;
        _inputs.UI.Pause.Enable();
        _inputs.UI.Pause.performed += Toggle;
    }
	#endregion
}
