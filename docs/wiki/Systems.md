# 공용 시스템 (풀링 / 팩토리 / 유틸)

## 오브젝트 풀링 — `Pool_Mng`

[`Pool_Mng.cs`](../../LikeVamSur/Assets/00_Scripts/MANAGER/Pool_Mng.cs)는 몬스터, 투사체, 이펙트, 데미지 텍스트 등 빈번히 생성/파괴되는 모든 오브젝트를 문자열 키 기반으로 관리하는 범용 풀입니다.

- `Dictionary<string, IPool> m_pool_Dictionary` — 키(예: `"Monster"`, `"Fireball"`, `"DamageFont"`)별로 별도의 풀을 보유
- `Pooling_OBJ(path)`: 해당 키의 풀이 없으면 새로 생성(`Add_Pool`, 씬에 `"{path}##POOL"` 부모 오브젝트 생성), 풀이 비어있으면 `Assets/Resources/POOL/{path}` 프리팹을 로드해 큐에 추가(`Add_Queue`) 후 풀 인스턴스 반환
- 실제 사용은 항상 `MANAGER.POOL.Pooling_OBJ("키").Get(action)` 형태로, `action` 콜백에서 꺼낸 오브젝트의 위치/초기화를 수행

`IPool` / `Object_Pool`([IPool.cs](../../LikeVamSur/Assets/00_Scripts/Interface/IPool.cs))은 내부적으로 `Queue<GameObject>`로 구현되어 있습니다.

```csharp
Get(action)     // 큐에서 Dequeue → SetActive(true) → action 콜백으로 초기화
Return(obj)     // 부모를 풀 오브젝트로 되돌리고 SetActive(false) 후 Enqueue
```

풀링을 사용하는 대표 항목: `Monster`, `Orb`, `DamageFont`, `DeadEffect`, `Fireball`, `Lightning`, `Earthquake`, `FrostField`, `Meteor`, `Sword` (모두 `Assets/Resources/POOL/`에 대응 프리팹 존재)

## 파츠 팩토리 패턴 — `IFactory` / `GenericPartFactory` / `PartDB`

몬스터(및 향후 확장 가능한 다른 엔티티)의 **외형(스킨) 구성을 데이터로 분리**하기 위한 패턴입니다.

- [`PartDB.cs`](../../LikeVamSur/Assets/00_Scripts/ScriptableObject/PartDB.cs): `{id, prefab}` 목록을 담는 ScriptableObject. `Get(id)`는 내부적으로 `Dictionary`로 지연 변환(lazy `ToDictionary`)해 조회
- [`IFactory<T>`](../../LikeVamSur/Assets/00_Scripts/Interface/IFactory.cs): `Build(T entity, string id)` 시그니처만 정의하는 제네릭 인터페이스
- [`GenericPartFactory<T>`](../../LikeVamSur/Assets/00_Scripts/Factory/GenericPartFactory.cs): `Build()` 호출 시 대상 오브젝트의 기존 자식들을 모두 비활성화한 뒤, id와 이름이 같은 자식이 이미 있으면 그것만 활성화하고, 없으면 `PartDB`에서 프리팹을 찾아 자식으로 인스턴스화

`MONSTER.Initialize()`에서 `new GenericPartFactory<MONSTER>(MANAGER.DB.Monster).Build(this, monsterid)` 형태로 사용되어, 몬스터 종류(스켈레톤 등)에 따라 미리 준비된 자식 파츠를 켜는 방식으로 외형을 전환합니다.

## 유틸리티

| 스크립트 | 기능 |
| --- | --- |
| [`Util_Coroutine.cs`](../../LikeVamSur/Assets/00_Scripts/Utils/Util_Coroutine.cs) | `Delay(seconds, action)` — 지연 후 콜백 실행. `ParabolaMove(...)` — 시작/끝 지점 사이를 사인 곡선 높이로 포물선 이동 (경험치 오브 드롭 연출에 사용) |
| [`Utils_UI.cs`](../../LikeVamSur/Assets/00_Scripts/Utils/Utils_UI.cs) | `FormatTime(seconds)` — `mm:ss` 문자열 포맷 |
| [`Utils_World.cs`](../../LikeVamSur/Assets/00_Scripts/Utils/Utils_World.cs) | `GetRandomCircleOffset(radius)` — XZ 평면상의 랜덤 원형 오프셋 (오브 드롭 위치 분산에 사용) |

## 공용 Enum

[`EnumHolder.cs`](../../LikeVamSur/Assets/00_Scripts/Enum/EnumHolder.cs)에는 현재 `Effect_Status { None, Burn }`만 정의되어 있으며, `Bullet`이 몬스터에게 어떤 상태이상을 부여할지 지정하는 데 사용됩니다. `CardState { Active, Passive }`는 [`CardDB.cs`](../../LikeVamSur/Assets/00_Scripts/ScriptableObject/CardDB.cs)에 함께 정의되어 있습니다.
