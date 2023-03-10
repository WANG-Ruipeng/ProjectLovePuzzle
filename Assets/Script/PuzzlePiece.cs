using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public AnimationCurve RotateCurve;
    public bool RotateClockWise;
    public GameObject LockIndicator;

    bool isRotating = false;
    bool isLocked = false;
    float rotateTimeLength;
    float rotateStartTime;
    float rotateStartAngle;

    SpriteRenderer indicatorSprite;

    // Start is called before the first frame update
    void Start()
    {
        rotateTimeLength = RotateCurve.keys[RotateCurve.length - 1].time;
        RotateClockWise = false;
        indicatorSprite = LockIndicator.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocked)
        {
            if (!isRotating)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    isRotating = true;
                    rotateStartTime = Time.time;
                    rotateStartAngle = transform.rotation.eulerAngles.z;
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    isLocked = true;
                    Color c = indicatorSprite.color;
                    c.a = 255;
                    indicatorSprite.color = c;
                }
            }

            if (isRotating)
            {
                float progress = (Time.time - rotateStartTime);
                if (progress >= rotateTimeLength)
                    isRotating = false;

                float rotateZ = Mathf.Clamp(RotateCurve.Evaluate(progress), 0, 1) * 90;
                //本来想写个归一化的但是太懒了，记得提醒策划拉曲线的时候在0~1之间的范围拉
                Vector3 rot = transform.rotation.eulerAngles;
                if (RotateClockWise)
                {
                    rot.z = (rotateStartAngle - rotateZ) % 360;
                }
                else
                {
                    rot.z = (rotateStartAngle + rotateZ) % 360;
                }
                Quaternion rotQuat = Quaternion.Euler(rot.x, rot.y, rot.z);
                transform.rotation = rotQuat;
            }
        }
        
    }
}
