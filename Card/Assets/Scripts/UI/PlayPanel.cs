﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : UIBase {

    private Button startBtn;
    private Button RegisterBtn;

	void Start ()
    {
        startBtn = transform.Find("BtnStart").GetComponent<Button>();
        RegisterBtn = transform.Find("BtnRegist").GetComponent<Button>();

        startBtn.onClick.AddListener(StartBtnClick);
        RegisterBtn.onClick.AddListener(RegistBtnClick);
    }

    void StartBtnClick()
    {

    }

    void RegistBtnClick()
    {

    }


    public override void Destroy()
    {
        base.Destroy();

        startBtn.onClick.RemoveAllListeners();
        RegisterBtn.onClick.RemoveAllListeners();
    }

}
