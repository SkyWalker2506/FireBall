using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    float movePassCheckTime = 1;
    private void OnEnable()
    {
        ActionSystem.OnGameStarted += StartMoving;
        ActionSystem.OnGameEnded += StopMoving;
    }
    private void OnDisable()
    {
        ActionSystem.OnGameStarted -= StartMoving;
        ActionSystem.OnGameEnded -= StopMoving;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (Vector3.Distance(transform.position, other.gameObject.transform.position) < .5f)
            {
                GameManager.Instance.CurrentScore += 10f;
            }
            else
            {
                GameManager.Instance.CurrentScore += 5f;
            }
            ActionSystem.OnPointPassed?.Invoke();
        }
    }

    void StartMoving()
    {
        StartCoroutine(MoveWhenPlayerPass());
    }

    void StopMoving()
    {
        StopCoroutine(MoveWhenPlayerPass());
    }

    IEnumerator MoveWhenPlayerPass()
    {
        while(true)
        {
            yield return new WaitForSeconds(movePassCheckTime);
            if (GameManager.Instance.Player.position.z -5 > transform.position.z)
                ActionSystem.OnPointPassed?.Invoke();
        }
    }
}
