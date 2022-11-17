using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractTest : MonoBehaviour
{
    public InputTest _testing;


    private InputAction _interact;


    private void OnEnable()
    {
        _interact = _testing.Interact.Interact;
        _interact.Enable();

        _interact.performed += Interact;
    }

    private void OnDisable()
    {
        _interact.Disable();
    }

    private void Interact(InputAction.CallbackContext context)
    {
        print("TEEEEST");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
