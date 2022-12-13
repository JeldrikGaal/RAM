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
        StartCoroutine(Cleaner());
        
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
    private static readonly Dictionary<(int user,int block), (EnemyController source, System.Action<EnemyController> cleaner)> _ToClean = new();
    /// <summary>
    /// For clearing user data from state blocks when they are dead.
    /// </summary>
    /// <param name="source">clean this users data</param>
    /// <param name="block">from this</param>
    /// <param name="cleaner">with this</param>
    public static void DoClean(EnemyController source, StateBlock block, System.Action<EnemyController> cleaner)
    {
        if (_ToClean.ContainsKey((source.GetInstanceID(),block.GetInstanceID())))
        {
            return;
        }
        _ToClean.Add((source.GetInstanceID(),block.GetInstanceID()), (source, cleaner));

    }
    private IEnumerator Cleaner()
    {
        while (enabled)
        {
            List<(int user, int block)> clear = new();
            yield return new WaitForSecondsRealtime(_timeForAICleanup);
            
            foreach (var item in _ToClean)
            {
                (EnemyController source, System.Action<EnemyController> cleaner) = item.Value;
                Debug.Log("scan" + item.Key);
                //Debug.Log(item.Value);
                if (source == null)
                {
                    cleaner?.Invoke(source);
                    clear.Add(item.Key);
                }
                else if (source.DoDie)
                {
                    cleaner?.Invoke(source);
                    clear.Add(item.Key);
                }
                yield return new WaitForEndOfFrame();
            }
            foreach (var item in clear)
            {
                Debug.Log(nameof(clear) + item);
                _ToClean.Remove(item);
                yield return new WaitForEndOfFrame();
            }
            
        }
    }

    private void OnDestroy()
    {
        enabled = false;
    }



}
