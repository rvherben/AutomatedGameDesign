using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public Transform gridCellHolder;

    //All cells
    List<GridCell> _gridCells;

    //Accumulated heat in the current wave per cell
    List<float> _cellsHeatThisWave;

    //The heat every cell starts with at the start of the game
    const float _DEFAULT_STARTING_HEAT_PER_CELL = 20;

    //The rate of which cells heat deteriorate every wave
    const float _DETERIORATION_RATE = 1f;

    //The rate of which heat increases per kill
    const float _HEAT_INCREASE_PER_KILL = 5f;

    bool _diagnosticsActive = true;

    void Start ()
    {
        _gridCells = new List<GridCell>();
        _cellsHeatThisWave = new List<float>();

        //Initalization of lists
        for (int i = 0; i<gridCellHolder.childCount; i++)
        {
            _gridCells.Add(gridCellHolder.GetChild(i).GetComponent<GridCell>());
            _cellsHeatThisWave.Add(0);
        }

        //Initializing and setting starting heat for each cell
        foreach(GridCell cell in _gridCells)
        {
            cell.Init(_DEFAULT_STARTING_HEAT_PER_CELL);
            cell.UpdateCell();
        }

        //Makes sure diagnostics are off at startup
        HandleDiagnosticsButtonPressed();
    }
	
    //Handler for when a kill occurs within a cell
	public void HandleKillInCell(int number)
    {
        //Updating the accumulated heat this wave for the cell in which the kill occured
        _cellsHeatThisWave[number] += _HEAT_INCREASE_PER_KILL;
    }

    //Handler for when the wave is over and the next wave needs to be started
    public void HandleEndOfWave()
    {
        for(int i = 0; i < _gridCells.Count; i++)
        {
            //Set new cell heat
            _gridCells[i].Heat -= _DETERIORATION_RATE;
            if (_gridCells[i].Heat < 0)
            {
                _gridCells[i].Heat = 0;
            }

            _gridCells[i].Heat += _cellsHeatThisWave[i];
            if (_gridCells[i].Heat > 100)
            {
                _gridCells[i].Heat = 100;
            }

            //Reset accumulated heat for this wave
            _cellsHeatThisWave[i] = 0;
            
            //Update cells
            _gridCells[i].UpdateCell();
        }  
    }

    //Handles reset button and resets all cells to starting state
    public void HandleReset()
    {
        for(int i = 0; i<_cellsHeatThisWave.Count; i++)
        {
            _cellsHeatThisWave[i] = 0;
            _gridCells[i].Heat = _DEFAULT_STARTING_HEAT_PER_CELL;
            _gridCells[i].UpdateCell();
        }
    }

    //Handles show diagnostics button
    public void HandleDiagnosticsButtonPressed()
    {
        _diagnosticsActive = !_diagnosticsActive;

        foreach (GridCell cell in _gridCells)
        {
            cell.SetDiagnosticsActive(_diagnosticsActive);
        }
    }
}
