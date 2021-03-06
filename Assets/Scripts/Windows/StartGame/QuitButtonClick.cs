using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButtonClick : MonoBehaviour
{
    public void Quit () 
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
