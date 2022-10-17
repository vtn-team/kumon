using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestUnity01
{
    // �e�X�g�{��
    [UnityTest]
    public IEnumerator MoveScriptCheck()
    {
        var player = GameObject.Find("Player");
        var goal = GameObject.Find("Goal");

        //�����Ȃ��̂ŃG���[
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

        //100�t���[�������ĂȂ�������R�[�h�������Ă��Ȃ��̂ŋp��
        int i = 0;
        for (; i < 100; ++i)
        {
            yield return null;

            //�����ł��������Ă��OK
            if ((pos - player.transform.position).magnitude > 1.0f) break;
        }

        //�����ĂȂ��̂ŃG���[
        if (i == 100)
        {
            Debug.LogError("Dont Move");
            Assert.That(false);
            yield return null;
        }

        float targetLength = (goal.transform.position - player.transform.position).magnitude;

        //�S�[���ւ̈ړ��I�������m����
        i = 0;
        while(true)
        {
            yield return null;

            if(goalCheck.IsHitToPlayer)
            {
                Debug.Log("Hit");
                break;
            }

            //�S�[�����牓�������Ă����NG
            if ((goal.transform.position - player.transform.position).magnitude > targetLength)
            {
                Debug.LogError("Wrong Move");
                Assert.That(false);
            }
        }

        pos = player.transform.position;

        //30�t���[���̊ԂɃS�[�����œ����Ă���R�[�h�������Ă��Ȃ��̂ŋp��
        i = 0;
        for (; i < 30; ++i)
        {
            yield return null;

            //�������Ă��NG
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

    // OneTimeSetUp�F�S�e�X�g�����s����O�Ɉ�x������������
    [OneTimeSetUp]
    public void InitializeTest()
    {
        sceneLoading = true;
        SceneManager.LoadSceneAsync("SampleScene").completed += _ => {
            sceneLoading = false;
            Debug.Log("Scene Load Complete");
        };
    }

    // Order�F�D��x���w�肵�čŏ��Ƀ��[�h�̊����҂����s��
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
