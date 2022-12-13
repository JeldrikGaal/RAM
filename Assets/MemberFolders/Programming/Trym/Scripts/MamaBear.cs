using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaBear : MonoBehaviour
{
    [SerializeField] int _area;


    [SerializeField] string _name;

    [SerializeField]private ExternalCollider _hammer;
    [SerializeField] private string _attackNameH;
    [SerializeField] private string _attackNameH2;
    private EnemyAttackStats _attackStatsH;
    private EnemyAttackStats _attackStatsH2;
    

    [SerializeField]private ExternalCollider _shield;
    [SerializeField] private string _attackNameS;
    [SerializeField] private string _attackNameS2;
    EnemyStats _stats;
    private EnemyAttackStats _attackStatsS;
    private EnemyAttackStats _attackStatsS2;
    [SerializeField] float _hammerForce;
    [SerializeField]private EnemyController _controller;
    RammyController _rammy;
    Rigidbody _rammyRigid;
    private bool _active;

    private void Awake()
    {
        _stats = ImportManager.GetEnemyStats(_name);
        _attackStatsH = _stats.Attacks[_attackNameH];
        _attackStatsH2 = _stats.Attacks[_attackNameH2];
        _attackStatsS = _stats.Attacks[_attackNameS];
        _attackStatsS2 = _stats.Attacks[_attackNameS2];

        // Initializing the hammer.
        _hammer.TriggerStayEvent += HETriggerStay;
        _hammer.TriggerEnterEvent += HETriggerEnter;
        _hammer.TriggerExitEvent += HETriggerExit;

        // Initializing the shield
        _shield.CollisionEnterEvent += SCollisionEnter;
    }

    private void SCollisionEnter(Collision obj)
    {
        if (CheckReturn(obj.collider))
        {
            return;
        }
        GetRammy(obj.collider);

        switch (AI_StageCheck.Check())
        {
            case 1:
                _rammy.TakeDamageRammy(_attackStatsS.Damage(1));
                break;
            case 3:
                _rammy.TakeDamageRammy(_attackStatsS2.Damage(1));
                break;
            default:
                break;
        }
    }

    private void HETriggerEnter(Collider other)
    {
        if (CheckReturn(other))
        {
            return;
        }
        GetRammy(other);

        switch (AI_StageCheck.Check())
        {
            case 1:
                _rammy.TakeDamageRammy(_attackStatsH.Damage(1));
                break;
            case 3:
                _rammy.TakeDamageRammy(_attackStatsH2.Damage(1));
                break;
            default:
                break;
        }
    }

    private void GetRammy(Collider other)
    {
        if (_rammy == null)
        {
            _rammy = other.GetComponent<RammyController>();
            _rammyRigid = other.GetComponent<Rigidbody>();
        }
    }

    private bool CheckReturn(Collider other) => !other.gameObject.HasTag("player") || !_active;

    private void HETriggerStay(Collider other)
    {
        if (CheckReturn(other))
        {
            return;
        }

        _rammyRigid.AddForce((_hammer.transform.position - _rammy.transform.position).normalized * _hammerForce);
        
    }

    private void HETriggerExit(Collider other)
    {
        if (CheckReturn(other))
        {
            return;
        }
        _rammyRigid.velocity = Vector3.zero;
    }
    public void Active(bool active) => _active = active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AI_StageCheck.Check() == 2)
        {
            _hammer.gameObject.SetActive(false);
            _shield.gameObject.SetActive(false);
        }
        else if(AI_StageCheck.Check() == 3)
        {
            _hammer.gameObject.SetActive(true);
            _shield.gameObject.SetActive(true);
        }
    }
}
