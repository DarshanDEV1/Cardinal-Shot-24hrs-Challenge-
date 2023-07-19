using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DT_UI_TUTORIAL;
using UnityEngine.SceneManagement;

public class TuTGameManager : MonoBehaviour
{
    #region GAME_MANAGER_BEGINS

    #region Variables, Objects, References

    //This is the game manager where  all the track of the progress of the games are detected or actions will be toook from here.
    [SerializeField] private TuTUIManager _ui_manager;
    [SerializeField] private TuTGridManager _grid_manager;
    public PlayerCoordinate playerPosition;
    public TMP_Text _score_Text;
    public int _score;
    public int _high_score;
    public Sprite[] playerSprites;
    public Sprite[] enemySprites;
    public int x;
    [SerializeField] AudioClip[] soundEffects;
    [SerializeField] GameObject TutorialPanel;
    [SerializeField] GameObject TutorialGameObject;
    [SerializeField] GameObject GamePlayGameObject;
    [SerializeField] PanelShift panelShift;
    public bool buttonTutorial;

    #endregion

    private void Awake()
    {
        _high_score = PlayerPrefs.GetInt("Score", 0);
        _ui_manager = FindObjectOfType<TuTUIManager>();
        _grid_manager = FindObjectOfType<TuTGridManager>();

        //At the beginning of the game the tutorial will be enabled and the gameplay will be disabled.
        //It will check the player prefs if the game is played by the user for the first time or not
        //If the user is not playing for the first time then the gameplay gameobject will be set active to true.
        //Other wise the tutorial will be there to give the user an idea about the game.
        TutorialGameObject.SetActive(true);
        GamePlayGameObject.SetActive(false);
        buttonTutorial = false;
    }

    private void Start()
    {
        bool config = PlayerPrefs.GetInt("Tutorial", 1) == 1 ? true : false;

        if (config)
        {
            TutorialGameObject.SetActive(true);
            GamePlayGameObject.SetActive(false);
        }
        else
        {
            GamePlayGameObject.SetActive(true);
            TutorialGameObject.SetActive(false);
        }


        x = Random.Range(0, 3);
        playerPosition = new PlayerCoordinate { row = 1, col = 1 };
        Movement();
        if (PlayerPrefs.GetInt("Tutorial") == 1 ? true : false)
        {
            StartCoroutine(TutorialAnimation());
        }
    }

    private IEnumerator TutorialAnimation()
    {
        TutorialPanel.SetActive(true);
        TutorialPanel.GetComponent<Animator>().Play("TutorialStartAnimation");
        yield return new WaitForSeconds(1f);

        TutorialPanel.GetComponent<Animator>().Play("TutorialEndAnimation");
        TutorialPanel.SetActive(false);
    }


    #region PLAYER_MOVEMENT_ACTIVITY

    private void Movement()
    {
        KeyValue key = new KeyValue();

        #region LEFT_BUTTON

        panelShift.ActivateDeactivatePanels(true, 0);
        _ui_manager.GetButton(key.left).onClick.AddListener(() =>
        {
            //Do something when the left button will be clicked.
            if (_grid_manager.LeftSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors(true);
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row, playerPosition.col - 1);
                _grid_manager.CheckPlayer();

            }
            if (!panelShift.bools[0])
            {

                panelShift.bools[0] = true;
                panelShift.ActivateDeactivatePanels(true, 1);
            }
        });

        #endregion


        #region RIGHT_BUTTON

        _ui_manager.GetButton(key.right).onClick.AddListener(() =>
        {
            //Do something when the right button will be clicked.
            if (_grid_manager.RightSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors(true);
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row, playerPosition.col + 1);
                _grid_manager.CheckPlayer();

            }
            if (!panelShift.bools[1])
            {

                panelShift.bools[1] = true;
                panelShift.ActivateDeactivatePanels(true, 2);
            }
        });

        #endregion


        #region UP_BUTTON

        _ui_manager.GetButton(key.up).onClick.AddListener(() =>
        {
            //Do something when the up button will be clicked.
            if (_grid_manager.UpSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors(true);
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row - 1, playerPosition.col);
                _grid_manager.CheckPlayer();

            }
            if (!panelShift.bools[2])
            {

                panelShift.bools[2] = true;
                panelShift.ActivateDeactivatePanels(true, 3);
            }
        });

        #endregion


        #region DOWN_BUTTON

        _ui_manager.GetButton(key.down).onClick.AddListener(() =>
        {
            //Do something when the down button will be clicked.
            if (_grid_manager.DownSafe(playerPosition.row, playerPosition.col))
            {
                _grid_manager.VanishColors(true);
                _grid_manager.Shift(playerPosition.row, playerPosition.col, playerPosition.row + 1, playerPosition.col);
                _grid_manager.CheckPlayer();

            }
            if (!panelShift.bools[3])
            {

                panelShift.bools[3] = true;
                panelShift.ActivateDeactivatePanels(true, 4);
            }
        });

        #endregion


        #region FIRE_BUTTON

        _ui_manager.GetButton(key.fire).onClick.AddListener(() =>
        {
            _grid_manager.VanishColors(true);

            PlayAudio(0);//Play shoot audio clip from the array;

            _grid_manager.Fire(playerPosition.row, playerPosition.col, 2);
            _grid_manager.CheckEnemy();

            if (!panelShift.bools[4])
            {

                panelShift.bools[4] = true;
                panelShift.ActivateDeactivatePanels(false, -1);
                buttonTutorial = true;
            }
        });

        #endregion


        #region BACK_BUTTON

        _ui_manager.GetButton(key.back).onClick.AddListener(() =>
        {
            SceneManager.LoadScene("StartScene");
        });

        #endregion
    }

    public void UpdatePlayerPosition(int r, int c)
    {
        playerPosition = new PlayerCoordinate { row = r, col = c };
    }

    #endregion

    public void PlayAudio(int index)
    {
        var audio = gameObject.AddComponent<AudioSource>();
        audio.clip = soundEffects[index];
        audio.Play();
    }

    public void HighScoreUpdate(int score)
    {
        if (score > _high_score)
        {
            PlayerPrefs.SetInt("Score", score);
        }
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
