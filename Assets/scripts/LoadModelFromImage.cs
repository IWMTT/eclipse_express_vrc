
using System;
using UdonSharp;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDKBase;
using VRC.Udon;

public class LoadModelFromImage : UdonSharpBehaviour
{
    [SerializeField]
    private Texture2D textureToRead;
    [SerializeField]
    private GameObject[] loadTargets;
    [SerializeField]
    private Color32[] colorPairs;

    [SerializeField]
    private float speed;

    private GameObject[][] gameObjectsArray; 


    private float[] time_vec;
    private float[][] val_matrix;
    private DateTime _pressed_time;
    private DateTime _updated_time;
    private bool in_sequence = false;
    private float _max_time;


    void Start()
    {
        Color32 color = textureToRead.GetPixel(0, 0);
        //Debug.Log("R: " + color.r + " G: " + color.g + " B: " + color.b);
        color = textureToRead.GetPixel(15, 15);
        //Debug.Log("R: " + color.r + " G: " + color.g + " B: " + color.b);
        color = textureToRead.GetPixel(15, 0);
        //Debug.Log("R: " + color.r + " G: " + color.g + " B: " + color.b);
        color = textureToRead.GetPixel(0, 15);
        //Debug.Log("R: " + color.r + " G: " + color.g + " B: " + color.b);



    }

    public override void Interact()
    {
        Debug.Log("time series pressed");
        //ボタンが押されたときの時間を、基準時間とする。
        _pressed_time = DateTime.Now;
        //インスペクターで指定されたスライダーを、一時的にインタラクト不可にする。


        

        Color32 color;
        gameObjectsArray = new GameObject[textureToRead.width][];
        for (int i = 0; i < textureToRead.width; i++)
        {
            gameObjectsArray[i] = new GameObject[textureToRead.height];
            for (int j = 0; j < textureToRead.height; j++)
            {
                gameObjectsArray[i][j] = null;
            }
        }

        for (int x = 0; x < textureToRead.width; x++)
        {
            for (int y = 0; y < textureToRead.height; y++)
            {
                color = textureToRead.GetPixel(x, y);

                int index = System.Array.IndexOf(colorPairs, color);
                if (index >= 0)
                {
                    Debug.Log("Color found at index: " + index);
                    gameObjectsArray[x][y] = Instantiate(loadTargets[index], this.transform);
                    //GameObject clone = Instantiate(loadTargets[index], this.transform);
                    gameObjectsArray[x][y].transform.position = new Vector3(x, 0, y);
                }
            }
        }
        in_sequence = true;


    }


    void Update()
    {
        if (in_sequence) //pressedされてシーケンスに入っていたら実行する。
        {
            _updated_time = DateTime.Now;
            //基準時間から何秒かを計算する。https://itsakura.com/csharp-diffdate
            TimeSpan _dt = _updated_time - _pressed_time;

            float _dtsec = (float)_dt.TotalMilliseconds / 1000;

            Color32 color;

            for (int x = 0; x < textureToRead.width; x++)
            {
                for (int y = 0; y < textureToRead.height; y++)
                {
                    color = textureToRead.GetPixel(x, y);

                    int index = System.Array.IndexOf(colorPairs, color);
                    if (index >= 0)
                    {
                        Debug.Log("Color found at index: " + index);
                        //gameObjectsArray[x][y] = Instantiate(loadTargets[index], this.transform);
                        //GameObject clone = Instantiate(loadTargets[index], this.transform);
                        gameObjectsArray[x][y].transform.position = new Vector3(x, 0, y+speed* _dtsec);
                    }
                }
            }
        }
        else
        {

        }


    }
    void TestUnit()
    {
        this.Start();
        this.Interact();
        this.Update();
    }
}
