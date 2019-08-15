## Desc
| File Name | Desc | 
|---|:---:|          
| AttackArea.cs | ������ ��뿡�� ��� ������ ��ũ��Ʈ�� �ٿ� ���.|
| AudioManager.cs | �÷��̾�, ����, �Ѿ� �� �Ҹ��� ���� ������Ʈ�� �ڽĿ� ������ ���|
| Damage.cs | �������� ��� �ڷᱸ��. ������ �������� �Ǵ��� Animator�� ���·� �Ǵ��ϱ� ������, attacker�� attackee�� Animator�� �־�� ���� ����. |
| InitDelegator.cs | ��Ȱ��ȭ�� ���·� �����ϰų� �ʱ�ȭ ������ �ΰ���, ���� �������� �ʱ�ȭ �ؾ� �ϴ� Ŭ�������� ������|
| Item.cs | Item Ŭ������ Item�� ���� ������ ��� ���. ������ �ʵ� ���� ǥ�õǴ� �������� �� ���� ������ �����ϴ� PickUpItem ��ũ��Ʈ�� ����ϰ�, Item Ŭ������ json ���Ͽ��� ���� �Ľ��� ��ƾ� �ϱ� ������, MonoBehavior�� ������� ����. (monster Ŭ������ ��������) |
| ItemParser.cs | �����۰� �� �Ӽ����� �Ľ��� ���� ����. Json �Ľ̿� LitJson�� �̿� �������� �Ľ��ϴ� �͸� �ϴ� �� �ƴϰ�, �ڽĿ�����Ʈ�� ���� �ִٰ� �ʿ��� �� ������Ʈ Ǯ������ ������ �� ���İ��� ���ҵ� �� (GeneratePickUpItem) |
| ItemSlot.cs | ItemSlot�� �κ��丮 â���� �ϳ� �ϳ��� ���Կ� ������. ������ ����Ŭ���ϰų�, ��Ŭ�� ���� ���� �̺�Ʈ ó�� ���� ItemSlot���� ��.|
| MonsterAdapter.cs | Monster�� Monobehavior�� ����� �� �����Ƿ� Adapter Ŭ������ �������.  ���� ������Ʈ�� Monster.cs�� �߰��ϱ� ���� ���� Ŭ���� �� ��������, Status�� ��� �÷��̾�, ���� � ��� �� �� �ֱ� ������ �����θ� �ʱ�ȭ���� �����Ƿ�, MonsterAdapter�� �ʱ�ȭ�Ѵ�. |
| MonsterDropItem.cs | monster�� Item Ŭ���� �� � ���� ���� �ʱ�ȭ (�Ľ�)�� �� �� �� ����, DropItem�� �˱� ���ؼ� Item Ŭ������ ID�� ������ �ȴ�. �׷��� Item ��ü�� �ƴ� Item�� ID���� �Ӽ����� �־� MonsterDropItem Ŭ������ �������. |
| MonsterParser.cs | ���Ϳ� ���� ������ json ���Ͽ��� ������.  ItemParser�� ����������, �ʵ� �� ���͵��� ������Ʈ Ǯ������ �����ϱ� ����, �� Ŭ������ ��ħ |
| MonsterPatrolArea.cs | patrol �� ������ ����Ƽ �����Ϳ��� �̸� ���س��´�. ������ �� �������� �Ǵ� �κп� �� ������Ʈ�� ����, �θ� ������Ʈ�� MonsterPatrolArea�� �߰��ϵ��� ����. ���Ͱ� �̵��� �� ���� ������ ������ (�ڽĿ�����Ʈ)�� �θ� ���Ͱ� �̵����� �ʴ� ��ó�� ���̴� ���װ� ���� �� �ִ�. MonsterPatrolArea�� SpawnPoint�� �ڽ����� ��, ���ӵȴ�. (SpawnPoint�� MonsterPatrolArea�� �����ϵ��� ��) |
| MusicManager.cs | �̱������� �������� ���� �����ϸ�, ��ҿ� ���� ��������� �ٲ� ���� ���Ͽ� �ش��ϴ� AudioClip�� ��ҿ� �ش��ϴ� ������Ʈ�鿡�� ���� ���� |
| Scene.cs | Scene�� �ʵ��� ��ũ��Ʈ�鿡�� ����� abstract class.|
| SpawnManager.cs | �������� ���� ��ġ�� �������̳� ������ ������ ������ ������. (�Ǵ� ���� Ȯ���� �������� ����.) ���� ��ġ��, �����Ǵ� ������(�̳� ����) ���� ����Ƽ �����Ϳ��� ���� �� SpawnManager���� �����ϴ� ������Ʈ�� ������ maxSpawnNumber�� ���� �� ���� ������ ó�� ������ ũ�⿡�� �� �þ�� ������, �������� ��� ���� ������� ����Ǵ� �������� ���� �Ѱ谡 ������ ���� �����Ƿ�, ItemPool���� ���� Ǯ���� ������ |
| SpawnPoint.cs | SpawnPoint�� ���Ϳ� �������� �������� ������ ��� ������ ��. Monster�� �����ϴ� ��쿡�� patrolArea�� ������ ����Ѵ�. SpawnPoint�� �߰��� ���ӿ�����Ʈ�� MonsterPatrolArea �̿��� �ڽ�������Ʈ�� �߰����� �� �� |

## Issue
| File Name | Issue | 
|---|:---:|          
| ItemSlot.cs | ���߿� ������ ������� ������ �������� ���� ���� (25��) �ʰ��� ȹ���� ��� ���� ������ �ٲٴ� �������� Ȯ���� ������. |
| Monster.cs | �� Ŭ������ Monobehavior�� ��ӹ��� �ʾƾ� �� �׷��� Adapter Ŭ������ ���� �������. |
| MyHouse.cs | ���� House ���� Village ���� ���� �� ���� ����|
| SightArea.cs | ������ ������ �̿��� �þ߿� �ִ����� �Ǵ���|
| Skill.cs | �÷��̾�� ������ ��ų�� ��� �ڷᱸ��.|
| Status.cs | ���� �÷��̾�, ���Ϳ� �ٿ� ����ϰ� �ִ�, Status ��ũ��Ʈ. ���� ��ũ��Ʈ�� �����ϹǷ�, ������ currentHP�� ���ص� �÷��̾��� ������ ������Ʈ �ϴ� �Լ��� (�ʿ� ���� ��쿡��) ȣ���Ѵٴ� ������ �ִ�. Status�� �����θ� �ʱ�ȭ���� �����Ƿ�, �ٸ� ��ũ��Ʈ���� �ʱ�ȭ �� ����ؾ� �� |

## Reference
| File Name | Reference | 
|---|:---:|          
| AttackArea.cs | http://www.yes24.com/24/goods/27894042|
| HitArea.cs | http://www.yes24.com/24/goods/27894042|
| ItemSlot.cs | https://www.youtube.com/watch?v=-ow-Dp17mYY|
| MonsterControl.cs | Character Controller�� NavMeshAgent�� �Բ� ����ϴ� ���� �Ʒ� ��ũ�� ������ https://forum.unity.com/threads/using-a-navmeshagent-with-a-charactercontroller.466902/  |
| SightArea.cs | https://tenlie10.tistory.com/137|
