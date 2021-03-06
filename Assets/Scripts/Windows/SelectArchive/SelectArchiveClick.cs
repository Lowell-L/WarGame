using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class SelectArchiveClick : MonoBehaviour
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
        openFileName.title = "选择存档文件";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (LocalDialog.GetOpenFileName(openFileName))
        {
            Global.archiveFile = openFileName.fileTitle;
            Global.loadMethod = 0;
            confirmButton.interactable = true;
            
            //去除后缀取文件名
            string[] archive = Global.archiveFile.Split('.');
            Global.archiveFile = archive[0];

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
