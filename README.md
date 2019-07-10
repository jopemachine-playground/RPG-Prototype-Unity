<h2>Outline</h2>
유니티와 RPG 게임에 대해 학습하기 위해 개인적으로 진행한 토이 프로젝트이다.



<h2>Details</h2>
게임은 미로 형식의 던전을 돌아다니며, 출구를 찾는 식으로 진행되는 RPG 게임이다.

기본적으로 던전 맵들은 모두 어둡게 되어 있지만, 출구를 찾은 맵은 쉽게 돌아다닐 수 있도록 환하게 변한다. 

(다른 던전으로 들어가는 입구를 찾으면 출구를 찾았다고 간주함)

4개의 애니메이션으로 이어지는 근접 공격과 폭탄을 던지는 공격, 대시 공격을 구현했고, 인벤토리 및 관련 UI 기능, 아이템 사용, 몬스터와의 상호작용, 랜덤 아이템 스폰, 플레이어 정보 및 게임 진행상황 (플래그 데이터로 관리) 의 세이브 및 로드 등이 구현되어 있다.

몬스터, 아이템, 스킬, 세이브 데이터 등 모든 데이터는 csv와 그에 대응하는 json 파일에 저장되어 있다. 게임에서 불러오는 파일은 json 파일이다. 이 파일들은 모두 `Assets > Custom > Resources`에 저장되어 있다.

게임에서 사용한 애니메이션 파일은 `Assets > Custom > Animation` 에, 

파티클 프리팹은 `Assets > Custom > Fabs > Effect` 에, 

맵에 사용한 프리팹은 `Assets > Custom > Fabs > Map` 에 저장되어 있다.

C#으로 작성된 모든 스크립트 파일은 `Assets > Scripts` 에 저장되어 있다. (에셋 스토어에서 다운로드 받은 몇몇 스크립트는 예외)



<h2> Game Manual</h2>
게임 내 캐릭터 조작 방법은 아래와 같다.  


```
조작 : 일반 방향키 
대시 : Left Shift
카메라 조작 : W, A, S, D
근접 공격 : Z (대시 중 누르면 대시 공격)
폭탄 공격 : X
```




<h2> Development Environment</h2>
스크립트는 모두 C#으로 작성했다. 

사용한 유니티 버전은 아래와 같다.

>Unity 2018.3.0f2 Personal, Unity 2019.3.0a5 Personal



<h2> Asset Sources</h2>
캐릭터의 FBX 모델은 에셋 스토어의 'Unity Chan! Model'이고,

미로 맵에 사용한 에셋은 'Free basic Dungeon', 'LowPoly Dungeon Modules' 이다.

(미로 맵은 맵을 생성하는 스크립트로 생성된 것이다.)

애니메이션의 일부는 'Animation Kick', 'Mixed Motion'을 사용했다.

사운드는 'Free Music Bundle by neocrey', '8-Bit Sfx'를 사용했다.

폰트는 무료폰트 'HS Summer.ttf'를 썼고, 몇몇 raw image는 인터넷 검색으로 구한 것을 포토샵으로 작업 해 넣었다.

애니메이션 스크립트와 Animator는 'Stardard Asset'의 'ThirdPersonControl.cs'를 수정해 사용했다.

인벤토리 시스템의 스크립트 일부는 'Inventory Master-uGUI' 에서 제공되던 cs 스크립트를 수정해 작성했고, 

아이템의 FBX 모델은 'Low Poly RPG Item Pack', 오크 몬스터 애니메이션 및 FBX 모델은 'Orc Sword'을 사용했다.

캐릭터의 카메라 처리는 유니티의 Package Manager에서 Cinemachine을 사용해 처리했다.




<h2> Screen Shots</h2>
