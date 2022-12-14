using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static MonoBehaviour _Instance;
    
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        
        
    }

    //[SerializeField] Stats.StatsData stats;
    // Start is called before the first frame update
    void Start()
    {
        // Just testing saving and loading
       
        

        

    }

    

    /// <summary>
    /// For continuing running coroutine despite the caller being destroid or disabeled.
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static Coroutine HandleCoroutine(IEnumerator enumerator) => _Instance.StartCoroutine(enumerator);


    [SerializeField] private float _timeForAICleanup = 10;
    

    private void OnDestroy()
    {
        enabled = false;
    }



}

public class Cleanup
{
    private static readonly Dictionary<int, System.Action<EnemyController>> _ToClean = new();
    /// <summary>
    /// For clearing user data from state blocks when they are dead.
    /// </summary>
    /// <param name="block">clean this</param>
    /// <param name="cleaner">with this</param>
    public void DoClean(StateBlock block, System.Action<EnemyController> cleaner)
    {
        if (_ToClean.ContainsKey(block.GetInstanceID()))
        {
            return;
        }
        _ToClean.Add(block.GetInstanceID(), cleaner);

    }


    public void Clean(EnemyController source)
    {

        //yield return new WaitForSecondsRealtime(_timeForAICleanup);

        foreach (var item in _ToClean)
        {
            System.Action<EnemyController> cleaner = item.Value;
            //Debug.Log("scan" + item.Key);
            //Debug.Log(item.Value);

            cleaner?.Invoke(source);
            //yield return new WaitForEndOfFrame();
        }

    }
}