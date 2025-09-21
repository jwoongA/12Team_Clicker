using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Clicker/EnemyData")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int baseHp;
    public int baseGold;
    public int basePoint;
    public Sprite enemySprite;
    public Sprite hitSprite;
}
