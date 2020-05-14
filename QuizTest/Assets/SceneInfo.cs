using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo
{
    private static SceneInfo instance;
    public static SceneInfo Instance { get {
           if (instance == null)
            {
                instance = new SceneInfo();
            }
            return instance;
    } }
    // Questionaire file
    public string file;
}
