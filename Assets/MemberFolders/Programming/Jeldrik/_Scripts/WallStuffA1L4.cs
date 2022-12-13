using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStuffA1L4 : MonoBehaviour
{

    [SerializeField] private GameObject wallPart1;
    [SerializeField] private GameObject wallPart2;

    public bool completed;

    public GameObject enemyHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHolder.transform.childCount == 0 && !completed) 
        {
            wallPart1.SetActive(false);
            wallPart2.SetActive(false);
            completed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TTTT");
        if (other.gameObject.HasTag("Player"))
        {
            wallPart1.SetActive(true);
            wallPart2.SetActive(true);
        }
    }
}
