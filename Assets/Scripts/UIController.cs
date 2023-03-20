using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Common")]
    [SerializeField]
    private StageController         stageController;

    [Header("InGame")]
    [SerializeField]
    private TextMeshProUGUI         textCurrentScore;
    [SerializeField]
    private TextMeshProUGUI         textHighScore;
    [SerializeField]
    private UIPausePanelAnimation   pausePanel;

    [Header("GameOver")]
    [SerializeField]
    private GameObject              panelGameOver;
    [SerializeField]
    private Screenshot              screenshot;
    [SerializeField]
    private Image                   imageScreenshot;
    [SerializeField]
    private TextMeshProUGUI         textResultScore;

    private void Update()
    {
        textCurrentScore.text   = stageController.CurrentScore.ToString();
        textHighScore.text      = stageController.HighScore.ToString();
    }

    public void BtnClickPause()
    {
        pausePanel.OnApper();
    }

    public void BtnClickHome()
    {
        SceneManager.LoadScene("01Main");
    }

    public void BtnClickReStart()
    {
        SceneManager.LoadScene("02Game");
    }

    public void BtnClickPlay()
    {
        pausePanel.OnDisappear();
    }

    public void GameOver()
    {
        imageScreenshot.sprite  = screenshot.ScreenshotToSprite();
        textResultScore.text = stageController.CurrentScore.ToString();

        panelGameOver.SetActive(true);
    }
}
