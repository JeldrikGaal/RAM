using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class SplinterManager : MonoBehaviour
{
    private static MonoBehaviour _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] Rigidbody _splinterPrefab;
    const int amount = 300;
    static Rigidbody[] _splinters = new Rigidbody[amount];
    static int _currentSplinter;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSplinters());
    }

    private IEnumerator SpawnSplinters()
    {
        for (int i = 0; i < amount; i++)
        {
            _splinters[i] = Instantiate(_splinterPrefab,null);

            yield return new WaitForEndOfFrame();
        }
    }

    public static bool GetSplinter(Vector3 position, Vector3 direction, float speed)
    {
        var splinter = _splinters[_currentSplinter];
        if (splinter.gameObject.activeSelf)
        {
            return false;
            
        }

        _currentSplinter = _currentSplinter + 1 < amount ? _currentSplinter + 1 : 0;
        
        splinter.gameObject.SetActive(true);
        splinter.position = position;
        splinter.transform.LookAt(position + direction);
        splinter.velocity = splinter.transform.forward * speed;

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
