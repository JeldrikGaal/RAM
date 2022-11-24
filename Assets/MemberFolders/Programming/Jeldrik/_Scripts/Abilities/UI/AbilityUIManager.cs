using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{
    [SerializeField] private RammyController _controller;

    private List<Transform> _abilityBlocks;
    private List<Image> _abilityImages;
    private List<Image> _coolDownCircles;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            _abilityBlocks.Add(transform.GetChild(i));
        }
        foreach (Transform t in _abilityBlocks)
        {
            _coolDownCircles.Add(t.GetComponentInChildren<Image>());  
            _abilityImages.Add(t.GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAbilityClockToPercent(int index, float percentage)
    {
        _coolDownCircles[index].fillAmount = percentage;
    }

    public void AbilityBeingUsed(int index)
    {
        for (int i = 0; i < _abilityBlocks.Count; i++)
        {
            if (i == index)
            {
                EnableAbility(i);
            }
            else 
            {
                DisableAbility(i);
            }
        }
    }

    private void EnableAbility(int index)
    {
        _abilityImages[index].color = Color.gray;
    }

    private void DisableAbility(int index)
    {
        _abilityImages[index].color = Color.white;
    }
}
