// C# 에선 하나의 함수에서 2개 이상의 리턴값을 주지 못하므로 구조체를 이용해 Damage를 정의함
// out 키워드를 이용하는 것도 좋을 것 같다.

public struct Damage
{
    public int value;
    public bool IsFatalBlow;

    public Damage(int _value, bool _IsFatalBlow)
    {
        value = _value;
        IsFatalBlow = _IsFatalBlow;
    }
}

