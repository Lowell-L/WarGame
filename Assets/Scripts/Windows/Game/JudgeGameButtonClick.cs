using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JudgeGameButtonClick : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Result");
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
