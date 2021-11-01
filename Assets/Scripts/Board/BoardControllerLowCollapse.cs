using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardControllerV2 : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup tileHolder;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private int tilesToInitialize;

    private Tile[,] boardTiles;
    private const int SUDOKU_BOARD_SIZE = 9;

    private void Awake()
    {
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

    private void InitializeAllTileValues()
    {
        List<int> currentTilePossibleValues = new List<int>();
        int random = 0;
        Vector2Int currentTilePos = new Vector2Int(0, 0);
        
        Tile selectedTile;
        int lowestOpenValues = 10;

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                Tile currentTile = boardTiles[i, j];

                int openValues = currentTile.GetOpenValuesList().Count;

                bool thing = 1 <= openValues && openValues < lowestOpenValues;
                bool thing2 = currentTile.CorrectlyAssigned;

                if (thing)
                {
                    lowestOpenValues = openValues;

                    selectedTile = currentTile;

                }
            }
        }
    }

    private void RefreshBoardOpenValues()
    {
        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                Vector2Int tilePos = new Vector2Int(i, j);
                CheckForPriorityValues(tilePos);
            }
        }
    }

    private List<int> CheckForPriorityValues(Vector2Int gridPosition)
    {
        Tile currentTile = boardTiles[gridPosition.x, gridPosition.y];
        List<int> currentTileOpenValues = currentTile.GetOpenValuesList();

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                bool isCurrentTile = i == gridPosition.x && j == gridPosition.y;

                if (isCurrentTile)
                {
                    continue;
                }

                bool ijTileIsLinked = CheckIfLinkedToCurrentTile(i,j);

                if (ijTileIsLinked)
                {
                    List<int> linkedCellOpenValues = boardTiles[i, j].GetOpenValuesList();

                    bool linkedCellHasPriority = linkedCellOpenValues.Count < 2; // currentTileOpenValues.Count;

                    if (!linkedCellHasPriority)
                    {
                        continue;
                    }

                    foreach (int openNumber in linkedCellOpenValues)
                    {
                        if (currentTileOpenValues.Contains(openNumber))
                        {
                            currentTile.LockValue(openNumber);
                        }
                    }
                }
            }
        }

        List<int> priorityValues = currentTile.GetOpenValuesList();

        return priorityValues;

        bool CheckIfLinkedToCurrentTile(int i, int j)
        {
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

            return isLinkedToCurrentTile;
        }
    }

    public void LockBoardValues(int value, Vector2Int gridPosition)
    {
        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                bool isCurrentTile = i == gridPosition.x && j == gridPosition.y;

                if (isCurrentTile)
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

                if (sameRowOrCollumn || withinClusterRange)
                {
                    boardTiles[i, j].LockValue(value);
                }
            }
        }
    }

    private void CheckBoardValidity()
    {
        // 1. Loop through the whole board,
        // 2. Look for tiles with only one valid entry,
        // 3. Block relevant linked tiles from said value

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                int currentTileOpenValues = boardTiles[i, j].GetOpenValuesList().Count;

                if (currentTileOpenValues == 1 && !boardTiles[i, j].CorrectlyAssigned)
                {
                    int tileValue = boardTiles[i, j].GetOpenValuesList()[0];
                    Vector2Int tilePos = new Vector2Int(i, j);
                    LockBoardValues(tileValue, tilePos);
                }
            }
        }
    }

    public void TryInput(Vector2Int tilePosition)
    {
        int x = tilePosition.x;
        int y = tilePosition.y;
        int inputValue = boardTiles[x, y].TileValue;

        if (inputValue <= 0) // input == 0 means empty tile
        {
            return;
        }

        bool inputOkay = VerifyInput(inputValue, tilePosition);

        if (!inputOkay)
        {
            // throw a red flag on the input tile
        }
        else
        {
            // write the current player selection into the clicked tile 
        }
    }

    private bool VerifyInput(int inputValue, Vector2Int tilePosition)
    {
        VerifyHorizontal(inputValue, tilePosition);
        VerifyCluster(inputValue, tilePosition);
        VerifyVertical(inputValue, tilePosition);

        bool horizontalOk = VerifyHorizontal(inputValue, tilePosition);
        bool verticalOk = VerifyVertical(inputValue, tilePosition);
        bool clusterOk = VerifyCluster(inputValue, tilePosition);

        bool inputOkay = horizontalOk && verticalOk && clusterOk;

        return inputOkay;

        bool VerifyHorizontal(int inputValue, Vector2Int tilePosition)
        {
            for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
            {
                if (inputValue == boardTiles[i, tilePosition.y].TileValue)
                {
                    return false;
                }
            }

            return true;
        }

        bool VerifyVertical(int inputValue, Vector2Int tilePosition)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                if (inputValue == boardTiles[tilePosition.x, j].TileValue)
                {
                    return false;
                }
            }

            return true;
        }

        bool VerifyCluster(int inputValue, Vector2Int tilePosition)
        {
            int xStartOffset = tilePosition.x % 3;
            int yStartOffset = tilePosition.y % 3;

            int xStart = tilePosition.x - xStartOffset;
            int yStart = tilePosition.y - yStartOffset;
            int xFinish = tilePosition.x + (3 - xStartOffset);
            int yFinish = tilePosition.y + (3 - yStartOffset);

            for (int i = xStart; i < xFinish; i++)
            {
                for (int j = yStart; j < yFinish; j++)
                {
                    if (inputValue == boardTiles[i, j].TileValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}