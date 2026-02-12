
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true; //true =　時間のカウントダウン計測
    public float gameTime = 0; //ゲームの最大時間
    bool isTimeOver = false; //true = タイマー停止
    float displayTime = 0; //UI用
    float times = 0; //経過時間

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isCountDown) //カウントダウン
        {
            displayTime = gameTime;　//UI表示を最大時間からにする
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeOver == false)
        {
            times += Time.deltaTime; //経過時間の算出

            if (isCountDown) //カウントダウン
            {
                displayTime = gameTime - times + 1; //最大値(gameTime)からtime変数で引いた数値 ＋1は誤差を有余時間
                if (displayTime <= 0.0f) //時間切れの場合
                {
                    displayTime = 0.0f; //UI用の表示を0に整える
                    IsTimeOver(); //停止フラグを立てる
                }
            }
            else //カウントアップ
            {
                displayTime = times - 1; //経過時間続ける(time)を表示　time数値-1は誤差を有余時間
                if (displayTime >= gameTime) //最大値(gametime)を超過した場合
                {
                    displayTime = gameTime; //最大値に到達を告げる
                    IsTimeOver(); //停止フラグを立てる
                }
            }
            //Debug.Log("TIMES:" + displayTime);
        }
    }

    //停止フラグを建てるメソッド　UIControllerから呼び出せる
    public void IsTimeOver()
    {
        isTimeOver = true;
    }

    //UI用の表示時間を取得するメソッド
    public float GetDisplayTime()
    {
        return displayTime;
    }
}
