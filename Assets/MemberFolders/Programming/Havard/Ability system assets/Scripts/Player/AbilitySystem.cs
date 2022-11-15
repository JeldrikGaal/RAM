using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySystem : MonoBehaviour
{
    // Ability selector:
    private bool[] abilities;
    public int Presented1;
    public int Presented2;

    // Ability upgrader:
    private bool[] abilitiesLeveled;
    public int[] EnemiesKilled;

    // Abilities:
    [SerializeField] private AbilityObject[] abilityObjects;

    // UI:
    [SerializeField] private Image lImage; 
    [SerializeField] private Image rImage;
    [SerializeField] private Button lButton;
    [SerializeField] private Button rButton;
    private bool isShowingOptions;
    [SerializeField] private GameObject leftHover;
    [SerializeField] private GameObject rightHover;
    [SerializeField] private Image leftHoverVisual;
    [SerializeField] private Image rightHoverVisual;
    [SerializeField] private TextMeshProUGUI leftHoverText;
    [SerializeField] private TextMeshProUGUI rightHoverText;



    void Start()
    {

        // Sets the array to contain all twenty abilities, and sets each to false, because we haven't unlocked them yet
        abilities = new bool[20];
        for (int i = 0; i < 20; i++)
        {
            abilities[i] = false;
        }
        // Does the same for the leveled abilities, to signify that they have not been leveled yet
        abilitiesLeveled = new bool[20];
        for (int i = 0; i < 20; i++)
        {
            abilitiesLeveled[i] = false;
        }
        // Aaand one last time for the amount of enemies killed.
        EnemiesKilled = new int[20];

    }

    public void SelectRandom()
    {

        // Sets the presented abilities to be random numbers between 0 and 20. If the number is already an active ability, it tries until it find one that is not.
        Presented1 = Random.Range(0, 20);
        while (abilities[Presented1])
        {
            Presented1 = Random.Range(0, 20);
        }
        // Does the same for the second, but also check that it's not the same ability
        Presented2 = Random.Range(0, 20);
        while (abilities[Presented1] && Presented1 != Presented2)
        {
            Presented2 = Random.Range(0, 20);
        }
        // the presented int will most likely be tied to an image/button that the user can press, which should invoke the next function, SelectedAbility()

        // Turns the sprites into the sprites on the objects
        lImage.sprite = abilityObjects[Presented1].icon;
        rImage.sprite = abilityObjects[Presented2].icon;
        // Also activates the buttons so the user can select
        lButton.enabled = true;
        rButton.enabled = true;

        isShowingOptions = true;

        print("First ability number: " + Presented1 + ". Second ability number: " + Presented2);

        // Changes the hover information to be the presented ones:

        leftHoverVisual.sprite = abilityObjects[Presented1].VisualOfAbility;
        rightHoverVisual.sprite = abilityObjects[Presented2].VisualOfAbility;
        leftHoverText.text = abilityObjects[Presented1].AbilityDescription;
        rightHoverText.text = abilityObjects[Presented2].AbilityDescription;
    }

    // Functions for buttons instead of a seperate script deciding the ability:
    public void LeftSelected()
    {
        SelectedAbility(Presented1);
    }
    public void RightSelected()
    {
        SelectedAbility(Presented2);
    }

    public void SelectedAbility(int selected)
    {
        // Sets the ability that was selected to be true. This happens when the user selects the ability they want.
        // If it's the first one, it would be "SelectedAbility(presented1)"
        abilities[selected] = true;
        lButton.enabled = false;
        rButton.enabled = false;

        print("Selected ability: " + selected);
    }

    // 4 brief functions for showing the player the abilites.
    /// Should definitely be swapped out with an animation later, but the designers shall decide that
    public void ShowLeft()
    {
        if (isShowingOptions)
        {
            leftHover.SetActive(true);
        }
    }
    public void ShowRight()
    {
        if (isShowingOptions)
        {
            rightHover.SetActive(true);
        }
    }
    public void HideLeft()
    {
        if (isShowingOptions)
        {
            leftHover.SetActive(false);
        }
    }
    public void HideRight()
    {
        if (isShowingOptions)
        {
            rightHover.SetActive(false);
        }
    }

    private void KillEnemies(int abilityNumber, int enemies)
    {
        // simple function to count amount of enemies killed.
        EnemiesKilled[abilityNumber] = 0;
        if (EnemiesKilled[abilityNumber] >= enemies)
        {
            abilitiesLeveled[abilityNumber] = true;
        }
    }

    private void KillWithinTime(int abilityNumber, int enemies, float time)
    {
        // Bit more advanced function to count amount of enemies killed within a certain amount of time
        EnemiesKilled[abilityNumber] = 0;
        if(EnemiesKilled[abilityNumber] >= enemies)
        {
            abilitiesLeveled[abilityNumber] = true;
        }
        StartCoroutine(ResetKills(abilityNumber, time));
    }

    IEnumerator ResetKills(int abilityNumber, float time)
    {
        print(abilityNumber);
        yield return new WaitForSeconds(time);
        EnemiesKilled[abilityNumber] = 0;
    }




    // Debugging:
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            //KillWithinTime(5, 2, 4);
            SelectRandom();
        }
    }
    

}
