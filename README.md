# Outline

미로 형식의 던전을 돌아다니며, 출구를 찾는 PRG 게임.

기본적으로 던전 맵들은 모두 어둡게 되어 있지만, 출구를 찾은 맵은 쉽게 돌아다닐 수 있도록 환하게 변한다. 

4개의 애니메이션으로 이어지는 근접 공격과 폭탄을 던지는 공격을 구현했고, 인벤토리 및 관련 UI 기능, 몬스터와의 상호작용, 랜덤 아이템 스폰 등이 구현 되어 있다.



# Game Manual

```
조작 : 일반 방향키
카메라 조작 : W, A, S, D
근접 공격 : Z
폭탄 공격 : X
```



# Development Environment

스크립트는 모두 C#으로 작성했다.

>Unity 2018.3.0f2 Personal, Unity 2019.3.0a5 Personal



# Asset Sources

캐릭터의 FBX 모델은 에셋 스토어의 'Unity Chan! Model'이고,

미로 맵에 사용한 에셋은 'Free basic Dungeon', 'LowPoly Dungeon Modules' 이다.

애니메이션의 일부는 'Animation Kick', 'Mixed Motion'을 사용했다.

사운드는 'Free Music Bundle by neocrey', '8-Bit Sfx'를 사용했다.

폰트는 무료폰트 'HS Summer.ttf'를 썼고, 몇몇 raw image는 인터넷 검색으로 구한 것을 포토샵으로 작업 해 넣었다.

애니메이션 스크립트와 Animator는 'Stardard Asset'의 'ThirdPersonControl.cs'를 수정해 사용했다.

인벤토리 시스템의 스크립트 일부는 'Inventory Master-uGUI' 에서 제공되던 cs 스크립트를 수정해 작성했고, 

아이템의 FBX 모델은 'Low Poly RPG Item Pack', 오크 몬스터 애니메이션 및 FBX 모델은 'Orc Sword'을 사용했다.

# Screen Shots
