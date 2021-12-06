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
        
    }

    // Update is called once per frame
    void Update()
    {
        //Testing purposes only
    }

    public void ChangeBackgroundColor(Color color)
    {
        backgroundImage.color = color;
    }

    public void SetPlayerText(int playerID)
    {
        playerText.SetText($"Player {playerID.ToString()}");
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
