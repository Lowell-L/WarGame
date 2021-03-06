using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    public void Click()
    {
        Debug.Log("Attack Button Clicked!");
        Global.attackFlag = 1;    //修改全局变量，处于可移动状态
        Global.Attacker = null;
        Global.Defender = null;
    }

}
