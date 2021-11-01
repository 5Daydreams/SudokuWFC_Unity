using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardControllerLowCollapse : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup tileHolder;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private int tilesToInitialize;
    [SerializeField] private bool mouseDebug;
    [SerializeField] private float debugInterval = 1.0f;

    private Tile[,] boardTiles;
    private const int SUDOKU_BOARD_SIZE = 9;
    private const int recursionLimit = 500;
    private int recursionCounter = 0;
    private float timerInternal = 0;

    private void Awake()
    {
        recursionCounter = 0;
        boardTiles = new Tile[SUDOKU_BOARD_SIZE, SUDOKU_BOARD_SIZE];

        for (int i = 0; i < SUDOKU_BOARD_SIZE * SUDOKU_BOARD_SIZE; i++)
        {
            Tile newTile = Instantiate(tilePrefab, tileHolder.transform);
            newTile.InitializeTile();
            newTile.gameObject.name = "(" + i % 9 + "," + i / 9 + ")";

            boardTiles[i % 9, i / 9] = newTile;
        }

        InitializeAllTileValues();
    }

    private void AbortAndRestart()
    {
        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                Destroy(boardTiles[i, j].gameObject);
            }
        }
        Awake();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            AbortAndRestart();
        }
        
        if (!mouseDebug)
        {
            return;
        }

        timerInternal -= Time.deltaTime;

        if (Input.GetMouseButton(0) && timerInternal < 0)
        {
            ContinueIteration();
            timerInternal = debugInterval;
        }
    }

    private void ContinueIteration()
    {
        InitializeAllTileValues();
    }

    private void InitializeAllTileValues()
    {
        Tile selectedTile = null;
        Vector2Int selectedTilePos = new Vector2Int();

        int lowestOpenValues = 10;

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                Tile currentTile = boardTiles[i, j];
                
                bool tileIsAlreadyAssigned = currentTile.CorrectlyAssigned;

                if (tileIsAlreadyAssigned)
                {
                    LockValuesOnLinkedTiles(currentTile.TileValue, i, j);
                    continue;
                }

                int currentTileOpenValues = currentTile.GetOpenValuesList().Count;
                
                bool currentTileHasFewerOpenValues = currentTileOpenValues < lowestOpenValues;

                if (currentTileHasFewerOpenValues)
                {
                    lowestOpenValues = currentTileOpenValues;

                    selectedTile = currentTile;
                    selectedTilePos.x = i;
                    selectedTilePos.y = j;
                }
            }
        }

        if (selectedTile == null) // no tiles found means no tiles to collapse
        {
            return;
        }

        int collapsedValue = GetFilteredValue(selectedTilePos);
        
        selectedTile.TileValue = collapsedValue;
        LockValuesOnLinkedTiles(collapsedValue, selectedTilePos);

        if (recursionCounter > recursionLimit)
        {
            Debug.LogError("Reached recursion limit");
            return;
        }

        if (mouseDebug)
        {
            return;
        }

        recursionCounter++;
        
        InitializeAllTileValues(); // Loop all over again after setting one value
    }

    private int GetFilteredValue(int i, int j)
    {
        // 1.1. Given a tile, find its open values
        List<int> openValues = boardTiles[i,j].GetOpenValuesList();
        List<Tile> linkedTiles = GetLinkedTiles(i,j);

        int mostFrequentCount = 0;
        List<int> chosenIndexes = new List<int>();
        List<int> openValuePriorityCounters = new List<int>();

        for (int k = 0; k < openValues.Count; k++)
        {
            int currentNumber = openValues[k];
            openValuePriorityCounters.Add(0);
            
            foreach (Tile tile in linkedTiles)
            {
                // 2. Given those open values, find which of them are LOCKED on linked tiles
                bool tileDoesNotContainTheSelectedValue = !tile.GetOpenValuesList().Contains(currentNumber);
                
                if (tileDoesNotContainTheSelectedValue) 
                {
                    // 3. The value which is LOCKED on the most linked cells should be the picked value.
                    openValuePriorityCounters[k]++;
                }
            }

            if (openValuePriorityCounters[k] > mostFrequentCount)
            {
                mostFrequentCount = openValuePriorityCounters[k];
                chosenIndexes.Clear();
                chosenIndexes.Add(k);
            }

            if (openValuePriorityCounters[k] == mostFrequentCount)
            {
                chosenIndexes.Add(k);
            }
        }

        if (chosenIndexes.Count == 0)
        {
            if (openValues.Count == 0)
            {
                Debug.LogError("Found an empty list on Tile: " + i + "," + j);
                AbortAndRestart();
            }
            
            int randomIndex = Random.Range(0, openValues.Count);
            chosenIndexes.Add(randomIndex);
        }

        int chosenIndex = chosenIndexes.CopyRandomElement();

        return openValues[chosenIndex];
    }
    
    private int GetFilteredValue(Vector2Int tilePos)
    {
        return GetFilteredValue(tilePos.x, tilePos.y);
    }

    private void LockValuesOnLinkedTiles(int inputValue, Vector2Int tilePos)
    {
        List<Tile> linkedTilesList = GetLinkedTiles(tilePos);

        foreach (Tile tile in linkedTilesList)
        {
            tile.LockValue(inputValue);
        }
    }

    private void LockValuesOnLinkedTiles(int inputValue, int i, int j)
    {
        Vector2Int tilePos = new Vector2Int(i, j);
        LockValuesOnLinkedTiles(inputValue, tilePos);
    }

    private List<Tile> GetLinkedTiles(Vector2Int gridPosition)
    {
        List<Tile> positionList = new List<Tile>();

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                bool isOriginalTile = gridPosition.x == i && gridPosition.y == j;
                
                if (isOriginalTile)
                {
                    continue;
                }
                
                bool sameRowOrCollumn = (j == gridPosition.y) || (i == gridPosition.x);

                int xClusterStart = gridPosition.x % 3;
                int xStart = gridPosition.x - xClusterStart;
                int xFinish = gridPosition.x + (3 - xClusterStart);

                int yClusterStart = gridPosition.y % 3;
                int yStart = gridPosition.y - yClusterStart;
                int yFinish = gridPosition.y + (3 - yClusterStart);

                bool xWithinCluster = xStart <= i && i < xFinish;
                bool yWithinCluster = yStart <= j && j < yFinish;
                bool withinClusterRange = xWithinCluster && yWithinCluster;

                bool isLinkedToCurrentTile = sameRowOrCollumn || withinClusterRange;

                if (isLinkedToCurrentTile)
                {
                    Tile currentTile = boardTiles[i, j];
                    positionList.Add(currentTile);
                }
            }
        }

        return positionList;
    }

    private List<Tile> GetLinkedTiles(int i, int j)
    {
        Vector2Int gridPosition = new Vector2Int(i, j);
        return GetLinkedTiles(gridPosition);
    }
}