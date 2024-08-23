using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    const int DefaultCandyAmount = 30;  //変更不可のためconstにしておく
    const int RecoverySeconds = 10; //変更不可のためconstにしておく

    // 現在のキャンディのストック数
    public int candy = DefaultCandyAmount;
    // ストック回復までの残り秒数 
    int counter;

    public void ConsumeCandy()  //消費
    {
        if (candy > 0) candy--;
    }

    public int GetCandyAmount() //追加
    {
        return candy;
    }

    public void AddCandy(int amount)   //残数取得
    {
        candy += amount;
    }

    void OnGUI()//Update（フレーム毎）よりライトな更新
    {
        GUI.color = Color.black;    //簡易UIの文字色黒

        // キャンディのストック数を表示
        string label = "Candy : " + candy;

        // 回復カウントしている時だけ秒数を表示
        if (counter > 0) label = label + " (" + counter + "s)";

        GUI.Label(new Rect(50, 50, 100, 30), label);//上からの距離、左からの距離、幅、高さ
    }

    void Update()
    {
        // キャンディのストックがデフォルトより少なく、
        // 回復カウントをしていないときにカウントをスタートさせる
        if (candy < DefaultCandyAmount && counter <= 0)
        {
            StartCoroutine(RecoverCandy()); //30を下回ったらコルーチンの発動
        }
    }

    IEnumerator RecoverCandy()
    {
        counter = RecoverySeconds;

        // 1秒ずつカウントを進める
        while (counter > 0)
        {
            yield return new WaitForSeconds(1.0f);
            counter--;
        }

        candy++;
    }
}