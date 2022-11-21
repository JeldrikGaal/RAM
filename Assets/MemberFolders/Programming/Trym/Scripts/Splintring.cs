using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splintring : MonoBehaviour, IRammable
{
    [SerializeField] int _amountOfSplinters;
    [SerializeField] float _velosity;
    [Tooltip("how far from midpoint of object, in opposit direction off hit modifyed by spread")]
    [SerializeField] float _spawnOffset;
    [SerializeField] float _spread;
    [SerializeField] float _maxLifetime, _minLifetime;

    /// <summary>
    /// the IRammable Hit funktion.
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public bool Hit(GameObject g)
    {
        Splinter(g);
        return true;
    }

    // "Spawns" all the splinters if there is not enough available, it will try again next frame.
    private void Splinter(GameObject g )
    {
        for (int i = 0; i < _amountOfSplinters; i++ )
        {
            var dir = (((
                transform.position * 2 - g.transform.position) * 3)+ new Vector3(
                    Random.Range(-_spread, _spread), 
                    Random.Range(-_spread, _spread), 
                    Random.Range(-_spread, _spread)
                    )).normalized;
            SplinterManager.RequestSplinter(transform.position + (dir * _spawnOffset), dir, _velosity, Random.Range(_minLifetime,_maxLifetime));
        }
    }



    
}
