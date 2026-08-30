# 아키텍처 & 매니저 구조

## 씬 구성

프로젝트에는 현재 `Assets/Scenes/SampleScene.unity` 단일 씬만 존재하며, 타이틀/결과 화면 없이 바로 게임 플레이가 시작되는 구조입니다.

## 전역 매니저 — `MANAGER`

[`MANAGER.cs`](../../LikeVamSur/Assets/00_Scripts/MANAGER/MANAGER.cs)는 씬에 배치된 매니저 오브젝트를 `Awake()`에서 싱글턴으로 등록하고(`DontDestroyOnLoad`), 자식으로 붙어있는 4개 하위 매니저를 static 필드로 캐싱해 프로젝트 전역에서 `MANAGER.XXX`로 접근할 수 있게 합니다.

```csharp
MANAGER.POOL     // Pool_Mng     — 오브젝트 풀링
MANAGER.DB       // Database_Mng — 카드/파츠 데이터베이스, 스프라이트 아틀라스
MANAGER.SESSION  // Session_Mng  — 플레이어 스탯/경험치/레벨/시간
MANAGER.SKILL    // Skill_Mng    — 보유 스킬 등록/틱
```

`MANAGER.Run(IEnumerator)`는 `MANAGER` 자신이 `MonoBehaviour`이므로, static 컨텍스트(스킬 클래스 등)에서도 코루틴을 실행할 수 있게 해주는 헬퍼입니다.

## 게임 루프 개요

```
Spawner ── 주기적으로 ──▶ Pool_Mng.Get("Monster") ──▶ MONSTER.Initialize()
                                                              │
                                                    Monster_Movement가 Player를 추적
                                                              │
                                            Player 충돌 시 Player.GetDamage()
                                                              │
Skill_Mng.Update() ── 각 SkillBase.Tick() ──▶ 쿨다운마다 Fire() ──▶ 대상 MONSTER.GetDamage()
                                                              │
                                                     HP <= 0 이면 사망 처리
                                                              │
                                                  DropEXP() → Orb 생성(파라볼라 낙하)
                                                              │
                                     Player_Detector가 자석 반경 내 Orb를 흡수 ──▶ Session_Mng.AddExp()
                                                              │
                                        레벨업 시 Base_Canvas.SelectCard() → 시간 정지 + 카드 3장 선택
```

## 파츠(스킨) 팩토리 패턴

몬스터는 하나의 프리팹(`MONSTER`/`Monster_Movement`)에 여러 종류의 자식 파츠(스켈레톤 스킨 등)를 미리 비활성 상태로 붙여두고, `GenericPartFactory<T>`가 `PartDB`에서 `monsterid`(예: `Skeleton_01`, `Skeleton_02`)에 해당하는 자식만 활성화하거나, 없으면 `PartDB`의 프리팹을 인스턴스화해 붙이는 방식으로 외형을 결정합니다. 자세한 내용은 [공용 시스템](Systems.md) 참고.

## 데이터 흐름

- **카드 데이터**: `Assets/Resources/DB/Card/Active`, `Assets/Resources/DB/Card/Passive` 아래의 `CardDB` ScriptableObject 에셋들을 `Database_Mng.Start()`에서 `Resources.LoadAll`로 전부 로드
- **몬스터 파츠 데이터**: `Assets/Resources/DB/Part/Monster`를 `PartDB`로 로드
- **오브젝트 풀 프리팹**: `Assets/Resources/POOL/{키}` 경로의 프리팹을 `Pool_Mng`가 최초 요청 시점에 지연 로드(lazy load)
