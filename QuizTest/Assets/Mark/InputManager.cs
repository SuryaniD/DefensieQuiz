using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameObject inputfield;
    [SerializeField]
    GameObject sprite;
    string inputfieldtext;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) //set as u to currently test.
        {
            inputfieldtext = inputfield.gameObject.transform.GetChild(2).GetComponent<Text>().text;
            Debug.Log(inputfieldtext);
            Sprite sp = Resources.Load("QuizImages/quiz1/" + inputfieldtext + ".png") as Sprite;
            sprite.GetComponent<SpriteRenderer>().sprite = sp;
        }
    }
}
