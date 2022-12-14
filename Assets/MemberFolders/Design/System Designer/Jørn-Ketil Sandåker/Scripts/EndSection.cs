using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndSection : MonoBehaviour
{
    public GameObject enemies;
    public WinCondition _winCondition;

    [SerializeField] private GameObject sectionWall;

    public enum WinCondition // What type of condition to be met to end the section
    {
        Enemies,
        Letters,
        Elites,
        Towers
    };

    public bool done;

    void Update()
    {
        //Debug.Log(_winCondition);
        if (_winCondition == WinCondition.Enemies || _winCondition == WinCondition.Elites)
        {
            if (enemies.transform.childCount == 0)
            {
                done = true;
                sectionWall.SetActive(false);
            }
        }
        else if (_winCondition == WinCondition.Letters)
        {
			//Lettercode
		}
		else if (_winCondition == WinCondition.Towers)
		{
            //Insert tower code
		}
    }

	private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.HasTag("player") && done)
        {
            sectionWall.SetActive(true);
        }
    }
}
