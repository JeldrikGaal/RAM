using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public float damage = 1.5f;

    [SerializeField] private GameObject _eggsplosion;
    [SerializeField] private GameObject _crackedEgg;
    [SerializeField] private float _effectOffset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        _eggsplosion.transform.parent = this.transform;
        _eggsplosion.transform.localPosition = new Vector3(0, 0, 0);
        _eggsplosion.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RammyController>() != null)
        {
            collision.gameObject.GetComponent<RammyController>().TakeDamageRammy(damage);

            // VFX:

            _eggsplosion.SetActive(true);
            _eggsplosion.transform.parent = null;
            _eggsplosion.transform.localScale = new Vector3(1,1,1);

            RaycastHit hit;
            var layer = 1 << 10;
            if (Physics.Raycast(collision.transform.position + new Vector3(0, 100, 0), /*transform.TransformDirection(-Vector3.up)*/ -Vector3.up, out hit, Mathf.Infinity, layer))
            {
                var crackedEgg = Instantiate(_crackedEgg, hit.point + _effectOffset * Vector3.up, Quaternion.Euler(0,Random.Range(0,360),0));
                Destroy(crackedEgg, 20f);
            }


            gameObject.SetActive(false);
        }
        else
        {
            //Destroy(gameObject, 5);
        }
    }
}
