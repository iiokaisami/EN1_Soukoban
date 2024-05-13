using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    //完了までにかかる時間
    //private float timeTaken = 0.2f;

    //経過時間
    //private float timeErapsed;

    //目的地
    //private Vector3 destination;

    //出発地
    //private Vector3 origin;

    public void MoveTo(Vector3 destination)
    {
        transform.position = destination;

        //経過時間を初期化
        //timeErapse = 0;

        //移動中の可能性があるので、現在地とpositionに前回移動の目的地を代入
        //origin = destination;
        //transform.position = origin;

        //新しい目的地を代入
        //destination = newDestination;
    }

    // Start is called before the first frame update
    void Start()
    {
        //目的地・出発地を現在地で初期化
        //destination = transform.position;
        //origin = destination;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
