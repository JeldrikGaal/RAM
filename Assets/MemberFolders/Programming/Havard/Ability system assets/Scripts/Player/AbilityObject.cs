using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Ability")]
public class AbilityObject : ScriptableObject
{
    public int abilityNumber; // Naming convention is wrong here, needs UpperCamelCase. Waiting until we have the real sprites to fix.
    public Sprite icon;
    public int AbilityKills;
    public string AbilityDescription;
    public Sprite VisualOfAbility;
}
