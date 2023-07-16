using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DT_UI;
using Unity.VisualScripting;

#region DATA_STRUCTURES
class GraphIndicator
{
    private Button[,] _buttons;
    private int _rows;
    private int _cols;

    public GraphIndicator(Button[,] buttons)
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

    public List<Button> GetBFS(int row, int col, int mode)
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

            switch (mode)
            {
                // Check neighbors in row and column directions
                case 0:
                    CheckAndEnqueue(currentRow + 1, currentCol, visited, queue);//Vertical //Down
                    break;
                case 1:
                    CheckAndEnqueue(currentRow - 1, currentCol, visited, queue); //Up
                    break;


                case 2:
                    CheckAndEnqueue(currentRow, currentCol + 1, visited, queue);//Horizontal //Right
                    break;
                case 3:
                    CheckAndEnqueue(currentRow, currentCol - 1, visited, queue); //Left
                    break;



                // Check neighbors diagonally
                case 4:
                    CheckAndEnqueue(currentRow + 1, currentCol + 1, visited, queue);
                    break;
                case 5:
                    CheckAndEnqueue(currentRow - 1, currentCol - 1, visited, queue);
                    break;


                case 6:
                    CheckAndEnqueue(currentRow - 1, currentCol + 1, visited, queue);
                    break;
                case 7:
                    CheckAndEnqueue(currentRow + 1, currentCol - 1, visited, queue);
                    break;
            }
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
#endregion

public class IndicatorGrid : MonoBehaviour
{
    [SerializeField] private GridManager _grid_Manager;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Button[,] buttons;
    // Start is called before the first frame update
    void Start()
    {
        _grid_Manager = FindObjectOfType<GridManager>();
        CreateGrid();
    }
    void CreateGrid()
    {
        buttons = new Button[5, 5];

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                Button button = Instantiate(buttonPrefab, transform);
                button.transform.SetParent(transform);
                button.transform.localScale = Vector3.one;

                buttons[row, col] = button;

                int buttonRow = row;
                int buttonCol = col;
                button.name = "BTN: " + buttonRow.ToString() + " " + buttonCol.ToString();
                button.onClick.AddListener(() =>
                {
                    //Do Something if the button is clicked here

                });
            }
        }
    }
    public void ChangeSignal(Queue<int> moveQueue)
    {
        Graph graph = new Graph(buttons);
        Queue<int> queue = new Queue<int>();
        queue = moveQueue;
        if (queue.Count > 0)
        {
            int current_move = queue.Peek();
            ActivateColors(graph.GetBFS(2, 2, current_move));
        }
    }
    private void ActivateColors(List<Button> nodes)
    {
        VanishColors();
        Debug.Log("Called Activate Colors Method");
        foreach (Button node in nodes)
        {
            node.image.color = Color.red;
        }
    }
    public void VanishColors()
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (buttons[row, col].image.color == Color.red)
                {
                    buttons[row, col].image.color = Color.white;
                }
            }
        }
    }
}
