using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class SelectMapClick : MonoBehaviour
{
    public OpenFileName openFileName = new OpenFileName();
    public Button confirmButton;

    public void Onclick()
    {
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Txt文件(*.txt)\0*.txt";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        openFileName.title = "选择地图文件";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (LocalDialog.GetOpenFileName(openFileName))
        {
            Global.mapFile = openFileName.fileTitle;
            confirmButton.interactable = true;
            //去除后缀取文件名
            string[] map = Global.mapFile.Split('.');
            Global.mapFile = map[0];
            //Debug.Log(map[0]);

            string temp = map[0];
            string[] num = temp.Split('-');
            string no = num[1];
            Global.unitFile = "CombatUnit-" + no;
            //Debug.Log(openFileName.file);
            //Debug.Log(openFileName.fileTitle);
        }
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
