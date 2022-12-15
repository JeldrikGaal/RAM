using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    [SerializeField] GameObject _pageParent;
    [SerializeField] Sprite[] _pageSprites;
    [SerializeField] GameObject[] _pageObjects;
    [SerializeField] List<Pages> _pages = new List<Pages>();
    [SerializeField] Image _pageLeft;
    [SerializeField] Image _pageRight;
    [SerializeField] int _currentLeftNum;
    [SerializeField] int _currentRightNum;
    [SerializeField] int[] _tabs = new int[6];

    private void Awake()
    {
        //Takes all the sprites from the array and puts them in a list with their numbers
        for (int i = 0; i < _pageObjects.Length; i++)
        {
            _pages.Add(new Pages() { pageNum = i, pageObject = _pageObjects[i] });
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //For test purposes
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    OpenTheBook();
        //}
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    ChangeThePage(false);
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    ChangeThePage(true);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    GoToTab(4);
        //}
    }

    /// <summary>
    /// Call this any time you open the journal.
    /// </summary>
    public void OpenTheBook()
    {
        if (_pages.Count == 0)
        {
            Debug.LogError("Pages are not assigned in the inspector.");
        }
        else
        {
            //_pageLeft.sprite = _pages[0].pageObject;
            _pages[0].pageObject.SetActive(true);
            _pages[0].pageObject.transform.parent = _pageLeft.transform;
            _pages[0].pageObject.transform.localPosition = Vector3.zero;
            _currentLeftNum = 0;
            //_pageRight.sprite = _pages[1].pageObject;
            _pages[1].pageObject.SetActive(true);
            _pages[1].pageObject.transform.parent = _pageRight.transform;
            _pages[1].pageObject.transform.localPosition = Vector3.zero;
            _currentRightNum = 1;
        }
    }

    /// <summary>
    /// Changes the page forward or backward. (true = forward, false = backward)
    /// </summary>
    /// <param name="direction"></param>
    public void ChangeThePage(bool direction)
    {
        //Checks the direction and if there are enough pages to said direction
        if (direction && _currentLeftNum < _pageObjects.Length - 3)
        {
            _pages[_currentLeftNum].pageObject.SetActive(false);
            _pages[_currentLeftNum].pageObject.transform.parent = _pageParent.transform;
            _pages[_currentRightNum].pageObject.SetActive(false);
            _pages[_currentRightNum].pageObject.transform.parent = _pageParent.transform;
            _currentLeftNum += 2;
            _currentRightNum += 2;
            _pages[_currentLeftNum].pageObject.SetActive(true);
            _pages[_currentLeftNum].pageObject.transform.parent = _pageLeft.transform;
            _pages[_currentLeftNum].pageObject.transform.localPosition = Vector3.zero;
            _pages[_currentRightNum].pageObject.SetActive(true);
            _pages[_currentRightNum].pageObject.transform.parent = _pageRight.transform;
            _pages[_currentRightNum].pageObject.transform.localPosition = Vector3.zero;
            //_pageLeft.sprite = _pages[_currentLeftNum].pageObject;
            //_pageRight.sprite = _pages[_currentRightNum].pageObject;
        }
        else if (direction == false && _currentLeftNum > 1)
        {
            _pages[_currentLeftNum].pageObject.SetActive(false);
            _pages[_currentLeftNum].pageObject.transform.parent = _pageParent.transform;
            _pages[_currentRightNum].pageObject.SetActive(false);
            _pages[_currentRightNum].pageObject.transform.parent = _pageParent.transform;
            _currentLeftNum -= 2;
            _currentRightNum -= 2;
            _pages[_currentLeftNum].pageObject.SetActive(true);
            _pages[_currentLeftNum].pageObject.transform.parent = _pageLeft.transform;
            _pages[_currentLeftNum].pageObject.transform.localPosition = Vector3.zero;
            _pages[_currentRightNum].pageObject.SetActive(true);
            _pages[_currentRightNum].pageObject.transform.parent = _pageRight.transform;
            _pages[_currentRightNum].pageObject.transform.localPosition = Vector3.zero;
            //_pageLeft.sprite = _pages[_currentLeftNum].pageObject;
            //_pageRight.sprite = _pages[_currentRightNum].pageObject;
        }
    }

    /// <summary>
    /// Use this to go to a specific page using the tabs.
    /// </summary>
    /// <param name="tabLocation"></param>
    public void GoToTab(int tabLocation)
    {
        //Checks if the tab number is even or odd and places the page on the correct side acccordingly
        if (tabLocation % 2 == 0)
        {
            _pages[_currentLeftNum].pageObject.SetActive(false);
            _pages[_currentLeftNum].pageObject.transform.parent = _pageParent.transform;
            _pages[_currentRightNum].pageObject.SetActive(false);
            _pages[_currentRightNum].pageObject.transform.parent = _pageParent.transform;
            _currentLeftNum = tabLocation;
            _currentRightNum = tabLocation + 1;
            _pages[_currentLeftNum].pageObject.SetActive(true);
            _pages[_currentLeftNum].pageObject.transform.parent = _pageLeft.transform;
            _pages[_currentLeftNum].pageObject.transform.localPosition = Vector3.zero;
            _pages[_currentRightNum].pageObject.SetActive(true);
            _pages[_currentRightNum].pageObject.transform.parent = _pageRight.transform;
            _pages[_currentRightNum].pageObject.transform.localPosition = Vector3.zero;
            //_pageLeft.sprite = _pages[_currentLeftNum].pageObject;
            //_pageRight.sprite = _pages[_currentRightNum].pageObject;
        }
        else if (tabLocation % 2 == 1)
        {
            _pages[_currentLeftNum].pageObject.SetActive(false);
            _pages[_currentLeftNum].pageObject.transform.parent = _pageParent.transform;
            _pages[_currentRightNum].pageObject.SetActive(false);
            _pages[_currentRightNum].pageObject.transform.parent = _pageParent.transform;
            _currentLeftNum = tabLocation - 1;
            _currentRightNum = tabLocation;
            _pages[_currentLeftNum].pageObject.SetActive(true);
            _pages[_currentLeftNum].pageObject.transform.parent = _pageLeft.transform;
            _pages[_currentLeftNum].pageObject.transform.localPosition = Vector3.zero;
            _pages[_currentRightNum].pageObject.SetActive(true);
            _pages[_currentRightNum].pageObject.transform.parent = _pageRight.transform;
            _pages[_currentRightNum].pageObject.transform.localPosition = Vector3.zero;
            //_pageLeft.sprite = _pages[_currentLeftNum].pageObject;
            //_pageRight.sprite = _pages[_currentRightNum].pageObject;
        }
    }
}

public class Pages
{
    public int pageNum;
    public GameObject pageObject;
}
