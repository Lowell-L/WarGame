using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;
using System.Text;
using static UnityEngine.Mathf;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Action : MonoBehaviour
{
    public Tilemap tilemap;
    public Text RoundShow;
    public Text BattlefieldSituation;
    public string BattlefieldSituationInfo;
   
    public Vector3Int source;       //作战单位的源地址

    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        graphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        //记录战场局势
        FileStream fs = new FileStream("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", FileMode.Truncate, FileAccess.ReadWrite);
        fs.Close();
        File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", "Battlefield Situation:\n",Encoding.Default);
        BattlefieldSituationInfo = "Battlefield Situation:\n";
    }

    // Update is called once per frame
    void Update()
    {
        if(Global.round % 2 != 0)
        {
            Move();
            Attack();
        }
        else if(Global.round % 2 == 0)
        {
            //机器人AI移动攻击
            ComputerPlayer();

            //下一轮
            Global.round += 1;
            Debug.Log("Round: " + Global.round);
            RoundShow.text = "Round: " + Global.round;
        }
        TextAsset txt = Resources.Load("BattlefieldSituation") as TextAsset;
        //Debug.Log(BattlefieldSituationInfo);
        BattlefieldSituation.text = BattlefieldSituationInfo;

    }

    void OnDestroy()
    {
        //计算最终血量
        //calLastBlood();
    }

    //关于移动
    void Move()
    {
        //鼠标点击获取要移动的作战单位
        if(Global.MoveUnit == null && Input.GetMouseButtonDown(0))
        {
            GameObject temp = SelectCombatUnit();
            if(temp != null)
            {
                Global.MoveUnit = temp;
            }
        }

        //当作战单位不空时，根据键盘输入获取移动方向
        if(Global.MoveUnit != null)
        {
            getDirection();

            //查看要移动的位置上是否已经有作战单位
            for(int i = 0; i < Global.CombatUnitRealNum; i++)
            {
                if(Global.combatUnit[i] != null && Global.direction == Global.combatUnit[i].GetComponent<CombatUnit>().position)
                {
                    Debug.Log("目标移动位置已有作战单位，无法移动！");
                    string info = "Blue-Move:" + "目标移动位置" + Global.direction + "已有作战单位，无法移动！\n";
                    File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);
                    BattlefieldSituationInfo += info;
                    //移动数据清零
                    Global.moveFlag = 0;
                    Global.MoveUnit = null;
                    Global.direction = new Vector3Int(0, 0, 0);
                    break;
                }
            }
        }
        
        //可以移动的条件：1、moveFlag 为真；2、移动向量不为 0 向量；3、要移动的作战单位已确定；4、要移动的作战单位所属方在此轮次拥有走棋权 5、消耗机动点数
        if(Global.direction != new Vector3Int(0, 0, 0) && Global.moveFlag == 1)
        {
            //判断选择的游戏对象在本轮次是否可以移动
            if(Global.round % 2 != 0 && Global.MoveUnit.GetComponent<CombatUnit>().party == 1)
            {
                //计算消耗的机动点数
                if(Global.MoveUnit.GetComponent<CombatUnit>().motorPoints - getMapTileConsumeMotorPoint(Global.direction) >= 0)
                {
                    //更新机动点数
                    Global.MoveUnit.GetComponent<CombatUnit>().motorPoints = Global.MoveUnit.GetComponent<CombatUnit>().motorPoints - getMapTileConsumeMotorPoint(Global.direction);
                    Global.MoveUnit.transform.Translate((tilemap.CellToWorld(Global.direction) - tilemap.CellToWorld(source)), Space.Self);
                    string info = "Blue-Move:" + "移动成功！" + "From" + Global.MoveUnit.GetComponent<CombatUnit>().position + "To" + Global.direction + "\n";
                
                    Global.MoveUnit.GetComponent<CombatUnit>().position = Global.direction;
                    
                    File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);
                    BattlefieldSituationInfo += info;

                    Debug.Log("移动成功！");

                    Global.moveFlag = 0;
                    Global.MoveUnit = null;
                    Global.direction = new Vector3Int(0, 0, 0);
                }
                else
                {
                    // 机动点数不足
                    Debug.Log("机动点数不足！");
                    string info = "Blue-Move:" + "机动点数不足！\n";
                    File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);
                    BattlefieldSituationInfo += info;

                    Global.moveFlag = 0;
                    Global.MoveUnit = null;
                    Global.direction = new Vector3Int(0, 0, 0);
                }
            }
            else if(Global.round % 2 != 0 && Global.MoveUnit.GetComponent<CombatUnit>().party == 1)
            {
                if(Global.MoveUnit.GetComponent<CombatUnit>().motorPoints - getMapTileConsumeMotorPoint(Global.direction) >= 0)
                {
                    Global.MoveUnit.GetComponent<CombatUnit>().motorPoints = Global.MoveUnit.GetComponent<CombatUnit>().motorPoints - getMapTileConsumeMotorPoint(Global.direction);
                    Global.MoveUnit.transform.Translate((tilemap.CellToWorld(Global.direction) - tilemap.CellToWorld(source)), Space.Self);
                    Global.MoveUnit.GetComponent<CombatUnit>().position = Global.direction;
                    Debug.Log("移动成功！");

                    Global.moveFlag = 0;
                    Global.MoveUnit = null;
                    Global.direction = new Vector3Int(0, 0, 0);
                }
                else
                {
                    // 机动点数不足
                    Debug.Log("机动点数不足！");
                    Global.moveFlag = 0;
                    Global.MoveUnit = null;
                    Global.direction = new Vector3Int(0, 0, 0);
                }
            }
            else
            {
                Debug.Log("本方此轮次无法移动！");
                string info = "Blue-Move:" + "本方此轮次无法移动！\n";
                BattlefieldSituationInfo += info;
                File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);   
            }

            //移动数据清零
            Global.moveFlag = 0;
            Global.MoveUnit = null;
            Global.direction = new Vector3Int(0, 0, 0);
        }
    }

    //人机推演操作
    void ComputerPlayer()
    {
        for(int i = 0 ; i < Global.CombatUnitRealNum ; i++)
        {
            //寻找电脑操作单位
            if(Global.combatUnit[i] != null && Global.combatUnit[i].GetComponent<CombatUnit>().party == 2)
            {
                GameObject Attacker = Global.combatUnit[i]; //攻击对象
                GameObject Defender = null;                 //最近被攻击对象
                double minDistance = 100000.0;              //最近攻击距离

                //寻找玩家作战单位作为被攻击对象
                for(int j = 0 ; j < Global.CombatUnitRealNum ; j++)
                {
                    if(Global.combatUnit[j] != null && Global.combatUnit[j].GetComponent<CombatUnit>().party == 1)
                    {
                        double distance = Distance(Attacker.GetComponent<CombatUnit>().position, Global.combatUnit[j].GetComponent<CombatUnit>().position);
                        //最小距离比较，确定攻击对象以及是否需要移动
                        if(distance < minDistance)
                        {
                            minDistance = distance;
                            Defender = Global.combatUnit[j];
                        }
                    }
                }

                //还有敌方单位存活则进行移动和攻击
                if(Defender != null)
                {
                    //不需要移动即可进行攻击
                    if(minDistance <= Attacker.GetComponent<CombatUnit>().attackRange)
                    {
                        AttackForComputerPlayer(Attacker, Defender);
                    }
                    //先移动再进行攻击
                    else
                    {
                        Vector3Int destPosition = new Vector3Int(0, 0, 0);
                        int moveFlag = 0;
                                                
                        for(int j = 1 ; j <= 6 ; j++)
                        {
                            //判断附近单元格是否为空可以移动
                            Vector3Int adjacent = adjacentPosition(Attacker, j);

                            if(isPositionValid(adjacent))
                            {
                                double distance = Distance(adjacent, Defender.GetComponent<CombatUnit>().position);
                                //计算附近单元格最近距离
                                if(distance < minDistance)
                                {
                                    minDistance = distance;
                                    destPosition = adjacent;
                                    moveFlag = 1;
                                }
                            }
                        }

                        //移动至附近单元格且攻击距离更近
                        if(moveFlag == 1)
                        {
                            MoveForComputerPlayer(Attacker, destPosition);
                            //移动后距离如果小于等攻击范围则可以攻击 否则不进行攻击
                            if(minDistance <= Attacker.GetComponent<CombatUnit>().attackRange)
                                AttackForComputerPlayer(Attacker, Defender);
                        }
                    }
                }
                //用户玩家失败
                else
                    SceneManager.LoadScene("Result");            
                
            }
        }
    }

    //判断单元格是否被占领
    bool isPositionValid(Vector3Int Position)
    {
        for(int i = 0 ; i < Global.CombatUnitRealNum ; i++)
        {
            if(Global.combatUnit[i] != null && Position == Global.combatUnit[i].GetComponent<CombatUnit>().position)
                return false;
        }
        return true;
    }

    void MoveForComputerPlayer(GameObject selectUnit, Vector3Int position)
    {
        Debug.Log("AI移动 From" + selectUnit.GetComponent<CombatUnit>().position + "To" + position);
        selectUnit.transform.Translate((tilemap.CellToWorld(position) - tilemap.CellToWorld(selectUnit.GetComponent<CombatUnit>().position)), Space.Self);
        selectUnit.GetComponent<CombatUnit>().position = position;
        Debug.Log("AI 移动成功" + selectUnit.GetComponent<CombatUnit>().position);
        
        string info = "Red--Move:" + "移动成功！" + "From" + selectUnit.GetComponent<CombatUnit>().position + "To" + position + "\n";
        File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);   
        BattlefieldSituationInfo += info;
                   
    }

    void AttackForComputerPlayer(GameObject Attacker, GameObject Defender)
    {
        int temp = (int)(Attacker.GetComponent<CombatUnit>().damage * 0.2 * (1 + (int)Random.Range(1, 6)));
        Defender.GetComponent <CombatUnit>().HP = 
                Defender.GetComponent <CombatUnit>().HP
                -
                temp;
        Debug.Log("攻击成功" + Attacker.GetComponent<CombatUnit>().position + Defender.GetComponent<CombatUnit>().position);
        string info1 = "Red--Attack:" + "攻击成功！" + "From" + Attacker.GetComponent<CombatUnit>().position + "To" + Defender.GetComponent <CombatUnit>().position + " HP -" + temp + "\n";
        File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info1,Encoding.Default);   
        BattlefieldSituationInfo += info1;
                
        //如果被攻击者 HP <= 0，销毁此游戏对象
        if(Defender.GetComponent <CombatUnit>().HP <= 0)
        {
            for(int i = 0; i < Global.CombatUnitRealNum; i++)
                if(Global.combatUnit[i] != null && Defender == Global.combatUnit[i])
                {
                    string info2 = "Blue:" + "我方作战单位" + Defender.GetComponent<CombatUnit>().position + "被摧毁！\n";
                    File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info2,Encoding.Default);   
                    BattlefieldSituationInfo += info2;
                
                    Global.combatUnit[i] = null;
                    break;                                    
                }
            GameObject.Destroy(Defender, 0.5f);
        }

        calLastBlood();
    }

    Vector3Int adjacentPosition(GameObject Attacker, int KeyCode)
    {
        Vector3Int source = Attacker.GetComponent<CombatUnit>().position;
        Vector3Int direction = source;

        switch(KeyCode)
        {
            case 1:
                direction.x += 1;break;
            case 2:
                if(source.y % 2 == 0)  // y 坐标为偶数
                {
                    direction.y += 1;
                }
                else                   // y 坐标为奇数
                {
                    direction.x += 1;
                    direction.y += 1;
                }
                break;
            case 3:
                if(source.y % 2 == 0)  // y 坐标为偶数
                {
                    direction.x -= 1;
                    direction.y += 1;
                }
                else                   // y 坐标为奇数
                {
                    direction.y += 1;
                }  
                break;
            case 4:
                direction.x -= 1;
                break;
            case 5:
                if(source.y % 2 == 0)  // y 坐标为偶数
                {
                    direction.x -= 1;
                    direction.y -= 1;
                }
                else                   // y 坐标为奇数
                {
                    direction.y -= 1;
                }   
                break;
            case 6:
                if(source.y % 2 == 0)  // y 坐标为偶数
                {
                    direction.y -= 1;
                }
                else                   // y 坐标为奇数
                {
                    direction.x += 1;
                    direction.y -= 1;
                }
                break;
        }
        return direction;
    }

    //鼠标点击选中作战单位 存储到 selectUnit
    GameObject SelectCombatUnit()
    {
        GameObject selectUnit = null;

        if (CheckGuiRaycastObjects()) 
        {
            return null;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    RaycastHit hit;
            //获取鼠标点击坐标
            source = tilemap.WorldToCell(ray.GetPoint(-ray.origin.z / ray.direction.z));
            //Debug.Log("源坐标为：" + source);

            //获取移动的作战单位
            if (Physics.Raycast(ray, out hit))
            {
                selectUnit = hit.collider.gameObject;    //获得选中物体
            }
        }
        return selectUnit;
    }

    //获取键盘输入 确定作战单位移动方向
    void getDirection()
    {
        Global.direction = source;

        if(Input.GetKey(KeyCode.Alpha1))        //向上移动
        {
            Global.direction.x += 1;
        }
        else if(Input.GetKey(KeyCode.Alpha2))   //向右上移动
        {
            if(source.y % 2 == 0)  // y 坐标为偶数
            {
                Global.direction.y += 1;
            }
            else                   // y 坐标为奇数
            {
                Global.direction.x += 1;
                Global.direction.y += 1;
            }
        }
        else if(Input.GetKey(KeyCode.Alpha3))   //向右下移动
        {
            if(source.y % 2 == 0)  // y 坐标为偶数
            {
                Global.direction.x -= 1;
                Global.direction.y += 1;
            }
            else                   // y 坐标为奇数
            {
                Global.direction.y += 1;
            }            
        }
        else if(Input.GetKey(KeyCode.Alpha4))   //向下移动
        {
            Global.direction.x -= 1;
        }
        else if(Input.GetKey(KeyCode.Alpha5))   //向左下移动
        {
            if(source.y % 2 == 0)  // y 坐标为偶数
            {
                Global.direction.x -= 1;
                Global.direction.y -= 1;
            }
            else                   // y 坐标为奇数
            {
                Global.direction.y -= 1;
            }           
        }
        else if(Input.GetKey(KeyCode.Alpha6))   //向左上移动
        {
            if(source.y % 2 == 0)  // y 坐标为偶数
            {
                Global.direction.y -= 1;
            }
            else                   // y 坐标为奇数
            {
                Global.direction.x += 1;
                Global.direction.y -= 1;
            }          
        }
        else
            Global.direction = new Vector3Int(0, 0, 0);
    }

    int getMapTileConsumeMotorPoint(Vector3Int tile)
    {
        int consumeMotorPoints = 0;
        int type = 0;
        for(int i = 0; i < Global.MapTileRealNum; i++)
        {
            if( tile.x == (int)(Global.MapTile[i][0]) && tile.y == (int)(Global.MapTile[i][1]) && tile.z == (int)(Global.MapTile[i][2]) )
                type = (int)(Global.MapTile[i][3]);
        }

        switch (type) 
        {
            case 1 :
                consumeMotorPoints = 6;
                break;
            case 2 :
                consumeMotorPoints = 2;
                break;
            case 3 :
                consumeMotorPoints = 4;
                break;
            case 4 :
                consumeMotorPoints = 3;
                break;
        }
        return consumeMotorPoints;
    }

    void Attack()
    {
        //按下 Attack 按钮 处于攻击状态
        if(Global.attackFlag == 1 && Input.GetMouseButtonDown(0))
        {
            //第一次按下鼠标 选择攻击者
            if(Global.Attacker == null && Input.GetMouseButtonDown(0))
            {
                GameObject temp = SelectCombatUnit();
                if(temp != null)
                {
                    if(Global.round % 2 != 0 && temp.GetComponent<CombatUnit>().party == 1)
                    {
                        Global.Attacker = SelectCombatUnit();
                        Global.Defender = null;
                        Debug.Log("Get attacker!");
                    }
                    else if(Global.round % 2 == 0 && temp.GetComponent<CombatUnit>().party == 2)
                    {
                        Global.Attacker = SelectCombatUnit();
                        Global.Defender = null;
                        Debug.Log("Get attacker!");
                    }
                    else
                    {
                        Debug.Log("本方此轮次无法攻击！");
                        string info = "Blue-Attack:" + "本方此轮次无法攻击！\n";
                        File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);   
                        BattlefieldSituationInfo += info;
                
                        Global.attackFlag = 0;
                        Global.Attacker = null;
                        Global.Defender = null;
                    }
                }
            }
            
            //第二次按下鼠标 选择被攻击者
            else if(Global.Attacker != null && Global.Defender == null && Input.GetMouseButtonDown(0))
            {
                GameObject temp = SelectCombatUnit();
                if(temp != null && temp != Global.Attacker)
                {
                    if(Global.round % 2 != 0 &&  temp != null && temp.GetComponent<CombatUnit>().party == 2)
                    {
                        Global.Defender = SelectCombatUnit();
                        Debug.Log("Get defender!");
                    }
                    else if(Global.round % 2 == 0 && temp != null && temp.GetComponent<CombatUnit>().party == 1)
                    {
                        Global.Defender = SelectCombatUnit();
                        Debug.Log("Get defender!");
                    }
                    else if(Global.round % 2 != 0 && temp != null && temp.GetComponent<CombatUnit>().party == 1)
                    {
                        Debug.Log("禁止攻击友军！");
                        string info = "Blue-Attack:" + "禁止攻击友军！\n";
                        File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);   
                        BattlefieldSituationInfo += info;
                
                        Global.attackFlag = 0;
                        Global.Attacker = null;
                        Global.Defender = null;
                    }
                    else if(Global.round % 2 == 0 && temp != null && temp.GetComponent<CombatUnit>().party == 2)
                    {
                        Debug.Log("禁止攻击友军！");
                        Global.attackFlag = 0;
                        Global.Attacker = null;
                        Global.Defender = null;
                    }  
                }
            }
            
            //当攻击者和防御者（被攻击者）都选择好
            if(Global.Attacker != null && Global.Defender != null)
            {

                if(Global.round % 2 != 0 && Global.Attacker.GetComponent<CombatUnit>().party == 1 && Global.Defender.GetComponent <CombatUnit>().party == 2)
                {
                    //两者距离在攻击距离之内
                    if( (int)Distance(Global.Attacker.GetComponent<CombatUnit>().position, Global.Defender.GetComponent <CombatUnit>().position) 
                        <= 
                        Global.Attacker.GetComponent<CombatUnit>().attackRange
                      )
                    {
                        //攻击： HP = HP - damage * 20% * (1 + 骰子数)
                        int temp = (int)(Global.Attacker.GetComponent<CombatUnit>().damage * 0.2 * (1 + (int)Random.Range(1, 6)));
                        Global.Defender.GetComponent <CombatUnit>().HP = Global.Defender.GetComponent <CombatUnit>().HP - temp;

                        //如果被攻击者 HP <= 0，销毁此游戏对象
                        if(Global.Defender.GetComponent <CombatUnit>().HP <= 0)
                        {
                            for(int i = 0; i < Global.CombatUnitRealNum; i++)
                                if(Global.combatUnit[i] != null && Global.Defender == Global.combatUnit[i])
                                {
                                    string info1 = "Red:" + "我方作战单位" + Global.Defender.GetComponent<CombatUnit>().position + "被摧毁！\n";
                                    File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info1,Encoding.Default);   
                                    BattlefieldSituationInfo += info1;
                                    Global.combatUnit[i] = null;
                                    break;                                    
                                }
                            GameObject.Destroy(Global.Defender, 0.5f);
                        }
                        
                        //某个作战单位在每个轮次只允许一次成功攻击！
                        //本轮次结束后 恢复attackRange
                        Global.Attacker.GetComponent<CombatUnit>().attackRange = 0;
                        Debug.Log("红方生命值损失！");
                        string info = "Blue-Attack:" + "攻击成功！" + "From" + Global.Attacker.GetComponent<CombatUnit>().position + "To" + Global.Defender.GetComponent <CombatUnit>().position + " HP -" + temp + "\n";
                        File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);   
                        BattlefieldSituationInfo += info;
                
                        //计算红方作战单位是否全部被摧毁
                        int flag = 0;
                        for(int j = 0 ; j < Global.CombatUnitRealNum ; j++)
                        {
                            if(Global.combatUnit[j] != null && Global.combatUnit[j].GetComponent<CombatUnit>().party == 2)
                            {
                                flag = 1;
                            }
                        }
                        if(flag == 0)
                        {
                            SceneManager.LoadScene("Result");
                        }

                        Global.Attacker = null;
                        Global.Defender = null;
                        Global.attackFlag = 0;
                    }
                    else
                    {
                        Debug.Log("超出攻击距离！");
                        string info = "Blue-Attack:" + "超出攻击距离！\n";
                        File.AppendAllText("E:\\Code\\unity\\WarGame\\Assets\\Resources\\BattlefieldSituation.txt", info,Encoding.Default);   
                        BattlefieldSituationInfo += info;
                
                        Global.Attacker = null;
                        Global.Defender = null;
                        Global.attackFlag = 0;
                    }
                }

                //轮次为偶数 攻击者是红方 防御者（被攻击者）必须是蓝方 蓝方是1 红方是2 
                else if(Global.round % 2 == 0 && Global.Attacker.GetComponent<CombatUnit>().party == 2 && Global.Defender.GetComponent <CombatUnit>().party == 1)
                {
                    //两者距离在攻击距离之内
                    if( (int)Distance(Global.Attacker.GetComponent<CombatUnit>().position, Global.Defender.GetComponent <CombatUnit>().position) 
                        <= 
                        Global.Attacker.GetComponent<CombatUnit>().attackRange
                      )
                    {
                        //攻击： HP = HP - damage * 20% * (1 + 骰子数)
                        int temp = (int)(Global.Attacker.GetComponent<CombatUnit>().damage * 0.2 * (1 + (int)Random.Range(1, 6)));
                        Global.Defender.GetComponent <CombatUnit>().HP = Global.Defender.GetComponent <CombatUnit>().HP - temp;

                        //如果被攻击者 HP <= 0，销毁此游戏对象
                        if(Global.Defender.GetComponent <CombatUnit>().HP <= 0)
                        {
                            for(int i = 0; i < Global.CombatUnitRealNum; i++)
                                if(Global.combatUnit[i] != null && Global.Defender == Global.combatUnit[i])
                                {
                                    Global.combatUnit[i] = null;
                                    break;                                    
                                }
                            GameObject.Destroy(Global.Defender, 0.5f);
                        }

                        //某个作战单位在每个轮次只允许一次成功攻击！
                        //本轮次结束后 恢复attackRange
                        Global.Attacker.GetComponent<CombatUnit>().attackRange = 0;
                        Debug.Log("蓝方生命值损失！");

                        //计算蓝方作战单位是否全部被摧毁
                        int flag = 0;
                        for(int j = 0 ; j < Global.CombatUnitRealNum ; j++)
                        {
                            if(Global.combatUnit[j] != null && Global.combatUnit[j].GetComponent<CombatUnit>().party == 1)
                            {
                                flag = 1;
                            }
                        }
                        if(flag == 0)
                        {
                            SceneManager.LoadScene("Result");
                        }

                        Global.Attacker = null;
                        Global.Defender = null;
                        Global.attackFlag = 0;
                    }
                    else
                    {
                        Debug.Log("超出攻击距离！");
                        Global.Attacker = null;
                        Global.Defender = null;
                        Global.attackFlag = 0;
                    }
                    
                }
                //如果攻击者和防御者属于同一方，则无法攻击（不能伤害友军）
                Global.Attacker = null;
                Global.Defender = null;
                Global.attackFlag = 0;
            }
        }
    }

    //计算攻击距离
    double Distance(Vector3Int source, Vector3Int dest) 
    {
        int length = Abs(source.y - dest.y);
        int height = Abs(source.x - dest.x);

        double len, hei;

        if(length % 2 == 1) {                    //两列之间有上下偏移
            len = length * 1.5;
            hei = height * 1.732;

            if(source.y % 2 == 1) {
                if(source. x < dest. x) 
                    hei -= 0.866;
                else
                    hei += 0.866;
            }
            else {
                if(source. x <= dest. x) 
                    hei += 0.866;
                else
                    hei -= 0.866;
            }
        }
        else {                              //两列之间同一高度无偏移
            len = length * 1.5;
            hei = height * 1.732;
        }

        return System.Math.Sqrt(len * len + hei * hei);
    }

    void calLastBlood()
    {
        //计算最终血量
        Debug.Log("计算最终血量" + Global.CombatUnitRealNum);
        Global.BlueTotalBloodReal = 0;
        Global.RedTotalBloodReal = 0;
        for(int j=0; j<Global.CombatUnitRealNum; j++)
        {
            if(Global.combatUnit[j] != null && Global.combatUnit[j].GetComponent<CombatUnit>().party == 1)
            {
                Debug.Log(Global.combatUnit[j].GetComponent<CombatUnit>().HP);
                Global.BlueTotalBloodReal += Global.combatUnit[j].GetComponent<CombatUnit>().HP;
            }
            else if(Global.combatUnit[j] != null && Global.combatUnit[j].GetComponent<CombatUnit>().party == 2)
            {
                Debug.Log(Global.combatUnit[j].GetComponent<CombatUnit>().HP);
                Global.RedTotalBloodReal += Global.combatUnit[j].GetComponent<CombatUnit>().HP;
            }
            else if(Global.combatUnit[j] == null)
                Debug.Log("No CombatUnit");
        }
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

}
