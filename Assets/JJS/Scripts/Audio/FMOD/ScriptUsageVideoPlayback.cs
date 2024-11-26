// This is a Unity behavior script that demonstrates how to capture
// audio data from a VideoPlayer using Unity's AudioSampleProvider and
// play it back through an FMOD.Sound. This example uses the
// VideoPlayer's APIOnly output mode and can be used to get audio from
// a video when UnityAudio is disabled.
//
// Steps to use:
// 1. Add a Unity VideoPlayer component to the same GameObject as this
//    script.
// 2. Untick the VideoPlayer component's Play On Awake option.
// 3. Set the VideoPlayer component's Source to a VideoClip.
// 4. Set the VideoPlayer component's Renderer to a Mesh.
//
// More information on how to configure a Unity VideoPlayer component
// can be found here:
// https://docs.unity3d.com/Manual/class-VideoPlayer.html
//
// For documentation on writing audio data to an FMOD.Sound. See
// https://fmod.com/docs/2.02/api/core-api-sound.html#sound_lock
//
// This document assumes familiarity with Unity scripting. See
// https://unity3d.com/learn/tutorials/topics/scripting for resources
// on learning Unity scripting.
//
//--------------------------------------------------------------------

using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Audio;
using UnityEngine.Experimental.Video;
using UnityEngine.Video;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScriptUsageVideoPlayback : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    private bool _isPlaybackTriggered;
    
    private const int LATENCY_MS = 50;
    private const int DRIFT_MS = 1;
    private const float DRIFT_CORRECTION_PERCENTAGE = 0.5f;

    private AudioSampleProvider mProvider;

    private FMOD.CREATESOUNDEXINFO mExinfo;
    private FMOD.Channel mChannel;
    private FMOD.Sound mSound;

    private List<float> mBuffer = new List<float>();

    private int mSampleRate;
    private uint mDriftThreshold;
    private uint mTargetLatency;
    private uint mAdjustedLatency;
    private int mActualLatency;

    private uint mTotalSamplesWritten;
    private uint mMinimumSamplesWritten = uint.MaxValue;

    private uint mLastReadPosition;
    private uint mTotalSamplesRead;

    private void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        if (_videoPlayer == null)
        {
            Debug.LogWarning("A VideoPlayer is required to use this script. " +
                "See Unity Documentation on how to use VideoPlayer: " +
                "https://docs.unity3d.com/Manual/class-VideoPlayer.html");
            return;
        }

        _videoPlayer.audioOutputMode = VideoAudioOutputMode.APIOnly;
        _videoPlayer.prepareCompleted += Prepared;
        _videoPlayer.loopPointReached += VideoEnded;
        _videoPlayer.Prepare();

        mSampleRate = (int)(_videoPlayer.GetAudioSampleRate(0) * _videoPlayer.playbackSpeed);

        mDriftThreshold = (uint)(mSampleRate * DRIFT_MS) / 1000;
        mTargetLatency = (uint)(mSampleRate * LATENCY_MS) / 1000;
        mAdjustedLatency = mTargetLatency;
        mActualLatency = (int)mTargetLatency;

        mExinfo.cbsize = Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
        mExinfo.numchannels = _videoPlayer.GetAudioChannelCount(0);
        mExinfo.defaultfrequency = mSampleRate;
        mExinfo.length = mTargetLatency * (uint)mExinfo.numchannels * sizeof(float);
        mExinfo.format = FMOD.SOUND_FORMAT.PCMFLOAT;

        FMODUnity.RuntimeManager.CoreSystem.createSound("", FMOD.MODE.LOOP_NORMAL | FMOD.MODE.OPENUSER, ref mExinfo, out mSound);

#if UNITY_EDITOR
        EditorApplication.pauseStateChanged += EditorStateChange;
#endif
    }

#if UNITY_EDITOR
    private void EditorStateChange(PauseState state)
    {
        if (mChannel.hasHandle())
        {
            mChannel.setPaused(state == PauseState.Paused);
        }
    }
#endif

    private void OnDestroy()
    {
        mChannel.stop();
        mSound.release();

#if UNITY_EDITOR
        EditorApplication.pauseStateChanged -= EditorStateChange;
#endif
    }

    private void VideoEnded(VideoPlayer vp)
    {
        if (!vp.isLooping)
        {
            mChannel.setPaused(true);
        }
    }

    private void Prepared(VideoPlayer vp)
    {
        mProvider = vp.GetAudioSampleProvider(0);
        mProvider.sampleFramesAvailable += SampleFramesAvailable;
        mProvider.enableSampleFramesAvailableEvents = true;
        mProvider.freeSampleFrameCountLowThreshold = mProvider.maxSampleFrameCount - mTargetLatency;
    }

    public void StartPlayback()
    {
        if (_videoPlayer == null) return;
        if (_videoPlayer.isPlaying) return;
        _isPlaybackTriggered = true;
        _videoPlayer.Play();
    }

    public void StopPlayback()
    {
        if (_videoPlayer == null) return;
        if (_videoPlayer.isPlaying)
        {
            _isPlaybackTriggered = false;
            _videoPlayer.Stop();
        }
    }

    private void SampleFramesAvailable(AudioSampleProvider provider, uint sampleFrameCount)
    {
        using (NativeArray<float> buffer = new NativeArray<float>((int)sampleFrameCount * provider.channelCount, Allocator.Temp))
        {
            uint written = provider.ConsumeSampleFrames(buffer);
            mBuffer.AddRange(buffer);

            /*
             * Drift compensation
             * If we are behind our latency target, play a little faster
             * If we are ahead of our latency target, play a little slower
             */
            uint samplesWritten = (uint)buffer.Length;
            mTotalSamplesWritten += samplesWritten;

            if (samplesWritten != 0 && (samplesWritten < mMinimumSamplesWritten))
            {
                mMinimumSamplesWritten = samplesWritten;
                mAdjustedLatency = Math.Max(samplesWritten, mTargetLatency);
            }

            int latency = (int)mTotalSamplesWritten - (int)mTotalSamplesRead;
            mActualLatency = (int)((0.93f * mActualLatency) + (0.03f * latency));

            int playbackRate = mSampleRate;
            if (mActualLatency < (int)(mAdjustedLatency - mDriftThreshold))
            {
                playbackRate = mSampleRate - (int)(mSampleRate * (DRIFT_CORRECTION_PERCENTAGE / 100.0f));
            }
            else if (mActualLatency > (int)(mAdjustedLatency + mDriftThreshold))
            {
                playbackRate = mSampleRate + (int)(mSampleRate * (DRIFT_CORRECTION_PERCENTAGE / 100.0f));
            }
            mChannel.setFrequency(playbackRate);
        }
    }

    private void Update()
    {
        /*
         * Need to wait before playing to provide adequate space between read and write positions
         */
        if (!mChannel.hasHandle() && mTotalSamplesWritten > mAdjustedLatency && _isPlaybackTriggered)
        {
            FMOD.ChannelGroup mMasterChannelGroup;
            FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out mMasterChannelGroup);
            FMODUnity.RuntimeManager.CoreSystem.playSound(mSound, mMasterChannelGroup, false, out mChannel);
        }

        if (mBuffer.Count > 0 && mChannel.hasHandle())
        {
            uint readPosition;
            mChannel.getPosition(out readPosition, FMOD.TIMEUNIT.PCMBYTES);

            /*
             * Account for wrapping
             */
            uint bytesRead = readPosition - mLastReadPosition;
            if (readPosition < mLastReadPosition)
            {
                bytesRead += mExinfo.length;
            }

            if (bytesRead > 0 && mBuffer.Count >= bytesRead)
            {
                /*
                 * Fill previously read data with fresh samples
                 */
                IntPtr ptr1, ptr2;
                uint len1, len2;
                var res = mSound.@lock(mLastReadPosition, bytesRead, out ptr1, out ptr2, out len1, out len2);
                if (res != FMOD.RESULT.OK) Debug.LogError(res);

                /*
                 * Though exinfo.format is float, data retrieved from Sound::lock is in bytes,
                 * therefore we only copy (len1+len2)/sizeof(float) full float values across
                 */
                int sampleLen1 = (int)(len1 / sizeof(float));
                int sampleLen2 = (int)(len2 / sizeof(float));
                int samplesRead = sampleLen1 + sampleLen2;
                float[] tmpBuffer = new float[samplesRead];

                mBuffer.CopyTo(0, tmpBuffer, 0, tmpBuffer.Length);
                mBuffer.RemoveRange(0, tmpBuffer.Length);

                if (len1 > 0)
                {
                    Marshal.Copy(tmpBuffer, 0, ptr1, sampleLen1);
                }
                if (len2 > 0)
                {
                    Marshal.Copy(tmpBuffer, sampleLen1, ptr2, sampleLen2);
                }

                res = mSound.unlock(ptr1, ptr2, len1, len2);
                if (res != FMOD.RESULT.OK) Debug.LogError(res);
                mLastReadPosition = readPosition;
                mTotalSamplesRead += (uint)samplesRead;
            }
        }
    }
}
