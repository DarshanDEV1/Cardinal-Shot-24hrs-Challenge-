using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

#region DATA_STRUCTURES
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

public class GridManager : MonoBehaviour
{
    #region TUTORIALS ARE TO BE ADDED
    //Tutorials are to be added for the improvement of the user interaction
    #endregion

    [SerializeField] Button buttonPrefab;
    [SerializeField] Button[,] buttons;
    [SerializeField] GameManager _game_Manager;
    /*    [SerializeField] Stack<Button> nodes;*/
    public int current_move;
    [SerializeField] Queue<int> shapesStack = new Queue<int>();
    [SerializeField] IndicatorGrid _indicator_Grid;
    [SerializeField] EnemyPosition _enemy_Position;
    [SerializeField] GameObject _gameOverPanel;

    int x;

    private void Start()
    {
        _game_Manager = FindObjectOfType<GameManager>();
        _indicator_Grid = FindObjectOfType<IndicatorGrid>();
        _enemy_Position = new EnemyPosition { row = 2, col = 2 };

        _gameOverPanel.SetActive(false);

        CreateGrid();
        EnemyAISpawn();
        StartCoroutine(EnemyAIActivityInterval());
        Shapes();
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

                PlayerRespawn(buttonRow, buttonCol);
                button.onClick.AddListener(() =>
                {
                    //Do Something if the button is clicked here

                });
            }
        }
    }

    public void Shift(int r, int c, int r1, int c1) //This method is used to update the player sprite location.
    {
        if (buttons[r1, c1].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled == false
            && buttons[r1, c1].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled == false)
        {
            //Color s_color = buttons[r, c].image.color;
            //Color s_color = buttons[r, c].transform.GetChild(0).GetComponent<SpriteRenderer>().color;

            buttons[r, c].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            //buttons[r, c].image.color = Color.white;
            //buttons[r, c].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;

            buttons[r1, c1].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            //buttons[r1, c1].image.color = s_color;
            //buttons[r1, c1].transform.GetChild(0).GetComponent<SpriteRenderer>().color = s_color;

            _game_Manager.UpdatePlayerPosition(r1, c1);
        }
    }

    public void Fire(int r, int c, int d)
    {
        Graph graph = new Graph(buttons);
        if (Safe(r, c))
        {
            if (shapesStack.Count > 0)
            {
                current_move = shapesStack.Dequeue();
                ActivateColors(graph.GetBFS(r, c, current_move), true);
                _indicator_Grid.ChangeSignal(shapesStack);
            }
        }
    }

    private void ActivateColors(List<Button> nodes, bool value)
    {
        foreach (Button node in nodes)
        {
            if (value)
            {
                if (node != buttons[_game_Manager.playerPosition.row, _game_Manager.playerPosition.col])
                {
                    //node.image.color = Color.red;
                    StartCoroutine(PathVanisher(Color.red, node, 1f));
                }
            }
            else
            {
                if (node != buttons[_enemy_Position.row, _enemy_Position.col])
                {
                    //node.image.color = Color.cyan;
                    StartCoroutine(PathVanisher(Color.cyan, node, 1.5f));
                }
            }
        }
    }

    private IEnumerator PathVanisher(Color color, Button button, float time)
    {
        button.image.color = color;
        yield return new WaitForSeconds(time);
        button.image.color = Color.white;
    }


    public void VanishColors(bool value)
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (value)
                {
                    if (buttons[row, col].image.color == Color.red)
                    {
                        buttons[row, col].image.color = Color.white;
                    }
                }
                else
                {
                    if (buttons[row, col].image.color == Color.cyan)
                    {
                        buttons[row, col].image.color = Color.white;
                    }
                }
            }
        }
    }

    private void Shapes()
    {
        StartCoroutine(ShapePush(true));
        //StopAllCoroutines();
    }

    private IEnumerator ShapePush(bool value)
    {
        while (value)
        {

            yield return new WaitForSeconds(1f);
            int m = Random.Range(0, 8);
            shapesStack.Enqueue(m);
        }
    }

    public void CheckPlayer()
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (buttons[row, col].image.color == Color.cyan)
                {
                    if (buttons[row, col].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled)
                    {
                        if (_game_Manager._score > 0)
                        {
                            _game_Manager._score--;
                            _game_Manager.PlayAudio(2);
                            _game_Manager.HighScoreUpdate(_game_Manager._score);
                            _game_Manager._score_Text.text = "Score : " + _game_Manager._score.ToString();
                            buttons[row, col].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                            PlayerSpawn();
                        }
                        else
                        {
                            //Switch On The Game Over Panel
                            StartCoroutine(GameOverPanel());
                        }
                    }
                }
            }
        }
    } //Check if the player is dies.

    private IEnumerator GameOverPanel()
    {
        _gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        _gameOverPanel.SetActive(false);
        SceneManager.LoadScene("StartScene");
        StopAllCoroutines();
    }

    private void PlayerRespawn(int buttonRow, int buttonCol)
    {
        //Player position
        if (buttonRow == 0 && buttonCol == 0)
        {
            var m = buttons[buttonRow, buttonCol];
            m.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

            //m.image.color = color[_game_Manager.x];
            //m.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color[_game_Manager.x];
            //Debug.Log(color[_game_Manager.x]);
            //x = _game_Manager.x;
            /*m.transform.GetChild(0).GetComponent<SpriteRenderer>().color = (x == 0 ? Color.green :
                                                                            x == 1 ? Color.blue : Color.yellow);*/
        }
    }

    private void PlayerSpawn()
    {
        int row = Random.Range(0, 5);
        int col = Random.Range(0, 4);

        while (row == _enemy_Position.row || col == _enemy_Position.col
            || buttons[row, col].image.color == Color.cyan)
        {
            row = Random.Range(0, 5);
            col = Random.Range(0, 4);
        }
        if (!buttons[row, col].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled
            && !buttons[row, col].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled)
        {
            buttons[row, col].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            _game_Manager.UpdatePlayerPosition(row, col);
            PlayerRespawn(row, col);
        }
    }

    #region ENEMY_ACTIVITY

    public void CheckEnemy()
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (buttons[row, col].image.color == Color.red)
                {
                    if (buttons[row, col].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled)
                    {
                        _game_Manager._score++;
                        _game_Manager.HighScoreUpdate(_game_Manager._score);
                        _game_Manager.PlayAudio(1);
                        _game_Manager._score_Text.text = "Score : " + _game_Manager._score.ToString();
                        buttons[row, col].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;


                        /*int number = 0;
                        if (
                        _game_Manager._score >

                            (_game_Manager._score > 5 ? number = Random.Range(5, 11) :
                            _game_Manager._score > 10 ? number = Random.Range(11, 16) :
                            _game_Manager._score > 15 ? number = Random.Range(16, 21) :
                            _game_Manager._score > 20 ? number = Random.Range(21, 26) :
                        _game_Manager._score > 25 ? number = _game_Manager._score + 10 :
                        number = Random.Range(1, 101))

                            && _game_Manager._score < number + 5

                           )
                        {
                            for (int i = 0;

                                i < (number > 5 && number < 10 ? 1 :
                                    ((Random.Range(0, 2)) == 1 ? (Mathf.Ceil(number / 10)) :
                                    Mathf.Floor(number / 10)));

                                i++) EnemyAISpawn(); //This is the for loop for spawning random number of enemies
                        }*///Depricated Logic


                        int numberOfEnemies = _game_Manager._score < 25 ? 1 :
                                Mathf.CeilToInt(_game_Manager._score / 20.0f);

                        // Spawn the enemies
                        for (int i = 0; i < numberOfEnemies; i++)
                        {
                            EnemyAISpawn();
                        }
                    }
                }
            }
        }
    } //Check if the enemy is dies.

    private void EnemyAISpawn()
    {
        int row = Random.Range(0, 5);
        int col = Random.Range(0, 4);

        while (row == _game_Manager.playerPosition.row || col == _game_Manager.playerPosition.col
            || buttons[row, col].image.color == Color.red)
        {
            row = Random.Range(0, 5);
            col = Random.Range(0, 4);
        }
        if (!buttons[row, col].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled
            && !buttons[row, col].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled)
        {
            buttons[row, col].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            //buttons[row, col].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            _enemy_Position = new EnemyPosition { row = row, col = col };
        }
    }

    private void EnemyAIActivity()
    {
        Graph graph = new Graph(buttons);
        ActivateColors(graph.GetBFS(_enemy_Position.row, _enemy_Position.col, Random.Range(0, 8)), false);
    }

    private IEnumerator EnemyAIActivityInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            VanishColors(false);
            EnemyAIActivity();
            CheckPlayer();
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

    #region STRUCTURES

    public struct EnemyPosition
    {
        public int row;
        public int col;
    }

    #endregion
}