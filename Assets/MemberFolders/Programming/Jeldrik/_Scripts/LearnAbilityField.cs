using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnAbilityField : MonoBehaviour
{
    [SerializeField] private int _abilityToLearn;
    [SerializeField] private Sprite _abilityIcon;

    [TextArea]
    [SerializeField] private string _description;

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
            GameObject.FindObjectOfType<UIAbilityUnlock>().EnableUI(_abilityIcon, _description);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagManager.HasTag(other.gameObject, "player"))
        {
            GameObject.FindObjectOfType<UIAbilityUnlock>().DisableUI();
        }
          
    }
}
