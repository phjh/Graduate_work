using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class SpineAnimator
{
    public static bool isEmpty = false;
    public static bool Skip = false;

    public static void SetAnimation(SkeletonAnimation animator, AnimationReferenceAsset animation, int track = 0, bool loop = false, float startTime = 0f, float speed = 1f, bool reversed = false, float mixtime = 0.0f)
    {
        if (animation == null)
            return;

        TrackEntry tracks = new();
        tracks = animator.AnimationState.SetAnimation(track, animation, loop);
        tracks.MixTime = mixtime;
        //tracks.Reverse = reversed;
        tracks.AnimationStart = startTime;
    }

    public static void AddAnimation(SkeletonAnimation animator, AnimationReferenceAsset animation,float delay, int track = 0, bool loop = false, float startTime = 0f, float speed = 1f, bool reversed = false)
    {
        if (animation == null)
            return;

        TrackEntry tracks = new();
        tracks = animator.AnimationState.AddAnimation(track, animation, loop, delay);
        //tracks.Reverse = reversed;
        tracks.AnimationStart = startTime;
    }

    public static void SetEmptyAnimation(SkeletonAnimation animator, int track = 0, float time = 0f, bool skip = false)
    {
        isEmpty = true;
        animator.AnimationState.SetEmptyAnimation(track, time + 0.1f);
        Skip = skip;
        Task.Run(async() =>
        {
            await Task.Delay(Mathf.RoundToInt(time * 1000));
            isEmpty = false;
        });
    }

    public static void SetSortedAnimation(SkeletonAnimation animator, List<AnimationReferenceAsset> animations, int dir,
        int track = 0, bool loop = false, float startTime = 0f, float speed = 1f, bool reversed = false, float mixtime = 0f)
    {
        AnimationReferenceAsset animationAsset = GetSortedAnimation(animations, dir);

        SetAnimation(animator, animationAsset, track, loop, startTime, speed, reversed, mixtime);
    }


    static int[] arr = { 1, 7, 3, 5, 0, 4, 2, 6 };
    public static AnimationReferenceAsset GetSortedAnimation(List<AnimationReferenceAsset> animation, int dir)
    {
        if (dir < 0 || dir > 7) 
        {
            Debug.LogError(dir);
            return null;
        }
        return animation[arr[dir]];
    }

    public static int A() => 10;
}
