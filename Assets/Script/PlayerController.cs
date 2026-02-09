
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; //Rigidbody2D型の変数
    float axisH = 0.0f; //入力
    public float speed = 3.0f;
    public float jump = 9.0f;
    public LayerMask groundLayer; //レイヤーを指定することで判定のチェックを行う前準備
    bool goJump = false; //ジャンプ中の判定に使う変数
    bool onGround = false; //地面にいる判定に使う変数

    Animator animator; //アニメーション対応

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
        //Rigitbody2Dを取ってくる
        rbody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = stopAnime; //停止から開始する
        oldAnime = stopAnime; //停止から開始する
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.InGame)
        {
            return; //Updateを中断 この条件に当てはまるのなら全ての行動がこの宣言が全てにおいて有線されるので入力が遮断させられる
        }


        //地上判定
        onGround = Physics2D.CircleCast(transform.position, 0.2f, Vector2.down, 0.0f, groundLayer);

        if (Input.GetButtonDown("Jump"))
        //ジャンプをさせる //Input.Get~~　Key,Button,Axisのどれかがある
        {
            goJump = true; //ジャンプフラグを立てる
        }

        //水平方向の入力をチェックする

        axisH = Input.GetAxisRaw("Horizontal");

        if (axisH > 0.0f) //向きの条件
        {
            //Debug.Log("右入力");
            transform.localScale = new Vector2(1, 1); //右向き
        }
        else if (axisH < 0.0f)
        {
            //Debug.Log("左入力");
            transform.localScale = new Vector2(-1, 1); //左向き
        }

        if (onGround)
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime; //停止中
            }
            else
            {
                nowAnime = moveAnime; //移動
            }
        }
        else
        {
            nowAnime = jumpAnime; //空中
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); //アニメーション再生
        }


    }

    void FixedUpdate()
    {
        //プレイヤーの停止
        void GameStop()
        {
            rbody.linearVelocity = new Vector2(0, 0);
        }

        if (onGround || axisH != 0)
        {
            //速度を更新する
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
        GameManager.gameState = GameState.GameClear; //ゲームステータスをクリアにする
        GameStop(); //プレイヤーのVelocityをストップ
    }

    //ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        GameManager.gameState = GameState.GameOver; //ゲームステータスをクリアにする
        GameStop(); //プレイヤーのVelocityをストップ

        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        Destroy(gameObject, 2.0f); //2秒後にヒエラルキーからオブジェクトを消去する gameObjectはthis.gameObjectの略症でこのオブジェクトをという意味

    }

    //プレイヤーの停止
    void GameStop()
    {
        rbody.linearVelocity = new Vector2(0, 0);
    }

}
