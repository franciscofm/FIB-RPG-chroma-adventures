using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour {
    CharacterMovement player;
    public float xOffset = .25f;

    [System.Serializable]
    public class Area
    {
        public float position = 0;
        public float size = 1;
        public float speed = 1;
        public Color color = Color.black;
        public int layer = 0;
    }

    public Area[] areas;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        SetPlayerSize();
    }

    // Update is called once per frame
    void LateUpdate () {
        SetPlayerSize();
	}

    void SetPlayerSize()
    {
        float yPos = player.transform.position.y;
        int index = -1;
        for (int nIndex = 0; nIndex < areas.Length; ++nIndex)
        {
            if (yPos <= areas[nIndex].position) break;
            ++index;
        }
        if (index == -1)
        {
            player.transform.localScale = Vector3.one * areas[0].size;
            player.speedMod = areas[0].speed;
            player.SetLayer(areas[0].layer);
        }
        else if (index == areas.Length - 1)
        {
            player.transform.localScale = Vector3.one * areas[index].size;
            player.speedMod = areas[index].speed;
            player.SetLayer(areas[index].layer);
        }
        else
        {
            float diff = (yPos - areas[index].position * transform.parent.parent.localScale.y) / (areas[index + 1].position * transform.parent.parent.localScale.y - areas[index].position * transform.parent.parent.localScale.y);
            player.transform.localScale = Vector3.one * Mathf.Lerp(areas[index].size, areas[index + 1].size, diff);
            player.speedMod = Mathf.Lerp(areas[index].speed, areas[index + 1].speed, diff);
            player.SetLayer(areas[index+1].layer);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 from = transform.position;
        from.x += xOffset;
        Vector3 to = from;
        for (int i = 1; i < areas.Length; ++i)
        {
            Gizmos.color = areas[i - 1].color;
            from.y = areas[i - 1].position*transform.parent.parent.localScale.y;
            to.y = areas[i].position*transform.parent.parent.localScale.y;
            Gizmos.DrawLine(from, to);
            from.x -= 2 * xOffset;
            to.x -= 2 * xOffset;
            Gizmos.DrawLine(from, to);
            from.x += 2 * xOffset;
            to.x += 2 * xOffset;
        }
    }
}
