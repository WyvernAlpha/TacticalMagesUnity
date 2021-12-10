using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    public int PlayerID { get; private set; } = 1;
    public Character Character { get; private set; }

    public List<GameObject> Pawns { get; private set; } = new List<GameObject>();

    /// <summary>
    /// Constructor for a game player.
    /// </summary>
    /// <param name="id">Player's ID (ie: Player 1, Player 2, etc.)</param>
    /// <param name="charType">Player's chosen character type for pawns.</param>
    public Player(int id, Character charType)
    {
        PlayerID = id;
        Character = charType;
    }

    public void AddPawn(GameObject pawn)
    {
        //Add pawn to list and assign it my player iD
        Pawns.Add(pawn);
        pawn.GetComponent<Pawn>().SetPlayerIDOwner(PlayerID);
    }

   
}
