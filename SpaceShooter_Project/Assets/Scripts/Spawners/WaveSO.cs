using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave")]
public class WaveSO : ScriptableObject
{
    // The message to be displayed at the start of a wave
    public string waveMessage = "NEW WAVE";

    // The list of objects that will be spawned. Each spawn contains an object and the number of objects of that type that will be released
    public Spawn[] spawns;

    //public float timeBetweenSpawns;

    // The total time to spawn all the objects in a wave. The individual time for each object is automatically calculated so that they are created all along the timespan
    public float spawnTime = 20.0f;
    internal float spawnTimeTemp;

    // If you set this to true, each group of enemies in the list will be spawned in sequence. Example: 5 buzzsaws spawn one after the other ( with the delay set by "spawnRate" ), and then
    // 10 battleships one after the other. If set to false, the spawn rate for each enemy will be calculated based on "spawnTime", making them spawn in a mixed way rather than in sequence.
    public bool spawnInSequence = false;

    public float waveStartDelay = 0.0f;

}
