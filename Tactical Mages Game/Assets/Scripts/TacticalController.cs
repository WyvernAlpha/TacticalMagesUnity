using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalController : MonoBehaviour
{
    public static TacticalController instance;
    //public bool isMovementPhase = false;

    [SerializeField]
    Pawn selectedPawn = null;
    Pawn previousPawn = null;

    public HashSet<Tile> selectableTiles = new HashSet<Tile>();
    public int hashSetLength;

    [SerializeField]
    Tile selectedTile = null;
    Tile previousTile = null;
    [SerializeField]
    LayerMask layerMask;

    private void Awake()
    {
        //Singleton of TacticalController instance
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Additional GameManager has been found and destroyed.");
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    public void StartTurn()
    {
        Debug.Log($"Player {GameManager.instance.currentTurn} turn started.");
        StartCoroutine(TakeTurn());
    }


    private IEnumerator TakeTurn()
    {
        //MOVEMENT PHASE
        GameSceneUIManager.instance.UpdateTurnPhaseUI(GameSceneUIManager.TurnPhase.MovePawn); // update UI
        Debug.Log($"Player {GameManager.instance.currentTurn} MOVEMENT PHASE");        
        bool isMovementPhase = true;
        while (isMovementPhase)
        {
            //If player presses spacebar, skip movement phase
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClearPrevSelectables(); //deselect tiles
                isMovementPhase = false;  //end movement phase phase
            }

            //If player clicks left mouse
            else if (Input.GetButtonDown("Fire1"))
            {
                //Raycast to check what they clicked on
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //If Raycast hit the tile layer (ie: layerMask)
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    //If the tile has a tile component
                    if (hit.collider.GetComponent<Tile>())
                    {
                        previousTile = selectedTile;                            //Store previous tile
                        selectedTile = hit.collider.GetComponent<Tile>();       //Selected tile is the one we just clicked on
                        previousPawn = selectedPawn;                            //Store Previous pawn is the one last selected
                        selectedPawn = hit.collider.GetComponent<Tile>().pawn;  //Current selected pawn is the one on top of this tile  

                        //If the player selected a pawn and it belongs to the current player
                        if (selectedPawn != null && GameManager.instance.Players[GameManager.instance.currentTurn - 1].Pawns.Contains(selectedPawn.gameObject))
                        {
                            //Clear previous selected highlights tiles
                            ClearPrevSelectables();
                            //Show movement for selected pawn
                            selectedPawn.ShowMovement();                            
                        }
                        ///Else if the player selected an empty tile that is able to move their previous pawn to
                        else if (selectedPawn == null && previousPawn != null && selectableTiles.Contains(selectedTile))
                        {  
                            MovePawn(previousPawn, selectedTile.transform.position);    //Move pawn to that tile                            
                            previousPawn.GetCurrentTile();                              //Assign tile to pawn
                            isMovementPhase = false;                                        //Selction phase is compelte
                            //GameManager.instance.EndTurn();                             //End Turn
                        }
                        else
                        {
                            Debug.Log("Unanticipated condition in movement phase.");
                        }
                    }
                }
            }
            
            yield return null;
        }

        //ATTACK PHASE
        GameSceneUIManager.instance.UpdateTurnPhaseUI(GameSceneUIManager.TurnPhase.AttackOrPass); // update UI
        Debug.Log($"Player {GameManager.instance.currentTurn} ATTACK PHASE");
        bool isAttackPhase = true;
        while (isAttackPhase)
        {
            //If player presses spacebar, skip attack phase
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClearPrevSelectables(); //deselect tiles
                isAttackPhase = false;  //end attach phase
            }

            //If player clicks left mouse
            else if (Input.GetButtonDown("Fire1"))
            {
                //Raycast to check what they clicked on
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //If Raycast hit the tile layer (ie: layerMask)
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    //If the tile has a tile component
                    if (hit.collider.GetComponent<Tile>())
                    {
                        selectedTile = hit.collider.GetComponent<Tile>();       //Selected tile is the one we just clicked on
                        previousPawn = selectedPawn;                            //Store Previous pawn is the one last selected
                        selectedPawn = selectedTile.pawn;                       //Current selected pawn is the one on top of this tile

                        //If the player selected a pawn and it belongs to the current player
                        if (selectedPawn != null && GameManager.instance.Players[GameManager.instance.currentTurn - 1].Pawns.Contains(selectedPawn.gameObject))
                        {
                            //Clear previous selected highlights tiles
                            ClearPrevSelectables();
                            //TODO: Show enemy's in range for selected pawn
                            selectedPawn.ShowMovement(); // change to ShowEnemysInRange();
                        }

                        //Else if the player selected an enemy pawn that is in range of their pawn
                        else if (selectedPawn != null && previousPawn != null && selectableTiles.Contains(selectedTile))                        
                        {
                            //if the previous pawn is mine
                            if (GameManager.instance.Players[GameManager.instance.currentTurn - 1].Pawns.Contains(previousPawn.gameObject))
                            {
                                //Clear the tile highlights
                                ClearPrevSelectables();

                                //Attack the enemy pawn with my pawn and end attack phase
                                previousPawn.Attack(selectedPawn);
                                isAttackPhase = false;
                            }
                        }
                        else
                        {
                            Debug.Log($"Unanticipated condition in Attack Phase.");
                        }
                    }
                }
            }

            yield return null;
        }


        //GameManger -> CheckForVictory() (actually better to do this in pawn OnDestroy() event)

        //End Turn
        GameManager.instance.EndTurn();
    }

    

    void ClearPrevSelectables()
    {
        //foreach(Tile tile in selectableTiles)
        //{
        //    tile.Unselect();
        //}

        Tile[] tiles = new Tile[selectableTiles.Count];

        selectableTiles.CopyTo(tiles);

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].Unselect();
        }

        selectableTiles.Clear();
        hashSetLength = selectableTiles.Count;
    }

    void MovePawn(Pawn pawn, Vector3 position)
    {
        pawn.currentTile.pawn = null;
        pawn.transform.position = new Vector3(position.x, 0.15f, position.z);
    }
    
}
