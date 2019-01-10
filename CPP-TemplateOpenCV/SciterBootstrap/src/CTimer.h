/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 

Title : CTimer.h
Author : Chad Vernon
URL : http://www.c-unit.com

Description : Handles timing and frames per second.

Created :  08/09/2005
Modified : 12/03/2005

* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

#pragma once

class CTimer
{
public:
    CTimer();
    void Start();
    void Stop();
    void Update();

    BOOL IsStopped() { return m_timerStopped; }
    float GetFPS() { return m_fps; }
    float GetRunningTime() { return m_runningTime; }
    float GetElapsedTime() { return m_timerStopped ? 0.0f : m_timeElapsed; }

private:
    INT64 m_ticksPerSecond;
    INT64 m_currentTime;
    INT64 m_lastTime;
    INT64 m_lastFPSUpdate;
    INT64 m_FPSUpdateInterval;
    UINT m_numFrames;
    float m_runningTime;
    float m_timeElapsed;
    float m_fps;
    BOOL m_timerStopped;
};