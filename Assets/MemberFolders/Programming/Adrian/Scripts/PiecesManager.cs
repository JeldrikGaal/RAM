using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesManager : MonoBehaviour
{
    // Input: Array of gameobjects that can spawn, Position to spawn at, Min and max x, y and z values to add force, a force multiplier, 
    // amount of objects to spawn, lifespan of the objects
    public void SpawnPieces(GameObject[] objects, Vector3 position, Vector2 XDirection, Vector2 YDirection, Vector2 ZDirection, float forceMultiplier, int amount, float lifespan)
    {
        // Loop to spawn correct amount of pieces
        for (int i = 0; i < amount; i++)
        {
            print(i);
            // Instatiate a randomly selected gameobject from the given array at the given position
            var instance = Instantiate(objects[Random.Range(0, objects.Length)], position, Quaternion.identity);
            // Add force in a random direction with the given parameters
            instance.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(XDirection.x, XDirection.y), Random.Range(YDirection.x, YDirection.y) * forceMultiplier, Random.Range(ZDirection.x, ZDirection.y)), ForceMode.Impulse);
            // Destroy the gameobject after a given amount of seconds
            Destroy(instance, lifespan);

            // Maybe remove rigidbodies after a while so we can keep the pieces
        }
    }
}
