using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private Canvas canvas;

    [SerializeField] private bool _dontStartOnLoad;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();

        if (!_dontStartOnLoad)
        {
            StartCoroutine(Show());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Show()
    {
        canvas.enabled = !canvas.enabled;
        yield return new WaitForSeconds(1);
        canvas.enabled = !canvas.enabled;
    }

    public IEnumerator NextLevel(int buildIndex)
    {
        StartCoroutine(Show());
        yield return new WaitForSeconds(0.9f);

        SceneManager.LoadScene(buildIndex);
    }
}
