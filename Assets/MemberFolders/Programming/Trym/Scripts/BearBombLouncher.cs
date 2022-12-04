using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBombLouncher : MonoBehaviour
{


    /// <summary>
    /// Instantiates the bomb and animates it so it follows the relativeTrajectory from the origin to the target, at speed times relativeSpeed over relative distance.
    /// </summary>
    /// <param name="bomb"></param>
    /// <param name="relativeTrajectory">trajectory relative to the distance</param>
    /// <param name="relativeSpeed"> animation curve modifying speed over relative distance </param>
    /// <param name="speed"></param>
    /// <param name="origin">Where it comes from</param>
    /// <param name="target">Where it's going</param>
    public void Lounch(GenericBearBomb bomb, AnimationCurve relativeTrajectory,AnimationCurve relativeSpeed, float speed, Vector3 origin, Vector3 target) => GameManager.HandleCoroutine(ManageTrajectory(Instantiate(bomb, origin,Quaternion.identity,null), relativeTrajectory, speed,relativeSpeed, origin, target));

    // does the actual work
    private static IEnumerator ManageTrajectory(GenericBearBomb bomb, AnimationCurve relativeTrajectory, float speed,AnimationCurve relativeSpeed, Vector3 origin, Vector3 target)
    {
        Vector2 target2D = new Vector2(target.x, target.z);
        Vector2 origin2D = new Vector2(origin.x, origin.z);
        Vector2 currentPosition2D;
        float originHeight = origin.y;
        Vector2 dir = (target2D - origin2D).normalized;
        var rigid = bomb.Rb;
        float distance = Vector2.Distance(target2D, origin2D);
        float relativePositionInSequence;

        while (!bomb.HitCheck)
        {
            currentPosition2D = new(bomb.Rb.position.x, bomb.Rb.position.z);



            relativePositionInSequence = (Vector2.Distance(origin2D, currentPosition2D) / distance);

            if (relativePositionInSequence <= 1)
            {

                // calculates the next position and moves the bomb to it
                Vector2 newPos = currentPosition2D + dir * (relativeSpeed.Evaluate(relativePositionInSequence) * speed * Time.fixedDeltaTime );
                float newHeight = originHeight + (relativeTrajectory.Evaluate(relativePositionInSequence) * distance);
                rigid.MovePosition(new Vector3(newPos.x, newHeight, newPos.y)); 
            }
            else
            {
                // for preventing it from crashing trough the ground.
                rigid.isKinematic = false;
                if (rigid.velocity.magnitude > 20)
                {
                    rigid.velocity /= 1.2f;
                }

            }

            

            if (bomb.transform.position.y <= -10)
            {
                Destroy(bomb.gameObject);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        //rigid.velocity = Vector3.zero;
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
