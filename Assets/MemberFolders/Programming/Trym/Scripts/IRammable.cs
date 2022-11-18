using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make it so something happens when rammy rams the object.
/// </summary>
public interface IRammable
{
    /// <summary>
    /// Registers that Rammy Rammed the object.
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public bool Hit(GameObject g);

}
