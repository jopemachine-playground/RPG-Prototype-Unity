// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanRPG
{
    public class MiniMapText: MonoBehaviour
    {
        public static MiniMapText Instance;

        private static Text placeName;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                placeName = GetComponent<Text>();
            }
        }

        public void NameUpdate()
        {
            placeName.text = MapNameIndicator.Instance.mapName.text;
        }

    }
}
