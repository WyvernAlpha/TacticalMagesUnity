using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private SpawnPointGroup[] spawnPointGroups;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Additional Level Manager was found and destroyed");
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPawns();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnPawns()
    {
        //For all players
        for (int i = 0; i < GameManager.instance.Players.Count; i++)
        {
            //Debug.Log(GameManager.instance.Players[i].Character.PawnSet.Length + " pawns for Player " + GameManager.instance.Players[i].PlayerID);

            //For all pawns in set
            for (int j = 0; j < GameManager.instance.Players[i].Character.PawnSet.Length; j++)
            {
                ////If number of pawns exceed number of spawn points, continue
                //if (j >= spawnPointGroups[i].spawnPoints.Length)
                //    continue;
                
                //Instantiate at spawnpoint group, spawn point position and rotation
                GameObject pawn = GameObject.Instantiate(GameManager.instance.Players[i].Character.PawnSet[j],
                    spawnPointGroups[i].spawnPoints[j].position,
                    spawnPointGroups[i].spawnPoints[j].rotation);

                pawn.GetComponent<Pawn>().playerID = i;

                //Add instantiated pawn to player's array of pawns
                GameManager.instance.Players[i].AddPawn(pawn);
            }
        }

        GameManager.instance.ProgressMatch();    
    }
   
}
