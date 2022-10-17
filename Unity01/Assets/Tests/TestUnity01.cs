using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestUnity01
{
    // テスト本体
    [UnityTest]
    public IEnumerator MoveScriptCheck()
    {
        var player = GameObject.Find("Player");
        var goal = GameObject.Find("Goal");

        //動かないのでエラー
        if (player == null)
        {
            Debug.LogError("Player is Null");
            Assert.Null(player);
            yield return null;
        }

        if(goal == null)
        {
            Debug.LogError("Goal is Null");
            Assert.Null(goal);
            yield return null;
        }

        Debug.Log("Test Start");

        var goalCheck = goal.AddComponent<GoalCheck>();
        Vector3 pos = player.transform.position;

        //100フレーム動いてなかったらコードが書けていないので却下
        int i = 0;
        for (; i < 100; ++i)
        {
            yield return null;

            //少しでもうごいてればOK
            if ((pos - player.transform.position).magnitude > 1.0f) break;
        }

        //動いてないのでエラー
        if (i == 100)
        {
            Debug.LogError("Dont Move");
            Assert.That(false);
            yield return null;
        }

        float targetLength = (goal.transform.position - player.transform.position).magnitude;

        //ゴールへの移動終了を検知する
        i = 0;
        while(true)
        {
            yield return null;

            if(goalCheck.IsHitToPlayer)
            {
                Debug.Log("Hit");
                break;
            }

            //ゴールから遠ざかっていればNG
            if ((goal.transform.position - player.transform.position).magnitude > targetLength)
            {
                Debug.LogError("Wrong Move");
                Assert.That(false);
            }
        }

        pos = player.transform.position;

        //30フレームの間にゴール内で動いてたらコードが書けていないので却下
        i = 0;
        for (; i < 30; ++i)
        {
            yield return null;

            //うごいてればNG
            if ((pos - player.transform.position).magnitude > 5.0f) break;
        }

        if (i == 30)
        {
            Assert.That(i == 30);
        }
        else
        {
            Debug.LogError("Overrun");
            Assert.That(false);
        }

        Debug.Log("Test End");
        yield return null;
    }

    bool sceneLoading;

    // OneTimeSetUp：全テストを実行する前に一度だけ処理する
    [OneTimeSetUp]
    public void InitializeTest()
    {
        sceneLoading = true;
        SceneManager.LoadSceneAsync("SampleScene").completed += _ => {
            sceneLoading = false;
            Debug.Log("Scene Load Complete");
        };
    }

    // Order：優先度を指定して最初にロードの完了待ちを行う
    [UnityTest]
    [Order(-100)]
    public IEnumerator LoadWait()
    {
        yield return new WaitWhile(() => sceneLoading);
    }


    public class GoalCheck : MonoBehaviour
    {
        public bool IsHitToPlayer { get; private set; }
        void CollisionTest(GameObject obj)
        {
            if(obj.name == "Player")
            {
                IsHitToPlayer = true;
            }
        }

        private void Start()
        {
            IsHitToPlayer = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            CollisionTest(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            CollisionTest(other.gameObject);
        }
    }
}
