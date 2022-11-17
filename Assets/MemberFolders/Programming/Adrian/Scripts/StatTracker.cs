using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/StatManager", order = 1)]
public class StatTracker : ScriptableObject
{
    public int Kills;
    public int Collectibles;

    public float DamageDone;
    public float DamageTaken;

    public float TimePlayed;
}
