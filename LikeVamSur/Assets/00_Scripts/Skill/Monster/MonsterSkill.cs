using System.Collections;
using UnityEngine;

public abstract class MonsterSkill : MonoBehaviour
{
    public abstract IEnumerator CastSkill();
}
