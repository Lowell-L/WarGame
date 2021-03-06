using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//不同作战单位的父类
public class CombatUnit : MonoBehaviour
{
    public int party;           //表示属于交战哪一方 蓝方是1 红方是2

    //作战单位的属性
    public int type;            //类型         ArmoredCar 1、Tank 2、Robot 3
    public int damage;          //攻击力
    public int attackRange;     //攻击范围
    public int HP;              //生命值
    public int motorPoints;     //机动点数
    public Vector3Int position; //位置

    //本轮次是否移动完毕
        //可以把消耗点数在移动时做减法 待到这一轮次结束后在把本轮次所有作战单位的机动点数恢复
    //本轮次是否攻击完毕

    // Start is called before the first frame update
    void Start()
    {
        //用于不同作战单位的初始化
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
