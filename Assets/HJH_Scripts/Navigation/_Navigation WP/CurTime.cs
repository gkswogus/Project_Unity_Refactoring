using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CurTime : MonoBehaviour
{
   [SerializeField]private TMP_Text time;

    private void Update()
    {
        time.text = string.Format("    {0}",  DateTime.Now.ToString("HH:mm"));

    }
}
