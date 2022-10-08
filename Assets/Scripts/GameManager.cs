using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Material pegMaterial; // used to give a color to peg
    public Material emptyMaterial; // used to give a color empty cell
    public Material greenMaterial;
    public Material noValidMoveMaterial;
    public int GameScore;
    public Text txt;
    public Canvas canvas;
    public Canvas SavedCanvas;
    private CellProperties firstCell;
    private CellProperties lastCell;
    private CellProperties midCell;
    private bool firstInputTaken;
    private bool lastInputTaken;
    public LayerMask CellLayer;
    public bool isFirstCellMoving=false;
    private bool isPegMoving = false;
    public float pegSpeed = 5f;
    private bool isScoreCalculated = false;
    public bool ActivateAutoMove = false;


    // Update is called once per frame
    void Update()
    {
        if (!isFirstCellMoving)
        {
            if (CheckValidMove() > 0)
            {
                if (ActivateAutoMove)
                {                 
                    IntelligentAutoMove();
                    ActivateAutoMove = false;                   
                }
                else if (firstInputTaken == false)
                {
                    GetFirstInput();
                }
                else if (lastInputTaken == false)
                {
                    GetSecondInput();
                }
                if (lastInputTaken == true)
                {
                    GetMiddleCell();
                }
                if (firstCell != null && lastCell != null)
                {
                    if (CheckMove(firstCell,lastCell,midCell))
                    {
                        MakeMove();
                    }
                    if (!isFirstCellMoving)
                    {
                        firstCell = null;
                        firstInputTaken = false;
                        lastCell = null;
                        lastInputTaken = false;
                        midCell = null;
                    }                   
                }
            }
            else
            {
                CalculateScore();
                txt.text = "Your Score is: " + GameScore;
                canvas.gameObject.SetActive(true);
            }
        }
        else
        { 
            MoveUpFirstCell();
        }             
    }
    // When player select a peg, possible positions where peg can go will be shown with this method
    private bool HighlightValidMoves(CellProperties first)
    {
        CellProperties [] cells = GetComponentsInChildren<CellProperties>();
        first = firstCell;
        bool validMoveFound = false;
        foreach(CellProperties last in cells)
        {
            if(last.getStatus() == "empty")
            {
                bool possibleCell = false;
                if (first.getColumn() == last.getColumn() || first.getColumn() - last.getColumn() == 2 || first.getColumn() - last.getColumn() == -2)
                {
                    if (first.getRow() - last.getRow() == 2 || first.getRow() - last.getRow() == -2)
                    {
                        possibleCell = true;
                    }
                }
                if (first.getRow() == last.getRow() || first.getRow() - last.getRow() == 2 || first.getRow() - last.getRow() == -2)
                {
                    if (first.getColumn() - last.getColumn() == 2 || first.getColumn() - last.getColumn() == -2)
                    {
                        possibleCell = true;
                    }
                }
                if (possibleCell)
                {
                    CellProperties mid = DetermineMid(first, last);
                    if (mid != null && mid.getStatus() == "peg")
                    {
                        validMoveFound = true;
                        last.GetComponent<MeshRenderer>().material = greenMaterial;
                    }
                }
            }
        }
        return validMoveFound;
    }
    //reset marked possible positions selected peg can go
    private void ResetHighlightedMoves()
    {
        CellProperties [] cells = GetComponentsInChildren<CellProperties>();
        foreach(CellProperties cell in cells)
        {
            if (cell.getStatus() == "empty")
            {
                cell.GetComponent<MeshRenderer>().material = emptyMaterial;
            }
            if (cell.getStatus() == "peg")
            {
                cell.GetComponent<MeshRenderer>().material = pegMaterial;
            }
        }
    }
    //Simply counts the pegs on the board
    private void CalculateScore()
    {
        if (!isScoreCalculated)
        {
            CellProperties[] AllCells = FindObjectsOfType<CellProperties>();
            foreach (CellProperties cell in AllCells)
            {
                if (cell.getStatus() == "peg")
                {
                    GameScore++;
                }
            }
            isScoreCalculated = true;
        }
    }
    // gets input from user mouse, till user click on a peg firstInputTaken variable stays false, when user clicks on
    // peg, then firstInputTaken variable becomes true and we can keep track of whether first given input true or false
    private void GetFirstInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                firstCell = hit.transform.gameObject.GetComponent<CellProperties>();
                if(firstCell != null && firstCell.status !="empty")
                {
                    if(HighlightValidMoves(firstCell))
                    {
                        firstInputTaken = true;
                    }
                    else{
                        SoundManagerScript.PlayWrongMoveSound();
                        StartCoroutine(NoValidMoveFound(firstCell));
                        firstCell = null;
                    }
                }
                Debug.Log(hit.transform.gameObject.name);
            }
        }
    }
    // When user select one peg, they should select the position for selected peg to go
    // I used GetSecondInput method to handle selecting position for first selected peg to go
    private void GetSecondInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ResetHighlightedMoves();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                lastCell = hit.transform.gameObject.GetComponent<CellProperties>();
                if(lastCell != null && firstCell!=lastCell && DetermineMid(firstCell,lastCell)!=null)
                {
                    lastInputTaken = true;       
                }
                else
                {
                    firstCell = null;
                    firstInputTaken = false;
                    lastCell = null;
                    lastInputTaken = false;
                }
            }
        }
    }
    // if player selects a peg which cannot go anywhere, warn the player
    private IEnumerator NoValidMoveFound(CellProperties cell)
    {
        cell.GetComponent<MeshRenderer>().material = noValidMoveMaterial;
        yield return new WaitForSeconds(0.5f);
        if(cell.getStatus() == "peg")
        {
            cell.GetComponent<MeshRenderer>().material = pegMaterial;
        }
        else
        {
            cell.GetComponent<MeshRenderer>().material = emptyMaterial;
        }
    }
    //Determine the cell in the middle of first and last selected cells with the help of DetermineMid function
    private void GetMiddleCell()
    {
        midCell = DetermineMid(firstCell,lastCell);
    }
    //check if selected cells are able to make the move or not
    // Diagnol moves are accepted
    private bool CheckMove(CellProperties first,CellProperties last, CellProperties mid)
    {
        // first selected cell will jump over midcell( the cell between first selected and last selected cells)
        // and will go to last selected cell's position so that first and midcell should be peg
        // lastcell should be empty
        if(first.getStatus() != "peg" || mid.getStatus()!="peg" || last.getStatus() != "empty")
        {
            return false;
        }
        //There should be 2 difference between first and last selected cell's rows(or columns which controlled below)
        if(first.getRow()- last.getRow() == 2 || first.getRow() - last.getRow() == -2)
        {
            //midcell should be between last and first cell
            if((mid.getColumn() == (first.getColumn()+ last.getColumn())/2))
            {
                return true;
            }
        }
        //There should be 2 difference between first and last selected cell's columns(or rows which controlled above)
        if (first.getColumn() - last.getColumn() == 2 || first.getColumn() - last.getColumn() == -2)
        {
            //midcell should be between last and first cell
            if ((mid.getRow() == (first.getRow() + last.getRow()) / 2))
            {
                return true;
            }
        }
        return false;
    }
    public void MakeMove()
    {
        // instantiate new empty cell at the position of first selected cell because the peg in firstcell will go to
        // the position of last cell and first cell will be empty
        CellProperties newPeg = Instantiate(lastCell, firstCell.transform.position, Quaternion.identity);
        newPeg.transform.parent = gameObject.transform;
        newPeg.GetComponent<MeshRenderer>().material = emptyMaterial;
        newPeg.SetColumn(firstCell.getColumn());// set column value correctly
        newPeg.SetRow(firstCell.getRow());//set row value correctly
        newPeg.SetStatus(lastCell.getStatus());//set status correctly
        //instantiate new empty cell at the position of midcell, when the peg on first cell move it will 
        //destroy the peg on mid cell so that mid cell will be empty as well
        newPeg = Instantiate(lastCell, midCell.transform.position, Quaternion.identity);
        newPeg.GetComponent<MeshRenderer>().material = emptyMaterial;
        newPeg.transform.parent = gameObject.transform;
        newPeg.SetColumn(midCell.getColumn());// set column value correctly
        newPeg.SetRow(midCell.getRow());//set row value correctly
        newPeg.SetStatus(lastCell.getStatus());//set status correctly
        midCell.mid = true;// to activate particles when destroying
        midCell.DestroySelf();
        firstCell.SetRow(lastCell.getRow());// set first cell's new row
        firstCell.SetColumn(lastCell.getColumn());//set first cell's new column
        isFirstCellMoving = true; // first cell will move to the position of last cell
        isPegMoving = true;
        pegSpeed = 5f;     
    }
    private void Start()
    {
        // set graphic quality to what is given on settings menu
        QualitySettings.SetQualityLevel(SettingsMenu.currentQualityIndex);
    }
    //moving animation of first selected cell
    private void MoveUpFirstCell()
    {
        float step = pegSpeed * Time.deltaTime; // calculate distance to move
        if(isPegMoving)
        {
            firstCell.transform.position = Vector3.MoveTowards(firstCell.transform.position, lastCell.transform.position + new Vector3(0, 1.5f, 0), step);
        }
        if (firstCell.transform.position.y > 0.75f)
        {
            isPegMoving = false;
        }
        // when the peg reach correct (x,z) position
        if (!isPegMoving)
        {
            // make it go down till the exact position found
            firstCell.transform.position = Vector3.MoveTowards(firstCell.transform.position, lastCell.transform.position, step);

            if (firstCell.transform.position.y-0.001 <= lastCell.transform.position.y)
            {
                // when the correct position found, destroy lastcell
                lastCell.DestroySelf();
                isFirstCellMoving = false; // peg is not moving anymore
                // reset cell values
                firstCell = null;
                firstInputTaken = false;
                lastCell = null;
                lastInputTaken = false;
                midCell = null;
            }
        }
    }


    //check all the cells if there is any valid move, if there are valid moves, return the number of valid moves 
    // Diagnol moves are accepted
    private int CheckValidMove()
    {
        int moveCount = 0;
        CellProperties[] AllCells = FindObjectsOfType<CellProperties>();
        // check all cells 
        foreach(CellProperties cell1 in AllCells)
        {
            // if first chosen cell is peg there could be valid move
            if (cell1.getStatus() == "peg")
            {
                // continue searching
                foreach(CellProperties cell2 in AllCells)
                {
                    // if an empty cell can be found in position where the difference between rows is 2
                    // this move may be done
                    if ((cell1.getRow() - cell2.getRow() == 2 || cell1.getRow() - cell2.getRow() == -2) && (cell2.getStatus()=="empty"))
                    {
                        if (cell1.getColumn() == cell2.getColumn() || cell1.getColumn()- cell2.getColumn() == 2 || cell1.getColumn() - cell2.getColumn() == -2)
                        {
                            // if the cell between 2 found  cells is a peg, then the move can be made for sure
                            foreach (CellProperties cell3 in AllCells)
                            {
                                if ((cell3.getColumn() == (cell1.getColumn()+cell2.getColumn())/2) && (cell3.getRow() == (cell2.getRow() + cell1.getRow()) / 2))
                                {
                                    if (cell3.getStatus() == "peg")
                                    {
                                        moveCount++;
                                    }
                                }
                            }
                        }
                    }
                    // if a cell can be found in position where the difference between columns is 2
                    // this move may be done
                    if ((cell1.getColumn() - cell2.getColumn() == 2 || cell1.getColumn() - cell2.getColumn() == -2) && (cell2.getStatus() == "empty"))
                    {
                        if (cell1.getRow() == cell2.getRow() ||cell1.getRow()-cell2.getRow()==2 || cell1.getRow()-cell2.getRow()==-2)
                        {
                            // if the cell between 2 found cells is a peg, then the move can be made for sure
                            foreach (CellProperties cell3 in AllCells)
                            {
                                if ((cell3.getRow() == (cell1.getRow() + cell2.getRow()) / 2) && (cell3.getColumn() == (cell2.getColumn() + cell1.getColumn()) / 2))
                                {
                                    if (cell3.getStatus() == "peg")
                                    {
                                        moveCount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return moveCount;
    }

    // This method gets 2 cells as parameter and returns the cell between these two if it exists, otherwise returns null
    public CellProperties DetermineMid(CellProperties first,CellProperties last)
    {
        CellProperties[] AllCells = FindObjectsOfType<CellProperties>();
        if ((first.getRow() - last.getRow() == 2 || first.getRow() - last.getRow() == -2) && (last.getStatus() == "empty"))
        {
            if (first.getColumn() == last.getColumn() || first.getColumn()-last.getColumn() == 2 || first.getColumn() - last.getColumn() == -2)
            {
                foreach (CellProperties cell3 in AllCells)
                {
                    if ((cell3.getColumn() == (first.getColumn()+last.getColumn()) / 2) && (cell3.getRow() == (last.getRow() + first.getRow()) / 2))
                    {
                        if (cell3.getStatus() == "peg")
                        {
                            return cell3;
                        }
                    }
                }
            }
        }
        if ((first.getColumn() - last.getColumn() == 2 || first.getColumn() - last.getColumn() == -2) && (last.getStatus() == "empty"))
        {
            if (first.getRow() == last.getRow() || first.getRow() - last.getRow() == 2 || first.getRow() - last.getRow() == -2)
            {
                foreach (CellProperties cell3 in AllCells)
                {
                    if ((cell3.getRow() == (first.getRow() + last.getRow())/2) && cell3.getColumn() == ((last.getColumn() + first.getColumn()) / 2))
                    {
                        if (cell3.getStatus() == "peg")
                        {
                            return cell3;
                        }
                    }
                }
            }
        }
        // to make sure not to miss anything, reset inputTaken values
        firstInputTaken = false;
        lastInputTaken = false;
        return null;
    }
    
    public void SaveBoard()
    {
        //if a cell is moving, then player should wait it to complete its movement
        if (!isFirstCellMoving)
        {
            SaveSystem.SaveBoard(this);
            StartCoroutine(DisplaySavedText());
        }
    }
    // display SAVED text on screen to inform user that the board is saved succesfully
    IEnumerator DisplaySavedText()
    {
        SavedCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SavedCanvas.gameObject.SetActive(false);

    }
    //Load saved cell properties
    public void LoadBoard()
    {
        //if a cell is moving, then player should wait it to complete its movement
        if (!isFirstCellMoving)
        {
            BoardData data = SaveSystem.LoadBoard();
            CellProperties[] cells = this.GetComponentsInChildren<CellProperties>();
            for (int i = 0; i < data.CellAmount; i++)
            {
                for (int j = 0; j < cells.Length; j++)
                {
                    if (data.row[i] == cells[j].getRow() && data.column[i] == cells[j].getColumn())
                    {
                        cells[j].SetStatus(data.status[i]);
                        if (cells[j].getStatus() == "peg")
                        {
                            cells[j].GetComponent<MeshRenderer>().material = pegMaterial;
                        }
                        else
                        {
                            cells[j].GetComponent<MeshRenderer>().material = emptyMaterial;

                        }
                    }
                }
            }
        }  
    }

    //This method makes one move for each cell one by one and calls CheckValidMove function to calculate how many move left
    // after that, compares all the move counts and chooses the cell which has the highest move left
    public void IntelligentAutoMove()
    {
        int MaxMove = 0;
        CellProperties[] cells = this.GetComponentsInChildren<CellProperties>();
        foreach(CellProperties first in cells)
        {
            if(first.getStatus() == "peg")
            {
                foreach(CellProperties last in cells)
                {
                    if (last.getStatus() == "empty")
                    {                        
                            bool possibleCell = false;
                            if (first.getColumn() == last.getColumn() || first.getColumn() - last.getColumn() == 2 || first.getColumn() - last.getColumn() == -2)
                            {
                                if (first.getRow() - last.getRow() == 2 || first.getRow() - last.getRow() == -2)
                                {
                                    possibleCell = true;
                                }
                            }
                            if (first.getRow() == last.getRow() || first.getRow() - last.getRow() == 2 || first.getRow() - last.getRow() == -2)
                            {
                                if (first.getColumn() - last.getColumn() == 2 || first.getColumn() - last.getColumn() == -2)
                                {
                                    possibleCell = true;
                                }
                            }
                            if (possibleCell)
                            {
                                CellProperties mid = DetermineMid(first, last);
                                if (mid != null && mid.getStatus() == "peg")
                                {
                                    // make the move if it can be made
                                    first.SetStatus("empty");
                                    mid.SetStatus("empty");
                                    last.SetStatus("peg");
                                    //get the count
                                    int validMoveCount = CheckValidMove();
                                    // compare the current count with max move count
                                    if (validMoveCount >= MaxMove)
                                    {
                                        MaxMove = validMoveCount;
                                        firstCell = first;
                                        lastCell = last;
                                        midCell = mid;
                                    }
                                    //revert the cell status
                                    first.SetStatus("peg");
                                    mid.SetStatus("peg");
                                    last.SetStatus("empty");
                                }
                            }                   
                    }
                }
            }
        }
        MakeMove();
    }
}
