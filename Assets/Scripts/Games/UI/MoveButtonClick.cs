using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    public void Click()
    {
        Debug.Log("Move Button Clicked!");
        Global.moveFlag = 1;    //修改全局变量，处于可移动状态
        Global.MoveUnit = null;
        Global.direction = new Vector3Int(0, 0, 0);
    }

}
