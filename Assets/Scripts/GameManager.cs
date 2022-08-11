using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GameObject gameLabel;

    Text gameText;

    PlayerMove player;

    public bool isESCKeyDowned;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }

    }

    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    public GameState gState;

    public GameObject gameOption;



    // Turn to Option Menu
    public void OpenOptionWindow()
    {
        print("옵션 버튼 누름");
        gameOption.SetActive(true);
        Time.timeScale = 0f;
        gState = GameState.Pause;
    }

    // Continue Game
    public void CloseOptionWindow()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1f;
        gState = GameState.Run;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // Reload Current Scene Number
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        print("게임 종료");
        Application.Quit();
    }



    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Ready;

        gameText = gameLabel.GetComponent<Text>();

        gameText.text = "Ready...";

        gameText.color = new Color32(255, 185, 0, 255);

        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();

        isESCKeyDowned = false;
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        gameLabel.SetActive(false);
        gState = GameState.Run;
    }

    // Update is called once per frame
    void Update()
    {
        // Open optin window with ESC Key
        if (Input.GetKeyDown(KeyCode.Escape) && isESCKeyDowned == false)
        {
            print("ESC KEY DOWNED, FALSE");
            OpenOptionWindow();
            isESCKeyDowned = true;
        }


        // Close optin window with ESC Key
        else if (Input.GetKeyDown(KeyCode.Escape) && isESCKeyDowned == true)
        {
            print("ESC KEY DOWNED, TRUE");
            CloseOptionWindow();
            isESCKeyDowned = false;
        }
        

        if (player.hp <= 0)
        {

            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            gameLabel.SetActive(true);
            gameText.text = "Game Over";

            gameText.color = new Color32(255, 0, 0, 255);

            Transform buttons = gameText.transform.GetChild(0);
            buttons.gameObject.SetActive(true);

            gState = GameState.GameOver;
        }
    }
}
