using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageObject : Pooltoy
{

    private Rigidbody _rigid;
    private FDOProperties _properties;
    private Transform _cameraTransform;
    private float _startTime;
    [SerializeField] private TextMeshProUGUI text;
    public override Rigidbody Rb => _rigid;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();

    }


    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    public override void SetProperties(Properties properties)
    {
        _properties = (FDOProperties)properties;

        text.text = _properties.ToBeDisplayed;
        _startTime = Time.time;
    }

    

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, _cameraTransform.up);
        var original = text.color;

        var alpha = _properties.FadeRate.Evaluate((Time.time - _startTime) / _properties.FadeTime);

        text.color = new Color(original.r, original.g, original.b, alpha);
    }
    

    

}
[System.Serializable]
public class FDOProperties : Properties
{
    public string ToBeDisplayed;
    public float FadeTime;
    public AnimationCurve FadeRate;


}