using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private StatTracker _stats;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // SPIN
        transform.Rotate(new Vector3(1f, 1f, 1f));


        // Testing because I don't have a player :(
        if (Input.GetKeyDown(KeyCode.P))
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        // Increase the stat to track number of collected items
        _stats.Collectibles++;

        // Maybe play some audio

        // DESTROY >:)
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CollectItem();
        }
    }
}
