using UnityEngine;
using System.Collections;

interface IQuest
{
    string GetQuestName();
    string GetReplica();
    string GetAnwser(int num);
    bool IsQuestActive();
    void Anwser1();
    void Anwser2();
    void Anwser3();
    void GoalAchieved();
    
}

interface IWeapon
{
    void Shoot(Transform target);
    void Refill();
    int GetAmmoNum();
    void SetAmmoNum(int n);
    Vector3 GetJointOffset();
    string GetWeaponName();
    string GetWeaponTitle();
    string GetWeaponDesc();
    int GetWeaponPrice();
    Vector3 GetGUIRotation();
    Vector3 GetGUIScale();
}


interface IDamageReciever
{
    void DoDamage(int amount);
    int GetHealth();
    int GetMaxHealth();

}

interface ISelectableTarget
{
    string GetName();
}

public class Interfaces : MonoBehaviour {

}
