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

    /// <summary>
    /// the IRammable Hit funktion.
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public bool Hit(GameObject g)
    {
        StartCoroutine(Splinter(g));
        return true;
    }

    // "Spawns" all the splinters if there is not enough available, it will try again next frame.
    private IEnumerator Splinter(GameObject g )
    {
        for (int i = 0; i < _amountOfSplinters; )
        {
            var dir = (((
                transform.position * 2 - g.transform.position) * 3)+ new Vector3(
                    Random.Range(-_spread, _spread), 
                    Random.Range(-_spread, _spread), 
                    Random.Range(-_spread, _spread)
                    )).normalized;
            if (SplinterManager.GetSplinter(transform.position + (dir* _spawnOffset ),dir,_velosity))
            {
                i++;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }



    
}
