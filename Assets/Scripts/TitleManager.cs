using UnityEngine;
using UnityEngine.SceneManagement; //シーン切り替えに利用
using UnityEngine.InputSystem; //InputSystemの利用

public class TitleManager : MonoBehaviour
{
    public string sceneName; //スタートボタンを押して読み込むシーン名
    public InputAction submitAction; //InputAction(ボタン入力の判別)を利用、ただし以下の様な雛形が必要   

    //void OnEnable()
    //{
    //    submitAction.Enable();
    //}

    //void OnDisable()
    //{
    //    submitAction.Disable();
    //}

    //InputSystemActionで決めたUIマップのSubmitアクションが押されたとき
    void OnSubmit(InputValue　value)
    {
        Load();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ////列挙型のKeyboard型の値を変数kbに代入
        //Keyboard kb = Keyboard.current;
        //if (kb != null)　//キーボードデバイスがあれば(繋がっていない事が前提)
        //{
        //    if (kb.enterKey.wasPressedThisFrame) //エンターキーが押されていれば
        //    {
        //        Load();
        //    }
        //}

        //if (submitAction.WasPressedThisFrame() )
        //{
        //    Load();
        //}
    }

    //シーンを読み込むメソッド作成
    public void Load()
    {
        GameManager.totalScore = 0; //新しくゲームを始めるにあたってスコアリセットを行う
        SceneManager.LoadScene(sceneName);
    }
}