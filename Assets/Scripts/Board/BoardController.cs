using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup tileHolder;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private int tilesToInitialize;

    private Tile[,] boardTiles;
    private const int SUDOKU_BOARD_SIZE = 9;
    private int random = 0;

    private void Awake()
    {
        boardTiles = new Tile[SUDOKU_BOARD_SIZE, SUDOKU_BOARD_SIZE];

        for (int i = 0; i < SUDOKU_BOARD_SIZE * SUDOKU_BOARD_SIZE; i++)
        {
            Tile newTile = Instantiate(tilePrefab, tileHolder.transform);

            newTile.OpenAllPossibleTileValues();
            newTile.TileValue = 0;

            newTile.gameObject.name = "(" + i % 9 + "," + i / 9 + ")";

            boardTiles[i % 9, i / 9] = newTile;
        }

        InitializeAllTileValues();
    }

    private void InitializeAllTileValues()
    {
        List<int> preCollapseList = new List<int>();

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                Tile currentTile = boardTiles[i, j];

                if (currentTile.TileValue == 0)
                {
                    preCollapseList = currentTile.GetPossibleTileValues();
                    
                    // must add a check to see if choosing this particular value nullifies neighbouring cells.
                    // idea: loop on i,j and clusters, find "extreme values"
                        // (numbers that must not be chosen) and ignore them.
                    // to define extreme values, I could lower the probability of choosing a given N
                        // based on how many times it already was chosen
                        
                        
                    // Brainstorm: the algorithm will always break on a number which was NOT previously chosen,
                    // therefore, forcing repetition seems to be a good idea?? Maybe??
                    
                    // Alternatively, I could just look more closely into a WFC implementation,
                        // but lets try to be raw first

                    if (preCollapseList.Count == 0)
                    {
                        Debug.LogError("Algorithm got empty list on index " + i + "," + j);
                    }

                    random = preCollapseList.CopyRandomElement();

                    boardTiles[i, j].TileValue = random;
                    Vector2Int tilePos = new Vector2Int(i, j);

                    LockBoardValues(random, tilePos);
                }
            }
        }
    }

    public void LockBoardValues(int value, Vector2Int gridPosition)
    {
        // Coarse lock
        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
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

                if (sameRowOrCollumn || withinClusterRange)
                {
                    boardTiles[i, j].LockValue(value);
                }
            }
        }
        
        
        // // Smart lock
        // for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        // {
        //     for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
        //     {
        //         // Code yet to be implemented
        //     }
        // }
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