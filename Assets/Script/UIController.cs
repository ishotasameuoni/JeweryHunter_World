
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject mainImage;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject panel;
    public GameObject restartButton;
    public GameObject nextButton;

    //時間制限追加
    public GameObject timeBar;
    public GameObject timeText;
    TimeController timeController;
    bool useTime = true;

    //プレイヤー情報
    GameObject player;
    PlayerController playerController;

    // スコア追加
    public GameObject scoreText;        // スコアテキスト
    public int stageScore = 0;          // ステージスコア

    //ヒエラルキーに存在するものは基本GameObject型(何かしらの機能を持つ物）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f);  // 1秒後に画像を非表示にする
        panel.SetActive(false);         // パネルを非表示にする

        //時間制限のプログラム
        timeController = GetComponent<TimeController>();   // TimeControllerを取得
        if (timeController != null)
        {
            if (timeController.gameTime == 0.0f) //もしgameTimeがもともと0なら時間制限は設けない
            {
                timeBar.SetActive(false);   // 制限時間なしなら隠す
                useTime = false;　//時間制限を使わないフラグ
            }
        }



        //プレイヤー情報とPlayerControllerコンポーネントの取得
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        //スコア追加
        UpdateScore();

    }

    void InactiveImage() //引数で参照する為にメソッド化をしている
    {
        mainImage.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.GameClear)
        {
            // ゲームクリア
            mainImage.SetActive(true);  // 画像を表示する
            panel.SetActive(true);      // ボタン（パネル）を表示する
                                        // RESTARTボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;  // 画像を設定する

            // 時間カウントを停止
            if (timeController != null)
            {
                timeController.IsTimeOver(); //停止フラグ

                // 整数に型変換することで小数を切り捨てる
                int time = (int)timeController.GetDisplayTime();
                GameManager.totalScore += time * 10; // 残り時間をスコアに加える
            }

            GameManager.totalScore += stageScore; //トータルスコアの最終確定
            stageScore = 0; //ステージスコアリセット

            UpdateScore();  //スコア表示の更新

        }
        else if (GameManager.gameState == GameState.GameOver)
        {
            //ゲームオーバー
            mainImage.SetActive(true);
            panel.SetActive(true);
            //Nextボタンを無効化する      
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;

            // 時間カウントを停止
            if (timeController != null)
            {
                timeController.IsTimeOver(); //停止フラグ
            }
        }
        else if (GameManager.gameState == GameState.InGame) // ゲーム中
        {
            if (player == null) { return; } //プレイヤー消滅後は何もしない

            //タイムを更新する
            if (timeController != null && useTime)
            {
                // float型のUI用表示変数を取得し、整数に型変換することで小数を切り捨てる
                int time = (int)timeController.GetDisplayTime();
                timeText.GetComponent<TextMeshProUGUI>().text = time.ToString();

                Debug.Log(time);

                if (useTime && timeController.isCountDown && time <= 0) //カウントダウンモードで時間が0なら
                {
                    playerController.GameOver(); // ゲームオーバーにする
                }
                else if (useTime && !timeController.isCountDown && time >= timeController.gameTime) //カウントアップモードで制限時間を超えたら
                {
                    playerController.GameOver(); // ゲームオーバーにする
                }

            }
        }
    }

    // 現在スコアのUI表示更新
    void UpdateScore()
    {
        int currentScore = stageScore + GameManager.totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = currentScore.ToString();
    }

    // プレイヤーから呼び出される 獲得スコアを追加した上でのUI表示更新
    public void UpdateScore(int score)
    {
        stageScore += score;
        int currentScore = stageScore + GameManager.totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = currentScore.ToString();
    }
}