# 스크립트 레퍼런스

`Assets/00_Scripts/` 아래 전체 40개 C# 스크립트를 폴더별로 정리한 표입니다. 링크는 저장소 내 실제 경로입니다.

## 루트 (`00_Scripts/`)

| 스크립트 | 역할 |
| --- | --- |
| [`Player.cs`](../../LikeVamSur/Assets/00_Scripts/Player.cs) | 플레이어 싱글턴, 몬스터 탐지/조준, 피격 연출(카메라 셰이크, 비네트, 이미션 플래시) |
| [`Player_Movenment.cs`](../../LikeVamSur/Assets/00_Scripts/Player_Movenment.cs) | 이동, 자동 조준 회전, 카메라 추적, 애니메이터 연동 |
| [`Player_Detector.cs`](../../LikeVamSur/Assets/00_Scripts/Player_Detector.cs) | 자석 반경 내 경험치 오브 감지/흡수 시작 |
| [`Player_Attacker.cs`](../../LikeVamSur/Assets/00_Scripts/Player_Attacker.cs) | 빈 클래스 (미구현) |
| [`MONSTER.cs`](../../LikeVamSur/Assets/00_Scripts/MONSTER.cs) | 몬스터 베이스: HP/피격/사망/경험치 드롭 |
| [`Monster_Movement.cs`](../../LikeVamSur/Assets/00_Scripts/Monster_Movement.cs) | `MONSTER` 상속, 스폰 연출 + 플레이어 추적 이동 |
| [`Spawner.cs`](../../LikeVamSur/Assets/00_Scripts/Spawner.cs) | 플레이어 주변 원형 경계에 주기적으로 몬스터 스폰 |
| [`Bullet.cs`](../../LikeVamSur/Assets/00_Scripts/Bullet.cs) | 직선 이동 투사체, 명중 시 피해+상태이상 부여 |
| [`Orb.cs`](../../LikeVamSur/Assets/00_Scripts/Orb.cs) | 경험치 오브: 포물선 드롭 → 자석 흡수 → 경험치 지급 |

## MANAGER (`00_Scripts/MANAGER/`)

| 스크립트 | 역할 |
| --- | --- |
| [`MANAGER.cs`](../../LikeVamSur/Assets/00_Scripts/MANAGER/MANAGER.cs) | 전역 싱글턴, 하위 매니저(POOL/DB/SESSION/SKILL) static 캐싱, 코루틴 실행 헬퍼 |
| [`Pool_Mng.cs`](../../LikeVamSur/Assets/00_Scripts/MANAGER/Pool_Mng.cs) | 문자열 키 기반 범용 오브젝트 풀 (`Object_Pool` 포함) |
| [`Database_Mng.cs`](../../LikeVamSur/Assets/00_Scripts/MANAGER/Database_Mng.cs) | 카드/파츠 DB 로드, 스프라이트 아틀라스, 레벨업 카드 3장 랜덤 추출 |
| [`Session_Mng.cs`](../../LikeVamSur/Assets/00_Scripts/MANAGER/Session_Mng.cs) | 플레이어 세션 상태(HP/스탯/경험치/레벨/시간), 이벤트 delegate |
| [`Skill_Mng.cs`](../../LikeVamSur/Assets/00_Scripts/MANAGER/Skill_Mng.cs) | 액티브 스킬 등록(리플렉션 동적 컴포넌트 추가) 및 매 프레임 Tick |

## Skill (`00_Scripts/Skill/`)

| 스크립트 | 역할 |
| --- | --- |
| [`SkillBase.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/SkillBase.cs) | 액티브 스킬 추상 기반: 쿨다운/레벨업/데미지 계산/타겟 조회 공통 로직 |
| [`Skill01_Lightning.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/Skill01_Lightning.cs) | 무작위 대상 낙뢰 (레벨=발동 횟수) |
| [`Skill02_Fireball.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/Skill02_Fireball.cs) | 전방 부채꼴 화염구 다발 발사 (화상 부여) |
| [`Skill03_Earthquake.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/Skill03_Earthquake.cs) | 플레이어 중심 범위 즉발 피해 |
| [`Skill04_FrostField.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/Skill04_FrostField.cs) | 추적형 냉기 장판, 주기 범위 피해 |
| [`Skill05_Meteor.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/Skill05_Meteor.cs) | 무작위 위치 순차 운석 낙하 |
| [`Skill06_MultiSlash.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/Skill06_MultiSlash.cs) | 전방 원뿔 범위 연속 베기 |
| [`PassiveMng.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/PassiveMng.cs) | 6종 패시브 카드 → `Session_Mng` 퍼센트 스탯 반영 |
| [`IStatusEffect.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/IStatusEffect.cs) | 상태이상 공통 인터페이스 (`Apply/Tick/End/IsFinished`) |
| [`FrostField.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/FrostField.cs) | 냉기 장판 이펙트가 플레이어를 따라다니게 하는 보조 컴포넌트 |

## StatusEffect (`00_Scripts/StatusEffect/`)

| 스크립트 | 역할 |
| --- | --- |
| [`StatusEffect.cs`](../../LikeVamSur/Assets/00_Scripts/StatusEffect/StatusEffect.cs) | 몬스터에 부착되는 상태이상 목록 관리자 (매 프레임 Tick/만료 처리) |
| [`Burn_Status.cs`](../../LikeVamSur/Assets/00_Scripts/StatusEffect/Burn_Status.cs) | 화상 상태이상: 4초간 1초 간격 도트 피해 |

## ScriptableObject (`00_Scripts/ScriptableObject/`)

| 스크립트 | 역할 |
| --- | --- |
| [`CardDB.cs`](../../LikeVamSur/Assets/00_Scripts/ScriptableObject/CardDB.cs) | 액티브/패시브 카드 데이터 정의, `SelectCard`/`CardState` |
| [`PartDB.cs`](../../LikeVamSur/Assets/00_Scripts/ScriptableObject/PartDB.cs) | id→프리팹 매핑 파츠(스킨) 데이터 |

## Factory / Interface (`00_Scripts/Factory/`, `00_Scripts/Interface/`)

| 스크립트 | 역할 |
| --- | --- |
| [`GenericPartFactory.cs`](../../LikeVamSur/Assets/00_Scripts/Factory/GenericPartFactory.cs) | `PartDB` 기반으로 엔티티의 자식 파츠를 활성화/생성 |
| [`IFactory.cs`](../../LikeVamSur/Assets/00_Scripts/Interface/IFactory.cs) | 파츠 팩토리 공통 인터페이스 |
| [`IPool.cs`](../../LikeVamSur/Assets/00_Scripts/Interface/IPool.cs) | 오브젝트 풀 공통 인터페이스 |

## UI (`00_Scripts/UI/`)

| 스크립트 | 역할 |
| --- | --- |
| [`Base_Canvas.cs`](../../LikeVamSur/Assets/00_Scripts/UI/Base_Canvas.cs) | HUD 총괄: 경험치/체력 바, 타이머, 몬스터 수, 스킬 프레임, 카드 선택 트리거 |
| [`Card.cs`](../../LikeVamSur/Assets/00_Scripts/UI/Card.cs) | 개별 카드 UI(아이콘/설명/호버 애니메이션) |
| [`CardSelector.cs`](../../LikeVamSur/Assets/00_Scripts/UI/CardSelector.cs) | 레벨업 시 카드 3장 표시 및 선택 처리, 시간 정지/재개 |
| [`DamageTMP.cs`](../../LikeVamSur/Assets/00_Scripts/UI/DamageTMP.cs) | 포물선 궤적의 플로팅 데미지 텍스트 (풀링) |
| [`SkillFrame.cs`](../../LikeVamSur/Assets/00_Scripts/UI/SkillFrame.cs) | 보유 액티브 스킬의 아이콘/레벨/쿨다운 게이지 UI |

## Enum (`00_Scripts/Enum/`)

| 스크립트 | 역할 |
| --- | --- |
| [`EnumHolder.cs`](../../LikeVamSur/Assets/00_Scripts/Enum/EnumHolder.cs) | `Effect_Status { None, Burn }` |

## Utils (`00_Scripts/Utils/`)

| 스크립트 | 역할 |
| --- | --- |
| [`Util_Coroutine.cs`](../../LikeVamSur/Assets/00_Scripts/Utils/Util_Coroutine.cs) | 지연 실행, 포물선 이동 코루틴 헬퍼 |
| [`Utils_UI.cs`](../../LikeVamSur/Assets/00_Scripts/Utils/Utils_UI.cs) | 시간 `mm:ss` 포맷 |
| [`Utils_World.cs`](../../LikeVamSur/Assets/00_Scripts/Utils/Utils_World.cs) | 랜덤 원형 오프셋 좌표 계산 |
