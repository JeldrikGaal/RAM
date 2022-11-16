using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Stats.StatsData stats;
    // Start is called before the first frame update
    void Start()
    {
        // Just testing serialization
        SaveNload.Save(new SaveData());
        print(SaveNload.Load());
        
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        stats = Stats.GetData();
    }
    

    
    

}
