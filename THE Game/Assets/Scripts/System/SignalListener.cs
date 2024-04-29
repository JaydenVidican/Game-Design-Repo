using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public GameSignal signal;
    public UnityEvent signalEvent;
    public void OnSignalRaised()
    {
        signalEvent.Invoke();
    }

    void OnEnable()
    {
        signal.RegisterListener(this);
    }
    void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}
