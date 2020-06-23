using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonParser : MonoBehaviour
{
    [System.Serializable]
    public class ListQuestion
    {
        public Question[] questions;
    }

    [System.Serializable]
    public class Question
    {
        public string text;
        public string[] answers;
        public string correctanswer;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*if (begin(@"input"))
        {
            // Read first question
            Question firstQuestion = getCurrent();
            Debug.Log(firstQuestion.question);

            // Check if there is a next question
            if (!moveToNext())
            {
                Debug.Log("End of File");
                return;
            }
            // Read second question
            Question secondQuestion = getCurrent();
            Debug.Log(secondQuestion.question);
            if (!moveToNext())
            {
                Debug.Log("End of File");
                return;
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ListQuestion json;
    int index;

    // Create the iterator from the given input json file
    // Return true if file has at least 1 question, false otherwise
    public bool begin(string inputFile)
    {
        json = JsonUtility.FromJson<ListQuestion>(Resources.Load<TextAsset>(inputFile).ToString());
        index = 0;
        return json.questions.Length != 0;
    }

    // This function moves the iterator to the next question
    // Return true if there is a next question whilst false if reached end of file (We are already at the last question)
    public bool moveToNext()
    {
        if (index < json.questions.Length - 1)
        {
            index++;
            return true;
        }
        return false;
    }

    // This functions check if the iterator has a next question without moving the iterator itself
    public bool hasNext()
    {
        return (index < json.questions.Length - 1);
    }

    // Retrieve the current question
    public Question getCurrent()
    {
        return json.questions[index];
    }

    // Reset the iterator to 0
    public void reset()
    {
        index = 0;
    }
}