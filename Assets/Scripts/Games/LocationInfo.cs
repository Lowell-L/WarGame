using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//存储全局变量：地图信息（每一个瓦片的坐标和类型）、作战单位信息（每一个作战单位的坐标和类型）
public static class Global
{
    public static int MapTileMaxNum = 1000;        //定义地图瓦片的最大数量
    public static int CombatUnitMaxNum = 100;      //定义作战单位的最大数量

    public static int MapTileRealNum;              //地图瓦片的实际数量
    public static int CombatUnitRealNum;           //作战单位的实际数量
    public static GameObject[]  combatUnit = new GameObject[CombatUnitMaxNum];     //数组存放每个作战单位的坐标和类型
    public static Vector4[]    MapTile= new Vector4[MapTileMaxNum];         //数组存放每个地图单元的坐标和类型

    public static string mapFile;               //加载地图文件
    public static string unitFile;              //加载作战单位文件
    public static string archiveFile;           //加载的存档
    public static string toArchiveFilePath;     //存档路径

    public static int round = 1;              //推演轮次
                                              // round = 1 3 5 7 …… 蓝方；round = 2 4 6 8 …… 红方
    public static int moveFlag = 0;           //是否处于移动状态
    public static int attackFlag = 0;         //是否处于攻击状态

    //关于移动
    public static GameObject MoveUnit = null;   //选中的需要移动的作战单位
    public static Vector3Int direction = new Vector3Int(0, 0, 0);    //实际移动的方向

    //关于攻击
    public static GameObject Attacker = null;
    public static GameObject Defender = null;

    public static int loadMethod = 1;           //区分新游戏和存档


    //关于胜负判断
    public static int RedTotalBloodInit = 0;        //初始红方总血量
    public static int BlueTotalBloodInit = 0;       //初始蓝方总血量
    public static int RedTotalBloodReal = 0;        //实际红方总血量
    public static int BlueTotalBloodReal = 0;       //实际蓝方总血量

}
