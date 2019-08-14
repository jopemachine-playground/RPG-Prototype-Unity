#--------------------------------------------------------
#
# This file is created by comment helper 2019-08-10
#
#--------------------------------------------------------

# Flags
flag.Author   =   1
flag.Last_Edited   =   0
flag.Desc   =   1
flag.Issue   =   1
flag.Sup_Div_Line   =   1
flag.Sub_Div_Line   =   1
flag.Email   =   0
flag.Telephone   =   0
flag.Github_Account   =   0
flag.Ref_URLs   =   1
flag.Created_Date   =   1
flag.Team   =   0
flag.Memo   =   0

# Globals
global.Extension        =  cs
global.Project_Path     =  C:\UnityChan\Assets\Scripts
global.Author           =  jopemachine
global.Separator        =  //
global.Sub_Div_Line     =  ==============================+===============================================================
global.Sup_Div_Line     =  ==============================+===============================================================
global.Email            =   
global.Telephone        =   
global.Github_Account   =   
global.Team             =   
global.Memo             =   

# Desc
AttackArea.cs::desc       +=       공격이 상대에게 닿는 영역에 스크립트를 붙여 사용.
AudioManager.cs::desc       +=       플레이어, 몬스터, 총알 등 소리를 내는 컴포넌트의 자식에 부착해 사용
Damage.cs::desc       +=       데미지를 담는 자료구조. 공격이 들어갔는지의 판단을 Animator의 상태로 판단하기 때문에,
Damage.cs::desc       +=       attacker와 attackee의 Animator가 있어야 생성 가능.
InitDelegator.cs::desc       +=       비활성화된 상태로 시작하거나 초기화 순서에 민감해, 따로 수동으로 초기화 해야 하는 클래스들을 관리함
Item.cs::desc       +=       Item 클래스는 Item에 대한 정보를 모두 담당. 실제로 필드 위에 표시되는 아이템은 더 많은 정보를 포함하는 PickUpItem 스크립트를
Item.cs::desc       +=       사용하고, Item 클래스엔 json 파일에서 값을 파싱해 담아야 하기 때문에, MonoBehavior를 상속하지 않음. (monster 클래스도 마찬가지)
ItemParser.cs::desc       +=       아이템과 그 속성들을 파싱해 갖고 있음. Json 파싱엔 LitJson을 이용
ItemParser.cs::desc       +=       아이템을 파싱하는 것만 하는 건 아니고, 자식오브젝트로 갖고 있다가 필요할 때 오브젝트 풀링으로 생성할 때 거쳐가는 역할도 함 (GeneratePickUpItem)
ItemSlot.cs::desc       +=       ItemSlot은 인벤토리 창에서 하나 하나의 슬롯에 대응함. 슬롯을 더블클릭하거나, 우클릭 했을 때의 이벤트 처리 역시 ItemSlot에서 함.
MonsterAdapter.cs::desc       +=       Monster가 Monobehavior를 상속할 수 없으므로 Adapter 클래스를 만들었다. 
MonsterAdapter.cs::desc       +=       몬스터 컴포넌트에 Monster.cs를 추가하기 위해 만든 클래스 일 뿐이지만, Status의 경우 플레이어, 몬스터 등에 모두 들어갈 수 있기 때문에
MonsterAdapter.cs::desc       +=       스스로를 초기화하지 않으므로, MonsterAdapter가 초기화한다.
MonsterDropItem.cs::desc       +=       monster와 Item 클래스 중 어떤 것이 먼저 초기화 (파싱)될 지 알 수 없고,
MonsterDropItem.cs::desc       +=       DropItem을 알기 위해선 Item 클래스의 ID만 있으면 된다. 그래서 Item 객체가 아닌 Item의 ID만을 속성으로 넣어 MonsterDropItem 클래스를 만들었다.
MonsterParser.cs::desc       +=       몬스터에 대한 정보를 json 파일에서 가져옴. 
MonsterParser.cs::desc       +=       ItemParser와 마찬가지로, 필드 위 몬스터들을 오브젝트 풀링으로 생성하기 위해, 이 클래스를 거침
MonsterPatrolArea.cs::desc       +=       patrol 할 영역을 유니티 에디터에서 미리 정해놓는다. 영역의 각 꼭지점이 되는 부분에 빈 오브젝트를 놓고, 부모 오브젝트에
MonsterPatrolArea.cs::desc       +=       MonsterPatrolArea를 추가하도록 했음. 몬스터가 이동할 수 없는 영역에 꼭지점 (자식오브젝트)을 두면 몬스터가 이동하지 않는 것처럼 보이는 버그가 생길 수 있다.
MonsterPatrolArea.cs::desc       +=       MonsterPatrolArea는 SpawnPoint의 자식으로 들어가, 종속된다. (SpawnPoint가 MonsterPatrolArea를 결정하도록 함)
MusicManager.cs::desc       +=       싱글톤으로 전역에서 접근 가능하며, 장소에 따라 배경음악을 바꿈
MusicManager.cs::desc       +=       음악 파일에 해당하는 AudioClip은 장소에 해당하는 컴포넌트들에서 갖고 있음
Scene.cs::desc       +=       Scene은 맵들의 스크립트들에서 사용할 abstract class.
SpawnManager.cs::desc       +=       랜덤으로 스폰 위치와 아이템이나 몬스터의 종류를 결정해 스폰함. (또는 일정 확률로 스폰하지 않음.)
SpawnManager.cs::desc       +=       스폰 위치와, 스폰되는 아이템(이나 몬스터) 들은 유니티 에디터에서 넣을 것
SpawnManager.cs::desc       +=       SpawnManager에서 스폰하는 오브젝트의 갯수는 maxSpawnNumber를 넘을 수 없기 때문에 처음 정해진 크기에서 더 늘어나지 않지만, 아이템의 경우
SpawnManager.cs::desc       +=       몬스터 사냥으로 드랍되는 아이템은 생성 한계가 정해져 있지 않으므로, ItemPool에서 따로 풀링해 관리함
SpawnPoint.cs::desc       +=       SpawnPoint는 몬스터와 아이템을 랜덤으로 생성할 장소 역할을 함. Monster를 생성하는 경우에만 patrolArea를 가져와 사용한다.
SpawnPoint.cs::desc       +=       SpawnPoint를 추가한 게임오브젝트에 MonsterPatrolArea 이외의 자식컴포넌트를 추가하지 말 것

# Issue
ItemSlot.cs::issue       +=       나중에 페이지 방식으로 구현해 아이템을 슬롯 갯수 (25개) 초과해
ItemSlot.cs::issue       +=       획득한 경우 슬롯 내용을 바꾸는 형식으로 확장할 생각임.
Monster.cs::issue       +=       이 클래스는 Monobehavior를 상속받지 않아야 함
Monster.cs::issue       +=       그래서 Adapter 클래스를 따로 만들었다.
MyHouse.cs::issue       +=       현재 House 씬과 Village 씬을 오갈 때 버그 있음
SightArea.cs::issue       +=       벡터의 내적을 이용해 시야에 있는지를 판단함
Skill.cs::issue       +=       플레이어와 몬스터의 스킬을 담는 자료구조.
Status.cs::issue       +=       현재 플레이어, 몬스터에 붙여 사용하고 있는, Status 스크립트. 같은 스크립트를 공유하므로, 몬스터의 currentHP가 변해도
Status.cs::issue       +=       플레이어의 정보를 업데이트 하는 함수를 (필요 없는 경우에도) 호출한다는 단점이 있다.
Status.cs::issue       +=       Status는 스스로를 초기화하지 않으므로, 다른 스크립트에서 초기화 해 사용해야 함

# Reference URLs
AttackArea.cs::refURLs       +=       http://www.yes24.com/24/goods/27894042
HitArea.cs::refURLs       +=       http://www.yes24.com/24/goods/27894042
ItemSlot.cs::refURLs       +=       https://www.youtube.com/watch?v=-ow-Dp17mYY
MonsterControl.cs::refURLs       +=       Character Controller와 NavMeshAgent를 함께 사용하는 법은 아래 링크를 참조함
MonsterControl.cs::refURLs       +=       https://forum.unity.com/threads/using-a-navmeshagent-with-a-charactercontroller.466902/ 
SightArea.cs::refURLs       +=       https://tenlie10.tistory.com/137

# Excluded files
DraggingEventHandler.cs::exclude
File Name::exclude
File Name::exclude
File Name::exclude
File Name::exclude
DraggingEventHandler.cs::exclude
File Name::exclude
File Name::exclude
File Name::exclude
File Name::exclude
DraggingEventHandler.cs::exclude
File Name::exclude
File Name::exclude
File Name::exclude
File Name::exclude
DraggingEventHandler.cs::exclude
File Name::exclude
File Name::exclude
File Name::exclude
File Name::exclude
DraggingEventHandler.cs::exclude
File Name::exclude
File Name::exclude
File Name::exclude
File Name::exclude
