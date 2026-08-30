# LikeVamSur3D 위키

Unity 6(6000.3.1f1) + URP로 제작 중인 3D 뱀서라이크(Vampire Survivors 스타일) 서바이벌 게임의 개발 문서입니다.

## 목차

- [아키텍처 & 매니저 구조](Architecture.md) — `MANAGER` 싱글턴, 씬 구성, 게임 루프
- [플레이어 시스템](Player.md) — 이동, 자동 조준, 피격 연출
- [몬스터 & 스폰](Monsters.md) — `MONSTER`/`Monster_Movement`, `Spawner`, 상태이상
- [스킬 · 패시브 · 카드](Skills-Cards.md) — `SkillBase` 6종 구현, 패시브, `CardDB`, 카드 선택 UI
- [세션 진행 & UI](Session-Progression.md) — 경험치/레벨/스탯, HUD
- [공용 시스템](Systems.md) — 오브젝트 풀링, 파츠 팩토리 패턴, 유틸리티
- [스크립트 레퍼런스](Script-Reference.md) — 전체 스크립트 목록과 역할

## 빠른 요약

플레이어는 **자동으로 가장 가까운 몬스터를 조준**하며, 보유한 액티브 스킬 카드가 쿨다운마다 자동으로 발동합니다.
`Spawner`가 플레이어 주변 원형 경계에서 몬스터를 계속 스폰하고, 처치 시 드롭되는 경험치 오브를 흡수해 레벨업하면
**시간이 멈추고 3장의 카드(액티브/패시브 혼합) 중 1장을 선택**해 스킬을 성장시키는, Vampire Survivors 특유의
"자동 전투 + 레벨업 빌드" 루프를 3D 환경에 구현한 프로젝트입니다.

## 기술 스택

- Unity 6000.3.1f1, URP(Universal Render Pipeline)
- `CharacterController` 기반 플레이어 이동, `Rigidbody` 기반 몬스터 추적 이동
- `ScriptableObject` 기반 데이터 정의(`CardDB`, `PartDB`) + `Resources.Load`/`LoadAll` 런타임 로드
- 문자열 키 기반 범용 오브젝트 풀(`Pool_Mng`)로 몬스터/투사체/이펙트/UI 텍스트 재사용
- 리플렉션(`Type.GetType`)으로 `CardDB.className`에 대응하는 스킬 컴포넌트를 동적으로 부착

## 참고

- 코드 내 한글 주석 상당수가 파일 인코딩 이슈로 깨져 있습니다(로직 자체에는 영향 없음).
- `Player_Attacker.cs`는 현재 빈 클래스이며, 일부 스크립트에 주석 처리된 이전 구현이 남아있어 아직 리팩토링/정리가 진행 중인 프로토타입 단계입니다.
