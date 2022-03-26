using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mass : MonoBehaviour
{
    private GameObject GameContoroller;
    private mainScript script;
    private GameObject Playerobject;
    private PlayerObject PlayerScript;


    public TextMeshProUGUI textOns; //駒のテキスト
    public TextMeshProUGUI textUps; //上空のテキスト

    bool textOn = true;
    bool textUp = false;
    bool inversion = false; //文字を反転させる

    Transform myTransform;
    Vector3 worldAngle;
    string s; //文字を取得

    string dir = " "; //方向取得
    int num = -1; //チェックポイントの数字or移動数字

    // Start is called before the first frame update
    void Start()
    {
        GameContoroller = GameObject.Find("GameContoroller");
        script = GameContoroller.GetComponent<mainScript>();
        Playerobject = GameObject.Find("GameCanvas/Slime2");
        PlayerScript = Playerobject.GetComponent<PlayerObject>();

        Text();
        
        myTransform = textUps.transform;
        //s = textUps.text.ToString();
        worldAngle = myTransform.eulerAngles;
        TextDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        switch (script.ActiveCamera)
        {
            case 0:
                textOn = true;
                textUp = false;
                inversion = false;
                break;
            case 1:
                textOn = false;
                textUp = true;
                inversion = false;
                break;
            case 2:
                textOn = false;
                textUp = true;
                inversion = true;
                break;
            default:
                //UnityEngine.Debug.Log("error");
                break;
        }

        TextDisplay();

    }
    public void TextDisplay()
    {
        //UnityEngine.Debug.Log(s);
        textOns.enabled = textOn;
        textUps.enabled = textUp;
        
        if (inversion)
        {
            if (this.name == "ActionMass(Clone)" && this.dir != " ")
            {
                dir_inversion(true);
            }
            //worldAngle.z = 180.0f;
            //worldAngle.x = 90.0f;
            //myTransform.localEulerAngles = worldAngle;

        }
        else
        {
            if (this.name == "ActionMass(Clone)" && this.dir != " ")
            {
                dir_inversion(false);
            }
            //worldAngle.z = 0.0f;
            //worldAngle.x = -90.0f;
            //myTransform.localEulerAngles = worldAngle;
        }
    }
    public void TextWrite(String dirction, int number)
    {
        this.dir = dirction;
        this.num = number;
        Text();
        switch (dirction)
        {
            case null:
                textOns.text = "";
                textUps.text = "";
                //チェックポイント
                break;
            case "L":
                textOns.text = "←";
                textUps.text = "←";
                break;
            case "R":
                textOns.text = "→";
                textUps.text = "→";
                break;
            case "D":
                textOns.text = "↓";
                textUps.text = "↓";
                break;
            case "U":
                textOns.text = "↑";
                textUps.text = "↑";
                break;
            case "LD":
                textOns.text = "↙";
                textUps.text = "↙";
                break;
            case "LU":
                textOns.text = "↖";
                textUps.text = "↖";
                break;
            case "RD":
                textOns.text = "↘";
                textUps.text = "↘";
                break;
            case "RU":
                textOns.text = "↗";
                textUps.text = "↗";
                break;
            default:
                break;
        }
        textOns.text += number.ToString();
        textUps.text += number.ToString();
    }
    void Text()
    {
        /*
        this.textOns = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        this.textUps = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        */
    }

    //当たり判定
    void OnCollisionEnter(Collision collision)
    {
        //Vector3 pos = this.trasnform.position();
        if (collision.gameObject.name == "Slime2" && this.name != "StartMass(Clone)")
        {
            PlayerScript.MassContact(dir,num);
        }
    }

    //左右反転
    void dir_inversion(bool inv)
    {
        switch (dir)
        {
            case null:
                return;
                //チェックポイント
            case "R":
                if (inv)
                {
                    textOns.text = "←";
                    textUps.text = "←";
                }
                else
                {
                    textOns.text = "→";
                    textUps.text = "→";
                }
                break;
            case "L":
                if (inv)
                {
                    textOns.text = "→";
                    textUps.text = "→";
                }
                else
                {
                    textOns.text = "←";
                    textUps.text = "←";
                }
                break;
            case "D":
                textOns.text = "↓";
                textUps.text = "↓";
                break;
            case "U":
                textOns.text = "↑";
                textUps.text = "↑";
                break;
            case "RD":
                if (inv)
                {
                    textOns.text = "↙";
                    textUps.text = "↙";
                }
                else
                {
                    textOns.text = "↘";
                    textUps.text = "↘";
                }
                break;
            case "RU":
                if (inv)
                {
                    textOns.text = "↖";
                    textUps.text = "↖";
                }
                else
                {
                    textOns.text = "↗";
                    textUps.text = "↗";
                }

                break;
            case "LD":
                if (inv)
                {
                    textOns.text = "↘";
                    textUps.text = "↘";
                }
                else
                {
                    textOns.text = "↙";
                    textUps.text = "↙";
                }
                break;
            case "LU":
                if (inv)
                {
                    textOns.text = "↗";
                    textUps.text = "↗";
                }
                else
                {
                    textOns.text = "↖";
                    textUps.text = "↖";
                }
 
                break;
            default:
                break;
        }
        textOns.text += num.ToString();
        textUps.text += num.ToString();
    }
}
