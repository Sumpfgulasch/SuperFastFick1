// using UnityEngine;
//
// public class EnemyManager : MonoBehaviour
// {
//     public static EnemyManager Instance;
//
//     private int fleeingEnemies = 0;
//     public int FleeingEnemies {
//         get {
//             return fleeingEnemies;
//         }
//         set {
//             fleeingEnemies = value;
//             AudioManager.Instance.SetGlobalParameter(FmodParameter.FLEEING_ENEMIES, fleeingEnemies);
//         }
//     }
//     
//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
//     void Start()
//     {
//         
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         
//     }
// }
