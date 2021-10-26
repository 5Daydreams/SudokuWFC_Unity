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

    private Tile[,] boardTiles = new Tile[SUDOKU_BOARD_SIZE, SUDOKU_BOARD_SIZE];
    private const int SUDOKU_BOARD_SIZE = 9;
    private int random = 0;

    private void Awake()
    {
        for (int i = 0; i < SUDOKU_BOARD_SIZE * SUDOKU_BOARD_SIZE; i++)
        {
            Tile newTile = Instantiate(tilePrefab, tileHolder.transform);
            boardTiles[i % 9, i / 9] = newTile;
        }

        foreach (var tile in boardTiles)
        {
            tile.TileValue = 0;
        }

        InitializeAllTileValues();
    }

    private void InitializeAllTileValues()
    {
        var randomList = GetRandomNumberList();
        random = randomList.PopRandomElement();

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            for (int j = 0; j < SUDOKU_BOARD_SIZE; j++)
            {
                if (boardTiles[i, j].TileValue == 0)
                {
                    Vector2Int tilePos = new Vector2Int(i, j);

                    int attemptCounts = 0;

                    while (!VerifyInput(random, tilePos))
                    {
                        random = randomList.PopRandomElement();
                        attemptCounts++;

                        if (randomList.Count == 0) // emergency exit condition
                        {
                            randomList = GetRandomNumberList();
                            continue;
                        }

                        if (attemptCounts > 500)
                        {
                            return;
                        }
                    }

                    boardTiles[i, j].TileValue = random;
                    InitializeAllTileValues();
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

    private List<int> GetRandomNumberList()
    {
        List<int> list1to9 = new List<int>();
        
        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            list1to9.Add(i+1);
        }

        return list1to9;
    }
}