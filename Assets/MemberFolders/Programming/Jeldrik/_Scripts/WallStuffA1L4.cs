using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStuffA1L4 : MonoBehaviour
{

    [SerializeField] private GameObject wallPart1;
    [SerializeField] private GameObject wallPart2;

    public bool secondDoor;
    public List<GameObject> enemiesToKill;
    public bool completed;

    public bool left;

    public GameObject leftHolder;
    public GameObject rightHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (secondDoor)
        {
            if (left)
            {
                if (leftHolder.transform.childCount == 0)
                {
                    wallPart1.SetActive(false);
                    wallPart2.SetActive(false);
                    completed = true;
                }
            }
            else
            {
                if (rightHolder.transform.childCount == 0)
                {
                    wallPart1.SetActive(false);
                    wallPart2.SetActive(false);
                    completed = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.HasTag("Player") && !secondDoor)
        {
            wallPart1.SetActive(true);
            wallPart2.SetActive(true);
        }
    }
}
