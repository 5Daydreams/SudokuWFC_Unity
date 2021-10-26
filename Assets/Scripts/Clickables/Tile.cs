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

    public void InitializePossibleTileValuesLikeADumbIdiot()
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
            if (openValues[i] == true)
            {
                possibleTileValues.Add(i+1);
            }
        }

        if (possibleTileValues.Count == 0)
        {
            Debug.LogError("Dear Unity, you are fucking retarded");
        }

        return possibleTileValues;
    }
}