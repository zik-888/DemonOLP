﻿using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
#pragma warning disable 0649
    public class Example03ScrollView : FancyScrollView<Example03CellDto, Example03ScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;

        new void Awake()
        {
            scrollPositionController.OnUpdatePosition.AddListener(UpdatePosition);

            // Add OnItemSelected event listener
            scrollPositionController.OnItemSelected.AddListener(CellSelected);

            SetContext(new Example03ScrollViewContext { OnPressedCell = OnPressedCell });
            base.Awake();
        }

        public void UpdateData(List<Example03CellDto> data)
        {
            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            UpdateContents();
        }

        void OnPressedCell(Example03ScrollViewCell cell)
        {
            scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
            context.SelectedIndex = cell.DataIndex;
            UpdateContents();
        }

        // An event triggered when a cell is selected.
        void CellSelected(int cellIndex)
        {
            // Update context.SelectedIndex and call UpdateContents for updating cell's content.
            context.SelectedIndex = cellIndex;
            UpdateContents();
        }
    }
}
