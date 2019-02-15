using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}

