using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadVolumeSlider : MonoBehaviour
{

    private MusicLogic _musicLogic;
    [SerializeField] Slider _masterVolume;
    [SerializeField] Slider _soundEffects;
    [SerializeField] Slider _musicEffects;

    // Start is called before the first frame update
    void Start()
    {
        //_musicLogic = Camera.main.GetComponentInChildren<MusicLogic>();
        _musicLogic = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MusicLogic>();
    }

    public void SetMasterVol()
    {
        _musicLogic.SetVolume(_masterVolume.value / 1000);
    }


}
