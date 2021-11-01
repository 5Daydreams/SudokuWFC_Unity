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
        Tile selectedTile = null;
        Vector2Int selectedTilePos = new Vector2Int();
        
        int lowestOpenValues = 10;

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                Tile currentTile = boardTiles[i, j];

                int currentTileOpenValues = currentTile.GetOpenValuesList().Count;

                bool tileIsAlreadyAssigned = currentTile.CorrectlyAssigned;

                if (tileIsAlreadyAssigned)
                {
                    continue;
                }
                
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

        int collapsedValue = selectedTile.GetOpenValuesList().CopyRandomElement();
        selectedTile.TileValue = collapsedValue;
        LockValuesOnLinkedTiles(collapsedValue, selectedTilePos);

        InitializeAllTileValues(); // Yes, we want to recurse
    }

    private void LockValuesOnLinkedTiles(int inputValue, Vector2Int tilePos)
    {
        List<Tile> linkedTilesList = GetLinkedTiles(tilePos);

        foreach (Tile tile in linkedTilesList)
        {
            tile.LockValue(inputValue);
        }
    }
    
    private void LockValuesOnLinkedTiles(int inputValue,int i, int j)
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