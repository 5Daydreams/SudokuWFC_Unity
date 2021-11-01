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

    private bool correctlyAssigned = false;
    public bool CorrectlyAssigned => correctlyAssigned;

    private const int SUDOKU_BOARD_SIZE = 9;

    public int TileValue
    {
        get => tileValue;

        set
        {
            if (CorrectlyAssigned)
            {
                Debug.LogWarning("Trying to set value of correct cell at: " + this.gameObject.name);
                return;
            }
            
            bool inputIsNotEmpty = value != 0;

            if (inputIsNotEmpty)
            {
                bool inputUnavailable = !openValues[value - 1];
                
                if (inputUnavailable)
                {
                    Debug.LogError("Attempting to set an unavailable value");
                    return;
                }

                for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
                {
                    if (i == value - 1)
                    {
                        openValues[i] = true;
                        continue;
                    }

                    openValues[i] = false;
                }

                correctlyAssigned = true;
            }

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

    public void InitializeTile()
    {
        TileValue = 0;
        correctlyAssigned = false;
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

    public void LockValue(int value, bool printValues = false)
    {
        if (this.CorrectlyAssigned)
        {
            return;
        }
        
        openValues[value - 1] = false;

        if (printValues)
        {
            PrintOpenValues();
        }
    }

    public void PrintOpenValues()
    {
        string openNumbersString = "";

        List<int> openNumbersList = GetOpenValuesList();

        for (int i = 0; i < openNumbersList.Count; i++)
        {
            openNumbersString += openNumbersList[i] + ",";
        }
        
        Debug.Log(this.gameObject.name + " has the following values open: " + openNumbersString);
    }

    public List<int> GetOpenValuesList()
    {
        List<int> possibleTileValues = new List<int>();
        bool tileIsWeird = CorrectlyAssigned && possibleTileValues.Count > 1;

        if (tileIsWeird)
        {
            Debug.LogWarning("Tile has weird values: " + this.gameObject.name);
        }

        for (int i = 0; i < SUDOKU_BOARD_SIZE; i++)
        {
            if (openValues[i])
            {
                possibleTileValues.Add(i + 1);
            }
        }

        return possibleTileValues;
    }
}