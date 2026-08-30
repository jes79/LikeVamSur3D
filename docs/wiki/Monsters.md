# 몬스터 & 스폰

## 스폰 — `Spawner`

[`Spawner.cs`](../../LikeVamSur/Assets/00_Scripts/Spawner.cs)는 `spawnInterval`(기본 1초)마다 플레이어 위치를 중심으로 반경 `spawnRadius`(기본 30) 원 둘레의 임의 지점(`GetRandomPointOnCircleEdge`)에 `Pool_Mng`를 통해 몬스터를 꺼내 배치하고, `MONSTER.Initialize(player)`를 호출합니다. 스폰과 동시에 인스턴스화하지 않고 항상 오브젝트 풀을 경유합니다.

## 몬스터 기반 클래스 — `MONSTER`

[`MONSTER.cs`](../../LikeVamSur/Assets/00_Scripts/MONSTER.cs)는 체력/사망/드롭 로직을 담당하는 베이스 클래스입니다.

- `Initialize(player)`: 세션에 몬스터 카운트 추가, HP를 20으로 초기화(현재 하드코딩), `Skeleton_01`/`Skeleton_02` 중 무작위로 외형 결정 후 `GenericPartFactory`로 파츠 부착, 타겟(플레이어) 저장
- `GetDamage(dmg)`: `Session_Mng.GetCritical()`로 치명타 여부를 굴려 실제 피해량 계산 → 데미지 폰트 표시 → HP 차감
  - HP가 0 이하가 되면: 몬스터 카운트 감소, 사망 이펙트 재생 후 0.5초 뒤 풀 반환, 몬스터 오브젝트 자신을 즉시 풀에 반환, `DropEXP()` 호출
- `DropEXP(pos, exp)`: 처치 시 1.0~5.0 사이의 무작위 경험치량을 `{3.0, 1.0, 0.25}` 단위로 최대한 쪼개 여러 개의 `Orb`를 생성 (예: 4.2 exp → 3.0 오브 1개 + 1.0 오브 1개 + 0.25 오브 미만 잔여분은 그대로 1개)

## 이동 — `Monster_Movement : MONSTER`

[`Monster_Movement.cs`](../../LikeVamSur/Assets/00_Scripts/Monster_Movement.cs)는 `MONSTER`를 상속한 구체 구현으로, 실제로 씬에서 사용되는 몬스터 컴포넌트입니다.

- `Initialize()` 오버라이드: 부모 로직 실행 후 즉시 플레이어 방향으로 스냅 회전, `SpawnStartCoroutine`으로 스케일 0 → 목표 크기까지 0.5초간 커지는 스폰 연출 재생, 끝나면 `isSpawned = true`로 전환하고 이동 애니메이션 트리거
- `FixedUpdate()`: 사망했거나(`isDead`) 아직 스폰 연출 중이면(`!isSpawned`) 이동하지 않음. 그 외에는 `Rigidbody.MovePosition`으로 플레이어 방향을 향해 직선 추적 이동 + 회전

`isSpawned` 플래그는 스폰 연출 중인 몬스터가 즉시 공격 대상이나 충돌 판정에 포함되지 않도록 `Player`, `SkillBase`의 타겟 검색 로직에서 함께 검사됩니다.

## 상태이상 — `StatusEffect` / `IStatusEffect`

몬스터에 붙는 [`StatusEffect.cs`](../../LikeVamSur/Assets/00_Scripts/StatusEffect/StatusEffect.cs) 컴포넌트가 `IStatusEffect` 구현체 목록(`activeEffects`)을 매 프레임 `Tick()`하고, `IsFinished`가 되면 `End()` 후 목록에서 제거하는 범용 상태이상 프레임워크입니다.

- [`IStatusEffect.cs`](../../LikeVamSur/Assets/00_Scripts/Skill/IStatusEffect.cs): `Apply / Tick / End / IsFinished` 인터페이스
- [`Burn_Status.cs`](../../LikeVamSur/Assets/00_Scripts/StatusEffect/Burn_Status.cs): 4초간 1초 간격으로 `Session_Mng.Damage * 0.5`만큼 도트 피해. `Bullet`이 `Effect_Status.Burn`을 들고 몬스터에 명중하면 `StatusEffect.ApplyBurn()`이 호출되어 부여됨(중첩 방지, 재적용 시 갱신)

## 투사체 — `Bullet`

[`Bullet.cs`](../../LikeVamSur/Assets/00_Scripts/Bullet.cs)는 스킬(현재 Fireball)이 생성하는 직선 이동형 투사체입니다. 지정 방향으로 등속 이동하다 `Monster` 레이어와 충돌하면 파티클 전환(이동 파티클 → 폭발 파티클), 대미지 적용, 상태이상 부여(`Effect_Status`) 후 `delay` 시간 뒤 풀로 반환됩니다. 5초 내 아무것도 맞지 않으면 자동으로 풀에 반환됩니다.

## 경험치 오브 — `Orb`

[`Orb.cs`](../../LikeVamSur/Assets/00_Scripts/Orb.cs)는 경험치 량에 따라 4단계(3.0 / 1.0 / 0.25 / 그 외)로 크기·색상이 달라집니다.

1. 몬스터 사망 위치 근처 랜덤 오프셋으로 포물선 낙하 연출(`Util_Coroutine.ParabolaMove`) 후 `isIdle = true`
2. `Player_Detector`가 자석 반경 안에서 발견하면 `StartFollow()` 호출
3. 먼저 플레이어 반대 방향으로 살짝 튕겨나갔다가(`ejectDir`), 이후 플레이어를 향해 가속 이동
4. 플레이어에 충분히 가까워지면 `Absorb()` — 풀 반환 + `Session_Mng.AddExp(expValue)` 호출
