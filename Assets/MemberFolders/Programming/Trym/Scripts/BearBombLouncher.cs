using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBombLouncher : MonoBehaviour
{



    public void Lounch(GenericBearBomb bomb, AnimationCurve relativeTrajectory,AnimationCurve relativeSpeed, float speed, Vector3 origin, Vector3 target) => GameManager.HandleCoroutine(ManageTrajectory(Instantiate(bomb, origin,Quaternion.identity,null), relativeTrajectory, speed,relativeSpeed, origin, target));


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
                Vector2 newPos = currentPosition2D + dir * (relativeSpeed.Evaluate(relativePositionInSequence) * speed * Time.deltaTime);
                float newHeight = originHeight + (relativeTrajectory.Evaluate(relativePositionInSequence) * distance);
                rigid.MovePosition(new Vector3(newPos.x, newHeight, newPos.y)); 
            }
            else
            {
                rigid.isKinematic = false;
                if (rigid.velocity.magnitude > 10)
                {
                    rigid.velocity = rigid.velocity / 2;
                }

            }

            

            if (bomb.transform.position.y <= -10)
            {
                Destroy(bomb.gameObject);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        rigid.velocity = Vector3.zero;
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
