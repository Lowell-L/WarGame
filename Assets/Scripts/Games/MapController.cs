using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//地图的移动与缩放
public class MapController : MonoBehaviour
{
    public float cameraMoveSpeed;
    public float borderWidth = 50f;
    public Camera mainCamera;
    public double rightRange;
    public double leftRange;
    public double upRange;
    public double downRange;
    
    // Start is called before the first frame update
    void Start()
    {
        rightRange = 5;
        leftRange = -5;
        upRange = 5;
        downRange = -3.5;
        GameObject gameObject = GameObject.Find("Main Camera");
        mainCamera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Zoom();
    }

    
    void Move()
    {
        //防止地图移除边界
        Vector3 mousePos = Input.mousePosition;
        Vector3 cameraPos = mainCamera.transform.position;
        
        if(mousePos.x > Screen.width - borderWidth && cameraPos.x <= rightRange){
            transform.Translate(transform.right * cameraMoveSpeed * Time.deltaTime, Space.World);
        }

        if(mousePos.x < borderWidth && cameraPos.x >= leftRange){
            transform.Translate(-transform.right * cameraMoveSpeed * Time.deltaTime, Space.World);
        }

        if(mousePos.y > Screen.height - borderWidth && cameraPos.y <= upRange){
            transform.Translate(transform.up * cameraMoveSpeed * Time.deltaTime, Space.World);
        }

        if(mousePos.y < borderWidth && cameraPos.y >= downRange){
            transform.Translate(-transform.up * cameraMoveSpeed * Time.deltaTime, Space.World);
        }
        
    }

    void Zoom()
    {
        double view = Camera.main.fieldOfView;
        //Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Debug.Log("small" + Camera.main.fieldOfView);
            Camera.main.fieldOfView += 2;
        }
        //Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(Camera.main.fieldOfView > 10){
                Camera.main.fieldOfView -= 2;
            }
        }
            
        


    }
}
