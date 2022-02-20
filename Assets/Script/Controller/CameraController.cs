﻿using System;
using Cinemachine;
using UnityEngine;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    private new CinemachineVirtualCamera camera;
    private Transform player;

    private void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player").transform;
        camera.Follow = player;
    }
}