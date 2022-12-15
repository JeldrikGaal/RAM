using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class KeyBoardRebinding : MonoBehaviour
{
    private RammyController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<RammyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLeftKey()
    {
        InputAction aR = _controller._move.actionMap.FindAction("left");

        /*var rebindOperation = aR.PerformInteractiveRebinding().WithControlsExcluding("Mouse/delta")
                    .WithControlsExcluding("Mouse/position")
                    .WithCancelingThrough("<Keyboard>/escape")
                    .Start()
                    .OnComplete((x) =>
                    {
                        Debug.Log("Done");
                        x.Dispose();
                    })
                    .OnCancel((x) =>
                    {
                        // deselect button
                        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
                        x.Dispose();
                        aR.Enable();
                    }); ;*/
        var rebindOperation2 = aR.PerformInteractiveRebinding().
                    Start()
                    .OnComplete((x) =>
                    {
                        Debug.Log("Done");
                        x.Dispose();
                    });
    }

    public void ChangeRightKey()
    {

    }

    public void ChangeUpKey()
    {

    }

    public void ChangeDownKey()
    {

    }

    public void ChangeA1Key()
    {

    }

    public void ChangeA2Key()
    {

    }
}
