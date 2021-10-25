using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private const int SUDOKU_BOARD_SIZE = 9;
    private Tile[,] boardTiles;

    public void VerifyCorrectInput(Vector2Int tilePosition)
    {
        int x = tilePosition.x;
        int y = tilePosition.y;
        int inputValue = boardTiles[x, y].TileValue;
        
        bool horizontalOk = VerifyHorizontal(inputValue, tilePosition);
        bool verticalOk = VerifyVertical(inputValue, tilePosition);
        bool clusterOk = VerifyCluster(inputValue, tilePosition);

        if (horizontalOk && verticalOk && clusterOk)
        {
            // do nothing, lol
        }
        else
        {
            // Tell the player they screwed up, paint a tile red or something
        }
    }

    private bool VerifyHorizontal(int inputValue, Vector2Int tilePosition)
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

    private bool VerifyVertical(int inputValue, Vector2Int tilePosition)
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

    private bool VerifyCluster(int inputValue, Vector2Int tilePosition)
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