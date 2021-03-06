using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class ShadowController : MonoBehaviour
{
    // Start is called before the first frame update
    //public Tilemap map;
    public Tilemap tilemap;
    public Tile shadow;
    
    void Start()
    {   
        // int j = 0;
        // foreach (var item in map.cellBounds.allPositionsWithin)
        // {
        //     if (map.HasTile(item) && j < Global.MapTileMaxNum)
        //     {

        //         var temp = map.GetTile(item);
        //         //同时存储到全局变量
        //         Global.MapTile[j].x = item.x;
        //         Global.MapTile[j].y = item.y;
        //         Global.MapTile[j].z = item.z;
        //         ++j;
        //     }
        //     Global.MapTileRealNum = j;
        // }
        Debug.Log("test\n");
        Vector3Int cellPosition = new Vector3Int(0, 0, 0);
        for(int i = 0; i < Global.MapTileRealNum; i++){
            cellPosition.x = (int)Global.MapTile[i].x;
            cellPosition.y = (int)Global.MapTile[i].y;
            cellPosition.z = (int)Global.MapTile[i].z;
            tilemap.SetTile(cellPosition, shadow);
        }  
        Debug.Log(Global.MapTileRealNum);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < Global.CombatUnitRealNum; i++){
            if(Global.combatUnit[i] != null && Global.combatUnit[i].GetComponent<CombatUnit>().party == 1){
                Debug.Log(Global.combatUnit[i].GetComponent<CombatUnit>().position);
                erase(Global.combatUnit[i].GetComponent<CombatUnit>().position);
            }
        }
    }

    void erase(Vector3Int position){
        if(tilemap.HasTile(position)){
            tilemap.SetTile(position, null);
        }
        
        Vector3Int position1 = new Vector3Int(position.x+1, position.y, 0); 
        Vector3Int position2 = new Vector3Int(0, 0, 0);
        if(position.y % 2 == 0)  // y 坐标为偶数
        {
            position2 = new Vector3Int(position.x, position.y+1, 0);
        }
        else                   // y 坐标为奇数
        {
            position2 = new Vector3Int(position.x+1, position.y+1, 0);
        }
        Vector3Int position3 = new Vector3Int(0, 0, 0);
        if(position.y % 2 == 0)  // y 坐标为偶数
        {
            position3 = new Vector3Int(position.x-1, position.y+1, 0);
        }
        else                   // y 坐标为奇数
        {
            position3 = new Vector3Int(position.x, position.y+1, 0);
        } 

        Vector3Int position4 = new Vector3Int(position.x-1, position.y, 0);

        Vector3Int position5 = new Vector3Int(0, 0, 0);  
        if(position.y % 2 == 0)  // y 坐标为偶数
        {
            position5 = new Vector3Int(position.x-1, position.y-1, 0);
        }
        else                   // y 坐标为奇数
        {
            position5 = new Vector3Int(position.x, position.y-1, 0);
        }  

        Vector3Int position6 = new Vector3Int(0, 0, 0); 
         if(position.y % 2 == 0)  // y 坐标为偶数
        {
            position6 = new Vector3Int(position.x, position.y-1, 0);
        }
        else                   // y 坐标为奇数
        {
            position6 = new Vector3Int(position.x+1, position.y-1, 0);
        }

        if(tilemap.HasTile(position1)){
            tilemap.SetTile(position1, null);
        }
        if(tilemap.HasTile(position2)){
            tilemap.SetTile(position2, null);
        }
        if(tilemap.HasTile(position3)){
            tilemap.SetTile(position3, null);
        }
        if(tilemap.HasTile(position4)){
            tilemap.SetTile(position4, null);
        }
        if(tilemap.HasTile(position5)){
            tilemap.SetTile(position5, null);
        }
        if(tilemap.HasTile(position6)){
            tilemap.SetTile(position6, null);
        }
        
    }
}
