using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using TMPro;

public class mainScript : Buttons
{
    //MAX_BOARD(1辺のボードの大きさ)を定義
    private static int MAX_BOARD = 5;
    private static int board_half = (MAX_BOARD - 1) / 2;

    //カメラ情報
    private GameObject board_camera; //ゲーム板のカメラ
    private RaycastHit hit;
    private GameObject player_camera; //Playerのカメラ
    private GameObject player_camera2;
    public int ActiveCamera = 0; //どのカメラがうごいているか
    private static int camera_number = 3; //カメラの視点の数
    private GameObject[] camera_str = new GameObject[camera_number];
    private Transform[] camera_Transform = new Transform[camera_number];
    private Vector3[] camera_position = new Vector3[camera_number];
    private Vector3[] camera_eulerAngles = new Vector3[camera_number];
    //prehabs
    public GameObject cube_bule;
    public GameObject cube_bule2;
    public GameObject[] Pre_Board = new GameObject[MAX_BOARD * MAX_BOARD]; //ゲーム板
    public GameObject[,] Board = new GameObject[MAX_BOARD,MAX_BOARD]; //ゲーム板
    public GameObject VerticalWall; //縦の壁
    public GameObject SideWall; //横の壁
    public GameObject GoalMass; //ゴールマス
    public GameObject StartMass; //スタートマス
    public GameObject ActionMass; //アクションマス
    private bool finish = false; //ゲーム終了
    public GameObject prehab_button = null;
    public GameObject canvas;
    private Image can_image;
    public GameObject GameCanvas;


    //ボタン（カメラ数×２）
    private static int button_number = 2; //ボタン数
    private GameObject[] Button = new GameObject[button_number];
    private TextMeshProUGUI[] text_result;
    Button ChoiceButton = null;

    //プレイヤーの初期位置
    private static Vector3 InitialPos; //= new Vector3(0,0,-board_half);


    //GameObject
    public GameObject GameContoroller;
    public CreateMaze script;
    public GameObject Player;
    public PlayerObject scriptP;

    // Start is called before the first frame update
    void Start()
    {
        finish = false;


        GameContoroller = GameObject.Find("GameContoroller");
        script = GameContoroller.GetComponent<CreateMaze>();
        Player = GameObject.Find("Slime2");
        scriptP = Player.GetComponent<PlayerObject>();

        //カメラの情報を取得
        board_camera = GameObject.Find("Main Camera");
        
        player_camera = GameObject.Find("GameCanvas/Slime2/PlayerCamera");
        player_camera2 = GameObject.Find("GameCanvas/Slime2/PlayerCamera2");

        //カメラの初期位置設定
        for (int i = 0;i < camera_position.Length ; i++)
        {
            switch (i)
            {
                case 0:
                    camera_position[i] = new Vector3((float)0.56, (float)10.78, 0);
                    camera_eulerAngles[i] = new Vector3(90f, 0f, 0f);
                    break;
                case 1:
                    camera_position[i] = new Vector3((float)0, (float)2.66, (float)-4.03);
                    camera_eulerAngles[i] = new Vector3(10f, 0f, 0f);
                    break;
                case 2:
                    camera_position[i] = new Vector3((float)0, (float)1.8, (float)4);
                    camera_eulerAngles[i] = new Vector3(0f, 180f, 0f);
                    break;
            }

        }

        
        player_camera.SetActive(false);
        player_camera2.SetActive(false);
        
        ActiveCamera = 0;
        Camera_Position(ActiveCamera);

        can_image = canvas.GetComponent<Image>();
        can_image.enabled = false;
        //Canvasの位置
        CanvasStatus();

        //盤面を生成
        GenarateBoard();
    }

    // Update is called once per frame
    void Update()
    {
        //スペースキーが押されたとき、プレイヤーカメラをアクティブにする
        if (Input.GetKey("1"))
        {
            ActiveCamera = 2;

            Camera_Position(ActiveCamera);
            Status();

        }
        else if (Input.GetKey("2")) 
        {
            ActiveCamera = 1;
            
            Camera_Position(ActiveCamera);
            Status();

        } 
        else if(Input.GetKey("3"/*KeyCode.Return*/))
        {
            ActiveCamera = 0;
            Camera_Position(ActiveCamera);
            Status();

        }

        FinishJudge();
    }
    private void Status()
    {
        if (finish)
        {
            for (int i = 0; i < text_result.Length; i++)
            {
                TextStatus(i);
            }
            for (int j = 0; j < button_number; j++)
            {
                ButtonStatus(j);
            }
            //Canvasの位置
            CanvasStatus();
        }
    }
    private void Camera_Position(int num)
    {
        if(num != 0)
        {
            board_camera.transform.parent = Player.transform;
        }
        else
        {
            board_camera.transform.parent = null;
        }
        board_camera.transform.localPosition = camera_position[num];
        board_camera.transform.eulerAngles = camera_eulerAngles[num];
    }
    private void GenarateBoard()
    {
       
        //ボードの置く場所を指定
        for (int i = 0 ; i < MAX_BOARD * MAX_BOARD; i += 2)
        {

            Pre_Board[i] = Instantiate(cube_bule);
            if ((i + 1) < MAX_BOARD * MAX_BOARD)
            {
                Pre_Board[i + 1] = Instantiate(cube_bule2);
            }


        }
        for (int i = -board_half; i < board_half + 1; i += 1)
        {

            for (int j = -board_half; j < board_half + 1; j += 1)
            {
                Board[i + board_half, j + board_half] = Pre_Board[(i + board_half) * MAX_BOARD + (j + board_half)];
                Board[i + board_half,j + board_half].transform.position = new Vector3(j , 0 , -i);
                Board[i + board_half, j + board_half].transform.parent = GameCanvas.transform;
            }
        }

        //壁を配置
        GameObject verticalwall1 = Instantiate(VerticalWall);
        GameObject sidewall1 = Instantiate(SideWall);
        GameObject verticalwall2 = Instantiate(VerticalWall);
        GameObject sidewall2 = Instantiate(SideWall);

        verticalwall1.transform.position = new Vector3(0 ,0, -(board_half + 1));
        verticalwall2.transform.position = new Vector3(0, 0, board_half + 1);
        sidewall1.transform.position = new Vector3(-(board_half + 1), 0, 0);
        sidewall2.transform.position = new Vector3(board_half + 1, 0, 0);

        verticalwall1.transform.parent = GameCanvas.transform;
        verticalwall2.transform.parent = GameCanvas.transform;
        sidewall1.transform.parent = GameCanvas.transform;
        sidewall2.transform.parent = GameCanvas.transform;

        script.ParentCanvas = GameCanvas;
        script.Initialize();
        script.Create();
    }
    
    public void Result(bool result)
    {
        GameObject[] camera = { board_camera};
        camera_str = camera;
        text_result = new TextMeshProUGUI[camera.Length];

        GameObject[] button = new GameObject[button_number];
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 scale = new Vector3(1, 1, 1);

        for (int i = 0; i < text_result.Length; i++)
        {
            text_result[i] = camera_str[i].transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            TextStatus(i);

            if (text_result[i].text != "")
            {
                return;
            }
            else
            {
                finish = true;
                
                can_image.enabled = true;
                CanvasStatus();

                ResultButton(button);



                if (result)
                {
                    text_result[i].text = "CLEAR";
                }
                else
                {
                    text_result[i].text = "MISS";
                }
            }
        }


    }
    public int Board_Mass(String s)
    {
            switch (s)
            {
                case "mass":
                    return MAX_BOARD;
                    //break;
                case "half":
                    return board_half;
                    //break;
                default:
                    //UnityEngine.Debug.Log("erorr");
                    return 0;
                    //break;
            }

    }
    void ResultButton(GameObject[] button)
    {


        for (int j = 0; j < button_number; j++)
        {
            button[j] = Instantiate(prehab_button);
            button[j].transform.SetParent(canvas.transform, false);
            Button[j] = button[j];

            ButtonStatus(j);

            TextMeshProUGUI text = button[j].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            if (j == 0)
            {
                text.text = "Retry";
                button[j].GetComponent<Button>().onClick.AddListener(GameSceneMove);
            }
            else if(j == 1)
            {
                text.text = "Title";
                button[j].GetComponent<Button>().onClick.AddListener(TitleSceneMove);
            }      
            ButtonChoice("Canvas/Button(Clone)");        


        }


    }
    void FinishJudge()
    {
        if (finish)
        {
            if (Input.GetKey("z"))
            {
                SceneManager.LoadScene("main");
            }
            else if (Input.GetKey("x"))
            {
                SceneManager.LoadScene("title");
            }else if (Input.GetKey("c"))
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                UnityEngine.Application.Quit();
                #endif
            }
        }
    }
    void ButtonStatus(int j)
    {
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 scale = new Vector3(1, 1, 1);

        switch (ActiveCamera)
        {
            case 0:
                position = new Vector3((float)(-18.6 + (2 * j * 18.6)), (float)-12.2, (float)-20);
                scale = new Vector3((float)0.5, (float)0.5, (float)0.5);
                break;
            case 1:
                position = new Vector3((float)(-0.9 + (2 * j * 0.9)), (float)-0.7, (float)0);
                scale = new Vector3((float)0.03, (float)0.03, (float)0.03);
                break;
            case 2:
                position = new Vector3((float)(-0.9 + (2 * j * 0.9)), (float)-0.7, (float)0);
                scale = new Vector3((float)0.03, (float)0.03, (float)0.03);
                break;

        }
        Button[j].transform.localPosition = position;
        Button[j].transform.localScale = scale;
    }
    void CanvasStatus()
    {
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 scale = new Vector3(1, 1, 1);

        switch (ActiveCamera)
        {
            case 0:
                position = new Vector3((float)0, (float)0, (float)10);
                scale = new Vector3((float)0.2, (float)0.2, (float)0.2);
                break;
            case 1:
                position = new Vector3((float)0, (float)0, (float)0.35);
                scale = new Vector3((float)0.2, (float)0.2, (float)0.2);
                break;
            case 2:
                position = new Vector3((float)0, (float)0, (float)0.35);
                scale = new Vector3((float)0.2, (float)0.2, (float)0.2);
                break;

        }
        canvas.transform.localPosition = position;
        canvas.transform.localScale = scale;
    }
    void TextStatus(int i)
    {

            Vector3 position = new Vector3(0, 0, 0);
            Vector3 scale = new Vector3(1, 1, 1);
            switch (ActiveCamera)
            {
                case 0:
                    position = new Vector3((float)0, (float)10, (float)-20);
                    scale = new Vector3((float)1, (float)1, (float)1);
                    break;
                case 1:
                    position = new Vector3((float)0, (float)0.6, (float)0);
                    scale = new Vector3((float)0.05, (float)0.05, (float)0.05);
                    break;
                case 2:
                position = new Vector3((float)0, (float)0.6, (float)0);
                scale = new Vector3((float)0.05, (float)0.05, (float)0.05);
                break;

            }

        text_result[i].transform.localPosition = position;
        text_result[i].transform.localScale = scale;

    }

    void ButtonChoice(String str)
    {
        

        if (ActiveCamera == 0)
        {
            ChoiceButton = GameObject.Find(str).GetComponent<Button>();
            //ボタンが選択された状態になる
            ChoiceButton.Select();
        }
    }
}
