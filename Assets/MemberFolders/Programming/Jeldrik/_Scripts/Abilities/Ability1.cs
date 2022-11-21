using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability1 : Abilities
{
    [SerializeField] private bool _jumping;

    /*[HideInInspector]*/
    public bool Upgraded;

    [SerializeField] private AnimationCurve _yPosCurve;

    [SerializeField] private GameObject _damageArea;
    [SerializeField] private GameObject _upgradedArea;

    private Vector3 _startPos;

    private float _jumpTimer;

    private Rigidbody _rb;


    public override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }
    override public void Update()
    {
        base.Update();

        if (_jumping)
        {
            _jumpTimer += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, _startPos.y + _yPosCurve.Evaluate(_jumpTimer), transform.position.z);


            _controller.BlockPlayerMovment();

            if (_jumpTimer > _yPosCurve.keys[_yPosCurve.keys.Length - 1].time)
            {
                _jumping = false;
                _controller.UnBlockPlayerMovement();

                // transform.position = new Vector3(transform.position.x, _startPos.y, transform.position.z);

                var jumpArea = Instantiate(_damageArea, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
                Destroy(jumpArea, 0.5f);
                if (Upgraded)
                {
                    StartCoroutine(SpawnUpgradedArea());
                }
            }
        }
    }
    override public void Activate()
    {
        _jumping = true;
        _jumpTimer = 0f;
        _startPos = transform.position;


        Debug.Log(_controller.transform.position);
        Debug.Log("Ability 1");
    }

    private IEnumerator SpawnUpgradedArea()
    {
        yield return new WaitForSeconds(0.5f);
        var upgradedArea = Instantiate(_upgradedArea, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        Destroy(upgradedArea, 1f);
    }
}
