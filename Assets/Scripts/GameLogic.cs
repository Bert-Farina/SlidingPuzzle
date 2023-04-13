using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private TileScript emptyTile;
    [SerializeField] private List<TileScript> tiles;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endPanelTimer;
    
    private Camera _camera;
    private Random _rand;
    private bool _isFinished = false;

    private void Start()
    {
        _camera = Camera.main;
        _rand = new Random();
        tiles.Remove(emptyTile);
        Shuffle();
        tiles.Add(emptyTile);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                var hitTile = hit.transform.GetComponent<TileScript>();
                if (Vector3.Distance(emptyTile.transform.position, hit.transform.position) < 0.75f)
                {
                    //Cambiar variables de posición de las fichas correspondientes
                    (emptyTile.TargetPosition, hitTile.TargetPosition) = (hitTile.TargetPosition, emptyTile.TargetPosition);

                    //Cambiar posición de las casillas en la lista
                    (tiles[tiles.IndexOf(emptyTile)], tiles[tiles.IndexOf(hitTile)]) = (tiles[tiles.IndexOf(hitTile)], tiles[tiles.IndexOf(emptyTile)]);
                }
            }
        }

        if (!_isFinished)
        {
            int correctTiles = 0;
            foreach (var tile in tiles)
            {
                if (tile.inRightPlace)
                {
                    correctTiles++;
                }
            }

            if (correctTiles == tiles.Count)
            {
                _isFinished = true;
                var timer = GetComponent<Timer>();
                timer.StopTimer();
                
                //TimeFormat
                endPanelTimer.text = (timer.minutes < 10 ? "0" : "") + timer.minutes + ":" + (timer.seconds < 10 ? "0" : "") + timer.seconds;
                endPanel.SetActive(true);
            }
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Shuffle()
    {
        //Shuffle the list while the total inversion is not even
        do
        {
            tiles = tiles.OrderBy(_ => _rand.Next()).ToList();
            Debug.Log("Puzzle Shuffled");
        } while (PuzzleInversion() % 2 != 0);
        
        //Reorder the tiles in the interface
        foreach (var tile in tiles)
        {
            var index = tiles.IndexOf(tile);
            if (index == 15)
            {
                tile.TargetPosition = emptyTile.CorrectPosition;
            }
            else
            {
                var targetTile = tiles.Find(item => item.transform.name == $"Tile ({index+1})");
                tile.TargetPosition = targetTile.CorrectPosition;
            }
        }
    }

    private int PuzzleInversion()
    {
        var inversionSum = 0;
        for (var i = 0; i < tiles.Count; i++)
        {
            var tileInversion = 0;
            for (var j = i; j < tiles.Count; j++)
            {
                if (tiles[i].tileNumber > tiles[j].tileNumber)
                {
                    tileInversion++;
                }
            }
            inversionSum += tileInversion;
        }
        return inversionSum;
    }
}
