using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Manage all the elements
/// </summary>
public class PatternListener : MonoBehaviour
{
    public static PatternListener Instance;

    protected PatternCoordinator patternCoordinator;

    protected void Start()
    {
        patternCoordinator = gameObject.GetComponent<PatternCoordinator>();
        distBetweenElements = patternCoordinator.distBetweenElements;
        Instance = this;
    }

    private float distBetweenElements = 0.75f;


}
