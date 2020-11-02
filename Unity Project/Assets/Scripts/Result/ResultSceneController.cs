using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*決定ボタンを押された時に呼び出される*/
        if (Input.GetButtonDown("Submit"))
        {
            /*シーン遷移コルーチンを呼び出す*/
            StartCoroutine(ChangeScene());
        }
    }

    /*シーン遷移コルーチン*/
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1);
        SceneChanger.LoadSelectingScene(SceneName.SelectStageScene);
    }
}
