using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel5Area1 : MonoBehaviour
{
    [SerializeField] private LoadingScreen _loadingScreen;

    [SerializeField] GameObject bossHolder;

    [SerializeField] GameObject _firstEnemies;
    [SerializeField] GameObject _bossFase;

    private bool _done;

    void Update()
    {
        if (bossHolder.transform.childCount == 0 && !_done)
        {
            StartCoroutine(_loadingScreen.NextLevel(0));
            Cursor.visible = true;
            _done = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(_loadingScreen.NextLevel(0));
            Cursor.visible = true;
        }
        if (_firstEnemies.transform.childCount == 0)
        {
            EnableBossStage();
        }
    }

    private void EnableBossStage()
	{
        _firstEnemies.SetActive(false);
        _bossFase.SetActive(true);
	}
}
