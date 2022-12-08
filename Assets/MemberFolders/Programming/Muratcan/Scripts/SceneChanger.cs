using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    bool _titleScreenPassed = false;
    [SerializeField] GameObject _backgroundTitle;
    [SerializeField] GameObject _background;
    [SerializeField] GameObject _buttons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && _titleScreenPassed == false)
        {
            OpenClosePanel(_backgroundTitle);
            OpenClosePanel(_background);
            OpenClosePanel(_buttons);
            _titleScreenPassed = true;
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
        forAnimator.GetComponent<Animator>().SetTrigger("OpenOptionsPanel");
    }
    public void CloseOptionMenu(GameObject forAnimator)
    {
        forAnimator.GetComponent<Animator>().SetTrigger("CloseOptionsPanel");
    }
}
