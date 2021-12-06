using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneUIManager : UIManager_Base
{
    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI instructionsText;
    [SerializeField] private string selectPawnInstructions;
    [SerializeField] private string movePawnInstructions;
    [SerializeField] private string attackOrPassTurnInstructions;
    private int mockPlayerID = 1; //testing only
    [SerializeField] private Character mockPlayerCharacter1; //testing only
    [SerializeField] private Character mockPlayerCharacter2; //testing only

    public enum TurnPhase
    {
        SelectPawn,
        MovePawn,
        AttackOrPass
    }
    public TurnPhase turnPhase = TurnPhase.SelectPawn;
    
    // Start is called before the first frame update
    void Start()
    {
        //Begin with player 1 background color
        ChangeBackgroundColor(mockPlayerCharacter1.DefaultColor);
    }

    // Update is called once per frame
    void Update()
    {
        //Testing purposes only
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((int)turnPhase <= 1)
                turnPhase++;
            else
            {
                turnPhase = 0;
                if (mockPlayerID == 1)
                {
                    mockPlayerID = 2;
                }
                else
                {
                    mockPlayerID = 1;
                }
                
            }

            ProgressTurnPhase(mockPlayerID, turnPhase);
        }
    }

    public void ChangeBackgroundColor(Color color)
    {
        backgroundImage.color = color;
    }

    public void SetPlayerText(int playerID)
    {
        playerText.SetText($"Player {playerID.ToString()}:");
    }

    private void SetInstructionsText(string text)
    {
        instructionsText.SetText(text);
    }

    private void ProgressTurnPhase(int playerID, TurnPhase phase)
    {
        switch (phase)
        {
            case TurnPhase.SelectPawn:
                SetPlayerText(playerID);
                SetInstructionsText(selectPawnInstructions);
                if (backgroundImage.color == mockPlayerCharacter1.DefaultColor)
                {
                    backgroundImage.color = mockPlayerCharacter2.DefaultColor;
                }
                else
                {
                    backgroundImage.color = mockPlayerCharacter1.DefaultColor;
                }
                break;

            case TurnPhase.MovePawn:
                SetInstructionsText(movePawnInstructions);
                break;

            case TurnPhase.AttackOrPass:
                SetInstructionsText(attackOrPassTurnInstructions);
                break;

            default:
                Debug.Log("Turn phase not implemented");
                break;
        }

    }
}
