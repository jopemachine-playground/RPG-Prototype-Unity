// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}