using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIncreaser : MonoBehaviour
{
    [Range(0, 1)] public float Value;

    [SerializeField] private int _amount = 4;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Vector3 _offset;

    private List<GameObject> _arrows;

    // Start is called before the first frame update
    void Start()
    {
        _arrows = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentAmount = (int) map(Value, 0, 1, 1, _amount);

        for (int i = 1; i < currentAmount; i++)
        {
            SpawnArrow(i);   
        }
        if(Value <= 0)
        {
            DestroyArrows();
        }
    }

    private void SpawnArrow(int number)
    {
        GameObject existanceCheck = null;
        for (int i = 0; i < _arrows.Count; i++)
        {
            if (_arrows[i].name == "Arrow"+number)
            {
                existanceCheck = _arrows[i];
                break;
            }
        }
        if (!_arrows.Contains(existanceCheck))
        {
            var newArrow = Instantiate(_arrow, this.transform);
            newArrow.transform.localPosition = _offset * number;
            newArrow.name = "Arrow" + number;
            _arrows.Add(newArrow);
        }
    }

    public void DestroyArrows()
    {
        Value = 0;
        foreach (var item in _arrows)
        {
            Destroy(item.gameObject);
        }
        _arrows = new List<GameObject>();
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
