using DT_UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;

    [SerializeField] private GameObject exitConfigPanel;

    private void Start()
    {
        ButtonClickDetection();
        exitConfigPanel.SetActive(false);
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
    }

    private IEnumerator ExitPanelShow()
    {
        yield return new WaitForSeconds(1.5f);
        StopAllCoroutines();
        Application.Quit();
    }
}
