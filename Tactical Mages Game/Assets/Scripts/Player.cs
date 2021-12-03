using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    public int PlayerID { get; private set; } = 1;
    public Character Character { get; private set; }

    public List<GameObject> Pawns { get; private set; } = new List<GameObject>();

    public bool isTurn = false;

    [SerializeField]
    Pawn selectedPawn = null;

    /// <summary>
    /// Constructor for a game player.
    /// </summary>
    /// <param name="id">Player's ID (ie: Player 1, Player 2, etc.)</param>
    /// <param name="type">Player's chosen character type for pawns.</param>
    public Player(int id, Character charType)
    {
        PlayerID = id;
        Character = charType;
    }

    public void AddPawn(GameObject pawn)
    {
        Pawns.Add(pawn);
    }


    IEnumerator TakeTurn()
    {
        if (isTurn)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.GetComponent<Tile>())
                    {
                        selectedPawn = hit.collider.GetComponent<Tile>().pawn;
                        if(selectedPawn != null)
                            selectedPawn.CheckToMove();
                    }
                }

                yield return null;
            }
            //Move pawn()
            //Attack / Wait phase()
            //GameManger -> CheckForVictory()
            //End turn()
        }
    }
}
