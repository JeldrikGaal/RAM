using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class BuildSceneUtility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn(GameObject g, Vector3 pos, Quaternion rot)
    {
        StartCoroutine(RespawnAfterTime(g, pos, rot));
    }

    IEnumerator RespawnAfterTime(GameObject g, Vector3 pos, Quaternion rot)
    {

        yield return new WaitForSeconds(3);
        Instantiate(g, pos, rot);

    }

}
