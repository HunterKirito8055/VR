using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomStudent : MonoBehaviour
{
    private Animator animator;
    public List<string> animationClips = new List<string>();
    public string idleAnimationClip;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        AnimationClip[] allAnimationClips = animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < allAnimationClips.Length; i++)
        {
            if (i == 0)
            {
                idleAnimationClip = allAnimationClips[i].name;
            }
            else
            {
                animationClips.Add(allAnimationClips[i].name);
            }
        }
        StartCoroutine(IStartAnimating());
    }

    IEnumerator IStartAnimating()
    {
        yield return new WaitUntil(()=> animator.GetCurrentAnimatorStateInfo(0).IsName(idleAnimationClip));
        yield return new WaitForSeconds(Random.Range(minimumWaitTime, maximumWaitTime));
        animator.CrossFade(animationClips[Random.Range(1,animationClips.Count)], 0.1f);
        yield return StartCoroutine(IStartAnimating());
    }
    private float minimumWaitTime = 10f;
    private float maximumWaitTime = 35f;
}
