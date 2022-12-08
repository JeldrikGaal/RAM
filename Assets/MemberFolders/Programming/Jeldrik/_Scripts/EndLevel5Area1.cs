using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel5Area1 : MonoBehaviour
{
    [SerializeField] private LoadingScreen _loadingScreen;

    [SerializeField] GameObject bossHolder;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bossHolder.transform.childCount == 0)
        {
            StartCoroutine(_loadingScreen.NextLevel(0));
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(_loadingScreen.NextLevel(0));
            Cursor.visible = true;
        }
    }
}
