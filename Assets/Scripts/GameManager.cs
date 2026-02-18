
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState //ゲームの状態
{
    InGame,　//ゲーム中
    GameClear, //ゲームクリア
    GameOver, //ゲームオーバー
    GameEnd, //ゲーム終了

    //enum...を最初に宣言することで自作した型を作ることができる。　stringやintとは違い何にでも使えるenum

}

public class GameManager : MonoBehaviour
{
    //ゲームの状態
    public static GameState gameState;
    public string nextSceneName;

    // スコア追加
    public static int totalScore; //合計スコア

    //サウンド関連
    public AudioClip meGameClear;
    public AudioClip meGameOver;
    AudioSource soundPlayer;

    //GameManagerの変数に用意
    public bool isGameClear = false; //ゲームクリア判定
    public bool isGameOver = false; //ゲームオーバー判定

    //ワールドマップで最後に入ったエントランスのドア番号
    public static int currentDoorNumber = 0;

    //所持アイテム　鍵番号
    public static int keys = 1;

    //どのステージの鍵が入手済みかを管理
    public static Dictionary<string, bool> keyGot; //シーン名、True/False

    //所持アイテム　矢の管理
    public static int arrows = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.InGame;　//ステータスをゲーム中にする 独自の型を使う時は右のようにしなくてはならない。
        soundPlayer = GetComponent<AudioSource>(); //使用するコンポーネントの取得

        //KeyGotが何もない状態だった時のみ初期化
        if (keyGot == null)
        {
            keyGot = new Dictionary<string, bool>();
        }

        //もしも現シーン名がDictionary(keyGot)に登録されていなければ
        if (!(keyGot.ContainsKey(SceneManager.GetActiveScene().name)))
        {
            //Dictionary(keyGot)に登録しておく(現シーン名、鍵の取得情報false)
            keyGot.Add(SceneManager.GetActiveScene().name, false);
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameState == GameState.GameClear)
        {
            soundPlayer.Stop();
            soundPlayer.PlayOneShot(meGameClear);
            isGameClear = true;
            gameState = GameState.GameEnd;
        }
        else if (gameState == GameState.GameOver)
        {
            soundPlayer.Stop();
            soundPlayer.PlayOneShot(meGameOver);
            isGameClear = true;
            gameState = GameState.GameEnd;
        }
    }

    //リスタート
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    //PlayerController経由で「UIマップのSubmit」が押されたら
    public void GameEnd()
    {
        //ＵＩ表示が終わって最後の状態であれば
        if (gameState == GameState.GameEnd)
        {
            if (isGameClear)
            {
                Next();
            }
            else if (isGameOver)
            {
                Restart();
            }
        }

    }
}
