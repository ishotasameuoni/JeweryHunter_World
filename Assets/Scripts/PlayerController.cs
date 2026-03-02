
using UnityEngine;
using UnityEngine.InputSystem;

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

    public int score = 0; //スコア

    InputAction moveAction; //MoveAction
    InputAction jumpAction; //Jump
    PlayerInput input; //PlayerInputコンポーネント

    GameManager gm; //GameManagerスクリプト

    public static int playerLife = 10;  //Playerの体力 (staticした変数はコンポーネントにアタッチしても表示はされない)

    bool inDamage; //ダメージ管理フラグ

    //弓矢の機構
    public float shootSpeed = 12.0f; //矢の速度
    public float shootDelay = 0.25f; //発射間隔
    public GameObject arrowPrefab; //矢のプレハブ
    public GameObject gate; //矢の発射位置

    static public void PlayerRecovery(int life) //Playerの体力回復メソッド
    {
        playerLife += life; //引数life分だけ回復
        if (playerLife > 10) playerLife = 10; //一行で収められるなら{}は必要なくても成立する
    }
    void OnMove(InputValue value)
    {
        //取得した情報をVector2形式で抽出
        Vector2 moveInput = value.Get<Vector2>();
        axisH = moveInput.x; //そのⅹ成分をaxisHに代入
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            goJump = true;
        }
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.arrows > 0)
        {
            ShootArrow();
        }
    }

    //矢を放つメソッド
    void ShootArrow()
    {
        GameManager.arrows--;
        Quaternion r;

        if (transform.localScale.x > 0)
        {
            r = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            r = Quaternion.Euler(0, 0, 180);
        }
        //Gateオブジェクトの位置にrの回転で矢を生成
        GameObject arrowObj = Instantiate(
            arrowPrefab, gate.transform.position, r);
        //生成した矢のRigidbody2Dを取得
        Rigidbody2D arrowRbody = arrowObj.GetComponent<Rigidbody2D>();
        //Playerの絵の向きに合わせた方向に矢を放つ
        arrowRbody.AddForce(new Vector2(transform.localScale.x, 0) * shootSpeed, ForceMode2D.Impulse);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();        // Animator を取ってくる
        nowAnime = stopAnime;                       // 停止から開始する
        oldAnime = stopAnime;                       // 停止から開始する

        input = GetComponent<PlayerInput>();
        //アクションやマップの取得
        moveAction = input.currentActionMap.FindAction("Move");
        jumpAction = input.currentActionMap.FindAction("jump");
        InputActionMap uiMap = input.actions.FindActionMap("UI");
        uiMap.Disable();

        //GameObject型のアタッチされている特定のコンポーネントを探してくるメソッド
        gm = GameObject.FindFirstObjectByType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.InGame || inDamage)
        {
            if (inDamage)
            {
                //Sin関数の角度に経過時間（一定リズム)を与えると、等間隔でプラスとマイナスの結果が得られる (通常だと遅いので50倍にすることで早くちらつかせる)
                float val = Mathf.Sin(Time.time * 50);

                //ダメージ点滅処理
                if (val > 0)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            return; //Updateを中断
        }

        //Groundの上にいるか
        onGround = Physics2D.CircleCast(
            transform.position,//発射位置
            0.2f,//円の半径
            Vector2.down,//発射方向
            0.0f,//距離
            groundLayer//対象レイヤー                       
            );

        ////ジャンプキーが押されたか
        //if (Input.GetButtonDown("Jump"))
        //{
        //    goJump = true;
        //}

        //if (jumpAction.WasPressedThisFrame())
        //{
        //    goJump = true;
        //}



        //if(moveAction.WasPressedThisFrame())
        //{
        //    moveAction = true;
        //}


        //axisH = Input.GetAxisRaw("Horizontal");

        //左右に関するキー入力をaxisHに代入
        //InputActionのPlayerマップの"Move"アクションに登録されたボタンをVector2形式で読み取り、そのうちのX成分を反映
        //axisH = moveAction.ReadValue<Vector2>(),x;

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
        //INゲームではなく、ダメージフラグがTrueの時(GameState.Ingame || inDamage == trueの略,falseの際は!inDamageと書く)
        if (GameManager.gameState != GameState.InGame || inDamage)
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
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // スコアアイテム
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();  // ScoreItemを得る			
            score = item.itemdata.value;                // スコアを得る
            UIController ui = Object.FindFirstObjectByType<UIController>();      // UIControllerを探す
            if (ui != null)
            {
                ui.UpdateScore(score);                  // スコア表示を更新する
            }
            score = 0; //次に備えてスコアをリセット
            Destroy(collision.gameObject);              // アイテム削除する
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (!inDamage) //ダメージ中でなければ
            {
                //ぶつかった相手のオブジェクト情報を引数
                GetDamage(collision.gameObject);
            }
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

        input.currentActionMap.Disable();
        input.SwitchCurrentActionMap("UI");
        input.currentActionMap.Enable();
    }

    //UI表示時にSubmitボタンが押されたら
    void OnSubmit(InputValue value)
    {
        if (GameManager.gameState != GameState.InGame)
        {
            //GameManagerスクリプトのGameEndメソッドを発動
            gm.GameEnd();
        }
    }


    //プレイヤーのaxisH()の値を取得
    public float GetAxisH()
    {
        return axisH;
    }

    void GetDamage(GameObject target)
    {
        if (GameManager.gameState == GameState.InGame)
        {
            playerLife -= 1;
            if (playerLife > 0)
            {
                rbody.linearVelocity = new Vector2(0, 0);
                Vector3 v = (transform.position - target.transform.position).normalized; //ノックバック用の具体的な数値(均一化することで力を一定にする)
                rbody.AddForce(new Vector2(v.x * 4, v.y * 4), ForceMode2D.Impulse); //ノックバックの処理
                inDamage = true;
                Invoke("DamageEnd", 0.25f); //発動させる時間を表現する方法がないためInvokeを用いている
            }
            else
            {
                GameOver();
            }
        }
    }

    void DamageEnd() //ダメージ管理フラグをオフにするメソッド
    {
        inDamage = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }

}