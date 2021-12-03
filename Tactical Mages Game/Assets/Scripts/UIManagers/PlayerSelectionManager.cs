using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSelectionManager : UIManager_Base
{
    [Space]
    [Header("PLAYER COUNT")]
    [SerializeField] private Toggle[] playerCountToggles;
    [SerializeField] private Toggle confirmPlayerCountToggle;
    [SerializeField] private TextMeshProUGUI[] textGreyedBeforePlayerCount;
    private Color[] originalTextColors;
    [SerializeField] private Color greyedTextColor = Color.grey;
    [SerializeField] private Selectable[] selectablesToGreyBeforePlayerCount;
    [SerializeField] private GameObject[] objectsToDisableBeforePlayerCount;

    [Space]
    [Header("CHARACTER SELECTION")]
    [SerializeField] private TextMeshProUGUI PlayerSelectText;
    [SerializeField] private List <Toggle> characterSelectionToggles;
    private bool isSelectingCharacters = false;
    [SerializeField] private Toggle confirmPlayerCharacterToggle;
    [SerializeField] private Selectable[] selectablesToActivateAfterCharactersConfirmed;

    //Track if player has confirmed their selection.
    bool isCharConfirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        //Store original text colors
        originalTextColors = new Color[textGreyedBeforePlayerCount.Length];
        for (int i = 0; i < originalTextColors.Length; i++)
        {
            originalTextColors[i] = textGreyedBeforePlayerCount[i].color;
        }

        //Begin with player count unconfirmed
        PlayerCountUnconfirmed();
    }

    public void ConfirmPlayerCountToggle()
    {
        if (confirmPlayerCountToggle.isOn)
        {
            PlayerCountConfirmed();
            if (!isSelectingCharacters)
            {
                StartCoroutine(DoCharacterSelection());
            }
            //Disable interactions in player count after confirmed.
            confirmPlayerCountToggle.interactable = false;
            for (int i = 0; i < playerCountToggles.Length; i++)
            {
                playerCountToggles[i].interactable = false;
            }
        }
        else
        {
            PlayerCountUnconfirmed();
            if (isSelectingCharacters)
            {
                isSelectingCharacters = false;
            }            
        }
    }

    public void ConfirmPlayerCharacterToggle()
    {
        if (confirmPlayerCharacterToggle.isOn)
        {
            isCharConfirmed = true;
        }
        else
        {
            isCharConfirmed = false;
            if (confirmPlayerCountToggle.isOn)
            {
                ConfirmPlayerCountToggle();
            }
        }
    }

    [HideInInspector]
    public int GetPlayerCount()
    { 
        //Identify the integer of the active count toggle.
        for (int i = 0; i < playerCountToggles.Length; i++)
        {           
            if (playerCountToggles[i].isOn)
            {
                return int.Parse(playerCountToggles[i].GetComponentInChildren<TextMeshProUGUI>().text);
            }            
        }

        //No toggles were found on, display log error.
        Debug.LogError("Integer not retrieved from text array");
        return 0;
    }
        
    private void PlayerCountUnconfirmed()
    {
        //Disable or grey objects before player count confirmed
        for (int i = 0; i < textGreyedBeforePlayerCount.Length; i++)
        {
            textGreyedBeforePlayerCount[i].color = greyedTextColor;
        }
        for (int i = 0; i < characterSelectionToggles.Count; i++)
        {
            characterSelectionToggles[i].isOn = false;
            characterSelectionToggles[i].interactable = false;
        }

        Toggle toggle;
        for (int i = 0; i < selectablesToGreyBeforePlayerCount.Length; i++)
        {
            toggle = selectablesToGreyBeforePlayerCount[i].GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.isOn = false;
            }
            
            selectablesToGreyBeforePlayerCount[i].interactable = false;
        }

        for (int i = 0; i < objectsToDisableBeforePlayerCount.Length; i++)
        {
            objectsToDisableBeforePlayerCount[i].SetActive(false);
        }
    }

    private void PlayerCountConfirmed()
    {
        //Enable objects after player count is confirmed
        for (int i = 0; i < textGreyedBeforePlayerCount.Length; i++)
        {
            textGreyedBeforePlayerCount[i].color = originalTextColors[i];
        }
        for (int i = 0; i < characterSelectionToggles.Count; i++)
        {            
            characterSelectionToggles[i].enabled = true;
            characterSelectionToggles[i].interactable = true;
        }
        for (int i = 0; i < selectablesToGreyBeforePlayerCount.Length; i++)
        {
            selectablesToGreyBeforePlayerCount[i].interactable = true;
        }
    }

    private Character GetPlayerCharacter()
    {
        Character character = null;

        //Identify the character of the active count toggle.
        for (int i = 0; i < characterSelectionToggles.Count; i++)
        {
            if (characterSelectionToggles[i].isOn)
            {
                //Get the character from the selectable
                character = characterSelectionToggles[i].GetComponent<CharacterSelectable>().GetCharacter();
            }
        }

        if (character == null)
        {
            Debug.LogError("No character was found from Character Selection.");
        }
        
        return character;
    }

    private void ChooseCharacter(int playerID)
    {
        //Identify the character of the active count toggle.
        for (int i = 0; i < characterSelectionToggles.Count; i++)
        {
            if (characterSelectionToggles[i].isOn)
            {
                //enable the player ID text bubble
                characterSelectionToggles[i].GetComponent<CharacterSelectable>().SetPlayerIDText(playerID);
                //disable toggle component to cease interaction with this character choice.
                characterSelectionToggles[i].interactable = false;
                characterSelectionToggles[i].enabled = false;
                characterSelectionToggles.RemoveAt(i);
            }
        }
    }

    private IEnumerator DoCharacterSelection()
    {
        //Coroutine is active
        isSelectingCharacters = true;

        //Get number of players to create
        int playerCount = GetPlayerCount();

        //List of players to transfer to Game Manager
        List<Player> players = new List<Player>();

        while (isSelectingCharacters)
        {
            //For each player
            for (int i = 0; i < playerCount; i++)
            {                                
                //This player has just begun the selection process
                isCharConfirmed = false;

                //Prompt player text
                PlayerSelectText.text = $"Player {i + 1}\n Select Your Character";

                //Wait until this player has confirmed their character
                while (!isCharConfirmed)
                {  
                    yield return null;
                }

                //create player and set id and character
                players.Add(new Player(i + 1, GetPlayerCharacter()));
                ChooseCharacter(i + 1);



                //If more players remain, reset character confirmed toggle for next player.
                if (i < playerCount - 1)
                {
                    confirmPlayerCharacterToggle.isOn = false;
                }
                //else disable non-picked options and selection inputs, enable objecst on confirmed
                else
                {                    
                    //Disable interactions
                    confirmPlayerCharacterToggle.interactable = false;
                    for (int j = 0; j < characterSelectionToggles.Count; j++)
                    {
                        characterSelectionToggles[j].interactable = false;
                    }

                    //enable objects after character select confirmed
                    for (int j = 0; j < selectablesToActivateAfterCharactersConfirmed.Length; j++)
                    {
                        selectablesToActivateAfterCharactersConfirmed[j].interactable = true;
                    }
                    //Set players in game manager
                    SetPlayers(players);
                }

            }

            //Player loop is complete. End Coroutine.
            isSelectingCharacters = false;
        }
    }

    private void SetPlayers(List<Player> playerList)
    {
        GameManager.instance.SetPlayers(playerList);
    }
}
