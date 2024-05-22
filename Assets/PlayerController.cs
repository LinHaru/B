using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 moveVec;//移動ベクトル
    [SerializeField] float speed;//移動速度

    Quaternion defaultCameraAngle;
    Vector3 defaultCameraDirection;
    int upDownCnt = 0;

    Vector2 playerAngle;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        controller = GetComponent<CharacterController>();
        moveVec = Vector3.zero;
        defaultCameraAngle = Camera.main.transform.rotation;
        defaultCameraDirection = Camera.main.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        //視点変更
        Angle();

        Vector3 globalDirection = Quaternion.Euler(0.0f, playerAngle.x, 0.0f) * moveVec;
        globalDirection *= Time.deltaTime;//1フレームごとの移動量に調整
        controller.Move(globalDirection);
    }

    void Move()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f)
        {
            Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            //斜め移動も同じ速度で移動
            inputVector = inputVector.normalized;
            moveVec.x = speed * inputVector.x;
            moveVec.z = speed * inputVector.z;

            //角度の設定
            float normalizedDir = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, playerAngle.x + normalizedDir, 0.0f);

            //歩行時の上下振動
        }
        else
        {
            //減速
            moveVec *= 0.9f;
        }

        //歩行時の上下振動
        float dir = moveVec.magnitude;
        Debug.Log(dir);
        upDownCnt += (int)(speed * dir);
        if(upDownCnt == 270) { Debug.Log("すこれよ！"); }
        upDownCnt %= 360;
        transform.Translate(0, Mathf.Sin(upDownCnt * Mathf.Deg2Rad) / 10.0f, 0);
    }

    void Angle()
    {
        //回転操作
        playerAngle.x += Input.GetAxis("Horizontal_R");
        playerAngle.y -= Input.GetAxis("Vertical_R");
        Camera.main.transform.rotation = Quaternion.Euler(playerAngle.y, playerAngle.x, 0.0f) * defaultCameraAngle;

        //カメラの上下を制限
        playerAngle.y = Mathf.Clamp(playerAngle.y, -10.0f, 45.0f);

        //カメラを回転
        Camera.main.transform.rotation = Quaternion.Euler(playerAngle.y, playerAngle.x, 0.0f) * defaultCameraAngle;
        Camera.main.transform.position = transform.position + Quaternion.Euler(playerAngle.y, playerAngle.x, 0.0f) * defaultCameraDirection;
    }
}
