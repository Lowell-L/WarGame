using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.EventSystems;

//根据文件实例化作战单位
//获取地图信息、作战单位信息并显示
public class InfoShow : MonoBehaviour
{
    public Tilemap tilemap;         //tilemap 的赋值在 Unity的 Inspector 面板
    public Text combatUnitInfoText;     //CombatUnitInfo 的赋值在 Unity的 Inspector 面板
    public Text battleGroundInfoText;   //BattleGroundInfo 的赋值
    
    public Tile tileGrass;          //tile 的赋值在 Unity 的Inspector 面板
    public Tile tileBrambles;
    public Tile tileMountains;
    public Tile tileOcean;   
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    void Start()
    {
        graphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        
        //加载新的游戏
        if(Global.loadMethod == 1)
        {
            //加载地图
            //Debug.Log(Global.mapFile);
            InitBattleGroundFromTxt(Global.mapFile);

            //获取地图信息
            //getMap();
            
            //实例化作战单位
            InitCombatUnitsFromTxt(Global.unitFile);
        }
        //读取存档
        else
            InitArchiveFromTxt(Global.archiveFile);
        
        //计算初始血量
        calInitBlood();

    }

    //手绘地图然后将其转化为文件存储
    void getMap()
    {
        //清空文本文件内容
        FileStream fs = new FileStream("E:\\Code\\unity\\WarGame\\Assets\\Resources\\Map.txt", FileMode.Truncate, FileAccess.ReadWrite);
        fs.Close();
        //遍历地图，获取地图信息
        int i = 0;
        foreach (var item in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(item) && i < Global.MapTileMaxNum)
            {
                string map = item.x +" " + item.y + " " + item.z + " ";
                var temp = tilemap.GetTile(item);
                map = map + temp.name + "\n";
                File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\Map.txt", map,Encoding.Default);

                //同时存储到全局变量
                Global.MapTile[i].x = item.x;
                Global.MapTile[i].y = item.y;
                Global.MapTile[i].z = item.z;
                if(string.Equals(temp.name, "Brambles"))
                    Global.MapTile[i].w = 1;
                else if(string.Equals(temp.name, "Grass"))
                    Global.MapTile[i].w = 2;
                else if(string.Equals(temp.name, "Mountains"))
                    Global.MapTile[i].w = 3;
                else if(string.Equals(temp.name, "Ocean"))
                    Global.MapTile[i].w = 4;
                ++i;
            }
            Global.MapTileRealNum = i;
        }
    }


    //读取文件，实例化作战单位
    void InitCombatUnitsFromTxt(string unitFile)
    {
        TextAsset txt = Resources.Load(unitFile) as TextAsset;
        //以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容
        string[] str = txt.text.Split('\n');
        
        // 将每行字符串的内容以空格作为分割点，并将每个逗号分隔的字符串内容遍历输出
        int i = 0;
        Global.CombatUnitRealNum = 0;
        for(i = 0; i < str.GetLength(0); i++)
        {
            //Debug.Log(str[i]);
            string[] ss = str[i].Split(' ');
            if(!string.Equals(ss[0], ""))
            {
                Global.CombatUnitRealNum++;
                InitCombatUnit(ss, i);
            }
        }

    }

    //实例化作战单位
    void InitCombatUnit(string[] ss, int i)
    {
        GameObject prefab;

        //世界坐标转换为单元格坐标
        Vector3Int cellPosition = new Vector3Int(0, 0, 0);
        cellPosition.x = int.Parse(ss[0]);
        cellPosition.y = int.Parse(ss[1]);
        cellPosition.z = int.Parse(ss[2]);
        Vector3 worldPosition = tilemap.CellToWorld(cellPosition);
        
        if(int.Parse(ss[3]) == 1)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/BlueArmoredCar");
            Global.combatUnit[i] = Instantiate(prefab, worldPosition, Quaternion.identity);
            Global.combatUnit[i].GetComponent<CombatUnit>().position = cellPosition;
            //Debug.Log("Init BlueArmoredCar");
        }
        else if(int.Parse(ss[3]) == 2)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/BlueRobot");
            Global.combatUnit[i] = Instantiate(prefab, worldPosition, Quaternion.identity);
            Global.combatUnit[i].GetComponent<CombatUnit>().position = cellPosition;
            //Debug.Log("Init BlueRobot");   
        }
        else if(int.Parse(ss[3]) == 3)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/BlueTank");
            Global.combatUnit[i] = Instantiate(prefab, worldPosition, Quaternion.identity);
            Global.combatUnit[i].GetComponent<CombatUnit>().position = cellPosition;
            //Debug.Log("Init BlueTank");
        }
        else if(int.Parse(ss[3]) == 4)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/RedArmoredCar");
            Global.combatUnit[i] = Instantiate(prefab, worldPosition, Quaternion.identity);
            Global.combatUnit[i].GetComponent<CombatUnit>().position = cellPosition;
            //Debug.Log("Init RedArmoredCar");   
        }
        else if(int.Parse(ss[3]) == 5)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/RedRobot");
            Global.combatUnit[i] = Instantiate(prefab, worldPosition, Quaternion.identity);
            Global.combatUnit[i].GetComponent<CombatUnit>().position = cellPosition;
            //Debug.Log("Init RedRobot");    
        }
        else if(int.Parse(ss[3]) == 6)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/RedTank");
            Global.combatUnit[i] = Instantiate(prefab, worldPosition, Quaternion.identity);
            Global.combatUnit[i].GetComponent<CombatUnit>().position = cellPosition;
            //Debug.Log("Init RedTank");  
        }
    }

    void calInitBlood()
    {
        //计算双方初始血量
        int j = 0;
        for(j=0; j<Global.CombatUnitRealNum; j++)
        {
            //Debug.Log("Type" + Global.combatUnit[j].GetComponent<CombatUnit>().type + "HP" + Global.combatUnit[j].GetComponent<CombatUnit>().HP);
            
            if(Global.combatUnit[j] != null && Global.combatUnit[j].GetComponent<CombatUnit>().party == 1)
            {
                switch(Global.combatUnit[j].GetComponent<CombatUnit>().type)
                {
                    case 1:Global.BlueTotalBloodInit += 50;break;
                    case 2:Global.BlueTotalBloodInit += 25;break;
                    case 3:Global.BlueTotalBloodInit += 80;break;
                }
            }
                
            else if(Global.combatUnit[j] != null && Global.combatUnit[j].GetComponent<CombatUnit>().party == 2)
            {
                switch(Global.combatUnit[j].GetComponent<CombatUnit>().type)
                {
                    case 4:Global.RedTotalBloodInit += 50;break;
                    case 5:Global.RedTotalBloodInit += 25;break;
                    case 6:Global.RedTotalBloodInit += 80;break;
                }
            }     
        }
        //Debug.Log("Global.BlueTotalBloodInit:" + Global.BlueTotalBloodInit);
        //Debug.Log("Global.RedTotalBloodInit:" + Global.RedTotalBloodInit);
    }
    // Update is called once per frame
    void Update()
    {
        ClickEffect();    
    }

    //鼠标点击显示地形或者作战单位信息
    void ClickEffect()
    {
        if (CheckGuiRaycastObjects()) 
        {
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    RaycastHit hit;

		    //获取鼠标点击坐标
            Vector3Int vector = tilemap.WorldToCell(ray.GetPoint(-ray.origin.z / ray.direction.z));
            
            //显示地形信息
            int maptile_w = 0;
            string tileInfo = "Position: (" + vector.x + ", " +vector.y + ", " + vector.z + ")  Type:";
            for(int i = 0; i < Global.MapTileRealNum; i++)
            {
                if( vector.x == (int)(Global.MapTile[i][0]) && vector.y == (int)(Global.MapTile[i][1]) && vector.z == (int)(Global.MapTile[i][2]) )
                    maptile_w = (int)(Global.MapTile[i][3]);
            }

            switch (maptile_w) 
            {
                case 1 :
                    tileInfo += " Brambles";
                    break;
                case 2 :
                    tileInfo += " Grass";
                    break;
                case 3 :
                    tileInfo += " Mountains";
                    break;
                case 4 :
                    tileInfo += " Ocean";
                    break;

            }
            //Debug.Log(tileInfo);
            battleGroundInfoText.text = tileInfo;
            
            string combatUnitInfo = "CombatUnit Type: ";
            if (Physics.Raycast(ray, out hit))
            {
                GameObject selectUnit = hit.collider.gameObject;    //获得选中物体
                int selectUnitType = selectUnit.GetComponent<CombatUnit>().type;
                //Debug.Log(selectUnitType);
                int selectUnitHP = selectUnit.GetComponent<CombatUnit>().HP;
                int MotorPoints = selectUnit.GetComponent<CombatUnit>().HP;

                switch (selectUnitType)
                {
                    case 1:combatUnitInfo = combatUnitInfo + "BlueArmoredCar  " + "Damage: 30  " + "AttackRange: 5  " + "HP: " + selectUnitHP + "  " + "MotorPoints: " + MotorPoints;    break;
                    case 2:combatUnitInfo = combatUnitInfo + "BlueRobot  " + "Damage: 15  " + "AttackRange: 3  " + "HP: " + selectUnitHP + "  " + "MotorPoints: " + MotorPoints;         break;
                    case 3:combatUnitInfo = combatUnitInfo + "BlueTank  " + "Damage: 50  " + "AttackRange: 4  " + "HP: " + selectUnitHP + "  " + "MotorPoints: " + MotorPoints;          break;
                    case 4:combatUnitInfo = combatUnitInfo + "RedArmoredCar  " + "Damage: 30  " + "AttackRange: 5  " + "HP: " + selectUnitHP + "  " + "MotorPoints: " + MotorPoints;     break;
                    case 5:combatUnitInfo = combatUnitInfo + "RedRobot  " + "Damage: 15  " + "AttackRange: 3  " + "HP: " + selectUnitHP + "  " + "MotorPoints: " + MotorPoints;          break;
                    case 6:combatUnitInfo = combatUnitInfo + "RedTank  " + "Damage: 50  " + "AttackRange: 4  " + "HP: " + selectUnitHP + "  " + "MotorPoints: " + MotorPoints;           break;
                }
                //Debug.Log(combatUnitInfo);
            }
            combatUnitInfoText.text = combatUnitInfo;
        }

        //int temp = Global.CombatUnit1.GetComponent<TankController>().damage;
        //print("CombatUnit1.damage=" + temp);
    }

    void InitBattleGroundFromTxt(string mapFile)
    {
        TextAsset txt = Resources.Load(mapFile) as TextAsset;
        //以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容
        //Debug.Log(txt);
        string[] str = txt.text.Split('\n');
        //Debug.Log(str[0]);

        int i = 0;
        Global.MapTileRealNum = 0;
        Debug.Log(str.GetLength(0));

        for(i = 0; i < str.GetLength(0); i++)
        {
            //Debug.Log(str[i]);
            string[] ss = str[i].Split(' ');
            if(!string.Equals(ss[0], ""))
            {
                Global.MapTileRealNum++;
                InitBattleGround(ss, i);
            }
        }
    }

    void InitBattleGround(string[] ss, int i)
    {
        //世界坐标转换为单元格坐标
        Vector3Int cellPosition = new Vector3Int(0, 0, 0);
        cellPosition.x = int.Parse(ss[0]);
        cellPosition.y = int.Parse(ss[1]);
        cellPosition.z = int.Parse(ss[2]);

        //在瓦片地图中设置瓦片
        if(string.Equals(ss[3], "Brambles"))
        {
            tilemap.SetTile(cellPosition, tileBrambles);
            Global.MapTile[i].w = 1;
        }
        else if(string.Equals(ss[3], "Grass"))
        {
            tilemap.SetTile(cellPosition, tileGrass);
            Global.MapTile[i].w = 2;
        }
        else if(string.Equals(ss[3], "Mountains"))
        {
            tilemap.SetTile(cellPosition, tileMountains);
            Global.MapTile[i].w = 3;
        }
        else if(string.Equals(ss[3], "Ocean"))
        {
            tilemap.SetTile(cellPosition, tileOcean);
            Global.MapTile[i].w = 4;
        }
        
        Global.MapTile[i].x = cellPosition.x;
        Global.MapTile[i].y = cellPosition.y;
        Global.MapTile[i].z = cellPosition.z;
    }

    //鼠标穿透问题
    public bool CheckGuiRaycastObjects()
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        graphicRaycaster.GetComponent<GraphicRaycaster>().Raycast(eventData, list);
        //Debug.Log(list.Count);
        return list.Count > 0;
    }
    void InitArchiveFromTxt(string archiveFile)
    {
        TextAsset txt = Resources.Load(archiveFile) as TextAsset;
        //以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容
        //Debug.Log(txt);
        string[] str = txt.text.Split('\n');
        //Debug.Log(str[0]);

        Global.mapFile = str[0];
        //Debug.Log(map.Length + " " + map);

        //第一行加载地图
        InitBattleGroundFromTxt(Global.mapFile);
        
        Global.CombatUnitRealNum = 0;
        for(int i = 1 ; i < str.GetLength(0) ; i++)
        {
            //Debug.Log(str[i]);
            string[] ss = str[i].Split(' ');
            if(!string.Equals(ss[0], ""))
            {
                Global.CombatUnitRealNum++;
                InitArchiveCombatUnit(ss, i);
            }
        }

    }

    void InitArchiveCombatUnit(string[] ss, int i)
    {
        GameObject prefab = null;

        //世界坐标转换为单元格坐标
        Vector3Int cellPosition = new Vector3Int(0, 0, 0);
        cellPosition.x = int.Parse(ss[0]);
        cellPosition.y = int.Parse(ss[1]);
        cellPosition.z = int.Parse(ss[2]);
        Vector3 worldPosition = tilemap.CellToWorld(cellPosition);
        
        if(int.Parse(ss[3]) == 1)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/BlueArmoredCar");
            //Debug.Log("Init BlueArmoredCar");
        }
        else if(int.Parse(ss[3]) == 2)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/BlueRobot");
            //Debug.Log("Init BlueRobot");   
        }
        else if(int.Parse(ss[3]) == 3)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/BlueTank");
            //Debug.Log("Init BlueTank");
        }
        else if(int.Parse(ss[3]) == 4)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/RedArmoredCar");
            //Debug.Log("Init RedArmoredCar");   
        }
        else if(int.Parse(ss[3]) == 5)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/RedRobot");
            //Debug.Log("Init RedRobot");    
        }
        else if(int.Parse(ss[3]) == 6)
        {
            prefab = Resources.Load<GameObject>("Prefabs/CombatUnit/RedTank");
            //Debug.Log("Init RedTank");  
        }

        Global.combatUnit[i] = Instantiate(prefab, worldPosition, Quaternion.identity);
        Global.combatUnit[i].GetComponent<CombatUnit>().position = cellPosition;
        Global.combatUnit[i].GetComponent<CombatUnit>().HP = int.Parse(ss[4]);

    }
}
