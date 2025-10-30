using NUnit.Framework; // 단위 테스트 프레임워크 (실제로는 사용되지 않음)
using System.Collections.Generic; 
using System.Linq; // LINQ 쿼리 메서드(ToDictionary 등) 사용을 위한 네임스페이스
using UnityEngine;

[System.Serializable] // Unity 에디터에서 이 클래스를 직렬화(저장/표시) 가능하게 만듦
public class PartData
{
    public string id; // 파트를 식별하는 고유 ID (예: "wheel", "engine")
    public GameObject prefab; // 실제 파트의 프리팹(게임오브젝트 템플릿)
}



/// <summary>
/// ScriptableObject: Unity의 특수한 에셋 타입
///프리팹이나 씬에 속하지 않고 독립적인 데이터 에셋으로 존재
///메모리 효율적이고 여러 곳에서 공유 가능
/// </summary>

[CreateAssetMenu(fileName = "Scriptable", menuName = "DB/Part", order = int.MinValue)]
public class PartDB : ScriptableObject
{
    public List<PartData> parts;

    private Dictionary<string, GameObject> partMap;

    public GameObject Get(string id)
    {
        // 1. Lazy Initialization (지연 초기화)
        if (partMap == null)
        {
            // parts 리스트를 딕셔너리로 변환
            // LINQ의 ToDictionary: 각 요소(p)를 키-값 쌍으로 변환
            partMap = parts.ToDictionary(
                p => p.id,      // 키 선택자: PartData의 id를 키로 사용
                p => p.prefab   // 값 선택자: PartData의 prefab을 값으로 사용
            );
        }

        // 2. 안전한 값 반환
        // 삼항 연산자: 조건 ? 참일때값 : 거짓일때값
        return partMap.ContainsKey(id)  // 해당 id가 딕셔너리에 있는지 확인
            ? partMap[id]               // 있으면 해당 프리팹 반환
            : null;                     // 없으면 null 반환
    }
}
