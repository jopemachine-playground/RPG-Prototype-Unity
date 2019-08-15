## Desc
| File Name | Desc | 
|---|:---:|          
| AttackArea.cs | 공격이 상대에게 닿는 영역에 스크립트를 붙여 사용.|
| AudioManager.cs | 플레이어, 몬스터, 총알 등 소리를 내는 컴포넌트의 자식에 부착해 사용|
| Damage.cs | 데미지를 담는 자료구조. 공격이 들어갔는지의 판단을 Animator의 상태로 판단하기 때문에, attacker와 attackee의 Animator가 있어야 생성 가능. |
| DamageIndicator.cs | 성능을 위해 데미지 플로팅 텍스트들을 오브젝트 풀링으로 관리 디폴트 갯수로 만들어 놓았던 플로팅 텍스트 갯수를 넘으면 두 배의 갯수로 만들어 놓는다. |
| DraggingItem.cs | 슬롯에서 드래그 이벤트가 일어나면 DraggingItem으로 아이템의 정보를 복사함.  드래그가 끝난 후의 이벤트 처리 역시 DraggingItem에서 처리함에 주의할 것 |
| FadeInOutObject.cs | Mesh Renderer에서 Material들을 모두 가져와 투명하게 만들어 페이드 아웃 시킨다. 플레이어 캐릭터, 몬스터, 아이템을 페이드 인, 아웃 시킬 때 이용. Screen Cover의 페이드 인, 아웃은 FadeInOutObject가 아닌 Scene에서 구현되어 있음에 주의 (작동 방식이 다름)   코드는 작성했는데, Stardard Shader의 Transparent 렌더링 모드에서, 에셋이 깨진다는 걸 확인. 이것도 Shader에서 하는거라, 아마도 Material을 투명하게 만드는 Shader 코드를 작성해야 되는 것 같다..? 셰이더 쪽은 아직 배우지 못해 일단 미뤄두기로 했다. |
| FloatingTextTweener.cs | 데미지 값을 나타내는 플로팅 텍스트 컴포넌트를 컨트롤. |
| InitDelegator.cs | 비활성화된 상태로 시작하거나 초기화 순서에 민감해, 따로 수동으로 초기화 해야 하는 클래스들을 관리함|
| InventorySystem.cs | 인벤토리 창의 전반적인 UI를 담당하는 클래스. 단, 각각의 슬롯에 대한 정보와 이벤트 처리는 ItemSlot에서 함 아이템들의 아이콘들을 정보에 맞게 업데이트 하는 함수는 InventorySystem에 있다 (ItemIconUpdate) |
| Item.cs | Item 클래스는 Item에 대한 정보를 모두 담당. 실제로 필드 위에 표시되는 아이템은 더 많은 정보를 포함하는 PickUpItem 스크립트를 사용하고, Item 클래스엔 json 파일에서 값을 파싱해 담아야 하기 때문에, MonoBehavior를 상속하지 않음. (monster 클래스도 마찬가지) |
| ItemAttribute.cs | 아이템이 갖고 있는 hp, mp 회복량, 스탯 변동, 특수 효과등을 attribute로 관리함 ItemParser가 게임 시작시에 Handle Item Effect의 각 함수들을 Item 클래스의 delegate에 등록해 ItemSlot이나 Hotbar 클래스에서 사용 요청이 들어오면 해당 아이템이 갖고 있는 Attribute에 맞게 사용한다.  |
| ItemParser.cs | 아이템과 그 속성들을 파싱해 갖고 있음. Json 파싱엔 LitJson을 이용 아이템을 파싱하는 것만 하는 건 아니고, 자식오브젝트로 갖고 있다가 필요할 때 오브젝트 풀링으로 생성할 때 거쳐가는 역할도 함 (GeneratePickUpItem) |
| ItemPool.cs | 몬스터 Pool을 뒤져, 몬스터들이 드롭할 수 있는 아이템들을 미리 생성해놓는다. 필드 위에 자동으로 스폰되는 아이템들은 ItemPool에서 풀링하지 않음 (SpawnManager에서 한다) |
| ItemSlot.cs | ItemSlot은 인벤토리 창에서 하나 하나의 슬롯에 대응함. 슬롯을 더블클릭하거나, 우클릭 했을 때의 이벤트 처리 역시 ItemSlot에서 함.|
| ItemType.cs | 아이템들의 타입을 나타내는 Enum. 후에 종류를 추가해 볼 생각.|
| MonsterAdapter.cs | Monster가 Monobehavior를 상속할 수 없으므로 Adapter 클래스를 만들었다.  몬스터 컴포넌트에 Monster.cs를 추가하기 위해 만든 클래스 일 뿐이지만, Status의 경우 플레이어, 몬스터 등에 모두 들어갈 수 있기 때문에 스스로를 초기화하지 않으므로, MonsterAdapter가 초기화한다. |
| MonsterDropItem.cs | monster와 Item 클래스 중 어떤 것이 먼저 초기화 (파싱)될 지 알 수 없고, DropItem을 알기 위해선 Item 클래스의 ID만 있으면 된다. 그래서 Item 객체가 아닌 Item의 ID만을 속성으로 넣어 MonsterDropItem 클래스를 만들었다. |
| MonsterParser.cs | 몬스터에 대한 정보를 json 파일에서 가져옴.  ItemParser와 마찬가지로, 필드 위 몬스터들을 오브젝트 풀링으로 생성하기 위해, 이 클래스를 거침 |
| MonsterPatrolArea.cs | patrol 할 영역을 유니티 에디터에서 미리 정해놓는다. 영역의 각 꼭지점이 되는 부분에 빈 오브젝트를 놓고, 부모 오브젝트에 MonsterPatrolArea를 추가하도록 했음. 몬스터가 이동할 수 없는 영역에 꼭지점 (자식오브젝트)을 두면 몬스터가 이동하지 않는 것처럼 보이는 버그가 생길 수 있다. MonsterPatrolArea는 SpawnPoint의 자식으로 들어가, 종속된다. (SpawnPoint가 MonsterPatrolArea를 결정하도록 함) |
| MusicManager.cs | 싱글톤으로 전역에서 접근 가능하며, 장소에 따라 배경음악을 바꿈 음악 파일에 해당하는 AudioClip은 장소에 해당하는 컴포넌트들에서 갖고 있음 |
| ParticlePool.cs | enum으로 타입을 분류해, 관리하는 파티클 풀을 다르게 할 수 있다. attackPool과 getItemPool를 이용해 싱글톤처럼 쓰지만, 싱글톤이 아님에 주의. 하지만, 각 파티클 풀은 1개만 있어야 한다. 파티클을 미리 생성하고 활성화 해 놓기 때문에, 모든 파티클은 PlayOnAwake가 false여야 한다. |
| PickUpItem.cs | 필드 위에 표시되는 아이템들을 다루는 클래스. 아이템 말고도, 필드 위에 표시되는 일시적인 회복 아이템이나,  돈 (코인) 역시 PickUpItem에서 처리할 목적으로 만들었다. |
| PlayerInfoSystem.cs | 플레이어의 현재 HP, 경험치 등의 정보를 실제로 UI에 표시하고 관리하는 싱글톤 클래스. Update에서 계속해서, 창을 관리하는 것은 낭비가 커보여, Status 프로퍼티의 값이 변경될 때 마다, PlayerInfoSystem 클래스의 업데이트 함수를 호출하게 했다. |
| Scene.cs | Scene은 맵들의 스크립트들에서 사용할 abstract class.|
| SpawnManager.cs | 랜덤으로 스폰 위치와 아이템이나 몬스터의 종류를 결정해 스폰함. (또는 일정 확률로 스폰하지 않음.) 스폰 위치와, 스폰되는 아이템(이나 몬스터) 들은 유니티 에디터에서 넣을 것 SpawnManager에서 스폰하는 오브젝트의 갯수는 maxSpawnNumber를 넘을 수 없기 때문에 처음 정해진 크기에서 더 늘어나지 않지만, 아이템의 경우 몬스터 사냥으로 드랍되는 아이템은 생성 한계가 정해져 있지 않으므로, ItemPool에서 따로 풀링해 관리함 |
| SpawnPoint.cs | SpawnPoint는 몬스터와 아이템을 랜덤으로 생성할 장소 역할을 함. Monster를 생성하는 경우에만 patrolArea를 가져와 사용한다. SpawnPoint를 추가한 게임오브젝트에 MonsterPatrolArea 이외의 자식컴포넌트를 추가하지 말 것 |

## Issue
| File Name | Issue | 
|---|:---:|          
| ItemSlot.cs | 나중에 페이지 방식으로 구현해 아이템을 슬롯 갯수 (25개) 초과해 획득한 경우 슬롯 내용을 바꾸는 형식으로 확장할 생각임. |
| Monster.cs | 이 클래스는 Monobehavior를 상속받지 않아야 함 그래서 Adapter 클래스를 따로 만들었다. |
| MyHouse.cs | 현재 House 씬과 Village 씬을 오갈 때 버그 있음|
| SightArea.cs | 벡터의 내적을 이용해 시야에 있는지를 판단함|
| Skill.cs | 플레이어와 몬스터의 스킬을 담는 자료구조.|
| Status.cs | 현재 플레이어, 몬스터에 붙여 사용하고 있는, Status 스크립트. 같은 스크립트를 공유하므로, 몬스터의 currentHP가 변해도 플레이어의 정보를 업데이트 하는 함수를 (필요 없는 경우에도) 호출한다는 단점이 있다. Status는 스스로를 초기화하지 않으므로, 다른 스크립트에서 초기화 해 사용해야 함 |

## Reference
| File Name | Reference | 
|---|:---:|          
| AttackArea.cs | http://www.yes24.com/24/goods/27894042|
| HitArea.cs | http://www.yes24.com/24/goods/27894042|
| ItemAttribute.cs | https://assetstore.unity.com/packages/tools/gui/inventory-master-ugui-26310|
| ItemSlot.cs | https://www.youtube.com/watch?v=-ow-Dp17mYY|
| MonsterControl.cs | Character Controller와 NavMeshAgent를 함께 사용하는 법은 아래 링크를 참조함 https://forum.unity.com/threads/using-a-navmeshagent-with-a-charactercontroller.466902/  |
| SightArea.cs | https://tenlie10.tistory.com/137|
