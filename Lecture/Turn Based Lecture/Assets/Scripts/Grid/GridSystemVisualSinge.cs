using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSinge : MonoBehaviour
{
   [SerializeField] private MeshRenderer meshRenderer;

   public void Show()
   {
      meshRenderer.enabled = true;
   }

   public void Hide()
   {
      meshRenderer.enabled = false;
   }
}
