using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DT_UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region GAME_MANAGER_BEGINS
    //This is the game manager where  all the track of the progress of the games are detected or actions will be toook from here.
    [SerializeField] private UIManager _ui_manager;
    [SerializeField] private GridManager _grid_manager;
    public PlayerCoordinate playerPosition;
    public TMP_Text _score_Text;
    public int _score;
    public Sprite[] playerSprites;
    public Sprite[] enemySprites;
    public int x;
    [SerializeField] AudioClip[] soundEffects;

    private void Awake()
    {
        x = Random.Range(0, 3);
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
                _grid_manager.VanishColors(true);
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row, playerPosition.col - 1);
            }
        });

        _ui_manager.GetButton(key.right).onClick.AddListener(() =>
        {
            //Do something when the right button will be clicked.
            if (_grid_manager.RightSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors(true);
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row, playerPosition.col + 1);
            }
        });

        _ui_manager.GetButton(key.up).onClick.AddListener(() =>
        {
            //Do something when the up button will be clicked.
            if (_grid_manager.UpSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors(true);
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row - 1, playerPosition.col);
            }
        });

        _ui_manager.GetButton(key.down).onClick.AddListener(() =>
        {
            //Do something when the down button will be clicked.
            if (_grid_manager.DownSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors(true);
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row + 1, playerPosition.col);
            }
        });

        _ui_manager.GetButton(key.fire).onClick.AddListener(() =>
        {
            _grid_manager.VanishColors(true);

            PlayAudio(0);//Play shoot audio clip from the array;

            _grid_manager.Fire(playerPosition.row, playerPosition.col, 2);
            _grid_manager.CheckEnemy();
        });

        _ui_manager.GetButton(key.back).onClick.AddListener(() =>
        {
            SceneManager.LoadScene("StartScene");
        });
    }

    public void UpdatePlayerPosition(int r, int c)
    {
        playerPosition = new PlayerCoordinate { row = r, col = c };
    }
    
    public void PlayAudio(int index)
    {
        var audio = gameObject.AddComponent<AudioSource>();
        audio.clip = soundEffects[index];
        audio.Play();
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
