using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using static UnityEngine.InputSystem.InputAction;

public class ReturnToMainMenuAfterCred : MonoBehaviour
{
   [SerializeField] VideoPlayer player;
    RammyInputActions inputActions;
    // Start is called before the first frame update
    void Start()
    {
        inputActions = new RammyInputActions();
        inputActions.UI.Enable();
        inputActions.UI.Pause.performed += (CallbackContext context) => Player_loopPointReached(player);
        player.loopPointReached += Player_loopPointReached;
    }

    private void Player_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
