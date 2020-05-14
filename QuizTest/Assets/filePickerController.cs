using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class filePickerController : MonoBehaviour
{ 
    //File loading same name as json
    static readonly string[] files = { "input", "input2", "input3", "input4" };
    public GameObject uiCanvas;
    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gen_UI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void gen_UI()
    {
        int ofst_y = 0;
        GameObject[] obj_file = new GameObject[files.Length];
        for (uint i = 0; i < files.Length; i++)
        {
            string txt = files[i];
            // Create Button
            obj_file[i] = GameObject.Instantiate(buttonPrefab) as GameObject;
            obj_file[i].name = "Answer";
            obj_file[i].transform.SetParent(uiCanvas.transform);
            // Set Button Text
            obj_file[i].GetComponentInChildren<Text>().text = txt;
            // Set Button OnClick Event
            GameObject btn = obj_file[i];
            obj_file[i].GetComponent<Button>().onClick.AddListener(() => {
                onFileSelected(btn, txt);
            });
            // Position On Canvas
            obj_file[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50 - ofst_y);
            // Increase Ofst
            ofst_y += 50;
        }
    }

    void onFileSelected(GameObject btn, string txt)
    {
        SceneInfo.Instance.file = txt;
        SceneManager.LoadScene("Questionaire");
    }
}
