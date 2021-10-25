using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Tile : MonoBehaviour
{
    private int tileValue;
    private Button _button;
    private Text _text;
    
    public int TileValue => tileValue;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<Text>();
        _button.onClick.AddListener(RefreshText);
    }

    private void ChangeIndex(int newValue)
    {
        tileValue = newValue;
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
