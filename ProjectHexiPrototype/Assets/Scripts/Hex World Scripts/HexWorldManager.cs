using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexWorldManager : BaseSceneManager
{
    public static int DEFAULT_WORLD_ENERGY = 30;
    public static float START_DELAY_AMOUNT = 0.5f;
    public static float RETURN_FROM_COMBAT_DELAY_AMOUNT = 1f;

    public static HexWorldManager instance;
    // set this hex world manager to be the only instance
    public override void Awake()
    {
        base.Awake(); // call base awake
        if (!instance)
        {
            instance = this;
        }
    }

    public HexGrid hex_grid;
    public Transform world;
    public HexEntity player;
    public GameObject hex_point;

    [Header("Hex Enemies")]
    public GameObject hex_entity_enemy;
    public int enemies_to_spawn;
    public int enemy_min_layer;

    private List<HexEntity> hex_enemies;
    private bool combat_init = false;

    [Header("Hex World UI")]
    public TextMeshProUGUI world_energy_text;
    // hex cell button
    public MyObject hex_cell_button;
    public MyObject hex_cell_button_text;
    // hex options
    public Transform hex_option_holder;
    public HexOptionPrefab hex_option_prefab;
    
    // private vars
    private bool inside_hex_cell = false;
    public bool GetInsideHexCell() { return inside_hex_cell; } // public getter
    private int max_world_energy;
    private int current_world_energy;
    private List<HexOptionPrefab> current_hex_option_prefabs;

    void Start()
    {
        // init options list
        current_hex_option_prefabs = new List<HexOptionPrefab>();
        // hide cell button
        hex_cell_button.transform.localScale = Vector3.zero;
        // set texts
        world_energy_text.text = "";


        // determine if loading hex data or creating new hex world
        HexWorldData data = GameManager.instance.GetHexWorldData();
        if (data == null)
        {
            // create new hex world
            hex_grid.CreateGrid();
            // spawn enemies
            SpawnEnemies();
            // set player position
            player.SetStartHexCell(hex_grid.GetHexCellFromCoordinates(Vector3Int.zero));
            // set world energy
            max_world_energy = DEFAULT_WORLD_ENERGY;
            current_world_energy = DEFAULT_WORLD_ENERGY;
            // start after delay
            StartCoroutine(StartDelay(START_DELAY_AMOUNT));
        }
        else
        {
            // load hex world data
            hex_grid.LoadGrid(data.hex_cells, data.world_radius);
            // load in player position and rotation
            player.SetStartHexCell(hex_grid.GetHexCellFromCoordinates(data.player.GetCurrentHexCell().GetHexCoordinates()));
            player.transform.position = data.player_position;
            player.transform.rotation = Quaternion.Euler(data.player_rotation);
            // load in enemies
            LoadEnemies(data.enemies);
            // load in combat enemy
            if (data.return_from_combat)
            {
                HexEntity current_enemy = GetCurrentEnemy();
                current_enemy.transform.position = data.combat_enemy_position;
                current_enemy.transform.rotation = Quaternion.Euler(data.combat_enemy_rotation);
            }
            // set world energy
            max_world_energy = DEFAULT_WORLD_ENERGY;
            current_world_energy = data.world_energy;
            // set camera location + zoom camera on hex cell
            HexCameraController.instance.SetFollowTarget(player.GetCurrentHexCell().transform);
            HexCameraController.instance.SetInsideHexCellFocus();
            HexCameraController.instance.GoToTargetInstant();
            // return from combat routine
            StartCoroutine(ReturnFromCombatRoutine(RETURN_FROM_COMBAT_DELAY_AMOUNT));
        }
    }

    private IEnumerator StartDelay(float delay_amount)
    {
        yield return new WaitForSeconds(delay_amount);
        // set current hex options
        SetHexOptions(hex_grid.GetHexCellFromCoordinates(Vector3Int.zero));
        EnterHexCell();
        // allow player input
        GameManager.instance.allow_player_input = true;
        UpdateUI();
    }

    private HexEntity GetCurrentEnemy()
    {
        foreach (HexEntity enemy_entity in hex_enemies)
        {
            if (enemy_entity.GetCurrentHexCell().GetHexCoordinates() == player.GetCurrentHexCell().GetHexCoordinates())
            {
                return enemy_entity;
            }
        }
        return null;
    }

    private IEnumerator ReturnFromCombatRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        // delete enemy
        HexEntity current_enemy = GetCurrentEnemy();
        if (current_enemy != null)
        {
            current_enemy.Delete();
            hex_enemies.Remove(current_enemy);
            yield return new WaitForSeconds(0.5f);
        }

        // unfocus camera from hex cell and set player as target
        HexCameraController.instance.SetFollowTarget(player.transform);

        // move player to center of hex tile
        player.GetMyObject().MoveToPosition(player.GetCurrentHexCell().transform.position, HexEntity.ENTITY_MOVE_DURATION * 0.5f, true, false);
        yield return new WaitForSeconds(0.5f);

        combat_init = false;
        GameManager.instance.allow_player_input = true;
        ClearHexOptions();
        SetHexOptions(player.GetCurrentHexCell());
        EnterHexCell();
        UpdateUI();
    }

    public void UpdateUI()
    {
        // set world energy amount
        world_energy_text.text = "world energy: " + current_world_energy + "/" + max_world_energy;

        // set hex cell button text
        if (inside_hex_cell)
        {
            hex_cell_button_text.SetTextMeshText("Exit Hex Cell");
        }
        else if (!inside_hex_cell)
        {
            hex_cell_button_text.SetTextMeshText("Enter Hex Cell");
        }
    }

    public bool EnemyInCell(HexCell hex_cell)
    {
        foreach (HexEntity enemy_entity in hex_enemies)
        {
            if (enemy_entity.GetCurrentHexCell().GetHexCoordinates() == hex_cell.GetHexCoordinates())
            {
                return true;
            }
        }
        return false;
    }

    private void LoadEnemies(HexEntity[] enemies)
    {
        hex_enemies = new List<HexEntity>();
        foreach (HexEntity enemy in enemies)
        {
            // spawn enemy
            HexEntity enemy_entity = Instantiate(hex_entity_enemy, world).GetComponent<HexEntity>();
            enemy_entity.SetStartHexCell(hex_grid.GetHexCellFromCoordinates(enemy.GetCurrentHexCell().GetHexCoordinates()));
            hex_enemies.Add(enemy_entity);
        }
    }

    private void SpawnEnemies()
    {
        // create list of eligible hex cells to spawn an enemy
        List<HexCell> eligible_hex_cells = new List<HexCell>();
        eligible_hex_cells.AddRange(hex_grid.GetHexCells());
        // remove cells with layer < enemy_min_layer
        List<HexCell> hex_cells_to_remove = new List<HexCell>();
        foreach (HexCell hex_cell in eligible_hex_cells)
        {
            if (hex_cell.GetHexLayer() < enemy_min_layer)
            {
                hex_cells_to_remove.Add(hex_cell);
            }
        }
        // TODO: remove other ineligible cells 
        foreach (HexCell hex_cell in eligible_hex_cells)
        {
            if (hex_cell.GetHexType() == HexType.Mountain)
            {
                hex_cells_to_remove.Add(hex_cell);
            }
        }
        // remove cells
        foreach (HexCell hex_cell in hex_cells_to_remove)
        {
            eligible_hex_cells.Remove(hex_cell);
        }

        // spawn enemies
        hex_enemies = new List<HexEntity>();
        for (int i = 0; i < enemies_to_spawn; i++)
        {
            // select hex cell
            HexCell cell = eligible_hex_cells[Random.Range(0, eligible_hex_cells.Count)];
            // add hex cell and neighbors to list to remove from eligible list
            hex_cells_to_remove = new List<HexCell>();
            hex_cells_to_remove.Add(cell);
            hex_cells_to_remove.AddRange(cell.GetNeighbors());
            // remove cells 
            foreach (HexCell hex_cell in hex_cells_to_remove)
            {
                eligible_hex_cells.Remove(hex_cell);
            }
            // spawn enemy
            HexEntity enemy_entity = Instantiate(hex_entity_enemy, world).GetComponent<HexEntity>();
            enemy_entity.SetStartHexCell(cell);
            hex_enemies.Add(enemy_entity);
        }
    }

    // use one world energy and update UI
    public void ConsumeWorldEnergy(int amount = 1)
    {
        current_world_energy -= amount;
        UpdateUI();
    }

    // moves player to cell and starts combat if enemy is present
    public void MovePlayerToHexCell(HexCell cell, HexDirection dir)
    {
        StartCoroutine(MovePlayerToHexCellRoutine(cell, dir));
    }
    private IEnumerator MovePlayerToHexCellRoutine(HexCell cell, HexDirection dir)
    {
        // rotate player iff rotation is not (0f, 0f, 0f)
        if (player.transform.rotation.eulerAngles != Vector3.zero)
        {
            player.GetMyObject().ChangeRotation(Vector3.zero, 0.25f);
        }

        // determine if there is an enemy on hex cell
        foreach (HexEntity enemy_entity in hex_enemies)
        {
            if (enemy_entity.GetCurrentHexCell().GetHexCoordinates() == cell.GetHexCoordinates())
            {
                // disable player input and set combat bool
                combat_init = true;
                GameManager.instance.allow_player_input = false;
                bool rotate_enemy = false;
                Vector3 enemy_pos = cell.transform.position;
                Vector3 player_pos = player.transform.position;
                // change enemy and player locations
                if (dir == HexDirection.N || dir == HexDirection.NE || dir == HexDirection.SE)
                {
                    rotate_enemy = true;
                    enemy_pos = HexMetrics.GetInnerPoint(cell.transform.position, HexDirection.SE);
                    player_pos = HexMetrics.GetInnerPoint(cell.transform.position, HexDirection.SW);
                }
                else if (dir == HexDirection.S || dir == HexDirection.SW || dir == HexDirection.NW)
                {
                    enemy_pos = HexMetrics.GetInnerPoint(cell.transform.position, HexDirection.SW);
                    player_pos = HexMetrics.GetInnerPoint(cell.transform.position, HexDirection.SE);
                }
                // move entities to positions
                enemy_entity.GetMyObject().MoveToPosition(enemy_pos, HexEntity.ENTITY_MOVE_DURATION * 0.5f, true, false);
                player.GetMyObject().MoveToPosition(player_pos, HexEntity.ENTITY_MOVE_DURATION, true, false);
                yield return new WaitForSeconds(HexEntity.ENTITY_MOVE_DURATION);
                // rotate entities
                if (rotate_enemy)
                {
                    // rotate enemy
                    enemy_entity.GetMyObject().ChangeRotation(new Vector3(0f, 180f, 0f), 0.25f);
                }
                else
                {
                    // rotate player
                    player.GetMyObject().ChangeRotation(new Vector3(0f, 180f, 0f), 0.25f);
                }
                // zoom camera on hex cell
                HexCameraController.instance.SetFollowTarget(cell.transform);
                HexCameraController.instance.SetInsideHexCellFocus();
                yield return new WaitForSeconds(1f);
                // save current hex world data
                HexWorldData hex_world_data = new HexWorldData();
                hex_world_data.hex_cells = hex_grid.GetHexCells().ToArray();
                hex_world_data.world_radius = hex_grid.grid_radius;
                hex_world_data.enemies = hex_enemies.ToArray();
                hex_world_data.return_from_combat = true;
                hex_world_data.combat_enemy_position = enemy_entity.transform.position;
                hex_world_data.combat_enemy_rotation = enemy_entity.transform.rotation.eulerAngles; 
                hex_world_data.player = player;
                hex_world_data.player_position = player.transform.position;
                hex_world_data.player_rotation = player.transform.rotation.eulerAngles;
                hex_world_data.world_energy = current_world_energy;
                GameManager.instance.SaveHexWorldData(hex_world_data);
                // begin enemy fight
                GameManager.instance.StartCombat(AISystem.instance.DetermineEnemies());
                yield break;
            }
        }
        player.GetMyObject().MoveToTransform(cell.transform, HexEntity.ENTITY_MOVE_DURATION, true, false);
    }

    public void SetHexOptions(HexCell cell)
    {
        List<HexOption> hex_options = cell.GetHexOptions();
        for (int i = 0; i < hex_options.Count; i++)
        {
            HexOptionPrefab new_option_prefab = Instantiate(hex_option_prefab, hex_option_holder);
            new_option_prefab.transform.localScale = Vector3.zero;
            new_option_prefab.SetHexOption(hex_options[i], i + 1);
            current_hex_option_prefabs.Add(new_option_prefab);
        }

        // show hex cell button 
        if (!combat_init && !cell.Discarded)
        {
            hex_cell_button.SquishyChangeScale(1.1f, 1f, 0.1f, 0.1f);
        }
    }

    public void ClearHexOptions()
    {
        foreach (HexOptionPrefab option in current_hex_option_prefabs)
        {
            option.Delete();
        }
        current_hex_option_prefabs.Clear();

        // hide hex cell button
        if (hex_cell_button.transform.localScale.x > 0f && !combat_init)
        {
            hex_cell_button.SquishyChangeScale(1.1f, 0f, 0.1f, 0.1f);
        }
    }

    public void OnHexCellButtonPressed()
    {
        hex_cell_button.SquishyChangeScale(0.9f, 1.0f, 0.1f, 0.1f);

        if (inside_hex_cell)
        {
            ExitHexCell();
        }
        else if (!inside_hex_cell)
        {
            EnterHexCell();
        }

        UpdateUI();
    }

    public void EnterHexCell()
    {
        inside_hex_cell = true;
        // zoom camera on player
        HexCameraController.instance.SetInsideHexCellFocus();
        // show hex cell option(s)
        foreach (HexOptionPrefab option in current_hex_option_prefabs)
        {
            option.my_object.ChangeScale(1f, HexOptionPrefab.OPTION_REVEAL_DURATION);
        }
    }

    public void ExitHexCell()
    {
        inside_hex_cell = false;
        // un-zoom camera on player
        HexCameraController.instance.SetDefaultFocus();
        // hide hex cell option(s)
        foreach (HexOptionPrefab option in current_hex_option_prefabs)
        {
            option.my_object.ChangeScale(0f, HexOptionPrefab.OPTION_REVEAL_DURATION);
        }
        // show available adjacent hex cells
        ShowAvailableAdjacentHexCellOutlines(player.GetCurrentHexCell());
    }

    public void ShowAvailableAdjacentHexCellOutlines(HexCell cell)
    {
        // get all neighbor hex cells
        List<HexCell> neighbor_cells = cell.GetNeighbors();
        // show hex cell outlines if hex cell exists
        foreach (HexCell hex_cell in neighbor_cells)
        {
            if (hex_cell != null)
            {
                // skip cell if not passable
                if (!hex_cell.GetHexPassable())
                {
                    print ("hex cell is not passable: " + hex_cell);
                    continue;
                }

                Color outline_color = Color.white;
                // check if hex cell is discarded
                if (hex_cell.Discarded)
                {
                    outline_color = Color.black;
                }
                else
                {
                    outline_color = Color.cyan;
                }
                // check if hex cell contains enemy
                foreach (HexEntity enemy in hex_enemies)
                {
                    if (enemy.GetCurrentHexCell().GetHexCoordinates() == hex_cell.GetHexCoordinates())
                    {
                        outline_color = Color.red;
                    }
                }
                hex_cell.ShowHexOutline(outline_color);
            }
        }   
    }

    public void ClearAdjacentHexCellOutlines(HexCell cell)
    {
        // get all neighbor hex cells
        List<HexCell> neighbor_cells = cell.GetNeighbors();
        // hide hex cell outlines if hex cell exists
        foreach (HexCell hex_cell in neighbor_cells)
        {
            if (hex_cell != null)
            {
                hex_cell.HideHexOutline();
            }
        }
    }
}
