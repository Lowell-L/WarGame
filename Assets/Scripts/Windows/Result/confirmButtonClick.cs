using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class confirmButtonClick : MonoBehaviour
{
    public Text ResultInfo;

    public void OnClick()
    {
        SceneManager.LoadScene("StartGame");//要切换到的场景名
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Blue blood initial:" + Global.BlueTotalBloodInit + "\tBlue blood remaining:" + Global.BlueTotalBloodReal + "\n"
                           + "Red  blood initial:" + Global.RedTotalBloodInit + "\tRed  blood remaining:" + Global.RedTotalBloodReal);
        ResultInfo.text = "Blue blood initial:" + Global.BlueTotalBloodInit + "\tBlue blood remaining:" + Global.BlueTotalBloodReal + "\n"
                           +"\tBlue Blood loss rate:" +  (int)((float)(Global.BlueTotalBloodInit - Global.BlueTotalBloodReal) / (float)Global.BlueTotalBloodInit * 100) + "%"
                           + "\n\nRed  blood initial:" + Global.RedTotalBloodInit + "\tRed  blood remaining:" + Global.RedTotalBloodReal + "\n"
                           +"\tRed  Blood loss rate:" +  (int)((float)(Global.RedTotalBloodInit - Global.RedTotalBloodReal) / (float)Global.RedTotalBloodInit * 100) + "%"
                           + "\n\n"
                           + ((((float)(Global.BlueTotalBloodInit - Global.BlueTotalBloodReal) / (float)Global.BlueTotalBloodInit * 100) <= ((float)(Global.RedTotalBloodInit - Global.RedTotalBloodReal) / (float)Global.RedTotalBloodInit * 100)) ? "Blue Player WIN!" : "Red Player WIN!")
                           ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
