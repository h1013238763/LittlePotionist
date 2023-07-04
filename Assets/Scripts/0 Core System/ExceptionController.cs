using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Exception Handler Module
/// </summary>
public class ExceptionController : BaseController<ExceptionController>
{

    /// <summary>
    /// Receive exception info from other class, analyze it and write to bug log file
    /// </summary>
    /// <param name="exception_source"> which class trigger it</param>
    /// <param name="exception"> the exception</param>
    public void ReceiveException(string exception_source, Exception exception)
    {
        // Debug.LogException(exception);
    }
}