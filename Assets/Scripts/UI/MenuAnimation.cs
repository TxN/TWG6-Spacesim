using UnityEngine;

public class MenuAnimation : MonoBehaviour {
    public GameObject Ship              = null;
    public GameObject Camera            = null;
    public int        MoveChanceInverse = 100;
	public float      MinMoveTime       = 2f;

	Vector3 moveTg;   
    float lastMoveT = 0f;

	void Awake() {
		Time.timeScale = 1f;
	}

    void Start() {
        moveTg = Camera.transform.localPosition;
    }

    void Update() {
        if (Time.time > lastMoveT + MinMoveTime) {
			var check = Random.value;
			if ( check < MoveChanceInverse ) {
				lastMoveT = Time.time;
				moveTg = Random.insideUnitSphere + Camera.transform.localPosition;
			}
        }     
        Camera.transform.localPosition = Vector3.Slerp(Camera.transform.localPosition, moveTg, Time.deltaTime * 0.1f);
        Ship.transform.Translate(Vector3.right * 0.5f);
    }
}
