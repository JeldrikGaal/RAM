using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndAreaLvL3 : MonoBehaviour
{
    [SerializeField] private LoadingScreen _loadingScreen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Scene scene = SceneManager.GetActiveScene();
            StartCoroutine(_loadingScreen.NextLevel(scene.buildIndex + 1));

            // SceneManager.LoadScene(4);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            if (other.GetComponent<RammyController>().lettersCollected >= 3)
            {
                Scene scene = SceneManager.GetActiveScene();
                StartCoroutine(_loadingScreen.NextLevel(scene.buildIndex + 1));

                // SceneManager.LoadScene(4);
            }
        }
    }
}
