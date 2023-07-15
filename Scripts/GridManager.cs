using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] Button buttonPrefab;
    [SerializeField] Button[,] buttons;
    [SerializeField] GameManager _game_Manager;

    private void Start()
    {
        _game_Manager = FindObjectOfType<GameManager>();
        CreateGrid();
    }

    void CreateGrid()
    {
        buttons = new Button[5, 4];

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                Button button = Instantiate(buttonPrefab, transform);
                button.transform.SetParent(transform);
                button.transform.localScale = Vector3.one;

                buttons[row, col] = button;

                int buttonRow = row;
                int buttonCol = col;
                button.name = "BTN: " + buttonRow.ToString() + " " + buttonCol.ToString();
                //Player position
                if (buttonRow == 0 && buttonCol == 0)
                {
                    buttons[buttonRow, buttonCol].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }
                button.onClick.AddListener(() =>
                {
                    //Do Something if the button is clicked here

                });
            }
        }
    }

    public void Shift(int r, int c, int r1, int c1)
    {
        if (buttons[r1, c1].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled == false)
        {
            buttons[r, c].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            buttons[r1, c1].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            _game_Manager.UpdatePlayerPosition(r1, c1);
        }
    }
    private bool Safe(int row, int col)
    {
        if ((row >= 0 && row <= 4) &&
            (col >= 0 && col <= 3))
        {
            return true;
        }
        else
        {
            return false;
        }
    } //Border Check
    public bool LeftSafe(int row, int col)
    {
        if (Safe(row, col - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool RightSafe(int row, int col)
    {
        if (Safe(row, col + 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool UpSafe(int row, int col)
    {
        if (Safe(row - 1, col))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool DownSafe(int row, int col)
    {
        if (Safe(row + 1, col))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
