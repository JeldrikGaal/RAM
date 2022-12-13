using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public GameObject enemies;
    public WinCondition _winCondition;

    public enum WinCondition // your custom enumeration
    {
        Enemies,
        Letters,
        Elites
    };


    [SerializeField] private LoadingScreen _loadingScreen;

    public bool done;

    void Update()
    {
        Debug.Log(_winCondition);
		if (_winCondition == WinCondition.Enemies || _winCondition == WinCondition.Elites)
		{
            if (enemies.transform.childCount == 0)
            {
                done = true;
            }
        }
		else if (_winCondition == WinCondition.Letters)
		{
            //Lettercode
		}
        
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Scene scene = SceneManager.GetActiveScene();
            StartCoroutine(_loadingScreen.NextLevel(scene.buildIndex + 1));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player") && done)
        {
            Scene scene = SceneManager.GetActiveScene();
            StartCoroutine(_loadingScreen.NextLevel(scene.buildIndex + 1));
        }
    }
}
