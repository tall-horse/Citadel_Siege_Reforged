using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    [SerializeField] private Transform unitPlayer1;
    [SerializeField] private Transform unitPlayer2;
    private Quaternion unitPlayer1SpawnRotation;
    private Quaternion unitPlayer2SpawnRotation;
    void Start()
    {
        TestInput.OnWarrior1Spawned += SpawnUnit1;
        TestInput.OnWarrior2Spawned += SpawnUnit2;
        unitPlayer1SpawnRotation = SetUnitPlayer1SpawnRotation();
        unitPlayer2SpawnRotation = SetUnitPlayer2SpawnRotation();
    }
    void OnDestroy()
    {
        TestInput.OnWarrior1Spawned -= SpawnUnit1;
        TestInput.OnWarrior2Spawned -= SpawnUnit2;
    }
    private void SpawnUnit1()
    {

        Instantiate(unitPlayer1, spawnPoint1.position, unitPlayer1SpawnRotation);
    }

    private void SpawnUnit2()
    {
        Instantiate(unitPlayer2, spawnPoint2.position, unitPlayer2SpawnRotation);
    }
    private Quaternion SetUnitPlayer1SpawnRotation()
    {
        Vector3 unitEuler = unitPlayer1.transform.rotation.eulerAngles;
        Vector3 spawnEuler = spawnPoint1.rotation.eulerAngles;

        Vector3 newEuler = new Vector3(unitEuler.x, spawnEuler.y, unitEuler.z);

        Quaternion unitPlayer1SpawnRotation = Quaternion.Euler(newEuler);
        return unitPlayer1SpawnRotation;
    }
    private Quaternion SetUnitPlayer2SpawnRotation()
    {
        Vector3 unitEuler = unitPlayer2.transform.rotation.eulerAngles;
        Vector3 spawnEuler = spawnPoint2.rotation.eulerAngles;

        Vector3 newEuler = new Vector3(unitEuler.x, spawnEuler.y, unitEuler.z);

        Quaternion unitPlayer2SpawnRotation = Quaternion.Euler(newEuler);
        return unitPlayer2SpawnRotation;
    }

}
