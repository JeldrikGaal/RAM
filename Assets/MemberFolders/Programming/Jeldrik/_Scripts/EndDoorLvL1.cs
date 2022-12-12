using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class EndDoorLvL1 : MonoBehaviour
{
    public List<GameObject> AllEnemies = new List<GameObject>();

    public bool level2;

    [SerializeField] private LoadingScreen _loadingScreen;

    private bool _done;
    private bool _pannedCamera;
    private bool _panCamera;

    [SerializeField] private bool _testBool;

    [SerializeField] private int _xPos, _zPos;

    [SerializeField] private float _waitTime;


    private GameObject _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (level2 && Input.GetKeyDown(KeyCode.Alpha0))
        {
            Scene scene = SceneManager.GetActiveScene();
            StartCoroutine(_loadingScreen.NextLevel(scene.buildIndex + 1));

            // SceneManager.LoadScene(3);
        }
        if (!level2 && Input.GetKeyDown(KeyCode.Alpha0))
        {
            Scene scene = SceneManager.GetActiveScene();
            StartCoroutine(_loadingScreen.NextLevel(scene.buildIndex + 1));

            // SceneManager.LoadScene(2);
        }

        if ((GameObject.FindGameObjectsWithTag("wolf").Length == 0 && !_pannedCamera) || _testBool)
        {
            _done = true;
            _pannedCamera = true;
            StartCoroutine(PanCamera());
            _testBool = false;
        }

        if (_panCamera)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(transform.position.x + _xPos, _camera.transform.position.y, transform.position.z - _zPos), 0.5f * Time.deltaTime);
        }
    }

    private IEnumerator PanCamera()
    {
        _camera.GetComponent<CinemachineBrain>().enabled = false;
        _panCamera = true;

        yield return new WaitForSeconds(_waitTime);

        _panCamera = false;
        _camera.GetComponent<CinemachineBrain>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("player"))
        {
            if (_done)
            {
                if (!level2)
                {
                    Scene scene = SceneManager.GetActiveScene();
                    StartCoroutine(_loadingScreen.NextLevel(scene.buildIndex + 1));

                    // SceneManager.LoadScene(2);
                    Debug.Log("level completed");
                }
                else
                {
                    Scene scene = SceneManager.GetActiveScene();
                    StartCoroutine(_loadingScreen.NextLevel(scene.buildIndex + 1));

                    // SceneManager.LoadScene(3);
                    Debug.Log("level completed");
                }
            }
        }
    }
}
