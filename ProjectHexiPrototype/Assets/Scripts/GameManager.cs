using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static float SCENE_TRANSITION_TIME = 3f;

    public override void Awake()
    {
        base.Awake();
    }

    // TODO: move this somewhere else - not here
    public List<CardData> default_knight_deck;
    public bool allow_player_input = false;
    public GameObject default_scene_transition;
    

    private List<EnemyData> current_enemy_data;
    public List<EnemyData> GetEnemyData() { return current_enemy_data; } // public getter

    public void StartScene()
    {
        // find overlay canvas
        Transform overlay_canvas = GameObject.FindGameObjectWithTag("OverlayCanvas").transform;
        // instantiate transition object
        MyObject transition = Instantiate(default_scene_transition, overlay_canvas).GetComponent<MyObject>();
        transition.SetImageAlpha(1f);
        // start transition
        transition.ChangeImageAlpha(0f, SCENE_TRANSITION_TIME * 0.5f);
        StartCoroutine(DeleteTransitionRoutine(transition, SCENE_TRANSITION_TIME * 0.5f));
    }
    private IEnumerator DeleteTransitionRoutine(MyObject transition, float delay_time)
    {
        yield return new WaitForSeconds(delay_time);
        Destroy(transition.gameObject);
    }

    public void GoToScene(string scene_name)
    {
        // find overlay canvas
        Transform overlay_canvas = GameObject.FindGameObjectWithTag("OverlayCanvas").transform;
        // instantiate transition object
        MyObject transition = Instantiate(default_scene_transition, overlay_canvas).GetComponent<MyObject>();
        transition.SetImageAlpha(0f);
        // start transition
        transition.ChangeImageAlpha(1f, SCENE_TRANSITION_TIME * 0.5f);
        StartCoroutine(GoToSceneRoutine(scene_name, SCENE_TRANSITION_TIME * 0.5f));
    }
    private IEnumerator GoToSceneRoutine(string scene_name, float delay_time)
    {
        yield return new WaitForSeconds(delay_time);
        SceneManager.LoadScene(scene_name);
    }

    public void StartCombat(List<EnemyData> enemy_data)
    {
        // TODO: save hex map state
        current_enemy_data = new List<EnemyData>();
        current_enemy_data.AddRange(enemy_data);
        GoToScene("CombatScene");
    }

    public void ReturnToHexMap()
    {
        // TODO: load hex map state
        GoToScene("HexWorld");
    }
}
