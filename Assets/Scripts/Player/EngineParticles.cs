using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EngineParticles : MonoBehaviour
{
    public List<ParticleSystem> forwardEngines;
    public List<ParticleSystem> backEngines;
    public List<ParticleSystem> forwardRightEngines;
    public List<ParticleSystem> forwardLeftEngines;
    public List<ParticleSystem> backRightEngines;
    public List<ParticleSystem> backLeftEngines;

    public enum Engine
    {
        MAIN_BACK, 
        MAIN_FORWARD,
        SIDE_FORWARD_RIGHT,
        SIDE_FORWARD_LEFT,
        SIDE_BACK_RIGHT,
        SIDE_BACK_LEFT,
    }

    public void FullForward()
    {
        ChangeEngineStatus(EngineParticles.Engine.MAIN_BACK, true);
        ChangeEngineStatus(EngineParticles.Engine.MAIN_FORWARD, false);
    }

    public void FullBackward()
    {
        ChangeEngineStatus(EngineParticles.Engine.MAIN_FORWARD, true);
        ChangeEngineStatus(EngineParticles.Engine.MAIN_BACK, false);
    }

    public void StopMainEngines()
    {
        ChangeEngineStatus(EngineParticles.Engine.MAIN_FORWARD, false);
        ChangeEngineStatus(EngineParticles.Engine.MAIN_BACK, false);
    }

    public void MoveLeft()
    {
        ChangeEngineStatus(EngineParticles.Engine.SIDE_FORWARD_RIGHT, true);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_BACK_RIGHT, true);
    }

    public void MoveRight()
    {
        ChangeEngineStatus(EngineParticles.Engine.SIDE_FORWARD_LEFT, true);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_BACK_LEFT, true);
    }

    public void RotateLeft()
    {
        ChangeEngineStatus(EngineParticles.Engine.SIDE_BACK_LEFT, true);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_FORWARD_RIGHT, true);
    }

    public void RotateRight()
    {
        ChangeEngineStatus(EngineParticles.Engine.SIDE_BACK_RIGHT, true);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_FORWARD_LEFT, true);
    }

    public void ActivateSideEngines(bool[,] sideEngines)
    {
        ChangeEngineStatus(EngineParticles.Engine.SIDE_FORWARD_LEFT, sideEngines[0,0]);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_FORWARD_RIGHT, sideEngines[0, 1]);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_BACK_LEFT, sideEngines[1, 0]);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_BACK_RIGHT, sideEngines[1, 1]);
    }

    public void StopSideEngines()
    {
        ChangeEngineStatus(EngineParticles.Engine.SIDE_BACK_LEFT, false);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_FORWARD_RIGHT, false);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_BACK_RIGHT, false);
        ChangeEngineStatus(EngineParticles.Engine.SIDE_FORWARD_LEFT, false);
    }

    public void ChangeEngineStatus(Engine engine, bool isOn)
    {
        List<ParticleSystem> enginesToChange;

        switch (engine)
        {
            case Engine.MAIN_BACK:
                enginesToChange = backEngines;
                break;
            case Engine.MAIN_FORWARD:
                enginesToChange = forwardEngines;
                break;
            case Engine.SIDE_FORWARD_RIGHT:
                enginesToChange = forwardRightEngines;
                break;
            case Engine.SIDE_FORWARD_LEFT:
                enginesToChange = forwardLeftEngines;
                break;
            case Engine.SIDE_BACK_RIGHT:
                enginesToChange = backRightEngines;
                break;
            case Engine.SIDE_BACK_LEFT:
                enginesToChange = backLeftEngines;
                break;
            default:
                return;
        }

        foreach (var engineParticle in enginesToChange)
        {
            if (engineParticle.isEmitting == isOn)
            {
                return;
            }

            if (isOn)
            {
                engineParticle.Play();
            } 
            else
            {
                engineParticle.Stop();
            }
        }
    }
}
