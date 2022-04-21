using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace  JazzApps
{
    /// <summary>
    /// Manages states of a door animation. I lazily coded using various Animators because state machines are a lot of work :)
    /// </summary>
    public class DoorAnimationManager : MonoBehaviour
    {
        // Consts
        private static readonly int Trigger = Animator.StringToHash("Trigger");
    
        // Externals
        [SerializeField] private Animator doorAnimator, lockAnimator, bottomLockAnimator;

        // Internals
        private int _animationStage;
        
        private void Awake()
        {
            ResetAnimations();
        }
        
        public void Fire(InputAction.CallbackContext context)
        {
            if (context.performed)
                IncrementAnimation();
        }
        

        private void IncrementAnimation()
        {
            switch (_animationStage)
            {
                case 0:
                    bottomLockAnimator.SetBool(Trigger, true);
                    break;
                case 1:
                    lockAnimator.SetBool(Trigger, true);
                    break;
                case 2:
                    StartCoroutine(DelayedOpen());
                    break;
                case 3:
                    StartCoroutine(DelayedReset());
                    return;
            }
            _animationStage++;
        }

        private IEnumerator DelayedOpen()
        {
            doorAnimator.SetBool(Trigger, true);
            yield return (new WaitForSeconds(2f));
            bottomLockAnimator.SetBool(Trigger, false);
            lockAnimator.SetBool(Trigger, false);
        }

        private IEnumerator DelayedReset()
        {
            lockAnimator.SetBool(Trigger, true);
            bottomLockAnimator.SetBool(Trigger, true);
            yield return (new WaitForSeconds(1f));
            doorAnimator.SetBool(Trigger, false);
            yield return (new WaitForSeconds(2.5f));
            ResetAnimations();
        }
        
        private void ResetAnimations()
        {
            doorAnimator.SetBool(Trigger, false);
            lockAnimator.SetBool(Trigger, false);
            bottomLockAnimator.SetBool(Trigger, false);
            _animationStage = 0;
        }
    }
}
