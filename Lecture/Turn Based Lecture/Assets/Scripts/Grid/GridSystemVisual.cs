using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
   public static GridSystemVisual Instance { get; private set; }
   
   [SerializeField] private Transform gridSystemVisualSinglePrefab;
   private GridSystemVisualSinge[,] gridStstemSingleArray;

   private void Awake()
   {
      if (Instance != null)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
   }

   private void Start()
   {
      gridStstemSingleArray = new GridSystemVisualSinge[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
      
      for (int x = 0; x <LevelGrid.Instance.GetWidth() ;x++)
      {
         for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
         {
            var gridPosition = new GridPosition(x, z);
            Transform gridSystemVisualSingleTrsnform = Instantiate(gridSystemVisualSinglePrefab,LevelGrid.Instance.GetWorldGridPosition(gridPosition),quaternion.identity);

            gridStstemSingleArray[x, z] = gridSystemVisualSingleTrsnform.GetComponent<GridSystemVisualSinge>();
         }
      }
   }

   private void Update()
   {
      UpdateGridVisual();
   }
   
   public void HideAllGridPosition()
   {
      for (int x = 0; x <LevelGrid.Instance.GetWidth() ;x++)
      {
         for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
         {
            gridStstemSingleArray[x, z].Hide();
         }
      }
   }

   public void ShowGridPositionList(List<GridPosition> gridPositionList)
   {
      foreach (var gridPosition in gridPositionList)
      {
         gridStstemSingleArray[gridPosition.x,gridPosition.z].Show();
      }
   }

   private void UpdateGridVisual()
   {
      HideAllGridPosition();

      BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
      ShowGridPositionList(selectedAction.GetValidActionGridPositionList());
   }
}
