using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*全シーンをenumで管理*/
public enum SceneName
{
    TitleScene, SelectStageScene, GameScene, ResultScene
}

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*シーン遷移を処理する関数*/
    public static void LoadSelectingScene(SceneName scenename)
    {
        SceneManager.LoadScene((int)scenename);
    }
}
