using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    //�ǉ�
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;

    public GameObject clearText;

    // Start is called before the first frame update


    /*void PrintArray()
    {
        //�ǉ��A������̐錾�Ə�����
        string debugText = "";
        //�ύX�B��dfor���œ񎟌��z��̏����o��
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";

            }
            debugText += "\n";//���s
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
    /// <param name="number"><��������������/param>
    /// <param name="moveFrom"><���������̏ꏊ/param>
    /// <param name="moveTo"><��������̏ꏊ/param>
    /// <returns></returns>
    bool MoveNumber(string tagName, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //�ړ��悪�͈͊O��������ړ��s��
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            return false;
        }

        //�ړ����2(��)��������
        if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
         {
             //
             Vector2Int velocity = moveTo - moveFrom;

             //�v���C���[�̈ړ��悩��A����ɐ��2(��)���ړ�������B
             //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h��
             //�ĂсA�������ċA���Ă���B�ړ��s��bool�ŋL�^
             bool success = MoveNumber("Box", moveTo, moveTo + velocity);
            
            //���������ړ����s������A�v���C���[�̈ړ������s
             if (!success)
             {
                 return false;
             }
         }
        //�v���C���[�E��������炸�ړ��̏���
      
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
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y=0;y<map.GetLength(0);y++)
        {
            for(int x=0;x<map.GetLength(1);x++)
            {
                //�i�[�ꏊ���ۂ��𔻒f
                if (map[y,x]==3)
                {
                    //�i�[�ꏊ�̃C���f�b�N�X���T���Ă���
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //�v�f����goals.Count�Ŏ擾
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                //��ł���������������������B��
                return false;
            }
        }
        //�������B���łȂ���Ώ����B��
        return true;
    }

    //�z��̐錾
    int[,] map;          //���x���f�U�C���p�̔z��
    GameObject[,] field; //�Q�[���`��p�̔z��

    void Start()
    {

        //�z��̎��Ԃ̍쐬�Ə�����
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

        //�ǉ��A������̐錾�Ə�����
        string debugText = "";
        //�ύX�B��dfor���œ񎟌��z��̏����o��
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";

            }
            debugText += "\n";//���s
        }
        Debug.Log(debugText);

        //PrintArray();

    }

    // Update is called once per frame
    void Update()
    {
        //�E�ړ�
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
            //PrintArray();

        }

        //���ړ�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1,0));
            //PrintArray();

        }

        //��ړ�
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0,1));
            //PrintArray();

        }

        //���ړ�
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
