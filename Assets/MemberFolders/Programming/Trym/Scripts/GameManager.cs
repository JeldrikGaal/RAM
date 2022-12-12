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
        SaveNload.Save(new SaveData());
        print(SaveNload.Load());

        PauseGame.PauseEvent += (bool paused) => print(paused);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //stats = Stats.GetData();
    }

    /// <summary>
    /// For continuing running coroutine despite the caller being destroid or disabeled.
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static Coroutine HandleCoroutine(IEnumerator enumerator) => _Instance.StartCoroutine(enumerator);


    [SerializeField] private float _timeForAICleanup = 10;
    private static readonly Dictionary<int, (EnemyController source, System.Action<EnemyController> cleaner)> _ToClean = new();

    public static void DoClean(EnemyController source, System.Action<EnemyController> cleaner)
    {
        if (_ToClean.ContainsKey(source.GetInstanceID()))
        {
            return;
        }
        _ToClean.Add(source.GetInstanceID(), (source, cleaner));

    }
    private IEnumerator Cleaner()
    {
        while (enabled)
        {
            yield return new WaitForSecondsRealtime(_timeForAICleanup);
            List<int> clear = new();
            foreach (var item in _ToClean)
            {
                (EnemyController source, System.Action<EnemyController> cleaner) = item.Value;

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
                _ToClean.Remove(item);
                yield return new WaitForEndOfFrame();
            }
            
        }
    }
    

}
