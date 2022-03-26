using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Windows.Forms; //OpenFileDialog用に使う

public class CreateMaze : MonoBehaviour
{
    //アクションマスの個数の最大
    //private static int MAX_ACTION = 10;

    public GameObject Player; //プレイヤー
    public GameObject GoalMass; //ゴールマス
    public GameObject StartMass; //スタートマス
    public GameObject GameCanvas; //親
    GameObject goalmass;
    GameObject startmass;

    public GameObject[] ActionMass; //アクションマス
    int ActionCount = 0; //アクションマスの個数
    public GameObject action; //アクションマス（原本）

    //他のスクリプト呼び出し
    public GameObject GameContoroller;
    public mainScript script;
    public GameObject MassObject;
    public Mass[] MassScript;

    //PlayerObjectのスクリプト
    public PlayerObject scriptP;

    //テキスト
    TextAsset textasset;
    //テキストのパス
    string path;
    //テキスト名前
    string filename = "maze";
    //迷路（テキスト）の加工前の1行を入れる変数
    public string[] textMessage;
    //テキスト保存
    string TextLines;
    //迷路の複数行を入れる変数
    public string[,] textWords;
    //テキスト内の行数を取得する変数
    private int rowLength;
    //テキスト内の列数を取得する変数
    private int columnLength;

    //チェックポイントの個数
    private int checkpoint;

    public GameObject ParentCanvas
    {
        set
        {
            GameCanvas = value;
        }
        get
        {
            return GameCanvas;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //スクリプト（mainScript,Mass）を呼び出し
        GameContoroller = GameObject.Find("GameContoroller");
        script = GameContoroller.GetComponent<mainScript>();
        MassObject = GameObject.FindGameObjectWithTag("ActionMass");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //スタート位置、ゴール位置、プレイヤー駒を配置
    public void Initialize()
    {
        Start();
        //Player駒を配置
        //Player = GameObject.Find("GameCanvas/Slime2");
        //スクリプトを参照
        scriptP = Player.GetComponent<PlayerObject>();
        //読み込むテキストファイルを選択
        //OpenFile();
        LoadMaze();

    }
    public void OpenFile()
    {
        
        OpenFileDialog open_file_dialog = new OpenFileDialog();
        //ダイアログを開く
        open_file_dialog.ShowDialog();
        //取得したファイル名をstringに代入する
        path = open_file_dialog.FileName;
        /*
        //パスの取得
        path = EditorUtility.OpenFilePanel("Open txt", "/Assets/Resources", "txt");
        if (string.IsNullOrEmpty(path))
            return;
        */
        filename = System.IO.Path.GetFileNameWithoutExtension(Path.GetFileName(path));
        //UnityEngine.Debug.Log("filename:"+filename);
        
    }
    public void LoadMaze()
    {
   
        textasset = new TextAsset();//テキストファイルのデータを取得するインスタンス
        textasset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
        TextLines = textasset.text;
        textMessage = TextLines.Split('\n');
        
        //行数と列数を取得
        columnLength = textMessage[0].Split(',').Length;
        rowLength = textMessage.Length;
        if (script.Board_Mass("mass") != columnLength || columnLength != rowLength)
        {
            //UnityEngine.Debug.Log("erorr");
            return;
        }
        //アクションマスの最大値を設定
        ActionMass = new GameObject[columnLength * rowLength];
        MassScript = new Mass[columnLength * rowLength];
        //2次配列を定義
        textWords = new string[columnLength, rowLength];

        for (int i = 0; i < rowLength; i++)
        {
            //改行コードを削除
            textMessage[i] = textMessage[i].Replace("\r", "").Replace("\n", "");
            string[] tempWords = textMessage[i].Split(','); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength; n++)
            {
                textWords[n , i] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
                if(textWords[n, i] == "0")
                {
                    //UnityEngine.Debug.Log("textWords["+n+","+i+" ]");
                }
                //UnityEngine.Debug.Log("["+i+","+n+"] = "+textWords[i, n]); //[0,0] = [-4,0,4]
            }
        }
    }
    public void Create()
    {
        for (int i = 0; i < rowLength ; i++)
        {
            for(int j = 0; j < columnLength ; j++)
            {
                switch (textWords[j, i])
                {
                    case "0":
                        break;
                    case "S":
                        scriptP.setInitialPos(new Vector3 (j - ((columnLength - 1) / 2),0.05f, -i + ((rowLength - 1) / 2)));
                        Placement(StartMass , j - ((columnLength - 1) / 2) , 0 ,  -i + ((rowLength - 1) / 2));
                        PlayerSet();
                        break;
                    case "G":
                        Placement(GoalMass , j - ((columnLength - 1) / 2), 0 , -i + ((rowLength - 1) / 2));
                        break;
                    default:
                        Placement(action, j - ((columnLength - 1) / 2), 0, -i + ((rowLength - 1) / 2));     
                        ActionMassText(textWords[j,i]);
                        break;
                }
            }
        }
    }
    //駒の配置
    public void Placement(GameObject g,int x, int y, int z)
    {
        //マスを配置
        GameObject Gobject = Instantiate(g);
        Gobject.transform.position = new Vector3(x, y + 0.05f, z);
        Gobject.transform.SetParent(GameCanvas.transform,true);
        //Gobject.transform.parent = GameCanvas.transform;
        ActionMass[ActionCount] = Gobject;
        MassScript[ActionCount] = ActionMass[ActionCount].GetComponent<Mass>();
        ActionCount++;
    }

    //アクション駒のテキスト配置
    public void ActionMassText(string s)
    {
        string direction = null;
        int number = 0;
        foreach(char c in s)
        {
            switch (c)
            {
                case 'L':
                    direction += c.ToString();
                    break;
                case 'R':
                    direction += c.ToString();
                    break;
                case 'D':
                    direction += c.ToString();
                    break;
                case 'U':
                    direction += c.ToString();
                    break;
                default:
                    if(direction == null)
                    {
                        checkpoint += 1;
                    }
                    number = int.Parse(c.ToString());
                    MassScript[ActionCount - 1].TextWrite(direction, number);
                    break;
            }
        }
    }
    public void PlayerSet()
    {
        Player.transform.position = scriptP.getInitialPos();
    }
    public int GetCheckPoint()
    {
        return checkpoint;
    }
}
