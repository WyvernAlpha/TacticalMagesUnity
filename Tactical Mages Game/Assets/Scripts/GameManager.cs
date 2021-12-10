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
    [SerializeField] private Character[] defaultTestCharacters;

    public int WinnerID { get; private set; }
    [SerializeField]public List<Player> Players { get; private set; } = new List<Player>();
    [SerializeField]
    public int currentTurn { get; private set; } = 1;

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
            //Debug.Log("Additional GameManager has been found and destroyed.");
            Destroy(this.gameObject);
        }
                
        //Start with a default list of players if none currently exist (for testing game scene)
        if (Players.Count == 0)
        {            
            Player player;
            for (int i = 0; i < defaultTestCharacters.Length; i++)
            {
                player = new Player(i + 1, defaultTestCharacters[i]);
                Players.Add(player);
            }
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
        //Debug.Log("Fight!");

        if(!IsVictory())
        {
            //Debug.Log("Current turn: " + currentTurn);
            TacticalController.instance.StartTurn();
        }
        else
        {
            LoadScene(gameOverSceneName);
        }
    }

    public void EndTurn()
    {
        Debug.Log("Ending turn for player " + GetPlayerOfTurn().PlayerID + ". Current turn number is " + currentTurn);

        //If any players have lost all pawns, remove them from the list and adjust turn
        for (int i = Players.Count - 1; i >= 0; i--)
        {
            if (Players[i].Pawns.Count <= 0)
            {
                Debug.Log($"Player {Players[i].PlayerID} has died and been removed. Player count is now {Players.Count - 1}"); 
                Players.RemoveAt(i);                

                if (currentTurn > Players.Count)
                {
                    Debug.Log($"Due to player death, current turn (turn {currentTurn}) exceeds player count of {Players.Count}. Current turn is now set to {Players.Count}.");
                    currentTurn = Players.Count;
                }
                //else if (currentTurn < Players.Count)
                //{
                //    currentTurn++;
                //}
                                
            }
        }

        //Calculate turn number
        if (currentTurn < Players.Count && currentTurn != Players.Count)
        {
            currentTurn++;
            Debug.Log("Current turn is less than count of players. Current turn is now: " + currentTurn);
        }

        else
        {
            currentTurn = 1;
            Debug.Log("Last player in list has taken turn. First player's turn starts now.");
        }

        //Debug.Log("New turn for Player: " + currentTurn + ", who is Player " + Players[currentTurn -1].Character.Type);
        ProgressMatch();
    }

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }

    public string GetGameSceneName()
    {
        return gameSceneName;
    }

    public string GetGameOverSceneName()
    {
        return gameOverSceneName;
    }

    public void SetPlayers(List<Player> playerList)
    {
        if (Players.Count > 0)
        {
            Players.Clear();
            //Debug.Log($"Players cleared. Count is: {Players.Count}");
        }        

        Players = playerList;
        //Debug.Log("Player Count: " + Players.Count);
    }

    public Player GetPlayerByID(int playerID)
    {
        Player player = null;
        
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].PlayerID == playerID)
            {
                player = Players[i];
            }
        }

        if (player == null)
        {
            Debug.LogError("Error: Player not found when searched in GetPlayer()!");
        }

        return player;
    }

    public Player GetPlayerOfTurn()
    {
        //Debug.Log("Getting player of turn. Current turn = " + currentTurn);
        
        return Players[currentTurn - 1];
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
