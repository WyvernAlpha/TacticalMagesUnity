using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Tooltip("String name of the main game scene.")]
    [SerializeField] private string gameSceneName;
    [SerializeField] private string gameOverSceneName;
    public int WinnerID { get; private set; }
    [SerializeField]public List<Player> Players { get; private set; } = new List<Player>();
    [SerializeField]
    int currentTurn = 1;

    private void Awake()
    {
        //Singleton of GameManager instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Additional GameManager has been found and destroyed.");
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        //Cheat kill pawns
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CheatKillPawns(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CheatKillPawns(2);
        }

        //Cheat check for Victory
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (IsVictory())
            {
                LoadScene(gameOverSceneName);
            }
        }

        //Debug Pawns
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < Players[0].Pawns.Count; i++)
            {
                Debug.Log($"Player 1 Pawn #{i + 1}: {Players[0].Pawns[i].name}");
            }

            for (int i = 0; i < Players[1].Pawns.Count; i++)
            {
                Debug.Log($"Player 2 Pawn#{i + 1}: {Players[1].Pawns[i].name}");
            }

            Debug.Log($"Player 1 has {Players[0].Pawns.Count} pawns");
            Debug.Log($"Player 2 has {Players[1].Pawns.Count} pawns");
        }
    }

   public void ProgressMatch()
    {
        Debug.Log("Fight!");

        if(!IsVictory())
        {
            Debug.Log("Current turn: " + currentTurn);
            TacticalController.instance.StartTurn(currentTurn);
        }
    }

    public void EndTurn()
    {        
        if (currentTurn < Players.Count && currentTurn != Players.Count)
            currentTurn++;
        else
            currentTurn = 1;

        ProgressMatch();
    }

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public string GetGameSceneName()
    {
        return gameSceneName;
    }

    public void SetPlayers(List<Player> playerList)
    {
        if (Players.Count > 0)
        {
            Players.Clear();
        }        

        Players = playerList;
        Debug.Log("Player Count: " + Players.Count);
    }

    public Player GetPlayer(int playerID)
    {
        return Players[playerID-1];
    }

    public bool IsVictory()
    {
        int playersRemaining = 0;
        int tempPlayerID = 0;
        
        //Check all players
        for (int i = 0; i < Players.Count; i++)
        {
            //If players have pawns, they're still in the game
            if (Players[i].Pawns.Count > 0)
            {                
                //increment players remaining
                playersRemaining++;
                //store last known id of player with pawns remaining
                tempPlayerID = Players[i].PlayerID;
            }
        }

        //If more than 1 player remains, victory is false
        if (playersRemaining > 1)
        {            
            return false;
        }

        //Store winner id
        WinnerID = tempPlayerID;

        //last man standing, return true
        return true;
    }

    public void CheatKillPawns(int playerID)
    {
        for (int i = Players[playerID-1].Pawns.Count-1; i >= 0; i--)
        {
            Destroy(Players[playerID-1].Pawns[i]);
            Players[playerID-1].Pawns.RemoveAt(i);
        }
    }

    public int GetWinnerID()
    {
        return WinnerID;
    }
}
