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
flag.Created_Date   =   0
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
Damage.cs::desc       +=       데미지를 담는 자료구조. 공격이 들어갔는지의 판단을 Animator의 상태로 판단하기 때문에,
Damage.cs::desc       +=       attacker와 attackee의 Animator가 있어야 생성 가능.
AttackArea.cs::desc       +=       공격이 상대에게 닿는 영역에 스크립트를 붙여 사용.
SpawnPoint.cs::desc       +=       SpawnPoint는 몬스터와 아이템을 랜덤으로 생성할 장소 역할을 함. Monster를 생성하는 경우에만 patrolArea를 가져와 사용한다.
SpawnPoint.cs::desc       +=       SpawnPoint를 추가한 게임오브젝트에 MonsterPatrolArea 이외의 자식컴포넌트를 추가하지 말 것
Scene.cs::desc       +=       Scene은 맵들의 스크립트들에서 사용할 abstract class.
Item.cs::desc       +=       Item 클래스는 Item에 대한 정보를 모두 담당. 실제로 필드 위에 표시되는 아이템은 더 많은 정보를 포함하는 PickUpItem 스크립트를
Item.cs::desc       +=       사용하고, Item 클래스엔 json 파일에서 값을 파싱해 담아야 하기 때문에, MonoBehavior를 상속하지 않음. (monster 클래스도 마찬가지)
ItemParser.cs::desc       +=       아이템과 그 속성들을 파싱해 갖고 있음. Json 파싱엔 LitJson을 이용
ItemParser.cs::desc       +=       아이템을 파싱하는 것만 하는 건 아니고, 자식오브젝트로 갖고 있다가 필요할 때 오브젝트 풀링으로 생성할 때 거쳐가는 역할도 함 (GeneratePickUpItem)
ItemSlot.cs::desc       +=       ItemSlot은 인벤토리 창에서 하나 하나의 슬롯에 대응함. 슬롯을 더블클릭하거나, 우클릭 했을 때의 이벤트 처리 역시 ItemSlot에서 함.

# Issue
Status.cs::issue       +=       현재 플레이어, 몬스터에 붙여 사용하고 있는, Status 스크립트. 같은 스크립트를 공유하므로, 몬스터의 currentHP가 변해도
Status.cs::issue       +=       플레이어의 정보를 업데이트 하는 함수를 (필요 없는 경우에도) 호출한다는 단점이 있다.
Status.cs::issue       +=       Status는 스스로를 초기화하지 않으므로, 다른 스크립트에서 초기화 해 사용해야 함
Skill.cs::issue       +=       플레이어와 몬스터의 스킬을 담는 자료구조.
SightArea.cs::issue       +=       벡터의 내적을 이용해 시야에 있는지를 판단함
ItemSlot.cs::issue       +=       나중에 페이지 방식으로 구현해 아이템을 슬롯 갯수 (25개) 초과해
MyHouse.cs::issue       +=       현재 House 씬과 Village 씬을 오갈 때 버그 있음
ItemSlot.cs::issue       +=       획득한 경우 슬롯 내용을 바꾸는 형식으로 확장할 생각임.

# Reference URLs
SightArea.cs::refURLs       +=       https://tenlie10.tistory.com/137
HitArea.cs::refURLs       +=       http://www.yes24.com/24/goods/27894042
AttackArea.cs::refURLs       +=       http://www.yes24.com/24/goods/27894042
ItemSlot.cs::refURLs       +=       https://www.youtube.com/watch?v=-ow-Dp17mYY
