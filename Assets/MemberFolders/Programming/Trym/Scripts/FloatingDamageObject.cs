using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageObject : Pooltoy
{

    [SerializeField] private Rigidbody _rigid;
    private FDOProperties _properties;
    private Transform _cameraTransform;
    private float _startTime;
    [SerializeField] private TextMeshProUGUI text;
    public override Rigidbody Rb => _rigid;


    private void Start()
    {
        
        _cameraTransform = Camera.main.transform;
    }

    public override void SetProperties(IProperties properties)
    {
        _properties = (FDOProperties)properties;

        text.text = _properties.ToBeDisplayed;
        _startTime = Time.time;
        
    }


    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, _cameraTransform.up);

        // Modifying the transparrency over time.
        var original = text.color;
        var alpha = _properties.AlphaOverTime.Evaluate((Time.time - _startTime) / _properties.FadeTime);
        text.color = new Color(original.r, original.g, original.b, alpha);

        // Modifies the font size over time
        text.fontSize = _properties.MinSize + ((_properties.MaxSise - _properties.MinSize) * _properties.SizeOverTime.Evaluate((Time.time - _startTime) / _properties.FadeTime));

    }
    

    

}
[System.Serializable]
public class FDOProperties : IProperties
{
    [HideInInspector]
    public string ToBeDisplayed;

    public float FadeTime;
    [Tooltip("Text Transparrency over time")]
    public AnimationCurve AlphaOverTime;

    public float MinSize, MaxSise;
    [Tooltip("0 is Min Size, 1 is Max Size")]
    public AnimationCurve SizeOverTime;

}