using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStuffA1L4 : MonoBehaviour
{

    [SerializeField] private GameObject wallPart1;
    [SerializeField] private GameObject wallPart2;

    public bool completed;

    public GameObject enemyHolder;

    private int _childCount;
    // Start is called before the first frame update
    void Start()
    {
        _childCount = enemyHolder.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHolder.transform.childCount < _childCount * 0.25f && !completed) 
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
