using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 3.0f;
    public float jump = 9.0f;
    public LayerMask groundLayer; //レイヤー指定
    bool goJump = false; //true、false
    bool onGround = false; //true、false

    // アニメーション対応
    Animator animator; // アニメーター

    //値はあくまでアニメーションクリップ名
    public string stopAnime = "Idle";
    public string moveAnime = "Run";
    public string jumpAnime = "Jump";
    public string goalAnime = "Goal";
    public string deadAnime = "Dead";
    string nowAnime = "";
    string oldAnime = "";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();        // Animator を取ってくる
        nowAnime = stopAnime;                       // 停止から開始する
        oldAnime = stopAnime;                       // 停止から開始する
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.InGame)
        {
            return;　//Updateを中断
        }

        //Groundの上にいるか
        onGround = Physics2D.CircleCast(
            transform.position,//発射位置
            0.2f,//円の半径
            Vector2.down,//発射方向
            0.0f,//距離
            groundLayer//対象レイヤー                       
            );

        //ジャンプキーが押されたか
        if (Input.GetButtonDown("Jump"))
        {
            goJump = true;
        }


        axisH = Input.GetAxisRaw("Horizontal");

        if (axisH > 0.0f)                           // 向きの調整
        {
            //Debug.Log("右おされてる");
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            //Debug.Log("左おされてる");
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }

        // アニメーション更新
        if (onGround)       // 地面の上
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime; // 停止中
            }
            else
            {
                nowAnime = moveAnime; // 移動
            }
        }
        else                // 空中
        {
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // アニメーション再生
        }

    }

    private void FixedUpdate()
    {
        if (GameManager.gameState != GameState.InGame)
        {
            return;　//Updateを中断
        }

        if (onGround || axisH != 0)
        {
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)
        {
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }
    }

    //接触
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal(); //ゴール!
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver(); //ゲームオーバー
        }
    }

    //ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        GameManager.gameState = GameState.GameClear; //ステータス切り替え
        GameStop(); //プレイヤーのVelocityをストップ
    }

    //ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        GameManager.gameState = GameState.GameOver;
        GameStop();             //プレイヤーのVelocityをストップ

        // ゲームオーバー演出
        GetComponent<CapsuleCollider2D>().enabled = false;      // 当たりを消す
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 上に少し跳ね上げる

        Destroy(gameObject, 2.0f); // 2秒後にヒエラルキーからオブジェクトを抹消

    }

    //プレイヤー停止
    void GameStop()
    {
        rbody.linearVelocity = new Vector2(0, 0);
    }

}