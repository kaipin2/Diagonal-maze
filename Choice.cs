using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UI使うときに必要
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GameObject.Find("Canvas/Play").GetComponent<Button>();
        //ボタンが選択された状態になる
        button.Select();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Start();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            button = GameObject.Find("Canvas/Exit").GetComponent<Button>();
            //ボタンが選択された状態になる
            button.Select();
        }
    }
}
