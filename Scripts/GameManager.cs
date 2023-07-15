using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DT_UI;
using JetBrains.Annotations;
using TMPro.EditorUtilities;

public class GameManager : MonoBehaviour
{
    #region GAME_MANAGER_BEGINS
    //This is the game manager where  all the track of the progress of the games are detected or actions will be toook from here.
    [SerializeField] private UIManager _ui_manager;
    [SerializeField] private GridManager _grid_manager;
    [SerializeField] private PlayerCoordinate playerPosition;
    public Sprite[] playerSprites;
    public Sprite[] enemySprites;

    private void Awake()
    {
        _ui_manager = FindObjectOfType<UIManager>();
        _grid_manager = FindObjectOfType<GridManager>();
    }

    private void Start()
    {
        playerPosition = new PlayerCoordinate { row = 0, col = 0 };
        Movement();
    }

    private void Movement()
    {
        KeyValue key = new KeyValue();

        _ui_manager.GetButton(key.left).onClick.AddListener(() =>
        {
            //Do something when the left button will be clicked.
            if (_grid_manager.LeftSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors();
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row, playerPosition.col - 1);
            }
        });

        _ui_manager.GetButton(key.right).onClick.AddListener(() =>
        {
            //Do something when the right button will be clicked.
            if (_grid_manager.RightSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors();
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row, playerPosition.col + 1);
            }
        });

        _ui_manager.GetButton(key.up).onClick.AddListener(() =>
        {
            //Do something when the up button will be clicked.
            if (_grid_manager.UpSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors();
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row - 1, playerPosition.col);
            }
        });

        _ui_manager.GetButton(key.down).onClick.AddListener(() =>
        {
            //Do something when the down button will be clicked.
            if (_grid_manager.DownSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors();
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row + 1, playerPosition.col);
            }
        });

        _ui_manager.GetButton(key.fire).onClick.AddListener(() =>
        {
            _grid_manager.VanishColors();
            _grid_manager.Fire(playerPosition.row, playerPosition.col, 2);
        });
    }

    public void UpdatePlayerPosition(int r, int c)
    {
        playerPosition = new PlayerCoordinate { row = r, col = c };
    }

    #endregion

    #region STRUCTURES
    [System.Serializable]
    public struct PlayerCoordinate
    {
        public int row;
        public int col;
    }
    #endregion
}
