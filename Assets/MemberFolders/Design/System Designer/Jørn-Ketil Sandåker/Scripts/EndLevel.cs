using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public GameObject enemies;
    public WinCondition _winCondition;

    public bool _useKillCount = false;
    public int KillCount = 0;

    public GameObject ObjectToDelete;

    private StatManager _stats;

    public enum WinCondition // your custom enumeration
    {
        Enemies,
        Letters,
        Elites
    };


    [SerializeField] private LoadingScreen _loadingScreen;

    public bool done;

    private void Start()
    {
        _stats = GameObject.Find("HUD").GetComponent<StatManager>();
    }

    void Update()
    {
        //Debug.Log(_winCondition);
        if (_useKillCount && _stats.Stats.Kills >= KillCount)
        {
            done = true;
            if (ObjectToDelete != null)
                ObjectToDelete.SetActive(false);
        }

		if (_winCondition == WinCondition.Enemies || _winCondition == WinCondition.Elites)
		{
            if (enemies.transform.childCount == 0)
            {
                done = true;
                if (ObjectToDelete != null)
                    ObjectToDelete.SetActive(false);
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
