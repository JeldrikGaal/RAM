using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class SplinterManager : MonoBehaviour
{
    #region keeps it to one instance
    // Enshures only one instance
    private static SplinterManager _instance;
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
        #endregion
        #region Initialize Splinters
        _amount = _pool;
        _splinters = new Rigidbody[_amount];
    }
    
    
    [SerializeField] Rigidbody _splinterPrefab;
    [SerializeField] int _pool = 300;
    [SerializeField] float _lifetime = 5;
    static int _amount;
    static Rigidbody[] _splinters;
    static int _currentSplinter;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSplinters());

    }

    // populates the object pool
    private IEnumerator SpawnSplinters()
    {
        for (int i = 0; i < _amount; i++)
        {
            _splinters[i] = Instantiate(_splinterPrefab,null);
            _splinters[i].gameObject.SetActive(false);
            if (Time.deltaTime >= 1/60)
            {
                yield return new WaitForEndOfFrame();
            }
            
        }
    }
    #endregion





    #region For getting splinters
    static List<(Vector3 position, Vector3 direction, float speed, float time)> _requests = new();
    static bool _running = false;

    /// <summary>
    /// Requests a splinter from the object pool, will come when awailable.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    public static void RequestSplinter(Vector3 position, Vector3 direction, float speed, float time)
    {
        _requests.Add((position, direction, speed, time));
        if (!_running && _requests.Count>0)
        {
            _instance.StartCoroutine(ServesSplinters());
        }

    }
    // handles all requests for splinters
    static IEnumerator ServesSplinters()
    {
        _running = true;
        while (_requests.Count>0)
        {
            _running = true;
            var position = _requests[0].position;
            var direction = _requests[0].direction;
            var speed = _requests[0].speed;
            var time = _requests[0].time;
            if (GetSplinter(position, direction, speed, time))
            {
                _requests.RemoveAt(0);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        _running = false;
    }

    /// <summary>
    /// Pulls a splinter from the object pool.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private static bool GetSplinter(Vector3 position, Vector3 direction, float speed, float time)
    {
        var splinter = _splinters[_currentSplinter];

        if (splinter == null)
        {
            return false;
        }


        if (splinter.gameObject.activeSelf)
        {
            return false;
            
        }

        _currentSplinter = _currentSplinter + 1 < _amount ? _currentSplinter + 1 : 0;
        
        splinter.gameObject.SetActive(true);
        splinter.position = position;
        splinter.transform.LookAt(position + direction);
        splinter.velocity = splinter.transform.forward * speed;

        _instance.StartCoroutine(DisableSplinter(splinter, time));
        return true;
    }
    #endregion

    private static IEnumerator DisableSplinter(Rigidbody splinter, float time)
    {
        yield return new WaitForSeconds(time);
        splinter.gameObject.SetActive(false);
    }


}
