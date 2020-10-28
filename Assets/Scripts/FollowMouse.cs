using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition); //pointer position relative to world camera position.
        transform.position = new Vector2(mouseLocation.x, mouseLocation.y); //applied to game object position, excluding z pos as thats how close to cam and irrelevant.
    }
}
