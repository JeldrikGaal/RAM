using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnAbilityField : MonoBehaviour
{
    [SerializeField] private int _abilityToLearn;
    [SerializeField] private Sprite _abilityIcon;
    [SerializeField] private bool _stupid;

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
            if (!_stupid)
            {
                other.transform.GetComponent<RammyController>().LearnAbility(_abilityToLearn);
            }
            GameObject.FindObjectOfType<UIAbilityUnlock>().EnableUI(_abilityIcon, _description);
        }
    }
}
