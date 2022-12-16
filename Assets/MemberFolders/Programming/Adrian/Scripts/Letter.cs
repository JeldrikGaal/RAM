using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string _letterText;

    private bool _isCounted;
    private bool _isRead = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isRead && other.tag == "Player")
        {
            var letter = GameObject.FindObjectOfType<UIButtonReset>().transform.parent.gameObject;
            if (!_isCounted)
            {
                other.GetComponent<RammyController>().lettersCollected++;
                _isCounted = true;
            }
            letter.GetComponentInChildren<Image>().enabled = true;
            letter.GetComponentInChildren<TMP_Text>().enabled = true;
            letter.GetComponentInChildren<Button>().enabled = true;
            letter.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().enabled = true;
            letter.GetComponentInChildren<TMP_Text>().text = _letterText;
            // Cursor.visible = true;
            Time.timeScale = 0;
            _isCounted = true;
            _isRead = true;
            Destroy(gameObject);
        }
    }
}
