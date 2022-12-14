using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour
{
    private RammyController _playerController;
    private RammyVFX _playerVFX;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            // Halves the movespeed of the player when they enter the slow zone
            // _playerController = other.GetComponent<RammyController>();
            // _playerController.MovementSpeed /= 2;

            _playerVFX.InHoney = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Multiplies the speed with 2 again to return it to the default value (might cause problems later, Risk of Rain 1)
            // _playerController.MovementSpeed *= 2;
            _playerVFX.InHoney = false;
        }
    }
}
