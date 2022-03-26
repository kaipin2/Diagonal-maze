using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class TitleController : Buttons
{
    private static int ButtonNumber = 2; //Titleのボタンの個数
    public GameObject[] btn_object = new GameObject[ButtonNumber];

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //初期化
    private void Initialize()
    {
        //Buttons button = this.gameObject.AddComponent<Buutons>();
        for (int i = 0;i < btn_object.Length ;i++)
        {
            switch (i)
            {
                case 0:
                    btn_object[i].GetComponent<Button>().onClick.AddListener(GameSceneMove);
                    break;
                case 1:
                    btn_object[i].GetComponent<Button>().onClick.AddListener(Exit);
                    break;
                default:
                    break;
            }

        }

    }

}
