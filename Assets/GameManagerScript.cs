using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    //追加
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;

    public GameObject clearText;

    // Start is called before the first frame update


    /*void PrintArray()
    {
        //追加、文字列の宣言と初期化
        string debugText = "";
        //変更。二重for文で二次元配列の情報を出力
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";

            }
            debugText += "\n";//改行
        }
        Debug.Log(debugText);
    }*/

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y,x] == null)
                {
                   continue;
                }
                if (field[y,x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"><動かしたい数字/param>
    /// <param name="moveFrom"><動かす元の場所/param>
    /// <param name="moveTo"><動かす先の場所/param>
    /// <returns></returns>
    bool MoveNumber(string tagName, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //移動先が範囲外だったら移動不可
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            return false;
        }

        //移動先に2(箱)が居たら
        if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
         {
             //
             Vector2Int velocity = moveTo - moveFrom;

             //プレイヤーの移動先から、さらに先へ2(箱)を移動させる。
             //箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを
             //呼び、処理が再帰している。移動不可をboolで記録
             bool success = MoveNumber("Box", moveTo, moveTo + velocity);
            
            //もし箱が移動失敗したら、プレイヤーの移動も失敗
             if (!success)
             {
                 return false;
             }
         }
        //プレイヤー・箱かかわらず移動の処理
      
        field[moveFrom.y, moveFrom.x].transform.position = IndexToPosition(moveTo); ;
        
        Vector3 moveToPosition = IndexToPosition(moveTo);
        field[moveFrom.y, moveFrom.x].GetComponent<MoveScript>().MoveTo(moveToPosition);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;

        return true;
    }

    Vector3 IndexToPosition(Vector2Int index)
    {
        return new Vector3(
            index.x - map.GetLength(1) / 2 + 0.5f, 
            index.y - map.GetLength(0) / 2,
            0
            );
    }

    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y=0;y<map.GetLength(0);y++)
        {
            for(int x=0;x<map.GetLength(1);x++)
            {
                //格納場所か否かを判断
                if (map[y,x]==3)
                {
                    //格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //要素数はgoals.Countで取得
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                //一つでも箱が無かったら条件未達成
                return false;
            }
        }
        //条件未達成でなければ条件達成
        return true;
    }

    //配列の宣言
    int[,] map;          //レベルデザイン用の配列
    GameObject[,] field; //ゲーム描画用の配列

    void Start()
    {

        //配列の実態の作成と初期化
        map = new int[,] {
            { 0, 0, 0, 0, 0 } ,
            { 0, 3, 1, 3, 0 } ,
            { 0, 0, 2, 0, 0 } ,
            { 0, 2, 3, 2, 0 } ,
            { 0, 0, 0, 0, 0 } ,
        };
        field = new GameObject
        [
          map.GetLength(0),
          map.GetLength(1)
        ];

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y,x]==1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        IndexToPosition(new Vector2Int (x,y)),
                        Quaternion.identity
                        );
                }

                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                        );
                }

                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                        goalPrefab,
                        IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                        );
                }
            }
        }

        //追加、文字列の宣言と初期化
        string debugText = "";
        //変更。二重for文で二次元配列の情報を出力
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";

            }
            debugText += "\n";//改行
        }
        Debug.Log(debugText);

        //PrintArray();

    }

    // Update is called once per frame
    void Update()
    {
        //右移動
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
            //PrintArray();

        }

        //左移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1,0));
            //PrintArray();

        }

        //上移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0,1));
            //PrintArray();

        }

        //下移動
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
            //PrintArray();

        }

        if(IsCleard())
        {
            clearText.SetActive(true);
        }

    }
}
