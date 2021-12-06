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
    public List<Player> Players { get; private set; } = new List<Player>();

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
            Debug.Log($"Players cleared. Count is: {Players.Count}");
        }        

        Players = playerList;
    }

    public Player GetPlayer(int playerID)
    {
        return Players[playerID - 1];
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
