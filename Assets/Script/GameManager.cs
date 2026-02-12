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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.InGame;　//ステータスをゲーム中にする 独自の型を使う時は右のようにしなくてはならない。

    }

    // Update is called once per frame
    void Update()
    {

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
}
    