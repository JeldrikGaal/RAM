using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class ObjectPoolManager : MonoBehaviour
{
    #region keeps it to one instance per prefab
    // Enshures only one instance
    private static ObjectPoolManager _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of Object Pool Manager with same prefab was found");
            Destroy(this);
        }
        #endregion
        #region Initialize Objects
        foreach (var item in _objectsToManage)
        {
            _ObjectsPerType.Add(item.Prefab.GetType(), (item.Pool, new Pooltoy[item.Pool], 0));
        }
    }

    [SerializeField] List<ObjectToManage> _objectsToManage;

    [System.Serializable]
    public struct ObjectToManage
    {
        public Pooltoy Prefab;
        public int Pool;
    }

    static Dictionary<System.Type, (int amount, Pooltoy[] objects, int current)> _ObjectsPerType = new();
    

    // Start is called before the first frame update
    void Start()
    {

        foreach (var item in _objectsToManage)
        {
            StartCoroutine(SpawnObjects(item.Prefab,item.Pool));
        }
        

    }

    // populates the object pool
    private IEnumerator SpawnObjects(Pooltoy objectPrefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _ObjectsPerType[objectPrefab.GetType()].objects[i] = 
                Instantiate(objectPrefab,null);
            _ObjectsPerType[objectPrefab.GetType()].objects[i].gameObject.SetActive(false);
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
    /// Requests a object from the object pool with a spesified type, will come when awailable.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    public static void RequestObject(System.Type type, Vector3 position, Vector3 direction, Vector3 velocity, float time, Properties properties)
    {
        _requests.Add((type, position, direction, velocity, time, properties));
        if (!_running && _requests.Count>0)
        {
            _instance.StartCoroutine(ServesObjects());
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
    /// Pulls a object from the object pool, if awailable.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private static bool GetObject(System.Type type,Vector3 position, Vector3 direction, Vector3 velocity , float time, Properties properties)
    {
        var (amount, objects, current) = _ObjectsPerType[type];
        var @object = objects[current];

        if (@object == null)
        {
            return false;
        }


        if (@object.gameObject.activeSelf)
        {
            return false;
            
        }

        // gets the next number to cycle trough the list
        current = current + 1 < amount ? current + 1 : 0;
        _ObjectsPerType[type] = (amount, objects, current);

        // setting all relevant data
        @object.Rb.gameObject.SetActive(true);
        @object.Rb.position = position;
        @object.Rb.transform.LookAt(position + direction);
        @object.Rb.velocity = velocity;
        @object.SetProperties(properties);
        _instance.StartCoroutine(DisableObject(@object.Rb, time));
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

/// <summary>
/// Just to identify wha is properties for ObjectPoolManager
/// </summary>
public abstract class Properties
{
    

}

/// <summary>
/// base class for classes for prefabs that can ba managed by the ObjectPoolManager.
/// </summary>
public abstract class Pooltoy: MonoBehaviour 
{
    /// <summary>
    /// Sets the properties of the object that it spawns.
    /// </summary>
    /// <param name="properties"></param>
    public abstract void SetProperties(Properties properties );
    public abstract Rigidbody Rb { get;  }
}