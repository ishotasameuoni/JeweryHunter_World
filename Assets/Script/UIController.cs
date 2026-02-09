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

    //ヒエラルキーに存在するものは基本GameObject型(何かしらの機能を持つ物）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f);  // 1秒後に画像を非表示にする
        panel.SetActive(false);         // パネルを非表示にする
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
        }
        else if(GameManager.gameState == GameState.GameOver)
        {
            //ゲームオーバー
            mainImage.SetActive(true) ;
            panel.SetActive(true);
            //Nextボタンを無効化する      
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
        }
        else if (GameManager.gameState == GameState.InGame)
        {
        }
    }
}
