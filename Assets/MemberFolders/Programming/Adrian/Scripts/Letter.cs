using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    [SerializeField] private Canvas _letterCanvas;

    [SerializeField] private TMP_Text _letterTextbox;

    [TextArea]
    [SerializeField] private string _letterText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<RammyController>().lettersCollected += 1;
            _letterTextbox.text = _letterText;
            _letterCanvas.enabled = true;
            Time.timeScale = 0;
        }
    }

    public void LetterUiTimeReset()
    {
        Time.timeScale = 1;
        _letterCanvas.enabled = false;
    }
}