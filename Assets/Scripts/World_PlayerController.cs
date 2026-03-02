using UnityEngine;
using UnityEngine.InputSystem;


//プレイヤーがどちらに移動しているか
public enum Direction
{
    none,
    left,
    right
}


public class World_PlayerController : MonoBehaviour
{
    public float speed = 3.0f; //移動スピード
    Vector2 moveVec = Vector2.zero; //InputSystemからの入力値
    float angleZ; //プレイヤ－の向き　初期値を入力しない場合はゼロになる
    Rigidbody2D rbody;
    Animator animator;

    bool isActionButtonPressed; //ActionButtonが押されたらtrue
    public bool IsActionButtonPressed //同名の変数を大文字で書くことでプロパティ(パブリック化)として利用している
    {
        get { return isActionButtonPressed; }
        set { isActionButtonPressed = value; }
    }

    void OnActionButton(InputValue value)
    {
        IsActionButtonPressed = value.isPressed; // ボタンが押され続けている間はtrue
    }


    float GetAngle()
    {
        float angle = angleZ;
        if (moveVec != Vector2.zero)
        {
            float rad = Mathf.Atan2(moveVec.y, moveVec.x);
            angle = rad * Mathf.Rad2Deg;
        }
        return angle;
    }

    //その時のangleZの値に応じて右向きなのか、左向きなのかを判断している
    Direction AngleToDirection()
    {
        Direction dir;
        if (angleZ >= -89 && angleZ <= 89)
        {
            dir = Direction.right;
        }
        else if (angleZ >= 91 && angleZ <= 180 || angleZ >=-180 && angleZ <= -91 )
        {
            dir = Direction.left;
        }
        else
        {
            dir = Direction.none;
        }
        return dir;
    }

    void OnMove(InputValue value)
    {
        moveVec = value.Get<Vector2>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        angleZ = GetAngle();
        Direction dir = AngleToDirection();

        if (dir == Direction.right)
        {
            transform.localScale = new Vector2(1, 1); //絵はそのまま
        }
        else
        {
            transform.localScale = new Vector2(-1, 1); //絵の反転
        }

        if (moveVec != Vector2.zero)
        {
            animator.SetBool("Run", true); //Runパラメータをtrue
        }
        else
        {
            animator.SetBool("Run", false); //Runパラメータをfalse
        }

    }

    void FixedUpdate()
    {
        rbody.linearVelocity = moveVec * speed;
    }

}



