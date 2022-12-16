using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    bool _titleScreenPassed = false;
    
    [SerializeField] GameObject _backgroundTitle;
    [SerializeField] GameObject _background;
    [SerializeField] GameObject _buttons;
    [SerializeField] bool[] _tabBools = new bool[4];
    [SerializeField] GameObject[] _tabPanels = new GameObject[4];
    [SerializeField] GameObject[] _tabs = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_backgroundTitle != null)
        {
            if (Input.anyKeyDown && _titleScreenPassed == false)
            {
                OpenClosePanel(_backgroundTitle);
                OpenClosePanel(_background);
                OpenClosePanel(_buttons);
                _titleScreenPassed = true;
            }
        }
        
    }

    
    public void UpdateTabs(int tabId)
    {
        for (int i = 0; i < _tabBools.Length; i++)
        {
            if (i == tabId)
            {
                _tabBools[i] = true;
            }
            else
            {
                _tabBools[i] = false;
            }
        }

        for (int i = 0; i < _tabBools.Length; i++)
        {
            _tabPanels[i].SetActive(_tabBools[i]);
            _tabs[i].GetComponent<Image>().enabled = _tabBools[i];
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// Use this to load a spesific level or scene.
    /// </summary>
    /// <param name="levelBuildIndex"></param>
    public void LoadGivenLevel(int levelBuildIndex)
    {
        SceneManager.LoadScene(levelBuildIndex);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// Use this to (de)activate a gameObject.
    /// </summary>
    /// <param name="panel"></param>
    public void OpenClosePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf!);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    //Animation trigger methods
    public void OpenOptionMenu(GameObject forAnimator)
    {
        if (_backgroundTitle != null)
        {
            forAnimator.GetComponent<Animator>().SetTrigger("OpenOptionsPanel");
        }
        
    }
    public void CloseOptionMenu(GameObject forAnimator)
    {
        if (_backgroundTitle != null)
        {
            forAnimator.GetComponent<Animator>().SetTrigger("CloseOptionsPanel");
        }
        
    }
}
