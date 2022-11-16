using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holding functions for handling custom Tags to allow usage of more than one tag for each object
/// </summary>
public class TagManager : MonoBehaviour
{
    // Retrieve a List of all custom tags a GameObject has
    static public List<string> GetTags(GameObject g)
    {
        if (g.GetComponent<TagHolder>())
        {
            return g.GetComponent<TagHolder>().tags;
        }
        return new List<string>();
    }

    // Check if gameobject g has a certain custom tag
    static public bool HasTag(GameObject g, string tag)
    {
        tag = tag.ToUpper();
        if (g.GetComponent<TagHolder>())
        {
            return g.GetComponent<TagHolder>().tags.Contains(tag);
        }
        return false;
    }

}
