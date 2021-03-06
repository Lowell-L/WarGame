using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGameButtonClick : MonoBehaviour
{
    //public void Quit () 
    public void Onclick()
    {
        SceneManager.LoadScene("StartGame");
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
