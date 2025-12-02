using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Mng : MonoBehaviour
{
    private List<SkillBase> activeSkills = new List<SkillBase>();

    public void RegisterSkill(CardDB db, int level)
    {
        SkillBase existing = activeSkills.Find(x => x.skillid == db.id);
        if(existing != null)
        {
            existing.LevelUp(level);
            return;
        }

        SkillBase skill = CreateSkillFromDB(db);
        skill.Initalize(db, level);
        activeSkills.Add(skill);
    }

    SkillBase CreateSkillFromDB(CardDB db)
    {
        string scriptName = db.className;
        Type type = Type.GetType(scriptName); 

        if(type == null || !type.IsSubclassOf(typeof(SkillBase))  )
        {
            Debug.LogError($"[Skill_Mng] 잘못된 스킬 타입 : {scriptName}");
        }

        SkillBase skill = gameObject.AddComponent(type) as SkillBase;
        return skill;
    }

    private void Update()
    {
        foreach(var skill in activeSkills)
        {
            skill.Tick();
        }
    }
}
