using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsController : MonoBehaviour
{
    static int UI_LAYER = 5;

    // Main
    public GameObject uiCanvas;
    public GameObject buttonPrefab;
    // Questions handling
    jsonParser parser;
    jsonParser.Question question = null;
    bool loadNext = false;
    // Question dynamic objects
    GameObject questionText_obj;
    GameObject[] answers_obj = null;
    // Tracker
    List<string> given_answers = new List<string>();
    int correct_answers_counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Create Parser
        parser = GetComponent<jsonParser>();
        // Create Question Text
        questionText_obj = createUiObject(uiCanvas);
        questionText_obj.name = "Question";
        addText(questionText_obj, "");
        questionText_obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        // Initialize Parser
        if (!parser.begin(SceneInfo.Instance.file))
        {
            Debug.Log("File does not exit");
        } else
        {
            generateUIForQuestion();
        }
    }

    // Update is called once per frame
    DateTime lastUpdate;
    void Update()
    {
        if (loadNext)
        {
            if ((DateTime.Now - lastUpdate).Milliseconds >= 500) {
                // Correct answer move to next answer
                if (parser.hasNext())
                {
                    parser.moveToNext();
                    generateUIForQuestion();
                    loadNext = false;
                }
                else
                {
                    // Do something different when all questions have been answered
                    //Debug.Log("No More Questions");
                }
            }
        }
    }

    void generateUIForQuestion()
    {
        // Set correct question text
        question = parser.getCurrent();
        addText(questionText_obj, question.text);
        // Delete Existing Answer buttons if any
        if (answers_obj != null)
        {
            for (uint i = 0; i < answers_obj.Length; i++)
            {
                Destroy(answers_obj[i]);
            }
        }
        // Generate Answer buttons
        int ofst_y = 0;
        answers_obj = new GameObject[question.answers.Length];
        for (uint i = 0; i < question.answers.Length; i++)
        {
            string answer = question.answers[i];
            // Create Button
            answers_obj[i] = GameObject.Instantiate(buttonPrefab) as GameObject;
            answers_obj[i].name = "Answer";
            answers_obj[i].transform.SetParent(uiCanvas.transform);
            // Set Button Text
            answers_obj[i].GetComponentInChildren<Text>().text = question.answers[i];
            // Set color for correct and wrong answers
            ColorBlock cb = answers_obj[i].GetComponent<Button>().colors;
            cb.selectedColor = answer == question.correctanswer ? Color.green : Color.red;
            answers_obj[i].GetComponent<Button>().colors = cb;
            // Set Button OnClick Event
            GameObject obj = answers_obj[i];
            answers_obj[i].GetComponent<Button>().onClick.AddListener(() => {
                onAnswerClicked(obj, answer);
            });
            // Position On Canvas
            answers_obj[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50 - ofst_y);
            // Increase Ofst
            ofst_y += 50;
        }
    }

    GameObject createUiObject(GameObject parent)
    {
        GameObject obj = new GameObject();
        // Set Parent
        obj.transform.SetParent(parent.transform);
        // Set Layer
        obj.layer = UI_LAYER;
        // Add Rectangle Transform
        RectTransform rt = obj.AddComponent<RectTransform>();
        // Set Pivot to top center of the object
        rt.pivot = new Vector2(0.5f, 1.0f);
        // Set Anchor Point to Top Center
        rt.anchorMax = new Vector2(0.5f, 1.0f);
        rt.anchorMin = new Vector2(0.5f, 1.0f);

        return obj;
    }

    Text addText(GameObject obj, string txt)
    {
        Text txt_obj = obj.GetComponent<Text>();
        if (txt_obj == null)
        {
            // GameObject does not have Text component
            txt_obj = obj.AddComponent<Text>();
            // Set Font
            txt_obj.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            // Set Color
            txt_obj.color = Color.black;
        }
        // Set Text
        txt_obj.text = txt;

        return txt_obj;
    }

    void onAnswerClicked(GameObject btn, string answer)
    {
        if (!loadNext)
        {
            // Notify of update required
            lastUpdate = DateTime.Now;
            loadNext = true;
            // Set Color
            ColorBlock cb = btn.GetComponent<Button>().colors;
            cb.normalColor = answer == question.correctanswer ? Color.green : Color.red;
            btn.GetComponent<Button>().colors = cb;
            // Update tracker
            given_answers.Add(answer);
            correct_answers_counter += answer == question.correctanswer ? 1 : 0;
            // Disable buttons
            foreach (GameObject obj in answers_obj)
            {
                // Disable all buttons but the one that was clicked (Otherwise color won't update)
                if (btn != obj)
                {
                    obj.GetComponent<Button>().enabled = false;
                }
            }
            int percentage = (int)Math.Round((double)correct_answers_counter / (double)given_answers.Count * 100.0);
            Debug.Log("Score: " + percentage + "%");
        }
    }
}
