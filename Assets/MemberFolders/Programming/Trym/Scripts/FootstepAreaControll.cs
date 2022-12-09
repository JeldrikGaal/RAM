using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FootstepAreaControll : TagHolder
{
    [SerializeField] private FootstepsEmitter.Surface surface;
    [HideInInspector]
    //public List<string> tags { get; set; }

    public FootstepsEmitter.Surface GetSurface() => surface;
    // Start is called before the first frame update
    void Start()
    {
        tags.Add("ground_mat".ToUpper());
        tags.Add(surface.ToString().ToUpper());
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.HasTag("tag");
    }
}
