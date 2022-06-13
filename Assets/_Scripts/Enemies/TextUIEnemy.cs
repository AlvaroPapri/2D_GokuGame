using UnityEngine;

public class TextUIEnemy : MonoBehaviour
{
    public int speed;

    private void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
    }
}
