using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButtonReset : MonoBehaviour
{
    public void LetterUiTimeReset()
    {
        // Cursor.visible = false;
        Time.timeScale = 1;
        transform.parent.GetComponentInChildren<Image>().enabled = false;
        transform.parent.GetComponentInChildren<TMP_Text>().enabled = false;
        transform.parent.GetComponentInChildren<Button>().enabled = false;
        transform.parent.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().enabled = false;
    }
}
