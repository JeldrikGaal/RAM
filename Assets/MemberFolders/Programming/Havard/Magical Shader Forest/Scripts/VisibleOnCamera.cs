using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleOnCamera : MonoBehaviour
{
    [SerializeField] private GameObject _thisCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameVisible()
    {
        _thisCam.SetActive(true);
        print("Visible!");
    }

    void OnBecameInvisible()
    {
        _thisCam.SetActive(false);
        print("Hidden!");
    }

}
