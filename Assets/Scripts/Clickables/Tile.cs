using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Tile : MonoBehaviour
{
    [HideInInspector] public Vector2Int TilePosition = new Vector2Int();
    private int tileValue = 0;
    private Button _button;
    private Text _text;

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
}