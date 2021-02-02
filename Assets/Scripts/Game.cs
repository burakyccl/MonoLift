using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;

    public GameObject[,] positions = new GameObject[8, 8];
    public GameObject[] playerBlue = new GameObject[8];
    public GameObject[] playerPink = new GameObject[7];
    public GameObject[] playerBlock = new GameObject[49];

    private string currentPlayer = "blue";

    private bool gameOver = false;

    public bool twoPlayer = false;

    public void Start()
    {
        if (buttonClicked == true)
        {
            for (int i = 0; i < 15; i++)
            {
                RandomArrayValue();
            }

            for (int i = 0; i < 7; i++)
            {
                playerBlue[i] = Create("ml_fstone", (int)randmPosArray[i].x, (int)randmPosArray[i].y);
                if (twoPlayer == true)
                {
                    playerPink[i] = Create("ml_sstone", (int)randmPosArray[i + 7].x, (int)randmPosArray[i + 7].y);
                }
            }

            playerBlue[7] = Create("ml_fmain", (int)randmPosArray[14].x, (int)randmPosArray[14].y);

            for (int i = 0; i < playerBlue.Length; i++)
            {
                SetPosition(playerBlue[i]);
            }

            if (twoPlayer == true)
            {
                for (int i = 0; i < playerPink.Length; i++)
                {

                    SetPosition(playerPink[i]);
                }
            }
        }
    }



    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); 

        cm.name = name; 
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public Vector2 returnMain()
    {
        return randmPosArray[14];
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public int blueCount = 0;
    public int pinkCount = 0;

    public int countReturn()
    {
        if (currentPlayer == "blue")
        {
            blueCount++;
            return blueCount;
        }
        else
        {
            pinkCount++;
            return pinkCount;
        }
    }

    public void NextTurn()
    {
        if (currentPlayer == "blue")
        {
            currentPlayer = "pink";
        }
        else
        {
            currentPlayer = "blue";
        }
    }

    Vector2[] randmPosArray = new Vector2[15];
    int index = 0;
    void RandomArrayValue()
    {
    Start:
        Vector2 val;
        while (true)
        {
            val = new Vector2(UnityEngine.Random.Range(0, 8), UnityEngine.Random.Range(0, 8));
            for (int i = 0; i < randmPosArray.Length; i++)
            {
                if (val == randmPosArray[i]) goto Start;
            }
            goto Outer;
        }
    Outer:
        randmPosArray[index++] = val;

    }

    public void putBlock()
    {
        int i = 0;

        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        var clickX = Mathf.RoundToInt(worldPosition.x) + 3;
        var clickY = Mathf.RoundToInt(worldPosition.y) + 3;

        if (GetPosition(clickX, clickY) == null)
        {
            playerBlock[i] = Create("playerBlock", clickX, clickY);
            SetPosition(playerBlock[i]);
            i++;
        }
    }

    private bool buttonClicked = false;
    void onePlayerBtnClick()
    {
        GameObject.FindGameObjectWithTag("Menu").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("1player").SetActive(false);
        GameObject.FindGameObjectWithTag("2player").SetActive(false);
        twoPlayer = false;
        buttonClicked = true;
        Start();
    }

    void twoPlayerBtnClick()
    {
        GameObject.FindGameObjectWithTag("Menu").GetComponent<Image>().enabled = false;
        GameObject.FindGameObjectWithTag("1player").SetActive(false);
        GameObject.FindGameObjectWithTag("2player").SetActive(false);
        twoPlayer = true;
        buttonClicked = true;
        Start();
    }

    public void Update()
    {
        if (buttonClicked == false)
        {
            GameObject.FindGameObjectWithTag("1player").GetComponent<Button>().onClick.AddListener(onePlayerBtnClick);
            GameObject.FindGameObjectWithTag("2player").GetComponent<Button>().onClick.AddListener(twoPlayerBtnClick);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetMouseButtonDown(1))
        {
            putBlock();
        }

        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            
            SceneManager.LoadScene("Game"); 
        }
    }

    public void Winner()
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("Finish").GetComponent<Image>().enabled = true;
        
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = currentPlayer + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
