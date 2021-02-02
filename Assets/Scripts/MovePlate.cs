using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public GameObject chesspiece;
    public bool twoP;
    public int count;

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            count = controller.GetComponent<Game>().countReturn();
            if (count == 7) controller.GetComponent<Game>().Winner();

            Destroy(reference);
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(),
            reference.GetComponent<Chessman>().GetYBoard());

        
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        twoP = controller.GetComponent<Game>().twoPlayer;
        
        controller.GetComponent<Game>().SetPosition(reference);
        if (twoP == true)
        {
            controller.GetComponent<Game>().NextTurn();
        }
        
        reference.GetComponent<Chessman>().DestroyMovePlates();
        
        GameObject.FindGameObjectWithTag("num1").GetComponent<Text>().text = "";
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
