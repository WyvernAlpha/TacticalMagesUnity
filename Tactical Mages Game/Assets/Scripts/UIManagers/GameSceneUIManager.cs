using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneUIManager : UIManager_Base
{
    public static GameSceneUIManager instance;
    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI instructionsText;
    [SerializeField] private string movePawnInstructions = string.Empty;
    [SerializeField] private string attackOrPassTurnInstructions = string.Empty;
        
    public enum TurnPhase
    {
        MovePawn,
        AttackOrPass
    }
    //[HideInInspector]
    //public TurnPhase turnPhase = TurnPhase.MovePawn;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log($"Additional GameSceneUIManager was found and destroyed object {this.name}.");
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Begin proper UI for the first phase of the turn
        UpdateTurnPhaseUI((TurnPhase)1);
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

    public void UpdateTurnPhaseUI(TurnPhase phase)
    {
        //If it's the first phase of the turn, set appropriate player color and prompt
        if ((int)phase == 0)
        {
            SetPlayerText(GameManager.instance.currentTurn);
            ChangeBackgroundColor(GameManager.instance.Players[GameManager.instance.currentTurn - 1].Character.DefaultColor);
        }
        
        //Update instuctions text per phase
        switch (phase)
        {
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
