# 세션 진행 & UI

## 세션 상태 — `Session_Mng`

[`Session_Mng.cs`](../../LikeVamSur/Assets/00_Scripts/MANAGER/Session_Mng.cs)는 한 판(run) 동안의 플레이어 상태를 전부 보유하는 중앙 상태 저장소입니다. 씬 재시작 없이 계속 누적되는 값이며, 별도 세이브 파일은 없습니다(런 종료 시 초기화되는 로그라이크 방식).

### 기본 값 / 파생 값

| 필드 | 설명 |
| --- | --- |
| `baseDamage`, `DamagePercent` | `Damage` 프로퍼티 = `baseDamage * (1 + DamagePercent/100)` |
| `baseMaxHP`, `HPPercent` | `MaxHP` 프로퍼티 = `baseMaxHP * (1 + HPPercent/100)` |
| `HP` | 현재 체력 (직접 증감) |
| `magnetRadius`, `magnetRadiusPercent` | 자석 반경 기본값 + 퍼센트 보너스 (실제 사용 값은 `Player_Detector.Magnet()`에서 합산) |
| `expPlusPercent` | 경험치 획득량 보너스 |
| `CriticalPercent`, `CriticalDamagePercent` | 치명타 확률/피해 |
| `Exp`, `Level` | 현재 경험치, 레벨 |
| `CurrentWave`, `monsterCount`, `GameTime` | 웨이브(미사용/예비), 현재 몬스터 수, 경과 시간(`Time.unscaledDeltaTime` 누적 — 일시정지 중에도 흐름) |

### 이벤트 (delegate)

UI와의 결합도를 낮추기 위해 값 변경 시 delegate를 호출하는 옵저버 패턴을 사용합니다.

- `onExpChanged(float)` — 경험치 변화 시 (`Base_Canvas.EXPChange`가 구독)
- `onHpChanged(float)` — 체력 변화 시 (`Base_Canvas.HPChanged`가 구독)
- `onMonsterCountChanged(int)` — 몬스터 수 변화 시 (`Base_Canvas.M_CountText`가 구독)
- `onSelectedCard()` — 카드 선택 완료 시 (`Base_Canvas.SetSkillFrame`이 구독, 스킬 UI 프레임 갱신)

### 레벨 & 경험치 곡선 (`AddExp` / `GetRequiredExp`)

- `AddExp(exp)`로 경험치 누적, 요구치(`GetRequiredExp()`)를 넘기면 `Exp = 0`, `Level++`, `Base_Canvas.SelectCard()` 호출(카드 선택 UI 오픈 = 자동으로 `Time.timeScale = 0`)
- 요구 경험치는 레벨 구간별로 기울기가 달라지는 계단형 공식입니다: 1~19레벨은 `lv*10-5`, 20레벨에 `+600` 보정, 20~39레벨은 `lv*13-6`, 40레벨에 `+2400` 보정, 이후 `lv*16-8`

### 치명타 (`GetCritical`)

`Random.value * 100`이 `CriticalPercent` 이하이면 치명타. `MONSTER.GetDamage()`에서 호출되어 실제 피해량에 `CriticalDamagePercent`를 추가로 곱합니다.

## HUD — `Base_Canvas`

[`Base_Canvas.cs`](../../LikeVamSur/Assets/00_Scripts/UI/Base_Canvas.cs)는 싱글턴이며, `Start()`에서 `Session_Mng`의 이벤트들을 구독해 HUD를 갱신합니다.

- **경험치 바**: `EXPFill.fillAmount`, 레벨/퍼센트 텍스트
- **체력 바**: 두 겹의 Fill 이미지(`HpFill`은 즉시 반영, `HPFillSeconds`는 2초에 걸쳐 뒤따라오는 "잔상" 연출)로 피해를 시각적으로 강조
- **타이머**: `Update()`에서 매 프레임 `Utils_UI.FormatTime(GameTime)`으로 `mm:ss` 표시
- **몬스터 수**: 실시간 표시
- **스킬 프레임**(`SetSkillFrame`): 카드 선택이 끝날 때마다 기존 프레임 UI를 전부 파괴하고, 현재 보유한 `SelectedCards`를 기준으로 액티브/패시브 프레임을 다시 생성

## 스킬 쿨다운 UI — `SkillFrame`

[`SkillFrame.cs`](../../LikeVamSur/Assets/00_Scripts/UI/SkillFrame.cs)는 액티브 스킬 카드마다 아이콘 + 레벨 + 쿨다운 게이지를 표시합니다. `SkillBase`의 실제 타이머와는 별개로, UI 전용 코루틴이 카드 데이터의 쿨다운 공식을 다시 계산해 자체적으로 게이지를 채웁니다(스킬 로직과 UI 표시가 별도 타이머로 동작).

## 데미지 폰트 — `DamageTMP`

[`DamageTMP.cs`](../../LikeVamSur/Assets/00_Scripts/UI/DamageTMP.cs)는 피격 위치를 스크린 좌표로 변환해 TMP 텍스트를 표시하고, 위로 튀어오르는 포물선 궤적(중력 시뮬레이션) + 페이드 아웃 후 오브젝트 풀로 반환됩니다. 치명타 여부에 따라 별도의 `Critical` 자식 오브젝트를 활성화합니다.
