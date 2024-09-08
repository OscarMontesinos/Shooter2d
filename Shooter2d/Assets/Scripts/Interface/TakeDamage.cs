using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TakeDamage
{
    void TakeDamage(PjBase user,float value);
    void Stunn(float stunnTime);
    void Die(PjBase killer);
}
