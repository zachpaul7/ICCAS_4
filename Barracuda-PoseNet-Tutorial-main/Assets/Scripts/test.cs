using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public PoseEstimator pe;
    Vector2 leftEyePosition;
    Vector2 rightEyePosition;
    Vector2 leftEarPosition;
    Vector2 rightEarPosition;
    Vector2 leftShoulderPosition;
    Vector2 rightShoulderPosition;
    Vector2 leftHipPosition;
    Vector2 rightHipPosition;
    Vector2 leftKneePosition;
    Vector2 leftAnklePosition;

    public void Init()
    {
        leftEyePosition = pe.keypoints[1].position;
        rightEyePosition = pe.keypoints[2].position;
        leftEarPosition = pe.keypoints[3].position;
        rightEarPosition = pe.keypoints[4].position;
        leftShoulderPosition = pe.keypoints[5].position;
        rightShoulderPosition = pe.keypoints[6].position;
        leftHipPosition = pe.keypoints[11].position;
        rightHipPosition = pe.keypoints[12].position;
        leftKneePosition = pe.keypoints[13].position;
        leftAnklePosition = pe.keypoints[15].position;

    }



    public void Start()
    {
        StartCoroutine(StretchingCoroutine2());
    }

    // Start is called before the first frame update
    IEnumerator StretchingCoroutine0() // 고개 숙이기 스트레칭
    {
        int timeCount = 0;
        int successCount = 0;

        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("얼굴을 보여주세요");

        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기

        float originalleftEyePosition = leftEyePosition.y;

        Debug.Log(originalleftEyePosition);

        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 3)
        {
            Debug.Log("고개를 숙여주세요");
            yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기
            float currentleftEyePosition = leftEyePosition.y; // 현재 눈 위치 확인
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기


            // 원래 자세로 복귀
            if (currentleftEyePosition < originalleftEyePosition + 5)
            {
                Debug.Log("자세가 올바르지 못했어요! 고개를 충분히 숙이지 않았습니다.");
            }
            else
            {
                Debug.Log("잘했어요!");
                successCount++;
            }
            timeCount++;
            Debug.Log("고개를 들어주세요");
            yield return new WaitForSeconds(5f);

        }

        if (successCount >= 2)
        {
            Debug.Log("잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }

    }

    IEnumerator StretchingCoroutine1() // 옆목 스트레칭
    {

        int timeCount = 0;
        int successCount = 0;
        int lefttimeCount = 0;
        int righttimeCount = 0;
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기

        float originalleftEarPosition = leftEarPosition.y;
        float originalrightEarPosition = rightEarPosition.y;


        Debug.Log("스트레칭을 시작해볼까요?");
        Debug.Log("동작을 왼쪽부터 수행합니다");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 6)
        {
            if (lefttimeCount == righttimeCount)
            {
                Debug.Log("목을 왼쪽으로 늘려주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기
                float currentleftEarPosition = leftEarPosition.y; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentleftEarPosition <= originalleftEarPosition + 5)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                lefttimeCount++;
                Debug.Log("고개를 바로 해주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
            else
            {
                Debug.Log("목을 오른쪽으로 늘려주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기
                float currentrightEarPosition = rightEarPosition.y;
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentrightEarPosition <= originalrightEarPosition + 5)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                righttimeCount++;
                Debug.Log("고개를 바로 해주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
        }

        if (successCount >= 5)
        {
            Debug.Log("잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }
    }
    IEnumerator StretchingCoroutine2() // 사이드 스트레칭
    {
        int timeCount = 0;
        int successCount = 0;
        int lefttimeCount = 0;
        int righttimeCount = 0;
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("정면을 보고 전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기

        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("왼쪽동작부터 수행합니다");

        float originalleftEarPosition = leftEarPosition.y;
        float originalrightEarPosition = rightEarPosition.y;

        while (timeCount < 4)
        {
            if (lefttimeCount == righttimeCount)
            {
                Debug.Log("허리를 왼쪽으로 굽혀주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기
                float currentleftEarPosition = leftEarPosition.y; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentleftEarPosition <= originalleftEarPosition)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                lefttimeCount++;
                Debug.Log("제자리로 돌아오세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
            else
            {
                Debug.Log("허리를 오른쪽으로 굽혀주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기
                float currentrightEarPosition = rightEyePosition.y;
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentrightEarPosition <= originalrightEarPosition)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                righttimeCount++;
                Debug.Log("시작자세로 돌아와주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
        }

        if (successCount >= 5)
        {
            Debug.Log("잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }
    }

    IEnumerator StretchingCoroutine3() // 서서 허리 젖히기
    {
        int timeCount = 0;
        int successCount = 0;
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("우측을 바라보시고 전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 5)
        {
            Debug.Log("몸을 뒤로 젖혀주세요");
            yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기
            float currentAngle = CalculateAngle(leftShoulderPosition,
                                                leftHipPosition,
                                                leftAnklePosition);
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기


            // 원래 자세로 복귀
            if (currentAngle > 160)
            {
                Debug.Log("자세가 올바르지 못했어요!");
            }
            else
            {
                Debug.Log("잘했어요!");
                successCount++;
            }
            timeCount++;
            Debug.Log("시작자세로 돌아와주세요");
            yield return new WaitForSeconds(5f);

        }

        if (successCount >= 4)
        {
            Debug.Log("잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }
    }

    IEnumerator StretchingCoroutine4() // 상체내리기
    {
        int timeCount = 0;
        int successCount = 0;
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("우측을 바라보시고 전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 3)
        {
            Debug.Log("상체를 내려주세요");
            yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기
            float currentAngle = CalculateAngle(leftShoulderPosition,
                                                leftHipPosition,
                                                leftKneePosition);
            float legAngle = CalculateAngle(leftHipPosition,
                                                leftKneePosition,
                                                leftAnklePosition);
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기


            // 원래 자세로 복귀
            if (currentAngle > 90 && legAngle < 170)
            {
                Debug.Log("자세가 올바르지 못했어요!");
            }
            else
            {
                Debug.Log("잘했어요!");
                successCount++;
            }
            timeCount++;
            Debug.Log("시작자세로 돌아와주세요");
            yield return new WaitForSeconds(5f);

        }

        if (successCount >= 4)
        {
            Debug.Log("잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }

    }



    IEnumerator StretchingCoroutine5() // 반달자세
    {
        int timeCount = 0;
        int successCount = 0;
        int lefttimeCount = 0;
        int righttimeCount = 0;
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        float originalleftShoulderPosition = leftShoulderPosition.y;
        float originalrightHipPosition = rightHipPosition.x;
        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 4)
        {
            if (lefttimeCount == righttimeCount)
            {
                Debug.Log("상체를 왼쪽으로 기울이고 골반을 오른쪽으로 밀어주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기
                float currentleftShoulderPosition = leftShoulderPosition.y;
                float currentrightShoulderPosition = rightShoulderPosition.y;
                float currentleftHipPosition = leftHipPosition.x;
                float currentrightHipPosition = rightHipPosition.x;
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentleftShoulderPosition <= originalleftShoulderPosition && currentrightHipPosition >= originalrightHipPosition)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                lefttimeCount++;
                Debug.Log("시작자세로 돌아와주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
            else
            {
                Debug.Log("상체를 오른쪽으로 기울이고 골반을 왼쪽으로 밀어주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                float currentrightShoulderPosition = rightShoulderPosition.y;
                float currentleftHipPosition = leftHipPosition.x;
                yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기

                // 원래 자세로 복귀
                if (currentrightShoulderPosition <= originalleftShoulderPosition && currentleftHipPosition <= originalrightHipPosition)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                righttimeCount++;
                Debug.Log("시작자세로 돌아와주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
        }

        if (successCount >= 5)
        {
            Debug.Log("잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }
    }

    IEnumerator pose_measure()
    {

        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("우측을 바라보시고 전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        Debug.Log("자세측정을 시작하겠습니다.");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        float ab = Mathf.Abs(leftEarPosition.x - leftShoulderPosition.x);
        float ac = Mathf.Abs(leftEarPosition.x - leftHipPosition.x);
        float ad = Mathf.Abs(leftEarPosition.x - leftAnklePosition.x);
        float bc = Mathf.Abs(leftShoulderPosition.x - leftHipPosition.x);
        float bd = Mathf.Abs(leftShoulderPosition.x - leftAnklePosition.x);
        float cd = Mathf.Abs(leftHipPosition.x - leftAnklePosition.x);

        if (ab < 2 && ac < 2 && ad < 2 && bc < 2 && bd < 2 && bd < 2)
        {
            Debug.Log("올바른 자세입니다");
        }
        else
        {
            Debug.Log("올바르지 않아요");
        }

    }
    float CalculateAngle(Vector2 Point1, Vector2 Point2, Vector2 Point3)
    {
        Vector2 vector1 = Point1 - Point2;
        Vector2 vector2 = Point3 - Point2;
        return Vector2.Angle(vector1, vector2);
    }
}
