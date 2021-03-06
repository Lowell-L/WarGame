using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class NextRoundButtonClick : MonoBehaviour
{
    public Text RoundShow;

    // Start is called before the first frame update
    public void Click()
    {
        Global.round += 1;
        Debug.Log("Round: " + Global.round);
        RoundShow.text = "Round: " + Global.round;

        
        //遍历全局变量 恢复所有 HP 不为 0 的作战单位的攻击范围和机动点数
        for(int i = 0; i < Global.CombatUnitRealNum; i++)
        {
            if(Global.combatUnit[i] != null && Global.combatUnit[i].GetComponent<CombatUnit>().HP > 0)
            {
                switch(Global.combatUnit[i].GetComponent<CombatUnit>().type)
                {
                    case 1:
                    case 4:
                        Global.combatUnit[i].GetComponent<CombatUnit>().attackRange = 5;Global.combatUnit[i].GetComponent<CombatUnit>().motorPoints = 16;    break;
                    case 2:
                    case 5:
                        Global.combatUnit[i].GetComponent<CombatUnit>().attackRange = 3;Global.combatUnit[i].GetComponent<CombatUnit>().motorPoints = 8;     break;
                    case 3:
                    case 6:
                        Global.combatUnit[i].GetComponent<CombatUnit>().attackRange = 4;Global.combatUnit[i].GetComponent<CombatUnit>().motorPoints = 12;    break;
                }
            }
        }

        //实现补给功能
        //令 5 的倍数的推演轮次为补给轮次 每个作战单位的 HP 均可 + 【（6 - 骰子数）* 10% * 初始HP】
        
        if(Global.round % 5 == 0)
        {
            for(int i = 0; i < Global.CombatUnitRealNum; i++)
            {
                if(Global.combatUnit[i] != null && Global.combatUnit[i].GetComponent<CombatUnit>().HP > 0)
                {
                    switch(Global.combatUnit[i].GetComponent<CombatUnit>().type)
                    {
                        case 1:
                        case 4:
                            Global.combatUnit[i].GetComponent<CombatUnit>().HP += (int)(50 * (6 - (int)Random.Range(1, 6)) * 0.1);   break;
                        case 2:
                        case 5:
                            Global.combatUnit[i].GetComponent<CombatUnit>().HP += (int)(25 * (6 - (int)Random.Range(1, 6)) * 0.1);   break;
                        case 3:
                        case 6:
                            Global.combatUnit[i].GetComponent<CombatUnit>().HP += (int)(80 * (6 - (int)Random.Range(1, 6)) * 0.1);   break;
                    }
                }
            }
        }
        
    }
}
