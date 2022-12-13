using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnAbilityField : MonoBehaviour
{
    [SerializeField] private int _abilityToLearn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagManager.HasTag(other.gameObject, "player"))
        {
            other.transform.GetComponent<RammyController>().LearnAbility(_abilityToLearn);
        }
    }
}
