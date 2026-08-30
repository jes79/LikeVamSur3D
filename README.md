# LikeVamSur3D

"Vampire Survivors" 스타일의 3D 로그라이크 서바이벌(뱀서라이크) 게임 프로토타입입니다. 몰려오는 몬스터 무리 속에서 자동/반자동 공격 스킬로 살아남고, 레벨업 시 카드를 골라 스킬을 성장시키는 것이 핵심 루프입니다.

## 게임 개요

| 항목 | 내용 |
| --- | --- |
| 장르 | 3D 탑다운 뱀서라이크(bullet-heaven) 서바이벌 |
| 엔진 | Unity 6000.3.1f1 (Unity 6) |
| 렌더링 파이프라인 | URP (Universal Render Pipeline) |
| 씬 | `SampleScene` (단일 씬) |
| 카드 시스템 | 액티브 스킬 6종, 패시브 6종 (`Resources/DB/Card`) |

## 조작법

| 입력 | 동작 |
| --- | --- |
| 방향키(←→↑↓) / WASD 축(`Horizontal`, `Vertical`) | 이동 |
| 자동 조준·자동 공격 | 근처 몬스터를 자동으로 바라보고, 보유한 액티브 스킬이 쿨다운마다 자동 발동 |
| (레벨업 시) 마우스 클릭 | 카드 선택 UI에서 3장 중 1장 선택 |

이동은 아직 레거시 `Input` 클래스(`Input.GetAxisRaw`)를 사용하며, 패키지 목록엔 `com.unity.inputsystem`이 포함되어 있으나 아직 스크립트에는 적용되지 않았습니다.

## 핵심 게임 루프

1. `Spawner`가 플레이어 주변 원형 경계에서 몬스터를 주기적으로 스폰
2. 플레이어는 자동으로 가장 가까운 몬스터를 바라보며, 보유한 액티브 스킬이 쿨다운마다 자동 발동해 공격
3. 몬스터 처치 시 경험치 오브(`Orb`)를 드롭 → 자석 반경(`magnetRadius`) 안에 들어오면 플레이어에게 흡수
4. 경험치가 일정량 쌓이면 레벨업 → 시간 정지 후 액티브/패시브 카드 3장 중 1장 선택 (`CardSelector`)
5. 선택한 카드가 이미 보유 중이면 레벨업(최대 Lv.5), 새 카드면 신규 스킬로 등록
6. 몬스터에게 맞으면 체력 감소, 0 이하가 되면 게임 오버 흐름으로 연결

## 스킬 6종 (액티브 카드)

| 스킬 | 동작 |
| --- | --- |
| Lightning | 레벨 수만큼 무작위 대상에게 즉시 번개 낙뢰 |
| Fireball | 전방으로 화염구 발사, 레벨이 오르면 부채꼴로 다발 발사 (화상 효과 부여) |
| Earthquake | 플레이어 주변 범위 내 몬스터에게 즉시 피해 |
| FrostField | 플레이어를 따라다니는 냉기 장판, 주기적으로 범위 피해 |
| Meteor | 무작위 위치에 순차적으로 운석 낙하, 낙하 지점 범위 피해 |
| MultiSlash | 전방 부채꼴 범위의 몬스터를 연속 베기로 공격 |

## 패시브 6종

자석 반경, 공격력, 경험치 획득량, 치명타 확률, 치명타 피해량, 최대 체력을 퍼센트로 증가시킵니다. (`PassiveMng` 참고)

## 프로젝트 구조

```
LikeVamSur3D/
└─ LikeVamSur/                     # Unity 프로젝트 루트
   ├─ Assets/
   │  ├─ Scenes/SampleScene.unity
   │  ├─ 00_Scripts/
   │  │  ├─ MANAGER/                # 전역 싱글턴 매니저 (Pool/DB/Session/Skill)
   │  │  ├─ Skill/                  # SkillBase + 6개 스킬 구현체, 패시브
   │  │  ├─ StatusEffect/           # 상태이상(화상 등)
   │  │  ├─ ScriptableObject/       # CardDB, PartDB 데이터 정의
   │  │  ├─ Factory/, Interface/    # 파츠 팩토리 패턴(IFactory, IPool)
   │  │  ├─ UI/                     # HUD, 카드 선택, 데미지 폰트, 스킬 프레임
   │  │  ├─ Enum/                   # 공용 enum
   │  │  ├─ Utils/                  # 코루틴/시간 포맷/랜덤 좌표 헬퍼
   │  │  └─ (Player*.cs, MONSTER.cs, Monster_Movement.cs, Spawner.cs, Bullet.cs, Orb.cs)
   │  ├─ Resources/
   │  │  ├─ DB/Card/Active, Passive # 카드 ScriptableObject 에셋 (Resources.LoadAll 로드)
   │  │  ├─ DB/Part                 # 몬스터 파츠(스킨) DB
   │  │  └─ POOL/                   # 오브젝트 풀링용 프리팹 (Monster, Orb, Fireball 등)
   │  ├─ 02_prefabs/, 03_Animation/, 04_Materials/, 05_Images/
   │  └─ AssetPackages/              # 외부 에셋(Hovl Studio, Lana Studio, VFX_Klaus 등)
   ├─ Packages/                      # UPM 패키지 매니페스트
   └─ ProjectSettings/
└─ docs/wiki/                        # 프로젝트 위키 문서
```

## 시작하기

1. **Unity Hub**에서 Unity `6000.3.1f1` (또는 호환되는 Unity 6 버전) 설치
2. Unity Hub → `Open` → 이 저장소의 `LikeVamSur` 폴더 선택
3. `Assets/Scenes/SampleScene.unity` 씬을 열고 실행

```bash
git clone https://github.com/jes79/LikeVamSur3D.git
```

## 주요 패키지

- `com.unity.render-pipelines.universal` / `com.unity.shadergraph` — URP 렌더링
- `com.unity.inputsystem` — 신규 입력 시스템 (현재 이동 로직은 레거시 Input 사용 중)
- `com.unity.ai.navigation` — 내비게이션 (현재 몬스터 이동은 NavMesh 대신 Rigidbody 추적 방식)
- `com.unity.multiplayer.center`, `com.unity.timeline`, `com.unity.visualscripting`

전체 목록은 [`Packages/manifest.json`](LikeVamSur/Packages/manifest.json) 참고.

## 문서

- [위키 홈](docs/wiki/Home.md)
- [아키텍처 & 매니저 구조](docs/wiki/Architecture.md)
- [플레이어 시스템](docs/wiki/Player.md)
- [몬스터 & 스폰](docs/wiki/Monsters.md)
- [스킬 · 패시브 · 카드](docs/wiki/Skills-Cards.md)
- [세션 진행 & UI](docs/wiki/Session-Progression.md)
- [공용 시스템 (풀링/팩토리/유틸)](docs/wiki/Systems.md)
- [스크립트 레퍼런스](docs/wiki/Script-Reference.md)
