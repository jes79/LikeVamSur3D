# 플레이어 시스템

플레이어 관련 컴포넌트는 `Assets/00_Scripts/`에 흩어져 있으며(별도 Player 폴더 없이 루트에 위치), 하나의 플레이어 GameObject에 여러 컴포넌트가 조합되어 동작합니다.

| 컴포넌트 | 역할 |
| --- | --- |
| [`Player.cs`](../../LikeVamSur/Assets/00_Scripts/Player.cs) | 싱글턴, 몬스터 감지/조준 대상 계산, 피격 처리(카메라 셰이크·비네트·이미션 플래시) |
| [`Player_Movenment.cs`](../../LikeVamSur/Assets/00_Scripts/Player_Movenment.cs) | `CharacterController` 기반 이동, 몬스터를 향한 자동 회전, 카메라 추적, 애니메이터 속도 파라미터 |
| [`Player_Detector.cs`](../../LikeVamSur/Assets/00_Scripts/Player_Detector.cs) | 자석 반경 내 경험치 오브(`Orb`) 감지 및 흡수 시작 |
| [`Player_Attacker.cs`](../../LikeVamSur/Assets/00_Scripts/Player_Attacker.cs) | 현재 빈 클래스 (미구현/향후 확장용으로 추정) |

## 이동 & 자동 조준 (`Player_Movenment`)

- `FixedUpdate()`에서 매 프레임 `Move()` → `Rotate()` → `Animate()` → `CameraMove()` 순서로 처리
- `Input.GetAxisRaw("Horizontal"/"Vertical")`로 이동 방향을 구해 `CharacterController.SimpleMove()`로 이동
- 회전은 이동 방향이 아니라 **`Player.instance.target`(가장 가까운 몬스터) 방향을 우선**으로 바라보며, 감지된 몬스터가 없을 때만 이동 방향을 바라봄 — 뱀서라이크 특유의 "이동은 자유, 시선/공격은 자동 조준" 패턴
- 카메라는 매 프레임 플레이어 위치 + `cameraDir` 오프셋을 향해 `Lerp`로 부드럽게 따라감(탑다운 앵글)

## 몬스터 감지 (`Player.cs`)

- `detectionRadius` 반경 내 `monsterLayer`를 `Physics.OverlapSphere`로 검사
- `GetNearestMonster()`: 감지된 몬스터 중 가장 가까운 대상 반환 (스킬의 단일 타겟팅에 사용)
- `GetCollidersHitMonsters(radius)`: 지정 반경 내 스폰이 끝난(`isSpawned == true`) 몬스터 전체 리스트 반환 (범위형 스킬에 사용)
- `target` 프로퍼티는 접근할 때마다 `GetNearestMonster()`를 다시 계산(캐시하지 않음)

## 피격 연출 (`Player.GetDamage`)

몬스터와 충돌(`OnCollisionEnter`, `Monster` 레이어)하면 다음이 동시에 실행됩니다.

1. `FlashEmission` — 렌더러들의 `_EmissionColor`를 흰색으로 플래시했다가 서서히 검은색으로 복귀
2. `CameraShake` — 메인 카메라를 짧게 흔듦
3. `VignettPulse` — URP Volume의 Vignette 효과 강도를 순간적으로 올렸다 내림 (화면 가장자리 붉은 경고 연출)
4. 데미지 폰트(`DamageTMP`)를 오브젝트 풀에서 꺼내 화면에 표시
5. `MANAGER.SESSION.GetDamage(dmg)`로 실제 체력 차감

피격 판정은 `isHIt` 플래그로 `FlashEmission` 코루틴이 끝날 때까지(0.5초) 중복 피격을 막습니다. 현재 데미지 값(`10`)은 코드 내 주석상 임시값으로 하드코딩되어 있습니다.

## 자석(경험치 흡수) — `Player_Detector`

- 매 프레임 `Magnet()`으로 계산한 반경(`Session_Mng.magnetRadius` + 퍼센트 보너스) 내 `orbLayer`를 검사
- 반경 내 `Orb`를 찾으면 `Orb.StartFollow(transform)` 호출 → 오브가 플레이어 쪽으로 튕겨나갔다가 흡수되는 연출 시작 ([몬스터 & 스폰](Monsters.md) 참고)
