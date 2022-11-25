using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    [SerializeField] Sprite[] _pageSprites;
    [SerializeField] List<Pages> _pages = new List<Pages>();
    [SerializeField] Image _pageLeft;
    [SerializeField] Image _pageRight;
    [SerializeField] int _currentLeftNum;
    [SerializeField] int _currentRightNum;
    [SerializeField] int[] _tabs = new int[6];

    // Start is called before the first frame update
    void Start()
    {
        //Takes all the sprites from the array and puts them in a list with their numbers
        for (int i = 0; i < _pageSprites.Length; i++)
        {
            _pages.Add(new Pages() {pageNum = i, pageSprite = _pageSprites[i]});
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
            _pageLeft.sprite = _pages[0].pageSprite;
            _currentLeftNum = 0;
            _pageRight.sprite = _pages[1].pageSprite;
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
        if (direction && _currentLeftNum < _pageSprites.Length - 1)
        {
            _currentLeftNum += 2;
            _currentRightNum += 2;
            _pageLeft.sprite = _pages[_currentLeftNum].pageSprite;
            _pageRight.sprite = _pages[_currentRightNum].pageSprite;
        }
        else if (direction == false && _currentLeftNum > 1)
        {
            _currentLeftNum -= 2;
            _currentRightNum -= 2;
            _pageLeft.sprite = _pages[_currentLeftNum].pageSprite;
            _pageRight.sprite = _pages[_currentRightNum].pageSprite;
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
            _currentLeftNum = tabLocation;
            _currentRightNum = tabLocation + 1;
            _pageLeft.sprite = _pages[_currentLeftNum].pageSprite;
            _pageRight.sprite = _pages[_currentRightNum].pageSprite;
        }
        else if (tabLocation % 2 == 1)
        {
            _currentLeftNum = tabLocation - 1;
            _currentRightNum = tabLocation;
            _pageLeft.sprite = _pages[_currentLeftNum].pageSprite;
            _pageRight.sprite = _pages[_currentRightNum].pageSprite;
        }
    }
}

public class Pages
{
    public int pageNum;
    public Sprite pageSprite;
}
