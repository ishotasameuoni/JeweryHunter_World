using UnityEngine;

public class BossController : MonoBehaviour
{
    public int hp = 10;
    public float reactionDistance = 10.0f;
    public GameObject bulletPrefab;
    public float shootSpeed = 5.0f;
    public float bossSpeed = 3.0f;
    Animator animator;
    GameObject player;

    public GameObject gate;

    bool inDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //ダメージ中は点滅処理　※returnはない為、行動はキャンセルされず継続する
        if (inDamage)
        {
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //体力が残っている場合
        if (hp > 0)
        {
            if (player != null)
            {
                Vector2 playerPos = player.transform.position;
                float dist = Vector2.Distance(transform.position, playerPos); //PlayerとBossの距離の差
                animator.SetBool("InAttack", dist <= reactionDistance); //第二引数で基準の距離に入っているか
            }
            else
            {
                animator.SetBool("InAttack", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inDamage)
        {
            if (collision.gameObject.tag == "Arrow")　//ぶつかった相手がArrowだったら
            {
                //ぶつかった相手のゲームオブジェクトが持っているArrowControllerスクリプトを取得
                Arrow_Controller arrow = collision.gameObject.GetComponent<Arrow_Controller>();
                hp -= arrow.attackPower; //体力を減少
                inDamage = true;
                Invoke("DamageEnd", 0.25f); //0.25秒後にダメージフラグ解除
                
                //体力がなくなった時
                if (hp <= 0)
                {
                    //BossColliderを2つ持っているので、ある分だけの情報を取得する
                    CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
                    colliders[0].enabled = false;
                    colliders[1].enabled = false;

                    animator.SetTrigger("IsDead");
                    Invoke("BossSpriteOff", 1.0f);
                }
            }
        }
    }

    //反復移動
    private void FixedUpdate()
    {
        float val = Mathf.Sin(Time.time); //Sin係数を用いることで＋と－を行き来する波形を用いることで反復する作用を作る

        transform.position -= new Vector3(val * bossSpeed, 0, 0);  //TransformのX軸のみに係数とbossSpeedを掛けることで移動範囲を限定している
    }

    //ダメージ中フラグをOFFにして明確に描画をさせるメソッド
    void DamageEnd()
    {
        inDamage = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void Attack()
    {
        if (player != null)
        {
            //PlayerとGateのXY座標の差
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            //X（底辺)とY（高さ)を使ったラジアン係数で円周率を求める
            float rad = Mathf.Atan2(dy, dx);
            //オイラー角に変換する
            float angle = rad * Mathf.Rad2Deg;

            //生成されるプレハブの角度を予め変数rに計算
            Quaternion r = Quaternion.Euler(0, 0,angle);
            //Gateオブジェクトの位置にBulletの生成と情報の取得
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);

            //底辺と高さの差の情報は持っているが、長辺を１とした時の割合で底辺と高さを取得
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }

    void BossSpriteOff()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Invoke("BossDestroy", 3.0f);
    }

    void BossDestroy()
    {
        player.GetComponent<PlayerController>().Goal();
        Destroy(gameObject);
    }

    //オブジェクトを選択した際に攻撃範囲をGizmosとして表示している(ゲーム中はみえない)
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, reactionDistance);
    }


}
