using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Tile : MonoBehaviour
{
    private int tileValue = 0;
    private bool[] openValues = new bool[SUDOKU_BOARD_SIZE];
    private Button _button;
    private Text _text;
    
    private const int SUDOKU_BOARD_SIZE = 9;
    
    public int TileValue
    {
        get => tileValue;

        set
        {
            tileValue = value;
            RefreshText();
        }
    }

    void Awake()
    {
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<Text>();
        _button.onClick.AddListener(RefreshText);
    }

    public void OpenAllPossibleTileValues()
    {
        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            openValues[i] = true;
        }
    }
    
    private void RefreshText()
    {
        if (tileValue > 0)
        {
            _text.text = tileValue.ToString();
        }
        else
        {
            _text.text = "";
        }
    }

    public void LockValue(int value)
    {
        openValues[value - 1] = false;
    }

    public List<int> GetPossibleTileValues()
    {
        List<int> possibleTileValues = new List<int>();
        
        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            if (openValues[i])
            {
                possibleTileValues.Add(i+1);
            }
        }

        return possibleTileValues;
    }
}