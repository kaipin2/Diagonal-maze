using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public  GameObject GameContoroller;
    private mainScript script;
    private CreateMaze MazeScript;

    Vector3 MOVEX = new Vector3(1.0f,0,0); //x軸方向に1マス移動
    Vector3 MOVEZ = new Vector3(0, 0, 1.0f); //z軸方向に1マス移動

    float step = 2f; //移動速度
    Vector3 Player; //プレイヤーの位置
    Vector3 InitialPos; //初期位置
    Vector3 prePos; //移動前の位置

    int BoardSize; //盤面の大きさの半分

    int checkpoint =  0; //最後に通ったチェックポイント
    int checkpoint_number; //チェックポイントの個数

    // Start is called before the first frame update
    void Start()
    {
        //GameContoroller = GameObject.Find("GameContoroller");
        script = GameContoroller.GetComponent<mainScript>();
        MazeScript = GameContoroller.GetComponent<CreateMaze>();
        
        this.setInitialPos(new Vector3 (0,0,-script.Board_Mass("half")));
        Player = this.getInitialPos();
        BoardSize = script.Board_Mass ("half");
    }

    // Update is called once per frame
    void Update()
    {
        //復帰

        //transformを取得
        Transform myTransform = this.transform;
        Vector3 worldAngle = myTransform.eulerAngles;

        //座標を取得
        Vector3 pos = myTransform.position;


        //落ちたら初期位置に移動
        if (pos.y < -5)
        {
            myTransform.position = new Vector3(0,1,-4);
        }
        //移動中でなければ移動できる
        if (transform.position == Player){
            PlayerMove(pos);
        }
        else
        {
            //Player = this.getInitialPos();
            //UnityEngine.Debug.Log("transform.position:"+ transform.position);
            //UnityEngine.Debug.Log("Player:" + Player);
        }
        Move(true);

    }

    void PlayerMove(Vector3 pos)
    {
        prePos = Player;

        
        // 左上に移動
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) && (Player.x > -BoardSize && Player.z+0.1 < BoardSize))
        {
            Player = transform.position - MOVEX　+ MOVEZ;
            return;
        }
        // 右下に移動
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow) && Player.x < BoardSize && Player.z > -BoardSize)
        {
            Player = transform.position + MOVEX - MOVEZ;
            return;
        }
        // 右上に移動
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && Player.z < BoardSize && Player.x < BoardSize)
        {
            Player = transform.position + MOVEZ + MOVEX;
            return;
        }
        // 左下に移動
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow) && Player.z > -BoardSize && Player.x > -BoardSize)
        {
            Player = transform.position - MOVEZ - MOVEX;
            return;
        }
        
        //ジャンプ
        if (Input.GetKey("j") && pos.y < 0.05)
        {
            for(int j = 0; j < 10 ; j++)
            {
                this.transform.Translate(0.0f, 0.1f, 0.0f);
            }
        }

    }
    void Move(bool t)
    {
            if (t)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player, step * Time.deltaTime);

            }
            else
            {
                transform.position = Vector3.MoveTowards(prePos, Player, step * Time.deltaTime);
            }
        
       
    }



    public Vector3 getInitialPos()
    {
        return InitialPos;
    }
    public void setInitialPos(Vector3 v)
    {
        InitialPos = v;
    }
    public void MassContact(string dir,int num)
    {
        checkpoint_number = MazeScript.GetCheckPoint();
        //UnityEngine.Debug.Log("checkpoint_number =" + checkpoint_number);
        //UnityEngine.Debug.Log(dir + "," + num); // ログを表示する
        switch (dir)
        {
            case null:
                if(num == checkpoint + 1)
                {
                    checkpoint = num;
                }
                else
                {
                    script.Result(false);
                    //erorr!!
                }
                //チェックポイント
                break;
            case " ":
                if(checkpoint == checkpoint_number)
                {
                    script.Result(true);
                    //CLEAR
                }
                else
                {
                    script.Result(false);
                    //error
                }
                break;
            case "L":
                ForcedMove(-MOVEX);
                break;
            case "R":
               ForcedMove(MOVEX);
                break;
            case "D":
                ForcedMove(-MOVEZ);
                break;
            case "U":
                ForcedMove(MOVEZ);
                break;
            case "LD":
                ForcedMove(-MOVEX-MOVEZ);
                break;
            case "LU":
                ForcedMove(-MOVEX+MOVEZ);
                break;
            case "RD":
                ForcedMove(MOVEX-MOVEZ);
                break;
            case "RU":
                ForcedMove(MOVEX+MOVEZ);
                break;
            default:
                break;
        }
    }

    void ForcedMove(Vector3 v)
    {
 
        prePos = Player;
        Player = Player + v;
        Move(false);
    }

}
