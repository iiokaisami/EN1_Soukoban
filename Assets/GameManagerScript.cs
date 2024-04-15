using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update


    void PrintArray()
    {
        //追加、文字列の宣言と初期化
        string debugText = "";

        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ",";
        }

        Debug.Log(debugText);
    }

    int GetPlayerIndex()
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"><動かしたい数字/param>
    /// <param name="moveFrom"><動かす元の場所/param>
    /// <param name="moveTo"><動かす先の場所/param>
    /// <returns></returns>
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        //移動先が範囲外だったら移動不可
        if (moveTo < 0 || moveTo >= map.Length)
        {
            return false;
        }
        //移動先に2(箱)が居たら
        if (map[moveTo] == 2)
        {
            //
            int velocity = moveTo - moveFrom;
            //プレイヤーの移動先から、さらに先へ2(箱)を移動させる。
            //箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを
            //呼び、処理が再帰している。移動不可をboolで記録
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            //もし箱が移動失敗したら、プレイヤーの移動も失敗
            if (!success)
            {
                return false;
            }
        }
        //プレイヤー・箱かかわらず移動の処理
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

    //配列の宣言
    int[] map;

    void Start()
    {
        //配列の実態の作成と初期化
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };

        PrintArray();

    }

    // Update is called once per frame
    void Update()
    {
        //右移動
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();

        }

        //左移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();

        }

    }
}
