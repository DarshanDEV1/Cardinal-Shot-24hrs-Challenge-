using DT_UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;
    [SerializeField] private Button onlineModeButton;

    [SerializeField] private GameObject exitConfigPanel;
    [SerializeField] private GameObject goodByePanel;

    [SerializeField] private TMP_Text _high_Score;


    private void Start()
    {
        int score = PlayerPrefs.GetInt("Score");
        _high_Score.text = "High Score : " + score.ToString();

        ButtonClickDetection();
        exitConfigPanel.SetActive(false);
        goodByePanel.SetActive(false);
    }
    private void ButtonClickDetection()
    {
        KeyValue key = new KeyValue();

        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        exitButton.onClick.AddListener(() =>
        {
            exitConfigPanel.SetActive(true);
        });

        acceptButton.onClick.AddListener(() =>
        {
            StartCoroutine(ExitPanelShow());
        });

        rejectButton.onClick.AddListener(() =>
        {
            StopAllCoroutines();
            exitConfigPanel.SetActive(false);
        });

        onlineModeButton.onClick.AddListener(() =>
        {

        });
    }

    private IEnumerator ExitPanelShow()
    {
        exitConfigPanel.SetActive(false);
        goodByePanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        StopAllCoroutines();
        Application.Quit();
    }
}
