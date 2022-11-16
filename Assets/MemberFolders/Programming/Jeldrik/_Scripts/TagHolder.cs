using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagHolder : MonoBehaviour
{
    // List with all custom tags
    public List<string> tags = new List<string>();

    private void Start()
    {
        // making all tags uppercase so spelling issues get reduced
        int i = 0;
        foreach (string tag in tags)
        {
            tags[i] = tag.ToUpper();
            i++;
        }
    }
}
