using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseEvaluation : MonoBehaviour
{
    public static PoseEvaluation instance;

    private void Awake()
    {
        instance = this;
    }

    #region 기본 참조 변수
    public bool exerciseFin = false;
    public int eCountMax = 0;
    public int eCountCur = 0;
    public int pM = 0;
    #endregion

    #region PoseSkeleton 객체를 위한 참조를 생성합니다.
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
    #endregion

    #region 세팅
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
    #endregion

    #region ExerciseUI에서 불러서 사용
    public void GetStretchingSelect(int index)
    {
        UIManager.instance.exUI.announceText.text = string.Empty;
        UIManager.instance.exUI.countText.text = string.Empty;

        UIManager.instance.exUI.announcePanel.SetActive(true);

        switch (index)
        {
            case 0:
                StartCoroutine(StretchingCoroutine0());
                break;

            case 1:
                StartCoroutine(StretchingCoroutine1());
                break;
            case 2:
                StartCoroutine(StretchingCoroutine2());
                break;

            case 3:
                StartCoroutine(StretchingCoroutine3());
                break;

            case 4:
                StartCoroutine(StretchingCoroutine4());
                break;

            case 5:
                StartCoroutine(StretchingCoroutine5());
                break;

            case 6:
                StartCoroutine(pose_measure());
                break;

            default:
                Debug.Log("뭔가 문제가 있음");
                break;
        }
    }
    #endregion

    #region 스트레칭 종류 IEnumerator 
    IEnumerator pose_measure() // 초반 자세 설정
    {
        pM = 0;

        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        SoundManager.instance.PlaySFX("3");
        Debug.Log("우측으로 돌아 측면 전체가 보이게 서주세요");
        UIManager.instance.exUI.announceText.text = "Turn to your right so your entire side profile is visible.";


        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        SoundManager.instance.PlaySFX("M1");
        Debug.Log("자세측정을 시작하겠습니다.");
        UIManager.instance.exUI.announceText.text = "Let's start measuring your posture.";

        yield return new WaitForSeconds(6f); // 자세 준비시간 2초간 대기

        float ab = Mathf.Abs(leftEarPosition.x - leftShoulderPosition.x);
        float ac = Mathf.Abs(leftEarPosition.x - leftHipPosition.x);
        float ad = Mathf.Abs(leftEarPosition.x - leftAnklePosition.x);
        float bc = Mathf.Abs(leftShoulderPosition.x - leftHipPosition.x);
        float bd = Mathf.Abs(leftShoulderPosition.x - leftAnklePosition.x);
        float cd = Mathf.Abs(leftHipPosition.x - leftAnklePosition.x);

        if (ab < 2 && ac < 2 && ad < 2 && bc < 2 && bd < 2 && bd < 2)
        {
            UIManager.instance.lobbyUI.correct = 1;

            SoundManager.instance.PlaySFX("M2");
            yield return new WaitForSeconds(3f);
            Debug.Log("올바른 자세입니다! 함께 디스크 예방을 해봐요!");
            UIManager.instance.exUI.announceText.text = "Your posture is correct! Let's keep it up to prevent disc problems!";
        }
        else
        {
            UIManager.instance.lobbyUI.correct = 2;

            SoundManager.instance.PlaySFX("M3");
            yield return new WaitForSeconds(3f);

            Debug.Log("자세가 올바르지 못해요. 함께 교정해봐요. 전문가와의 상담도 추천드립니다! ");
            UIManager.instance.exUI.announceText.text = "Your posture isn't correct. Let's work on fixing it together. I also recommend consulting with a specialist!";
        }

        exerciseFin = true;
    }

    IEnumerator StretchingCoroutine0() // 고개 숙이기 스트레칭(안씀)
    {
        int timeCount = 0;
        int successCount = 0;

        yield return new WaitForSeconds(2f); // 준비시간 2초간 대기

        Debug.Log("얼굴을 보여주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기

        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 3)
        {
            float originalleftEyePosition = leftEyePosition.y;
            Debug.Log("고개를 숙여주세요");
            yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
            yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기
            float currentleftEyePosition = leftEyePosition.y; // 현재 눈 위치 확인
            yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

            // 원래 자세로 복귀
            if (currentleftEyePosition < originalleftEyePosition + 5)
            {
                Debug.Log("자세가 올바르지 못했어요. 고개를 충분히 숙이지 않았어요.");
            }
            else
            {
                Debug.Log("잘했어요!");
                successCount++;
            }
            timeCount++;
            Debug.Log("시작자세로 돌아와주세요.");
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

        yield return new WaitForSeconds(1f);

        eCountMax = 3;
        eCountCur = successCount;

        exerciseFin = true;
    }

    IEnumerator StretchingCoroutine1() // 옆목 스트레칭
    {
        int maxCount = 6;
        int timeCount = 0;
        int successCount = 0;
        int lefttimeCount = 0;
        int righttimeCount = 0;

        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("2");
        Debug.Log("정면을 보고 전신이 보이게 제대로 서주세요");
        UIManager.instance.exUI.announceText.text = "Stand facing forward with your whole body visible.";


        yield return new WaitForSeconds(5f); // 자세 준비시간 5초간 대기
        float originalleftEarPosition = leftEarPosition.y;
        float originalrightEarPosition = rightEarPosition.y;

        SoundManager.instance.PlaySFX("4");
        Debug.Log("스트레칭을 시작해볼까요?");
        UIManager.instance.exUI.announceText.text = "Shall we start stretching?";
        yield return new WaitForSeconds(3f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("5");
        Debug.Log("동작을 왼쪽부터 수행합니다");
        UIManager.instance.exUI.announceText.text = "Let's start with movements on the left side.";
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < maxCount)
        {
            UIManager.instance.exUI.countText.text = (maxCount - timeCount) + "Left";
            if (lefttimeCount == righttimeCount)
            {

                SoundManager.instance.PlaySFX("S11");
                Debug.Log("목을 왼쪽으로 늘려주세요");
                UIManager.instance.exUI.announceText.text = "Please stretch your neck to the left.";
                yield return new WaitForSeconds(7f); // 자세 준비시간 & 동작 준비 7초간 대기

                float currentleftEarPosition = leftEarPosition.y; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentleftEarPosition <= originalleftEarPosition + 5)
                {
                    SoundManager.instance.PlaySFX("S12");
                    Debug.Log("자세가 올바르지 못했어요. 목을 좀 더 늘려주세요.");
                    UIManager.instance.exUI.announceText.text = "Your posture isn't quite right. Stretch your neck a bit more.";
                    yield return new WaitForSeconds(3f);
                }
                else
                {
                    SoundManager.instance.PlaySFX("6");
                    Debug.Log("잘했어요!");
                    UIManager.instance.exUI.announceText.text = "Great job!";
                    yield return new WaitForSeconds(1f);

                    successCount++;

                }
                lefttimeCount++;

                SoundManager.instance.PlaySFX("7");
                Debug.Log("시작자세로 돌아와주세요.");
                UIManager.instance.exUI.announceText.text = "Return to the starting position.";
                yield return new WaitForSeconds(2f);

                timeCount++;
            }
            else
            {
                SoundManager.instance.PlaySFX("S13");
                Debug.Log("목을 오른쪽으로 늘려주세요");
                UIManager.instance.exUI.announceText.text = "Stretch your neck to the right.";
                yield return new WaitForSeconds(7f);

                float currentrightEarPosition = rightEarPosition.y;
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentrightEarPosition <= originalrightEarPosition + 5)
                {
                    SoundManager.instance.PlaySFX("S12");
                    Debug.Log("자세가 올바르지 못했어요. 목을 좀 더 늘려주세요.");
                    UIManager.instance.exUI.announceText.text = "Your posture isn't quite right. Stretch your neck a bit more.";
                    yield return new WaitForSeconds(3f);
                }
                else
                {
                    SoundManager.instance.PlaySFX("6");
                    Debug.Log("잘했어요!");
                    UIManager.instance.exUI.announceText.text = "Great job!";
                    yield return new WaitForSeconds(1f);

                    successCount++;
                }

                righttimeCount++;

                SoundManager.instance.PlaySFX("7");
                Debug.Log("시작자세로 돌아와주세요.");
                UIManager.instance.exUI.announceText.text = "Return to the starting position.";

                yield return new WaitForSeconds(3f);
                timeCount++;
            }
        }

        UIManager.instance.exUI.countText.text = string.Empty;

        if (successCount >= 5)
        {
            SoundManager.instance.PlaySFX("8");
            Debug.Log("잘했습니다! 성공입니다!");
            UIManager.instance.exUI.announceText.text = "Good job! You succeeded!";
            yield return new WaitForSeconds(3f);
        }
        else
        {
            SoundManager.instance.PlaySFX("9");
            Debug.Log("다음엔 좀 더 잘해봐요!!");
            UIManager.instance.exUI.announceText.text = "Let's try to do even better next time!";
            yield return new WaitForSeconds(3f);
        }

        eCountMax = maxCount;
        eCountCur = successCount;

        yield return YieldCache.WaitForSeconds(3);

        exerciseFin = true;
    }

    IEnumerator StretchingCoroutine2() // 사이드 스트레칭
    {
        int maxCount = 4;
        int timeCount = 0;
        int successCount = 0;
        int lefttimeCount = 0;
        int righttimeCount = 0;

        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("2");
        Debug.Log("정면을 보고 전신이 보이게 제대로 서주세요");
        UIManager.instance.exUI.announceText.text = "Stand facing forward with your whole body visible.";
        yield return new WaitForSeconds(5f); // 자세 준비시간 5초간 대기
        float originalleftEarPosition = leftEarPosition.y;
        float originalrightEarPosition = rightEarPosition.y;

        SoundManager.instance.PlaySFX("4");
        Debug.Log("스트레칭을 시작해볼까요?");
        UIManager.instance.exUI.announceText.text = "Shall we start stretching?";
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("5");
        Debug.Log("동작을 왼쪽부터 수행합니다.");
        UIManager.instance.exUI.announceText.text = "Let's start with movements on the left side.";
        yield return new WaitForSeconds(3f); // 자세 준비시간 2초간 대기

        while (timeCount < maxCount)
        {
            UIManager.instance.exUI.countText.text = (maxCount - timeCount) + "Left";
            if (lefttimeCount == righttimeCount)
            {

                SoundManager.instance.PlaySFX("S21");
                Debug.Log("허리를 왼쪽으로 굽혀주세요");
                UIManager.instance.exUI.announceText.text = "Please bend your waist to the left.";
                yield return new WaitForSeconds(7f); // 자세 준비시간 2초간 대기

                float currentleftEarPosition = leftEarPosition.y; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentleftEarPosition <= originalleftEarPosition + 2)
                {
                    SoundManager.instance.PlaySFX("S22");
                    Debug.Log("자세가 올바르지 못했어요. 허리를 좀 더 굽혀주세요.");
                    UIManager.instance.exUI.announceText.text = "Your posture isn't quite right. Bend your waist a bit more.";
                    yield return new WaitForSeconds(3f);
                }
                else
                {
                    SoundManager.instance.PlaySFX("6");
                    Debug.Log("잘했어요!");
                    UIManager.instance.exUI.announceText.text = "Great job!";
                    yield return new WaitForSeconds(1f);

                    successCount++;
                }

                lefttimeCount++;

                SoundManager.instance.PlaySFX("7");
                Debug.Log("시작자세로 돌아와주세요.");
                UIManager.instance.exUI.announceText.text = "Return to the starting position.";
                yield return new WaitForSeconds(2f);

                timeCount++;
            }
            else
            {

                SoundManager.instance.PlaySFX("S23");
                Debug.Log("허리를 오른쪽으로 굽혀주세요");
                UIManager.instance.exUI.announceText.text = "Bend your waist to the right.";
                yield return new WaitForSeconds(7f); // 자세 준비시간 2초간 대기

                float currentrightEarPosition = rightEyePosition.y;
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentrightEarPosition <= originalrightEarPosition + 2)
                {
                    SoundManager.instance.PlaySFX("S22");
                    Debug.Log("자세가 올바르지 못했어요. 허리를 좀 더 굽혀주세요.");
                    UIManager.instance.exUI.announceText.text = "Your posture isn't quite right. Bend your waist a bit more.";
                    yield return new WaitForSeconds(3f);
                }
                else
                {
                    SoundManager.instance.PlaySFX("6");
                    Debug.Log("잘했어요!");
                    UIManager.instance.exUI.announceText.text = "Great job!";
                    yield return new WaitForSeconds(1f);

                    successCount++;
                }

                righttimeCount++;

                SoundManager.instance.PlaySFX("7");
                Debug.Log("시작자세로 돌아와주세요");
                UIManager.instance.exUI.announceText.text = "Return to the starting position.";
                yield return new WaitForSeconds(3f);

                timeCount++;
            }
        }

        if (successCount >= 5)
        {
            SoundManager.instance.PlaySFX("8");
            Debug.Log("잘했습니다! 성공입니다!");
            UIManager.instance.exUI.announceText.text = "Good job! You succeeded!";
            yield return new WaitForSeconds(2f);
        }
        else
        {
            SoundManager.instance.PlaySFX("9");
            Debug.Log("다음엔 좀 더 잘해봐요!!");
            UIManager.instance.exUI.announceText.text = "Let's try to do even better next time!";
            yield return new WaitForSeconds(2f);
        }

        eCountMax = maxCount;
        eCountCur = successCount;

        yield return YieldCache.WaitForSeconds(2f);

        exerciseFin = true;
    }

    IEnumerator StretchingCoroutine3() // 서서 허리 젖히기
    {
        int maxCount = 5;
        int timeCount = 0;
        int successCount = 0;

        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("3");
        Debug.Log("우측으로 돌아 측면 전체가 보이게 서주세요");
        UIManager.instance.exUI.announceText.text = "Turn to your right so your entire side profile is visible.";
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("4");
        Debug.Log("스트레칭을 시작해볼까요?");
        UIManager.instance.exUI.announceText.text = "Shall we start stretching?";
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < maxCount)
        {
            UIManager.instance.exUI.countText.text = (maxCount - timeCount) + "Left";
            SoundManager.instance.PlaySFX("S31");
            Debug.Log("몸을 뒤로 젖혀주세요");
            UIManager.instance.exUI.announceText.text = "Please bend your body backward.";
            yield return new WaitForSeconds(7f); // 자세 준비시간 2초간 대기

            float currentAngle = CalculateAngle(leftShoulderPosition,
                                                leftHipPosition,
                                                leftAnklePosition);
            yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

            // 원래 자세로 복귀
            if (currentAngle > 160)
            {
                SoundManager.instance.PlaySFX("S32");
                Debug.Log("자세가 올바르지 못했어요. 좀 더 뒤로 젖혀주세요.");
                UIManager.instance.exUI.announceText.text = "Your posture isn't quite right. Bend your body a bit more backward.";
                yield return new WaitForSeconds(3f);
            }
            else
            {
                SoundManager.instance.PlaySFX("6");
                Debug.Log("잘했어요!");
                UIManager.instance.exUI.announceText.text = "Great job!";
                yield return new WaitForSeconds(1f);

                successCount++;
            }

            SoundManager.instance.PlaySFX("7");
            Debug.Log("시작자세로 돌아와주세요");
            UIManager.instance.exUI.announceText.text = "Return to the starting position.";
            yield return new WaitForSeconds(3f);

            timeCount++;
        }

        UIManager.instance.exUI.countText.text = string.Empty;

        if (successCount >= 4)
        {
            SoundManager.instance.PlaySFX("8");
            Debug.Log("잘했습니다! 성공입니다!");
            UIManager.instance.exUI.announceText.text = "Good job! You succeeded!";
            yield return new WaitForSeconds(2f);
        }
        else
        {
            SoundManager.instance.PlaySFX("9");
            Debug.Log("다음엔 좀 더 잘해봐요!!");
            UIManager.instance.exUI.announceText.text = "Let's try to do even better next time!";
            yield return new WaitForSeconds(2f);
        }

        eCountMax = maxCount;
        eCountCur = successCount;

        yield return YieldCache.WaitForSeconds(2f);

        exerciseFin = true;
    }

    IEnumerator StretchingCoroutine4() // 상체숙이기
    {
        int maxCount = 5;
        int timeCount = 0;
        int successCount = 0;
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("3");
        Debug.Log("우측으로 돌아 측면 전체가 보이게 서주세요");
        UIManager.instance.exUI.announceText.text = "Turn to your right so your entire side profile is visible.";
        yield return new WaitForSeconds(3f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("4");
        Debug.Log("스트레칭을 시작해볼까요?");
        UIManager.instance.exUI.announceText.text = "Shall we start stretching?";
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < maxCount)
        {
            UIManager.instance.exUI.countText.text = (maxCount - timeCount) + "Left";
            SoundManager.instance.PlaySFX("S41");
            Debug.Log("상체를 숙여주세요");
            UIManager.instance.exUI.announceText.text = "Please bend your upper body forward.";
            yield return new WaitForSeconds(7f); // 자세 준비시간 2초간 대기

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
                SoundManager.instance.PlaySFX("S42");
                Debug.Log("상체를 좀 더 숙여주세요.");
                UIManager.instance.exUI.announceText.text = "Your posture isn't quite right. Bend your upper body a bit more.";
                yield return new WaitForSeconds(3f);
            }
            else
            {
                SoundManager.instance.PlaySFX("6");
                Debug.Log("잘했어요!");
                UIManager.instance.exUI.announceText.text = "Great job!";
                yield return new WaitForSeconds(1f);

                successCount++;
            }

            SoundManager.instance.PlaySFX("7");
            Debug.Log("시작자세로 돌아와주세요");
            UIManager.instance.exUI.announceText.text = "Return to the starting position.";
            yield return new WaitForSeconds(3f);

            timeCount++;
        }
        UIManager.instance.exUI.countText.text = string.Empty;

        if (successCount >= 4)
        {
            SoundManager.instance.PlaySFX("8");
            Debug.Log("잘했습니다! 성공입니다!");
            UIManager.instance.exUI.announceText.text = "Good job! You succeeded!";
            yield return new WaitForSeconds(3f);
        }
        else
        {
            SoundManager.instance.PlaySFX("9");
            Debug.Log("다음엔 좀 더 잘해봐요!!");
            UIManager.instance.exUI.announceText.text = "Let's try to do even better next time!";
            yield return new WaitForSeconds(3f);
        }

        eCountMax = maxCount;
        eCountCur = successCount;

        yield return YieldCache.WaitForSeconds(2f);

        exerciseFin = true;
    }

    IEnumerator StretchingCoroutine5() // 반달자세
    {
        int maxCount = 6;
        int timeCount = 0;
        int successCount = 0;
        int lefttimeCount = 0;
        int righttimeCount = 0;

        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        SoundManager.instance.PlaySFX("2");
        Debug.Log("전신이 보이게 제대로 서주세요");
        UIManager.instance.exUI.announceText.text = "Stand facing forward with your whole body visible.";
        yield return new WaitForSeconds(3f); // 자세 준비시간 2초간 대기
        float originalleftShoulderPosition = leftShoulderPosition.y;
        float originalrightHipPosition = rightHipPosition.x;
        float originalrightShoulderPosition = rightShoulderPosition.y;
        float originalleftHipPosition = leftHipPosition.x;

        SoundManager.instance.PlaySFX("4");
        Debug.Log("스트레칭을 시작해볼까요?");
        UIManager.instance.exUI.announceText.text = "Shall we start stretching?";
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < maxCount)
        {
            UIManager.instance.exUI.countText.text = (maxCount - timeCount) + "Left";
            if (lefttimeCount == righttimeCount)
            {

                SoundManager.instance.PlaySFX("S51");
                Debug.Log("상체를 왼쪽으로 기울이고 골반을 오른쪽으로 밀어주세요");
                UIManager.instance.exUI.announceText.text = "Tilt your upper body to the left and push your pelvis to the right.";
                yield return new WaitForSeconds(7f); // 자세 준비시간 2초간 대기

                float currentleftShoulderPosition = leftShoulderPosition.y;
                float currentrightHipPosition = rightHipPosition.x;
                yield return new WaitForSeconds(5f); // 동작 수행시간 5초간 대기

                // 원래 자세로 복귀
                if (currentleftShoulderPosition <= originalleftShoulderPosition && currentrightHipPosition >= originalrightHipPosition)
                {
                    SoundManager.instance.PlaySFX("S52");
                    Debug.Log("자세가 올바르지 못했어요. 상체를 좀 더 기울이고 골반을 밀어주세요");
                    UIManager.instance.exUI.announceText.text = "Your posture isn't quite right. Tilt your upper body more and push your pelvis further.";
                    yield return new WaitForSeconds(4f);
                }
                else
                {
                    SoundManager.instance.PlaySFX("6");
                    Debug.Log("잘했어요!");
                    UIManager.instance.exUI.announceText.text = "Great job!";
                    yield return new WaitForSeconds(1f);

                    successCount++;
                }

                lefttimeCount++;

                SoundManager.instance.PlaySFX("7");
                Debug.Log("시작자세로 돌아와주세요");
                UIManager.instance.exUI.announceText.text = "Return to the starting position.";
                yield return new WaitForSeconds(3f);

                timeCount++;
            }
            else
            {

                SoundManager.instance.PlaySFX("S53");
                Debug.Log("상체를 오른쪽으로 기울이고 골반을 왼쪽으로 밀어주세요");
                UIManager.instance.exUI.announceText.text = "Tilt your upper body to the right and push your pelvis to the left.";
                yield return new WaitForSeconds(7f); // 자세 준비시간 2초간 대기

                float currentrightShoulderPosition = rightShoulderPosition.y;
                float currentleftHipPosition = leftHipPosition.x;
                yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기

                // 원래 자세로 복귀
                if (currentrightShoulderPosition <= originalrightShoulderPosition && currentleftHipPosition <= originalleftHipPosition)
                {
                    SoundManager.instance.PlaySFX("S52");
                    Debug.Log("자세가 올바르지 못했어요. 상체를 좀 더 기울이고 골반을 밀어주세요.");
                    UIManager.instance.exUI.announceText.text = "Your posture isn't quite right. Tilt your upper body more and push your pelvis further.";
                    yield return new WaitForSeconds(4f);
                }
                else
                {
                    SoundManager.instance.PlaySFX("6");
                    Debug.Log("잘했어요!");
                    UIManager.instance.exUI.announceText.text = "Great job!";
                    yield return new WaitForSeconds(1f);

                    successCount++;
                }

                righttimeCount++;

                SoundManager.instance.PlaySFX("7");
                Debug.Log("시작자세로 돌아와주세요");
                UIManager.instance.exUI.announceText.text = "Return to the starting position.";
                yield return new WaitForSeconds(3f);

                timeCount++;
            }
        }
        UIManager.instance.exUI.countText.text = string.Empty;

        if (successCount >= 5)
        {
            SoundManager.instance.PlaySFX("8");
            Debug.Log("잘했습니다! 성공입니다!");
            UIManager.instance.exUI.announceText.text = "Good job! You succeeded!";
            yield return new WaitForSeconds(3f);
        }
        else
        {
            SoundManager.instance.PlaySFX("9");
            Debug.Log("다음엔 좀 더 잘해봐요!!");
            UIManager.instance.exUI.announceText.text = "Let's try to do even better next time!";
            yield return new WaitForSeconds(3f);
        }

        eCountMax = maxCount;
        eCountCur = successCount;

        yield return YieldCache.WaitForSeconds(2f);

        exerciseFin = true;
    }

    float CalculateAngle(Vector2 Point1, Vector2 Point2, Vector2 Point3) // Angle 계산
    {
        Vector2 vector1 = Point1 - Point2;
        Vector2 vector2 = Point3 - Point2;
        return Vector2.Angle(vector1, vector2);
    }
    #endregion
}
