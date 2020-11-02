using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*決定ボタンが入力された際呼び出される*/
        if(Input.GetButtonDown("Submit"))
        {
            /*次のシーンへ遷移するコルーチンを呼び出す*/
            StartCoroutine(ChangeScene());
        }
    }

    /*次のシーンへ遷移するコルーチン*/
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1);
        SceneChanger.LoadSelectingScene(SceneName.SelectStageScene);
    }
}
