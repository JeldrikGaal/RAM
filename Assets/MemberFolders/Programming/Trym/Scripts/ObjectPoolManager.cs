using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class ObjectPoolManager : MonoBehaviour
{
    #region keeps it to one instance
    // Enshures only one instance
    private static Dictionary<System.Type, ObjectPoolManager> _instances = new();
    private void Awake()
    {
        if (!_instances.ContainsKey(_objectPrefab.GetType()))
        {
            _instances.Add(_objectPrefab.GetType(), this);
        }
        else
        {
            Destroy(this);
        }
        #endregion
        #region Initialize Objects
        _amount = _pool;
        _splinters = new Pooltoy[_amount];
    }
    
    
    [SerializeField] Pooltoy _objectPrefab;
    [SerializeField] int _pool = 300;
    
    static int _amount;
    static Pooltoy[] _splinters;
    static int _currentSplinter;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjects());

    }

    // populates the object pool
    private IEnumerator SpawnObjects()
    {
        for (int i = 0; i < _amount; i++)
        {
            _splinters[i] = 
                Instantiate(_objectPrefab,null);
            _splinters[i].gameObject.SetActive(false);
            if (Time.deltaTime >= 1/60)
            {
                yield return new WaitForEndOfFrame();
            }
            
        }
    }
    #endregion





    #region For getting objects
    // List of splinter requests to be handeled.
    static List<(System.Type type,Vector3 position, Vector3 direction, Vector3 velocity, float time, Properties properties)> _requests = new();
    static bool _running = false;

    /// <summary>
    /// Requests a splinter from the object pool, will come when awailable.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    public static void RequestObject(System.Type type, Vector3 position, Vector3 direction, Vector3 velocity, float time, Properties properties)
    {
        _requests.Add((type, position, direction, velocity, time, properties));
        if (!_running && _requests.Count>0)
        {
            _instances[type].StartCoroutine(ServesObjects());
        }

    }
    // handles all requests for Objects
    static IEnumerator ServesObjects()
    {
        _running = true;
        
        while (_requests.Count>0)
        {
            // extacts the dataand tries to get a splinter
            _running = true;
            var position = _requests[0].position;
            var direction = _requests[0].direction;
            var speed = _requests[0].velocity;
            var time = _requests[0].time;
            var properties = _requests[0].properties;
            var type = _requests[0].type;
            if (GetObject(type,position, direction, speed, time, properties))
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
    /// Pulls a splinter from the object pool, if awailable.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private static bool GetObject(System.Type type,Vector3 position, Vector3 direction, Vector3 velocity , float time, Properties properties)
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

        // gets the next number to cycle trough the list
        _currentSplinter = _currentSplinter + 1 < _amount ? _currentSplinter + 1 : 0;
        
        // setting all relevant data
        splinter.Rb.gameObject.SetActive(true);
        splinter.Rb.position = position;
        splinter.Rb.transform.LookAt(position + direction);
        splinter.Rb.velocity = velocity;
        splinter.SetProperties(properties);
        _instances[type].StartCoroutine(DisableObject(splinter.Rb, time));
        return true;
    }
    #endregion

    // disables the object after it's lifetime.
    private static IEnumerator DisableObject(Rigidbody splinter, float time)
    {
        yield return new WaitForSeconds(time);
        splinter.gameObject.SetActive(false);
    }


}

[System.Serializable]
public abstract class Properties
{
    

}

public abstract class Pooltoy: MonoBehaviour 
{
    public abstract void SetProperties(Properties properties );
    public abstract Rigidbody Rb { get;  }
}