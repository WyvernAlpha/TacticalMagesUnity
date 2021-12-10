using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalController : MonoBehaviour
{
    public static TacticalController instance;
    [SerializeField] private float pawnSpawnY;

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

    public bool isTurnComplete { get; private set; } = true;

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
        if (isTurnComplete)
        {
            Debug.Log($"Player {GameManager.instance.currentTurn} turn started.");
            StartCoroutine(TakeTurn());
        }
    }


    private IEnumerator TakeTurn()
    {
        isTurnComplete = false;
        //MOVEMENT PHASE
        GameSceneUIManager.instance.UpdateTurnPhaseUI(GameSceneUIManager.TurnPhase.MovePawn); // update UI
        Debug.Log($"Player {GameManager.instance.currentTurn} MOVEMENT PHASE. Has " + GameManager.instance.GetPlayerOfTurn().Pawns.Count + " pawns total.");
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
                                                                                //Clear previous selected highlights tiles


                        //If the player selected a pawn and it belongs to the current player
                        if (selectedPawn != null && GameManager.instance.GetPlayerOfTurn().Pawns.Contains(selectedPawn.gameObject))
                        {

                            ClearPrevSelectables();
                            //Show movement for selected pawn
                            selectedPawn.ShowMovement();
                        }
                        ///Else if the player selected an empty tile that is able to move their previous pawn to
                        else if (selectedPawn == null && previousPawn != null && selectableTiles.Contains(selectedTile))
                        {
                            ClearPrevSelectables();
                            MovePawn(previousPawn, selectedTile.transform.position);    //Move pawn to that tile                            
                            previousPawn.GetCurrentTile();                              //Assign tile to pawn
                            isMovementPhase = false;                                    //Selction phase is compelte                            
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


        previousPawn = null;
        selectedPawn = null;
        previousTile = null;
        selectedTile = null;
        bool isAttackPhase = false;
                
        Debug.Log("Entering for-loop to check for null pawns before attack()");

        //Check pawns in reverse in case any have been killed
        for (int i = GameManager.instance.GetPlayerOfTurn().Pawns.Count - 1; i >= 0; i--)
        {
            //If pawn is null, remove it from list and continue
            if (GameManager.instance.GetPlayerOfTurn().Pawns[i] == null)
            {
                GameManager.instance.GetPlayerOfTurn().Pawns.RemoveAt(i);
                continue;
            }

            //If pawn can attack, enter attack phase
            if (GameManager.instance.GetPlayerOfTurn().Pawns[i].GetComponent<Pawn>().CanAttack())
            {                
                isAttackPhase = true;
                break;
            }

            //Else no pawns can attack, do not enter attack phase
            else
            {
                isAttackPhase = false;
            }
        }

        Debug.Log("Out of pawn null-check. Updating UI attack phase");

        //ATTACK PHASE
        GameSceneUIManager.instance.UpdateTurnPhaseUI(GameSceneUIManager.TurnPhase.AttackOrPass); // update UI        
        while (isAttackPhase)
        {
            Debug.Log("Attack phase while loop");

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
                        if (selectedPawn != null && GameManager.instance.GetPlayerOfTurn().Pawns.Contains(selectedPawn.gameObject))
                        {
                            //Clear previous selected highlights tiles
                            ClearPrevSelectables();
                            if (selectedPawn.CanAttack())
                            {

                                //TODO: Show enemy's in range for selected pawn
                                selectedPawn.ShowMovement(1); // change to ShowEnemysInRange();
                            }
                        }

                        //Else if the player selected an enemy pawn that is in range of their pawn
                        else if (selectedPawn != null && previousPawn != null && selectableTiles.Contains(selectedTile))
                        {
                            //if the previous pawn is mine
                            if (GameManager.instance.GetPlayerOfTurn().Pawns.Contains(previousPawn.gameObject) && previousPawn.CanAttack())
                            {
                                //Clear the tile highlights

                                //Attack the enemy pawn with my pawn 
                                previousPawn.Attack(selectedPawn);
                                ClearPrevSelectables();
                                previousPawn.GetCurrentTile();

                                //Wait for opponent pawn to receive damage or die
                                while (selectedPawn != null && !selectedPawn.isDamageCompleted)
                                {
                                    yield return null;
                                }

                                if (selectedPawn == null)
                                {
                                    Debug.Log("Pawn is DEAD");
                                }
                                else if (selectedPawn.isDamageCompleted == true)
                                {
                                    Debug.Log($"{selectedPawn.name} is damaged but alive...");
                                    //end attack phase                                   
                                }

                                isAttackPhase = false;
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

            yield return null;   
        }

        //GameManger -> CheckForVictory() (actually better to do this in pawn OnDestroy() event)

        isTurnComplete = true;

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
        pawn.transform.position = new Vector3(position.x, pawnSpawnY, position.z);
        pawn.GetCurrentTile();
    }

}


