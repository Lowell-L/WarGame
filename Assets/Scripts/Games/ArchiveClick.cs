using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

public class ArchiveClick : MonoBehaviour
{
    public OpenFileName openFileName = new OpenFileName();

    public void Onclick()
    {
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Txt文件(*.txt)\0*.txt";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        openFileName.title = "游戏存档";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (LocalDialog.GetSaveFileName(openFileName))
        {
            Global.toArchiveFilePath = openFileName.file + ".txt";
            
            //Debug.Log(openFileName.file);
            //Debug.Log(openFileName.fileTitle);
            Debug.Log(Global.toArchiveFilePath);

            FileStream fs = new FileStream(Global.toArchiveFilePath, FileMode.Create, FileAccess.ReadWrite);
            fs.Close();

            //地图存档
            File.AppendAllText(Global.toArchiveFilePath, Global.mapFile + "\n" ,Encoding.Default);

            //作战单位存档
            for(int i = 0 ; i < Global.CombatUnitRealNum ; i++)
            {
                //作战单位坐标
                Vector3Int cellPosition = new Vector3Int(0, 0, 0);
                if(Global.combatUnit[i] != null)
                {
                    cellPosition =  Global.combatUnit[i].GetComponent<CombatUnit>().position;
                    string unitInfo = cellPosition.x + " " + cellPosition.y + " " + cellPosition.z + " ";
                    unitInfo += Global.combatUnit[i].GetComponent<CombatUnit>().type + " ";
                    unitInfo += Global.combatUnit[i].GetComponent<CombatUnit>().HP + "\n";
                    File.AppendAllText(Global.toArchiveFilePath, unitInfo, Encoding.Default);
                }
            }
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
