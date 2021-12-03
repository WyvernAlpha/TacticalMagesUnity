using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndManager : UIManager_Base
{
    [SerializeField] private TextMeshProUGUI winnerText;
    
    // Start is called before the first frame update
    void Start()
    {
        winnerText.text = $"Player  {GameManager.instance.GetWinnerID()} Wins!!!";
    }

    
}
