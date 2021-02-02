using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Chessman : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    public string player;

    public Sprite ml_fstone;
    public Sprite ml_fmain;
    public Sprite ml_fblock;
    public Sprite ml_sstone;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        SetCoords();

        switch (this.name)
        {
            case "ml_fstone": this.GetComponent<SpriteRenderer>().sprite = ml_fstone; player = "blue"; break;
            case "ml_fmain": this.GetComponent<SpriteRenderer>().sprite = ml_fmain; player = "Main"; break;
            case "ml_sstone": this.GetComponent<SpriteRenderer>().sprite = ml_sstone; player = "pink"; break;
            case "playerBlock": this.GetComponent<SpriteRenderer>().sprite = ml_fblock; player = "playerBlock"; break;
        }
    }
    
    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            PutMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); 
        }
    }

    public void PutMovePlates()
    {
        switch (this.name)
        {
            case "ml_fstone":
            case "ml_sstone":
                SurroundMovePlate();
                break;
        }
        numDistanceWrite();
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard + 0);
    }
    public void numDistanceWrite()
    {
        GameObject.FindGameObjectWithTag("num1").GetComponent<Text>().text = distanceCal();

        float x = xBoard;
        float y = yBoard;
        
        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;
        
        GameObject.FindGameObjectWithTag("num1").GetComponent<Text>().transform.position = new Vector2(x, y);
    }

    /*public GameObject asd;*/
    public string distanceCal()
    {
        Game mainCall = controller.GetComponent<Game>();

        var main = mainCall.returnMain();

        var distance = Math.Abs(xBoard - main.x) + Math.Abs(yBoard - main.y);

        return distance.ToString();
    }
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player == "Main")
            {
                MovePlateAttackSpawn(x, y);
            }

        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;
        
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        
        MovePlate mpScript = mp.GetComponent<MovePlate>();
            
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.SetReference(gameObject);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.attack = true;
    }
}
