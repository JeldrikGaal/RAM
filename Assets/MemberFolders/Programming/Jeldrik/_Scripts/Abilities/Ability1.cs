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

        // If the ability is activated
        if (_jumping)
        {
            // Increased the jump timer
            _jumpTimer += Time.deltaTime;

            // Sets the y position based on the animation curve
            transform.position = new Vector3(transform.position.x, _startPos.y + _yPosCurve.Evaluate(_jumpTimer), transform.position.z);

            // Blocks the character from receiving movement inputs
            _controller.BlockPlayerMovment();

            // If the timer has passed the last keyframe in the animation
            if (_jumpTimer > _yPosCurve.keys[_yPosCurve.keys.Length - 1].time)
            {
                // Sets jumping to false
                _jumping = false;

                // Unblocks the movement
                _controller.UnBlockPlayerMovement();

                // Spawns the damage area at the players feet
                var jumpArea = Instantiate(_damageArea, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);

                // Destroy the damage area after 0,5 seconds
                Destroy(jumpArea, 0.5f);

                // Checks if the ability is upgraded or not
                if (Upgraded)
                {
                    // Starts the coroutine to spawn the upgraded damage area
                    StartCoroutine(SpawnUpgradedArea());
                }
            }
        }
    }
    override public void Activate()
    {
        // Activates the ability
        _jumping = true;

        // Resets the jumping timer
        _jumpTimer = 0f;

        // Saves the startpos of the jump
        _startPos = transform.position;


        Debug.Log(_controller.transform.position);
        Debug.Log("Ability 1");
    }

    private IEnumerator SpawnUpgradedArea()
    {
        // Waits for 0,5 seconds
        yield return new WaitForSeconds(0.5f);

        // Spawns the upgraded damage area at the players feet
        var upgradedArea = Instantiate(_upgradedArea, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);

        // Destroys the area after 0,5 seconds
        Destroy(upgradedArea, 0.5f);
    }
}
