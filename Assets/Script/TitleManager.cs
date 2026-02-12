using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string sceneName; //スタートボタンを押して読み込むシーン名

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //シーンを読み込むメソッド作成
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}