using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// monster와 Item 클래스 중 어떤 것이 먼저 초기화 (파싱)될 지 알 수 없고,
/// DropItem을 알기 위해선 Item 클래스의 ID만 있으면 된다. 그래서 Item 객체가 아닌 Item의 ID만을 속성으로 넣어 MonsterDropItem 클래스를 만들었다.
/// </summary>

namespace UnityChanRPG
{
    [Serializable]
    public class MonsterDropItem
    {
        public int ItemID;
        public float DropProb;
        public int DropMinNumber;
        public int DropMaxNumber;

        public MonsterDropItem(int _ItemID, float _DropProb, int _DropMinNumber, int _DropMaxNumber)
        {
            ItemID = _ItemID;
            DropProb = _DropProb;
            DropMinNumber = _DropMinNumber;
            DropMaxNumber = _DropMaxNumber;
        }
    }
}
