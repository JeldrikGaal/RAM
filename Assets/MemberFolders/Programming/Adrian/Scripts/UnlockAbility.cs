using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAbility : MonoBehaviour
{
    [SerializeField] private int _abilityIndex;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LearnAbility(_abilityIndex));
    }

    private IEnumerator LearnAbility(int index)
    {
        yield return new WaitForSeconds(1);
        GameObject.FindObjectOfType<RammyController>().LearnAbility(index);
    }
}
