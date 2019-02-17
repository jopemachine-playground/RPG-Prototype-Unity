namespace UnityChanRPG
{
    public enum MonsterState
    {
        Idle, // 대기 상태. Wait00

        Chasing, // 플레이어 추적 상태. OverlapSphere 으로 Trigger 발동되면 Chasing 으로 변화하되, 탐색 범위는 몬스터의 앞 쪽을 기준으로 해서 반구를 그려야 됨.

        Roaming, // 랜덤으로 운동. 이동 상태는 랜덤으로 정해지되, 자연스러운 움직임을 위해, 방향이 꺽이지 않고

        Attacking, // 공격 상태

        DashAttacking,

        Airbone, // 공중에 떠 있는 상태

        Stun, // 데미지를 받으면 일정 확률로 스턴 상태에 돌입

        Damaged, // 데미지를 받고 있는 상태

        Death, // 죽어 있는 상태
    }
}