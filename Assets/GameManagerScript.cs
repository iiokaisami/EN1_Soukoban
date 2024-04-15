using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update


    void PrintArray()
    {
        //�ǉ��A������̐錾�Ə�����
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
    /// <param name="number"><��������������/param>
    /// <param name="moveFrom"><���������̏ꏊ/param>
    /// <param name="moveTo"><��������̏ꏊ/param>
    /// <returns></returns>
    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        //�ړ��悪�͈͊O��������ړ��s��
        if (moveTo < 0 || moveTo >= map.Length)
        {
            return false;
        }
        //�ړ����2(��)��������
        if (map[moveTo] == 2)
        {
            //
            int velocity = moveTo - moveFrom;
            //�v���C���[�̈ړ��悩��A����ɐ��2(��)���ړ�������B
            //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h��
            //�ĂсA�������ċA���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            //���������ړ����s������A�v���C���[�̈ړ������s
            if (!success)
            {
                return false;
            }
        }
        //�v���C���[�E��������炸�ړ��̏���
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

    //�z��̐錾
    int[] map;

    void Start()
    {
        //�z��̎��Ԃ̍쐬�Ə�����
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };

        PrintArray();

    }

    // Update is called once per frame
    void Update()
    {
        //�E�ړ�
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();

        }

        //���ړ�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();

        }

    }
}
