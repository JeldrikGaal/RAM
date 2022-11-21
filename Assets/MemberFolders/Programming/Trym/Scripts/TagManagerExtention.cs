using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagManagerExtention 
{
    public static bool HasTag(this GameObject gameObject, string tag) => TagManager.HasTag(gameObject, tag);



}
