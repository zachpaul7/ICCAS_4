using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position index of joint points
/// </summary>
public enum PositionIndex : int
{
    rShldrBend = 0,
    rForearmBend,
    rHand,
    rThumb2,
    rMid1,

    lShldrBend,
    lForearmBend,
    lHand,
    lThumb2,
    lMid1,

    lEar,
    lEye,
    rEar,
    rEye,
    Nose,

    rThighBend,
    rShin,
    rFoot,
    rToe,

    lThighBend,
    lShin,
    lFoot,
    lToe,

    abdomenUpper,

    //Calculated coordinates
    hip,
    head,
    neck,
    spine,

    Count,
    None,
}

public static partial class EnumExtend
{
    public static int Int(this PositionIndex i)
    {
        return (int)i;
    }
}

public class VNectModel : MonoBehaviour
{
    public static VNectModel instance;
    public class JointPoint
    {
        public Vector2 Pos2D = new Vector2();
        public float score2D;

        public Vector3 Pos3D = new Vector3();
        public Vector3 Now3D = new Vector3();
        public Vector3[] PrevPos3D = new Vector3[6];
        public float score3D;

        // Bones
        public Transform Transform = null;
        public Quaternion InitRotation;
        public Quaternion Inverse;
        public Quaternion InverseRotation;

        public JointPoint Child = null;
        public JointPoint Parent = null;

        // For Kalman filter
        public Vector3 P = new Vector3();
        public Vector3 X = new Vector3();
        public Vector3 K = new Vector3();
    }

    public class Skeleton
    {
        public GameObject LineObject;
        public LineRenderer Line;

        public JointPoint start = null;
        public JointPoint end = null;
    }

    private List<Skeleton> Skeletons = new List<Skeleton>();
    public Material SkeletonMaterial;

    public bool ShowSkeleton;
    private bool useSkeleton;
    public float SkeletonX;
    public float SkeletonY;
    public float SkeletonZ;
    public float SkeletonScale;

    // Joint position and bone
    private JointPoint[] jointPoints;
    public JointPoint[] JointPoints { get { return jointPoints; } }

    private Vector3 initPosition; // Initial center position

    private Quaternion InitGazeRotation;
    private Quaternion gazeInverse;

    // UnityChan
    public GameObject ModelObject;
    public GameObject Nose;
    private Animator anim;

    // Move in z direction
    private float centerTall = 224 * 0.75f;
    private float tall = 224 * 0.75f;
    private float prevTall = 224 * 0.75f;
    public float ZScale = 0.8f;

    public GameObject parentTransform;
    public bool stretchingInProgress = true;
    public int steretchingSelect;
    Vector3 originalHeadPos;
    Vector3 original_lEarPos;
    Vector3 original_rEarPos;
    float startAngle;

    private int timeCount = 0; //  운동횟수를 저장할 변수 추가
    private int successCount = 0; // 성공 횟수를 저장할 변수 추가
    private int lEar_timeCount = 0; //  운동횟수를 저장할 변수 추가
    private int rEar_timeCount = 0; //  운동횟수를 저장할 변수 추가

    private void Awake()
    {
        instance = this;
    }

    public void GetStretchingSelect(int index)
    {
        //steretchingSelect = index;
        //stretchingInProgress = false;

        // 10초 후에 스트레칭 동작을 체크하는 메서드 호출
        /*if (Time.timeSinceLevelLoad > 10f && !stretchingInProgress)
        {
            stretchingInProgress = true;
            CheckStretchingPose(index);
        }*/

        CheckStretchingPose(index);
    }

    private void LateUpdate()
    {
        if (jointPoints != null)
        {
            PoseUpdate(); // 기존의 PoseUpdate 메서드 호출
        }
    }

    private void CheckStretchingPose(int index)
    {
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

            default:
                Debug.Log("뭔가 문제가 있음");
                break;

        }
    }

    IEnumerator StretchingCoroutine0() // 고개 숙이기 스트레칭
    {
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("전신이 보이게 제대로 서주세요");

        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기

        originalHeadPos = jointPoints[PositionIndex.head.Int()].Pos3D;
        original_lEarPos = jointPoints[PositionIndex.lEar.Int()].Pos3D;
        original_rEarPos = jointPoints[PositionIndex.rEar.Int()].Pos3D;

        Debug.Log(originalHeadPos + " / " + original_lEarPos + " / " + original_rEarPos);

        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 3)
        {
            Debug.Log("고개를 숙여주세요");
            yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
            Vector3 currentHeadPos = jointPoints[PositionIndex.head.Int()].Pos3D; // 현재 눈 위치 확인
            yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기


            // 원래 자세로 복귀
            if (currentHeadPos.y >= originalHeadPos.y)
            {
                Debug.Log("자세를 올바르지 못했어요! 고개를 충분히 숙이지 않았습니다.");
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
            Debug.Log("모두 잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }

        // 공격 로직
        UIManager.instance.exUI.Attack();

        parentTransform.SetActive(false);

        yield return YieldCache.WaitForSeconds(3.5f);

        UIManager.instance.exUI.OpenExerciseSelect();
    }

    IEnumerator StretchingCoroutine1() // 옆목 스트레칭
    {
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기

        originalHeadPos = jointPoints[PositionIndex.head.Int()].Pos3D;
        original_lEarPos = jointPoints[PositionIndex.lEar.Int()].Pos3D;
        original_rEarPos = jointPoints[PositionIndex.rEar.Int()].Pos3D;

        Debug.Log(originalHeadPos + " / " + original_lEarPos + " / " + original_rEarPos);

        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 6)
        {
            if (lEar_timeCount == rEar_timeCount)
            {
                Debug.Log("목을 왼쪽으로 늘려주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                Vector3 current_lEarPos = jointPoints[PositionIndex.lEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                Vector3 current_rEarPos = jointPoints[PositionIndex.rEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기

                // 원래 자세로 복귀
                if (current_lEarPos.y >= original_lEarPos.y)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                lEar_timeCount++;
                Debug.Log("고개를 바로 해주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
            else
            {
                Debug.Log("목을 오른쪽으로 늘려주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                Vector3 current_lEarPos = jointPoints[PositionIndex.lEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                Vector3 current_rEarPos = jointPoints[PositionIndex.rEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기

                // 원래 자세로 복귀
                if (current_rEarPos.y >= original_rEarPos.y)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                rEar_timeCount++;
                Debug.Log("고개를 바로 해주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
        }

        if (successCount >= 5)
        {
            Debug.Log("모두 잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }

        // 공격 로직
        UIManager.instance.exUI.Attack();

        parentTransform.SetActive(false);

        yield return YieldCache.WaitForSeconds(3.5f);

        UIManager.instance.exUI.OpenExerciseSelect();
    }

    IEnumerator StretchingCoroutine2() // 사이드 스트레칭
    {
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 4)
        {
            if (lEar_timeCount == rEar_timeCount)
            {
                Debug.Log("허리를 왼쪽으로 굽혀주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                Vector3 current_lEarPos = jointPoints[PositionIndex.lEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                Vector3 current_rEarPos = jointPoints[PositionIndex.rEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기

                // 원래 자세로 복귀
                if (current_lEarPos.y >= original_lEarPos.y)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                lEar_timeCount++;
                Debug.Log("고개를 바로 해주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
            else
            {
                Debug.Log("허리를 오른쪽으로 굽혀주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                Vector3 current_lEarPos = jointPoints[PositionIndex.lEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                Vector3 current_rEarPos = jointPoints[PositionIndex.rEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기

                // 원래 자세로 복귀
                if (current_rEarPos.y >= original_rEarPos.y)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                rEar_timeCount++;
                Debug.Log("고개를 바로 해주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
        }

        if (successCount >= 5)
        {
            Debug.Log("모두 잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }

        // 공격 로직
        UIManager.instance.exUI.Attack();

        parentTransform.SetActive(false);

        yield return YieldCache.WaitForSeconds(3.5f);

        UIManager.instance.exUI.OpenExerciseSelect();
    }

    IEnumerator StretchingCoroutine3() // 서서 허리 젖히기
    {
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("전신이 보이게 측면으로 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 5)
        {
            Debug.Log("몸을 뒤로 젖혀주세요");
            yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기
            float currentAngle = CalculateAngle(jointPoints[PositionIndex.lShldrBend.Int()].Pos3D,
                                                jointPoints[PositionIndex.lThighBend.Int()].Pos3D,
                                                jointPoints[PositionIndex.lShin.Int()].Pos3D);
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기


            // 원래 자세로 복귀
            if (currentAngle > 160)
            {
                Debug.Log("자세를 올바르지 못했어요!");
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
        successCount = lEar_timeCount + rEar_timeCount;

        if (successCount >= 4)
        {
            Debug.Log("모두 잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }

        // 공격 로직
        UIManager.instance.exUI.Attack();

        parentTransform.SetActive(false);

        yield return YieldCache.WaitForSeconds(3.5f);

        UIManager.instance.exUI.OpenExerciseSelect();
    }

    IEnumerator StretchingCoroutine4() // 상체내리기
    {
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("전신이 보이게 측면으로 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 3)
        {
            Debug.Log("상체를 내려주세요");
            yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기
            float currentAngle = CalculateAngle(jointPoints[PositionIndex.lShldrBend.Int()].Pos3D,
                                                jointPoints[PositionIndex.lThighBend.Int()].Pos3D,
                                                jointPoints[PositionIndex.lShin.Int()].Pos3D);
            yield return new WaitForSeconds(5f); // 동작 수행시간 10초간 대기


            // 원래 자세로 복귀
            if (currentAngle > 90)
            {
                Debug.Log("자세를 올바르지 못했어요!");
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
            Debug.Log("모두 잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }

        // 공격 로직
        UIManager.instance.exUI.Attack();

        parentTransform.SetActive(false);

        yield return YieldCache.WaitForSeconds(3.5f);

        UIManager.instance.exUI.OpenExerciseSelect();
    }

    IEnumerator StretchingCoroutine5() // 반달자세
    {
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
        Debug.Log("전신이 보이게 제대로 서주세요");
        yield return new WaitForSeconds(5f); // 자세 준비시간 2초간 대기
        Debug.Log("스트레칭을 시작해볼까요?");
        yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기

        while (timeCount < 4)
        {
            if (lEar_timeCount == rEar_timeCount)
            {
                Debug.Log("상체를 왼쪽으로 기울이고 골반을 오른쪽으로 밀어주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                Vector3 current_lEarPos = jointPoints[PositionIndex.lEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                Vector3 current_rEarPos = jointPoints[PositionIndex.rEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기

                // 원래 자세로 복귀
                if (current_lEarPos.y >= original_lEarPos.y)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                lEar_timeCount++;
                Debug.Log("고개를 바로 해주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
            else
            {
                Debug.Log("상체를 오른쪽으로 기울이고 골반을 왼쪽으로 밀어주세요");
                yield return new WaitForSeconds(2f); // 자세 준비시간 2초간 대기
                Vector3 current_lEarPos = jointPoints[PositionIndex.lEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                Vector3 current_rEarPos = jointPoints[PositionIndex.rEar.Int()].Pos3D; // 현재 왼쪽 귀 위치 확인
                yield return new WaitForSeconds(10f); // 동작 수행시간 10초간 대기

                // 원래 자세로 복귀
                if (current_rEarPos.y >= original_rEarPos.y)
                {
                    Debug.Log("자세가 올바르지 못했어요!");
                }
                else
                {
                    Debug.Log("잘했어요!");
                    successCount++;
                }
                rEar_timeCount++;
                Debug.Log("고개를 바로 해주세요");
                yield return new WaitForSeconds(5f);
                timeCount++;
            }
        }

        if (successCount >= 5)
        {
            Debug.Log("모두 잘했습니다! 성공입니다!");
        }
        else
        {
            Debug.Log("다음엔 좀 더 잘해봐요!!");
        }

        // 공격 로직
        UIManager.instance.exUI.Attack();

        parentTransform.SetActive(false);

        yield return YieldCache.WaitForSeconds(3.5f);

        UIManager.instance.exUI.OpenExerciseSelect();
    }

    float CalculateAngle(Vector3 headPos, Vector3 neckPos, Vector3 spinePos)
    {
        Vector3 vector1 = headPos - neckPos;
        Vector3 vector2 = spinePos - neckPos;
        return Vector3.Angle(vector1, vector2);
    }

    // Init() 메서드 및 PoseUpdate() 메서드는 이전과 동일하게 유지됨

    // AddSkeleton() 메서드도 이전과 동일하게 유지됨

    /// <summary>
    /// Initialize joint points
    /// </summary>
    /// <returns></returns>
    public JointPoint[] Init()
    {
        jointPoints = new JointPoint[PositionIndex.Count.Int()];
        for (var i = 0; i < PositionIndex.Count.Int(); i++) jointPoints[i] = new JointPoint();

        anim = ModelObject.GetComponent<Animator>();

        // Right Arm
        jointPoints[PositionIndex.rShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        jointPoints[PositionIndex.rForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
        jointPoints[PositionIndex.rHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightHand);
        jointPoints[PositionIndex.rThumb2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);
        jointPoints[PositionIndex.rMid1.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
        // Left Arm
        jointPoints[PositionIndex.lShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        jointPoints[PositionIndex.lForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        jointPoints[PositionIndex.lHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftHand);
        jointPoints[PositionIndex.lThumb2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);
        jointPoints[PositionIndex.lMid1.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);

        // Face
        jointPoints[PositionIndex.lEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex.lEye.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftEye);
        jointPoints[PositionIndex.rEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex.rEye.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightEye);
        jointPoints[PositionIndex.Nose.Int()].Transform = Nose.transform;

        // Right Leg
        jointPoints[PositionIndex.rThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        jointPoints[PositionIndex.rShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        jointPoints[PositionIndex.rFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        jointPoints[PositionIndex.rToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightToes);

        // Left Leg
        jointPoints[PositionIndex.lThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        jointPoints[PositionIndex.lShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        jointPoints[PositionIndex.lFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        jointPoints[PositionIndex.lToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftToes);

        // etc
        jointPoints[PositionIndex.abdomenUpper.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);
        jointPoints[PositionIndex.hip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
        jointPoints[PositionIndex.head.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex.neck.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Neck);
        jointPoints[PositionIndex.spine.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);

        // Child Settings
        // Right Arm
        jointPoints[PositionIndex.rShldrBend.Int()].Child = jointPoints[PositionIndex.rForearmBend.Int()];
        jointPoints[PositionIndex.rForearmBend.Int()].Child = jointPoints[PositionIndex.rHand.Int()];
        jointPoints[PositionIndex.rForearmBend.Int()].Parent = jointPoints[PositionIndex.rShldrBend.Int()];

        // Left Arm
        jointPoints[PositionIndex.lShldrBend.Int()].Child = jointPoints[PositionIndex.lForearmBend.Int()];
        jointPoints[PositionIndex.lForearmBend.Int()].Child = jointPoints[PositionIndex.lHand.Int()];
        jointPoints[PositionIndex.lForearmBend.Int()].Parent = jointPoints[PositionIndex.lShldrBend.Int()];

        // Fase

        // Right Leg
        jointPoints[PositionIndex.rThighBend.Int()].Child = jointPoints[PositionIndex.rShin.Int()];
        jointPoints[PositionIndex.rShin.Int()].Child = jointPoints[PositionIndex.rFoot.Int()];
        jointPoints[PositionIndex.rFoot.Int()].Child = jointPoints[PositionIndex.rToe.Int()];
        jointPoints[PositionIndex.rFoot.Int()].Parent = jointPoints[PositionIndex.rShin.Int()];

        // Left Leg
        jointPoints[PositionIndex.lThighBend.Int()].Child = jointPoints[PositionIndex.lShin.Int()];
        jointPoints[PositionIndex.lShin.Int()].Child = jointPoints[PositionIndex.lFoot.Int()];
        jointPoints[PositionIndex.lFoot.Int()].Child = jointPoints[PositionIndex.lToe.Int()];
        jointPoints[PositionIndex.lFoot.Int()].Parent = jointPoints[PositionIndex.lShin.Int()];

        // etc
        jointPoints[PositionIndex.spine.Int()].Child = jointPoints[PositionIndex.neck.Int()];
        jointPoints[PositionIndex.neck.Int()].Child = jointPoints[PositionIndex.head.Int()];
        //jointPoints[PositionIndex.head.Int()].Child = jointPoints[PositionIndex.Nose.Int()];

        useSkeleton = ShowSkeleton;
        if (useSkeleton)
        {
            // Line Child Settings
            // Right Arm
            AddSkeleton(PositionIndex.rShldrBend, PositionIndex.rForearmBend);
            AddSkeleton(PositionIndex.rForearmBend, PositionIndex.rHand);
            AddSkeleton(PositionIndex.rHand, PositionIndex.rThumb2);
            AddSkeleton(PositionIndex.rHand, PositionIndex.rMid1);

            // Left Arm
            AddSkeleton(PositionIndex.lShldrBend, PositionIndex.lForearmBend);
            AddSkeleton(PositionIndex.lForearmBend, PositionIndex.lHand);
            AddSkeleton(PositionIndex.lHand, PositionIndex.lThumb2);
            AddSkeleton(PositionIndex.lHand, PositionIndex.lMid1);

            // Fase
            AddSkeleton(PositionIndex.lEar, PositionIndex.Nose);
            AddSkeleton(PositionIndex.rEar, PositionIndex.Nose);

            // Right Leg
            AddSkeleton(PositionIndex.rThighBend, PositionIndex.rShin);
            AddSkeleton(PositionIndex.rShin, PositionIndex.rFoot);
            AddSkeleton(PositionIndex.rFoot, PositionIndex.rToe);

            // Left Leg
            AddSkeleton(PositionIndex.lThighBend, PositionIndex.lShin);
            AddSkeleton(PositionIndex.lShin, PositionIndex.lFoot);
            AddSkeleton(PositionIndex.lFoot, PositionIndex.lToe);

            // etc
            AddSkeleton(PositionIndex.spine, PositionIndex.neck);
            AddSkeleton(PositionIndex.neck, PositionIndex.head);
            AddSkeleton(PositionIndex.head, PositionIndex.Nose);
            AddSkeleton(PositionIndex.neck, PositionIndex.rShldrBend);
            AddSkeleton(PositionIndex.neck, PositionIndex.lShldrBend);
            AddSkeleton(PositionIndex.rThighBend, PositionIndex.rShldrBend);
            AddSkeleton(PositionIndex.lThighBend, PositionIndex.lShldrBend);
            AddSkeleton(PositionIndex.rShldrBend, PositionIndex.abdomenUpper);
            AddSkeleton(PositionIndex.lShldrBend, PositionIndex.abdomenUpper);
            AddSkeleton(PositionIndex.rThighBend, PositionIndex.abdomenUpper);
            AddSkeleton(PositionIndex.lThighBend, PositionIndex.abdomenUpper);
            AddSkeleton(PositionIndex.lThighBend, PositionIndex.rThighBend);
        }

        // Set Inverse
        var forward = TriangleNormal(jointPoints[PositionIndex.hip.Int()].Transform.position, jointPoints[PositionIndex.lThighBend.Int()].Transform.position, jointPoints[PositionIndex.rThighBend.Int()].Transform.position);
        foreach (var jointPoint in jointPoints)
        {
            if (jointPoint.Transform != null)
            {
                jointPoint.InitRotation = jointPoint.Transform.rotation;
            }

            if (jointPoint.Child != null)
            {
                jointPoint.Inverse = GetInverse(jointPoint, jointPoint.Child, forward);
                jointPoint.InverseRotation = jointPoint.Inverse * jointPoint.InitRotation;
            }
        }
        var hip = jointPoints[PositionIndex.hip.Int()];
        initPosition = jointPoints[PositionIndex.hip.Int()].Transform.position;
        hip.Inverse = Quaternion.Inverse(Quaternion.LookRotation(forward));
        hip.InverseRotation = hip.Inverse * hip.InitRotation;

        // For Head Rotation
        var head = jointPoints[PositionIndex.head.Int()];
        head.InitRotation = jointPoints[PositionIndex.head.Int()].Transform.rotation;
        var gaze = jointPoints[PositionIndex.Nose.Int()].Transform.position - jointPoints[PositionIndex.head.Int()].Transform.position;
        head.Inverse = Quaternion.Inverse(Quaternion.LookRotation(gaze));
        head.InverseRotation = head.Inverse * head.InitRotation;

        var lHand = jointPoints[PositionIndex.lHand.Int()];
        var lf = TriangleNormal(lHand.Pos3D, jointPoints[PositionIndex.lMid1.Int()].Pos3D, jointPoints[PositionIndex.lThumb2.Int()].Pos3D);
        lHand.InitRotation = lHand.Transform.rotation;
        lHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(jointPoints[PositionIndex.lThumb2.Int()].Transform.position - jointPoints[PositionIndex.lMid1.Int()].Transform.position, lf));
        lHand.InverseRotation = lHand.Inverse * lHand.InitRotation;

        var rHand = jointPoints[PositionIndex.rHand.Int()];
        var rf = TriangleNormal(rHand.Pos3D, jointPoints[PositionIndex.rThumb2.Int()].Pos3D, jointPoints[PositionIndex.rMid1.Int()].Pos3D);
        rHand.InitRotation = jointPoints[PositionIndex.rHand.Int()].Transform.rotation;
        rHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(jointPoints[PositionIndex.rThumb2.Int()].Transform.position - jointPoints[PositionIndex.rMid1.Int()].Transform.position, rf));
        rHand.InverseRotation = rHand.Inverse * rHand.InitRotation;

        jointPoints[PositionIndex.hip.Int()].score3D = 1f;
        jointPoints[PositionIndex.neck.Int()].score3D = 1f;
        jointPoints[PositionIndex.Nose.Int()].score3D = 1f;
        jointPoints[PositionIndex.head.Int()].score3D = 1f;
        jointPoints[PositionIndex.spine.Int()].score3D = 1f;


        return JointPoints;
    }

    public void PoseUpdate()
    {
        // caliculate movement range of z-coordinate from height
        var t1 = Vector3.Distance(jointPoints[PositionIndex.head.Int()].Pos3D, jointPoints[PositionIndex.neck.Int()].Pos3D);
        var t2 = Vector3.Distance(jointPoints[PositionIndex.neck.Int()].Pos3D, jointPoints[PositionIndex.spine.Int()].Pos3D);
        var pm = (jointPoints[PositionIndex.rThighBend.Int()].Pos3D + jointPoints[PositionIndex.lThighBend.Int()].Pos3D) / 2f;
        var t3 = Vector3.Distance(jointPoints[PositionIndex.spine.Int()].Pos3D, pm);
        var t4r = Vector3.Distance(jointPoints[PositionIndex.rThighBend.Int()].Pos3D, jointPoints[PositionIndex.rShin.Int()].Pos3D);
        var t4l = Vector3.Distance(jointPoints[PositionIndex.lThighBend.Int()].Pos3D, jointPoints[PositionIndex.lShin.Int()].Pos3D);
        var t4 = (t4r + t4l) / 2f;
        var t5r = Vector3.Distance(jointPoints[PositionIndex.rShin.Int()].Pos3D, jointPoints[PositionIndex.rFoot.Int()].Pos3D);
        var t5l = Vector3.Distance(jointPoints[PositionIndex.lShin.Int()].Pos3D, jointPoints[PositionIndex.lFoot.Int()].Pos3D);
        var t5 = (t5r + t5l) / 2f;
        var t = t1 + t2 + t3 + t4 + t5;


        // Low pass filter in z direction
        tall = t * 0.7f + prevTall * 0.3f;
        prevTall = tall;

        if (tall == 0)
        {
            tall = centerTall;
        }
        var dz = (centerTall - tall) / centerTall * ZScale;

        // movement and rotatation of center
        var forward = TriangleNormal(jointPoints[PositionIndex.hip.Int()].Pos3D, jointPoints[PositionIndex.lThighBend.Int()].Pos3D, jointPoints[PositionIndex.rThighBend.Int()].Pos3D);
        jointPoints[PositionIndex.hip.Int()].Transform.position = jointPoints[PositionIndex.hip.Int()].Pos3D * 0.005f + new Vector3(initPosition.x, initPosition.y, initPosition.z + dz);
        jointPoints[PositionIndex.hip.Int()].Transform.rotation = Quaternion.LookRotation(forward) * jointPoints[PositionIndex.hip.Int()].InverseRotation;

        // rotate each of bones
        foreach (var jointPoint in jointPoints)
        {
            if (jointPoint.Parent != null)
            {
                var fv = jointPoint.Parent.Pos3D - jointPoint.Pos3D;
                jointPoint.Transform.rotation = Quaternion.LookRotation(jointPoint.Pos3D - jointPoint.Child.Pos3D, fv) * jointPoint.InverseRotation;
            }
            else if (jointPoint.Child != null)
            {
                jointPoint.Transform.rotation = Quaternion.LookRotation(jointPoint.Pos3D - jointPoint.Child.Pos3D, forward) * jointPoint.InverseRotation;
            }
        }

        // Head Rotation
        var gaze = jointPoints[PositionIndex.Nose.Int()].Pos3D - jointPoints[PositionIndex.head.Int()].Pos3D;
        var f = TriangleNormal(jointPoints[PositionIndex.Nose.Int()].Pos3D, jointPoints[PositionIndex.rEar.Int()].Pos3D, jointPoints[PositionIndex.lEar.Int()].Pos3D);
        var head = jointPoints[PositionIndex.head.Int()];
        head.Transform.rotation = Quaternion.LookRotation(gaze, f) * head.InverseRotation;

        // Wrist rotation (Test code)
        var lHand = jointPoints[PositionIndex.lHand.Int()];
        var lf = TriangleNormal(lHand.Pos3D, jointPoints[PositionIndex.lMid1.Int()].Pos3D, jointPoints[PositionIndex.lThumb2.Int()].Pos3D);
        lHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex.lThumb2.Int()].Pos3D - jointPoints[PositionIndex.lMid1.Int()].Pos3D, lf) * lHand.InverseRotation;

        var rHand = jointPoints[PositionIndex.rHand.Int()];
        var rf = TriangleNormal(rHand.Pos3D, jointPoints[PositionIndex.rThumb2.Int()].Pos3D, jointPoints[PositionIndex.rMid1.Int()].Pos3D);
        //rHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex.rThumb2.Int()].Pos3D - jointPoints[PositionIndex.rMid1.Int()].Pos3D, rf) * rHand.InverseRotation;
        rHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex.rThumb2.Int()].Pos3D - jointPoints[PositionIndex.rMid1.Int()].Pos3D, rf) * rHand.InverseRotation;

        foreach (var sk in Skeletons)
        {
            var s = sk.start;
            var e = sk.end;

            sk.Line.SetPosition(0, new Vector3(s.Pos3D.x * SkeletonScale + SkeletonX, s.Pos3D.y * SkeletonScale + SkeletonY, s.Pos3D.z * SkeletonScale + SkeletonZ));
            sk.Line.SetPosition(1, new Vector3(e.Pos3D.x * SkeletonScale + SkeletonX, e.Pos3D.y * SkeletonScale + SkeletonY, e.Pos3D.z * SkeletonScale + SkeletonZ));
        }
    }

    Vector3 TriangleNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 d1 = a - b;
        Vector3 d2 = a - c;

        Vector3 dd = Vector3.Cross(d1, d2);
        dd.Normalize();

        return dd;
    }

    private Quaternion GetInverse(JointPoint p1, JointPoint p2, Vector3 forward)
    {
        return Quaternion.Inverse(Quaternion.LookRotation(p1.Transform.position - p2.Transform.position, forward));
    }

    /// <summary>
    /// Add skelton from joint points
    /// </summary>
    /// <param name="s">position index</param>
    /// <param name="e">position index</param>
    private void AddSkeleton(PositionIndex s, PositionIndex e)
    {
        var sk = new Skeleton()
        {
            LineObject = new GameObject("Line"),
            start = jointPoints[s.Int()],
            end = jointPoints[e.Int()],
        };

        sk.LineObject.transform.parent = parentTransform.transform;

        sk.Line = sk.LineObject.AddComponent<LineRenderer>();
        sk.Line.startWidth = 0.04f;
        sk.Line.endWidth = 0.01f;

        // define the number of vertex
        sk.Line.positionCount = 2;
        sk.Line.material = SkeletonMaterial;

        Skeletons.Add(sk);
    }


}
