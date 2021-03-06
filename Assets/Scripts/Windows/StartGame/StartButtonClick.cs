using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonClick : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("SelectMap");//要切换到的场景名
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
