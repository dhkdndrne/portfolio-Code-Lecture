using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(RectTransform))]
public class ScrollRecycleController<T> : MonoBehaviour
{
	protected List<T> tableData = new(); // 리스트 항목의 데이터를 저장

	[SerializeField]
	protected GameObject cellBase = null; // 복사 원본 셀

	[SerializeField]
	private RectOffset padding; // 스크롤할 내용의 패딩

	[SerializeField]
	private float spacingHeight; //각 셀의 간격

	[SerializeField]
	private RectOffset visibleRectPadding = null; // visibleRect의 패딩

	private LinkedList<ScrollRecycleCell<T>> cells = new();

	private Rect visibleRect;      //리스트 항목을 셀의 형태로 표시하는 범위를 나타내는 사각형
	private Vector2 prevScrollPos; //바로 전의 스크롤 위치 저장

	private ScrollRect CachedScrollRect => GetComponent<ScrollRect>();
	private RectTransform CachedRectTransform => GetComponent<RectTransform>();

	protected virtual void Start()
	{
		// 복사 원본 셀은 비활성화해둔다
		cellBase.SetActive(false);

		// Scroll Rect 컴포넌트의 OnvalueChanged 이벤트의 이벤트 리스너를 설정하다
		CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);
	}

	protected void InitializeTableView()
	{
		UpdateScrollViewSize();
		UpdateVisibleRect();

		if (cells.Count < 1)
		{
			//셀이 하나도 없을떄는 visibleREct의 범위에 들어가는 첫번째 리스트 항목을 찾아 그에 대응하는 셀을 작성
			Vector2 cellTop = new Vector2(0, -padding.top);

			for (int i = 0; i < tableData.Count; i++)
			{
				float cellHeight = GetCellHeightAtIndex(i);
				Vector2 cellBottom = cellTop + new Vector2(0, -cellHeight);

				if ((cellTop.y <= visibleRect.y && cellTop.y >= visibleRect.y - visibleRect.height) ||
				    (cellBottom.y <= visibleRect.y && cellBottom.y >= visibleRect.y - visibleRect.height))
				{
					ScrollRecycleCell<T> cell = CreateCellForIndex(i);
					cell.Top = cellTop;
					break;
				}
				cellTop = cellBottom + new Vector2(0, spacingHeight);
			}
		}
		else
		{
			LinkedListNode<ScrollRecycleCell<T>> node = cells.First;
			UpdateCellForIndex(node.Value, node.Value.Index);
			node = node.Next;

			while (node != null)
			{
				UpdateCellForIndex(node.Value, node.Previous.Value.Index + 1);
				node.Value.Top = node.Previous.Value.Bottom + new Vector2(0, -spacingHeight);
				node = node.Next;
			}
		}
		SetFillVisibleRectWithCells();
	}
	
	protected virtual float GetCellHeightAtIndex(int index)
	{
		return cellBase.GetComponent<RectTransform>().sizeDelta.y;
	}
	protected void UpdateScrollViewSize()
	{
		float contentHeight = 0;

		for (int i = 0; i < tableData.Count; i++)
		{
			contentHeight += GetCellHeightAtIndex(i);

			if (i > 0)
			{
				contentHeight += spacingHeight;
			}
		}

		Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
		sizeDelta.y = padding.top + contentHeight + padding.bottom;
		CachedScrollRect.content.sizeDelta = sizeDelta;
	}
	
	private ScrollRecycleCell<T> CreateCellForIndex(int i)
	{
		GameObject obj = Instantiate(cellBase) as GameObject;
		obj.SetActive(true);
		ScrollRecycleCell<T> cell = obj.GetComponent<ScrollRecycleCell<T>>();

		Vector3 scale = cell.transform.localScale;
		Vector2 sizeDelta = cell.CachedRectTransform.sizeDelta;
		Vector2 offsetMin = cell.CachedRectTransform.offsetMin;
		Vector2 offsetMax = cell.CachedRectTransform.offsetMax;

		cell.transform.SetParent(cellBase.transform.parent);

		cell.transform.localScale = scale;
		cell.CachedRectTransform.sizeDelta = sizeDelta;
		cell.CachedRectTransform.offsetMin = offsetMin;
		cell.CachedRectTransform.offsetMax = offsetMax;

		UpdateCellForIndex(cell, i);

		cells.AddLast(cell);
		return cell;
	}
	
	private void UpdateCellForIndex(ScrollRecycleCell<T> cell, int index)
	{
		cell.Index = index;

		if (cell.Index >= 0 && cell.Index <= tableData.Count - 1)
		{
			cell.gameObject.SetActive(true);
			cell.UpdateContent(tableData[cell.Index]);
			cell.Height = GetCellHeightAtIndex(cell.Index);
		}
		else
		{
			cell.gameObject.SetActive(false);
		}
	}
	
	private void UpdateVisibleRect()
	{
		visibleRect.x = CachedScrollRect.content.anchoredPosition.x + visibleRectPadding.left;
		visibleRect.y = -CachedScrollRect.content.anchoredPosition.y + visibleRectPadding.top;

		visibleRect.width = CachedRectTransform.rect.width + visibleRectPadding.left + visibleRectPadding.right;
		visibleRect.height = CachedRectTransform.rect.height + visibleRectPadding.top + visibleRectPadding.bottom;
	}
	
	private void SetFillVisibleRectWithCells()
	{
		if (cells.Count < 1)
			return;

		ScrollRecycleCell<T> lastCell = cells.Last.Value;
		int nextCellDataIndex = lastCell.Index + 1;
		Vector2 nextCellTop = lastCell.Bottom + new Vector2(0, -spacingHeight);

		while (nextCellDataIndex < tableData.Count && nextCellTop.y >= visibleRect.y - visibleRect.height)
		{
			ScrollRecycleCell<T> cell = CreateCellForIndex(nextCellDataIndex);
			cell.Top = nextCellTop;

			lastCell = cell;
			nextCellDataIndex = lastCell.Index + 1;
			nextCellTop = lastCell.Bottom + new Vector2(0, -spacingHeight);
		}
	}
	
	public void OnScrollPosChanged(Vector2 ScrollPos)
	{
		UpdateVisibleRect();
		UpdateCells((ScrollPos.y < prevScrollPos.y) ? 1 : -1);

		prevScrollPos = ScrollPos;
	}
	private void UpdateCells(int scrollDirection)
	{
		if (cells.Count < 1) return;

		if (scrollDirection > 0)
		{
			ScrollRecycleCell<T> firstCell = cells.First.Value;
			while (firstCell.Bottom.y > visibleRect.y)
			{
				ScrollRecycleCell<T> lastCell = cells.Last.Value;
				UpdateCellForIndex(firstCell, lastCell.Index + 1);
				firstCell.Top = lastCell.Bottom + new Vector2(0, -spacingHeight);

				cells.AddLast(firstCell);
				cells.RemoveFirst();
				firstCell = cells.First.Value;
			}

			SetFillVisibleRectWithCells();
		}
		else if (scrollDirection < 0)
		{
			ScrollRecycleCell<T> lastCell = cells.Last.Value;
			while (lastCell.Top.y < visibleRect.y - visibleRect.height)
			{
				ScrollRecycleCell<T> firstCell = cells.First.Value;
				UpdateCellForIndex(lastCell, firstCell.Index - 1);
				lastCell.Bottom = firstCell.Top + new Vector2(0, spacingHeight);

				cells.AddFirst(lastCell);
				cells.RemoveLast();
				lastCell = cells.Last.Value;
			}
		}
	}
}