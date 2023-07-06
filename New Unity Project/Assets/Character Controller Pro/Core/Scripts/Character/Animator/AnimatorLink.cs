using UnityEngine;

namespace Lightbug.CharacterControllerPro.Core
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorLink : MonoBehaviour
    {

        Animator _animator;
        bool _resetIKWeightsFlag = false;

        public event System.Action OnAnimatorMoveEvent;
        public event System.Action<int> OnAnimatorIKEvent;

        public void ResetIKWeights() => _resetIKWeightsFlag = true;

        #region Messages

        void Awake()
        {
            _animator = GetComponent<Animator>();
            Debug.Log(" 注意下面这个函数要打开");
        }

        //void OnAnimatorMove()
        //{

        //    OnAnimatorMoveEvent?.Invoke();
        //}


        void OnAnimatorIK(int layerIndex)
        {
            if (_resetIKWeightsFlag)
            {
                _resetIKWeightsFlag = false;

                _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            }

            OnAnimatorIKEvent?.Invoke(layerIndex);
        }

        #endregion

    }

}