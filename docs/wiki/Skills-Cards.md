# 스킬 · 패시브 · 카드

## 카드 데이터 — `CardDB`

[`CardDB.cs`](../../LikeVamSur/Assets/00_Scripts/ScriptableObject/CardDB.cs)는 액티브 스킬과 패시브 능력치를 동일한 하나의 ScriptableObject 스키마로 표현합니다.

| 필드 | 용도 |
| --- | --- |
| `id` | 카드 식별자 겸 표시 이름 |
| `className` | 이 카드가 대응하는 C# 클래스 이름 (액티브는 `SkillBase` 파생 클래스명, 패시브는 `PassiveMng`의 스위치 키) |
| `description` | UI 표시용 설명 (`{0}`에 `baseDamage`가 포맷됨) |
| `state` | `CardState.Active` / `CardState.Passive` |
| `baseCooldown`, `cooldownPerLevel` | 액티브 스킬 쿨다운과 레벨당 감소량 |
| `baseDamage`, `damagePerLevel` | 액티브: 피해 배율(%), 패시브: 효과량(%) — 레벨당 증가량 |

카드 에셋은 `Assets/Resources/DB/Card/Active/Active_01~06.asset`, `Assets/Resources/DB/Card/Passive/Passive_01~06.asset`에 존재하며 `Database_Mng`가 `Resources.LoadAll`로 일괄 로드합니다.

## 액티브 스킬 프레임워크 — `SkillBase`

[`SkillBase.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/SkillBase.cs)는 모든 액티브 스킬의 추상 기반 클래스로, MonoBehaviour이며 **스킬을 획득할 때마다 `Skill_Mng`의 게임오브젝트에 동적으로 컴포넌트로 추가**됩니다.

- `Initalize(CardDB, level)` → `OnInitalize()` 호출, `cooldown = baseCooldown - cooldownPerLevel * (level+1)` 계산
- `LevelUp(newLevel)` → `OnLevelUp()` 호출, 쿨다운 재계산
- `Tick()`: 매 프레임 `timer`를 누적하다 `cooldown`을 넘기면 `Fire()` 실행 후 타이머 리셋
- `Damage()`: `Session_Mng.Damage`(플레이어 기본 공격력) × (카드의 레벨별 피해 퍼센트 / 100)
- 자식 클래스는 `OnInitalize / OnLevelUp / Fire`만 구현하면 됨

### 스킬 등록 흐름 (`Skill_Mng`)

1. 카드를 선택하면 `Session_Mng.SelectedCard(db)` → `Skill_Mng.RegisterSkill(db, level)` 호출
2. `db.state == Active`면: 이미 보유한 스킬이면 `LevelUp()`, 아니면 `Type.GetType(db.className)`으로 클래스 타입을 찾아 **리플렉션으로 컴포넌트를 동적 추가**(`CreateSkillFromDB`)하고 `Initalize()`
3. `db.state == Passive`면: `PassiveMng.SetPassiveCard(db, level)` 호출
4. `Skill_Mng.Update()`가 매 프레임 등록된 모든 액티브 스킬의 `Tick()`을 호출

> `className`은 실제 C# 타입 이름과 정확히 일치해야 하며(리플렉션 조회), 불일치 시 `Debug.LogError`로 경고만 남기고 조용히 실패합니다.

### 스킬 6종 구현

| 클래스 | 파일 | 레벨업 시 성장 | 동작 |
| --- | --- | --- | --- |
| `Skill01_Lightning` | [Skill01_Lightning.cs](../../LikeVamSur/Assets/00_Scripts/Skill/Skill01_Lightning.cs) | 레벨 = 발동 횟수 | 10m 내 무작위 몬스터에게 레벨 수만큼 즉시 낙뢰 피해 |
| `Skill02_Fireball` | [Skill02_Fireball.cs](../../LikeVamSur/Assets/00_Scripts/Skill/Skill02_Fireball.cs) | 발사 개수 `1+2*(lv-1)` | 전방 45° 부채꼴로 화염구(`Bullet`, 화상 부여) 다발 발사 |
| `Skill03_Earthquake` | [Skill03_Earthquake.cs](../../LikeVamSur/Assets/00_Scripts/Skill/Skill03_Earthquake.cs) | 범위/타겟 반경 증가 | 플레이어 위치에 즉발 지진 이펙트, 범위 내 전체 피해 |
| `Skill04_FrostField` | [Skill04_FrostField.cs](../../LikeVamSur/Assets/00_Scripts/Skill/Skill04_FrostField.cs) | 범위 증가 | 최초 획득 시 플레이어를 따라다니는 장판 1개 생성(`FrostField.cs`), 주기적으로 범위 내 피해 |
| `Skill05_Meteor` | [Skill05_Meteor.cs](../../LikeVamSur/Assets/00_Scripts/Skill/Skill05_Meteor.cs) | 낙하 개수 `lv` | 0.2초 간격으로 무작위 위치에 운석 낙하 예고 후 0.35초 뒤 범위 피해 |
| `Skill06_MultiSlash` | [Skill06_MultiSlash.cs](../../LikeVamSur/Assets/00_Scripts/Skill/Skill06_MultiSlash.cs) | 타격 횟수 `2+lv-1`, 범위/크기 증가 | 전방 45° 원뿔 범위(내적 기반 판정) 몬스터를 좌우 번갈아 연속 베기 |

`FrostField.cs`([FrostField.cs](../../LikeVamSur/Assets/00_Scripts/Skill/FrostField.cs))는 스킬 자체가 아니라, 필드 이펙트 오브젝트에 붙어 매 프레임 플레이어 위치를 따라다니게 하는 보조 컴포넌트입니다.

## 패시브 — `PassiveMng`

[`PassiveMng.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/PassiveMng.cs)는 `CardDB.className` 문자열(`Magnet`, `ATK`, `EXP`, `CP`, `CD`, `HP`)로 분기해 `Session_Mng`의 퍼센트 스탯 필드를 갱신합니다. 액티브와 달리 별도 컴포넌트를 만들지 않고 매 선택마다 값을 덮어씁니다(레벨 개념은 있지만 결과값은 "가장 최근 레벨 기준"으로 재계산됨).

| className | 효과 |
| --- | --- |
| `Magnet` | 자석 반경 % 증가 |
| `ATK` | 공격력 % 증가 |
| `EXP` | 경험치 획득량 % 증가 |
| `CP` | 치명타 확률 % |
| `CD` | 치명타 피해 % |
| `HP` | 최대 체력 % 증가 (증가 즉시 `Session_Mng.RefreshHpbyPercent`로 현재 체력 비율 유지) |

## 카드 선택 UI

- [`CardSelector.cs`](../../LikeVamSur/Assets/00_Scripts/UI/CardSelector.cs): `Database_Mng.GetRandomCardSet()`으로 3장을 뽑아 `Card[]` 슬롯에 채움. 카드 클릭 시 `Session_Mng.SelectedCard()` 호출 후 1초 뒤 선택 UI를 닫고 `Time.timeScale`을 복원
- [`Card.cs`](../../LikeVamSur/Assets/00_Scripts/UI/Card.cs): 개별 카드 UI. 마우스 오버/아웃 시 애니메이션 전환, 액티브/패시브에 따라 테두리 색상 구분
- **카드 뽑기 규칙** (`Database_Mng.GetRandomCardSet`, [Architecture](Architecture.md) 참고): 이미 보유 중이며 Lv.5 미만인 카드만 재등장 가능, 액티브 6장·패시브 6장을 모두 보유하면 해당 종류는 더 이상 뽑히지 않음. 레벨업 시 일반적으로는 액티브 1장 + 패시브 1장을 우선 보장 후 나머지 1장을 무작위로 채움 (`AllActive=true`인 최초 진입 시에는 액티브만 3장)
