using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

class Graph
{
    private Button[,] _buttons;
    private int _rows;
    private int _cols;

    public Graph(Button[,] buttons)
    {
        _buttons = buttons;
        _rows = buttons.GetLength(0);
        _cols = buttons.GetLength(1);
    }

    public List<Button> GetDFS(int row, int col)
    {
        List<Button> result = new List<Button>();
        bool[,] visited = new bool[_rows, _cols];
        DFS(row, col, visited, result);
        return result;
    }

    private void DFS(int row, int col, bool[,] visited, List<Button> result)
    {
        if (row < 0 || row >= _rows || col < 0 || col >= _cols || visited[row, col])
            return;

        Button button = _buttons[row, col];
        visited[row, col] = true;
        result.Add(button);

        // DFS in row and column directions
        DFS(row + 1, col, visited, result);
        DFS(row - 1, col, visited, result);
        DFS(row, col + 1, visited, result);
        DFS(row, col - 1, visited, result);

        // DFS diagonally
        DFS(row + 1, col + 1, visited, result);
        DFS(row - 1, col - 1, visited, result);
        DFS(row - 1, col + 1, visited, result);
        DFS(row + 1, col - 1, visited, result);
    }

    public List<Button> GetBFS(int row, int col)
    {
        List<Button> result = new List<Button>();
        bool[,] visited = new bool[_rows, _cols];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        visited[row, col] = true;
        queue.Enqueue(new Vector2Int(row, col));

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentRow = current.x;
            int currentCol = current.y;

            Button button = _buttons[currentRow, currentCol];
            result.Add(button);

            // Check neighbors in row and column directions
            CheckAndEnqueue(currentRow + 1, currentCol, visited, queue);
            CheckAndEnqueue(currentRow - 1, currentCol, visited, queue);
            CheckAndEnqueue(currentRow, currentCol + 1, visited, queue);
            CheckAndEnqueue(currentRow, currentCol - 1, visited, queue);

            // Check neighbors diagonally
            CheckAndEnqueue(currentRow + 1, currentCol + 1, visited, queue);
            CheckAndEnqueue(currentRow - 1, currentCol - 1, visited, queue);

            CheckAndEnqueue(currentRow - 1, currentCol + 1, visited, queue);
            CheckAndEnqueue(currentRow + 1, currentCol - 1, visited, queue);
        }

        return result;
    }

    private void CheckAndEnqueue(int row, int col, bool[,] visited, Queue<Vector2Int> queue)
    {
        if (row >= 0 && row < _rows && col >= 0 && col < _cols && !visited[row, col])
        {
            visited[row, col] = true;
            queue.Enqueue(new Vector2Int(row, col));
        }
    }
}

public class GridManager : MonoBehaviour
{
    [SerializeField] Button buttonPrefab;
    [SerializeField] Button[,] buttons;
    [SerializeField] GameManager _game_Manager;
    [SerializeField] Stack<Button> nodes;

    private void Start()
    {
        _game_Manager = FindObjectOfType<GameManager>();
        CreateGrid();
        EnemyAISpawn();
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

    public void Shift(int r, int c, int r1, int c1) //This method is used to update the player sprite location.
    {
        if (buttons[r1, c1].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled == false)
        {
            buttons[r, c].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            buttons[r1, c1].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            _game_Manager.UpdatePlayerPosition(r1, c1);
        }
    }

    public void Fire(int r, int c, int d)
    {
        Graph graph = new Graph(buttons);
        if(Safe(r, c))
        {
            ActivateColors(graph.GetBFS(r, c));
        }
/*        if (RightSafe(r, c + 1))
        {
            ActivateColors(graph.GetBFS(r, c + 1));
            
        }
        if(LeftSafe(r, c - 1))
        {
            ActivateColors(graph.GetBFS(r, c - 1));

        }
        if(UpSafe(r - 1, c))
        {
            ActivateColors(graph.GetBFS(r - 1, c));

        }
        if(DownSafe(r + 1, c))
        {
            ActivateColors(graph.GetBFS(r + 1, c));

        }*/
    }

    private void ActivateColors(List<Button> nodes)
    {
        foreach(Button node in nodes)
        {
            node.image.color = Color.red;
        }
    }

    public void VanishColors()
    {
        for(int row = 0; row < 5; row ++)
        {
            for(int col = 0; col < 4; col ++)
            {
                if (buttons[row, col].image.color == Color.red)
                {
                    buttons[row, col].image.color = Color.white;
                }
            }
        }
    }

    #region ENEMY_ACTIVITY
    private void EnemyAISpawn()
    {
        int row = Random.Range(0, 5);
        int col = Random.Range(0, 4);

        if (!buttons[row, col].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled)
        {
            buttons[row, col].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    #endregion

    #region CARDINAL_MATRIX_CHECK
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
    #endregion
}
