//---------------------------------------------------------------------
//  This file is part of the Microsoft .NET Framework SDK Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//This source code is intended only as a supplement to Microsoft
//Development Tools and/or on-line documentation.  See these other
//materials for detailed information regarding Microsoft code samples.
//
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------

using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

//  Identifies the editing control for the DataGridViewMaskedTextBox column type.  It
//  isn't too much different from a regular DataGridViewMaskedTextBox control,
//  except that it implements the IDataGridViewEditingControl interface.
[ToolboxItem(false)]
public class DataGridViewMaskedTextBoxEditingControl : MaskedTextBox, IDataGridViewEditingControl
{
    protected DataGridView dataGridView;
    protected int rowIndex;
    protected bool valueChanged = false;

    public DataGridViewMaskedTextBoxEditingControl()
    {
    }

    //  Notify DataGridView that the value has changed.
    protected virtual void NotifyDataGridViewOfValueChange()
    {
        this.valueChanged = true;
        if (this.dataGridView != null)
        {
            this.dataGridView.NotifyCurrentCellDirty(true);
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        // Let the DataGridView know about the value change
        NotifyDataGridViewOfValueChange();
    }

    #region IDataGridViewEditingControl Members

    //  Returns or sets the parent DataGridView.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DataGridView EditingControlDataGridView
    {
        get
        {
            return this.dataGridView;
        }
        set
        {
            this.dataGridView = value;
        }
    }

    //  Sets/Gets the formatted value contents of this cell.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public object EditingControlFormattedValue
    {
        set
        {
            this.Text = value.ToString();
            NotifyDataGridViewOfValueChange();
        }
        get
        {
            return this.Text;
        }
    }

    //  Indicates the row index of this cell.  This is often -1 for the
    //  template cell, but for other cells, might actually have a value
    //  greater than or equal to zero.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int EditingControlRowIndex
    {
        get
        {
            return this.rowIndex;
        }
        set
        {
            this.rowIndex = value;
        }
    }

    //  Gets or sets our flag indicating whether the value has changed.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool EditingControlValueChanged
    {
        get
        {
            return valueChanged;
        }
        set
        {
            this.valueChanged = value;
        }
    }

    //  Indicates the cursor that should be shown when the user hovers their
    //  mouse over this cell when the editing control is shown.
    public Cursor EditingPanelCursor
    {
        get
        {
            return Cursors.IBeam;
        }
    }

    //  Indicates whether or not the parent DataGridView control should
    //  reposition the editing control every time value change is indicated.
    //  There is no need to do this for the DataGridViewMaskedTextBox.
    public bool RepositionEditingControlOnValueChange
    {
        get
        {
            return false;
        }
    }

    //  Make the DataGridViewMaskedTextBox control match the style and colors of
    //  the host DataGridView control and other editing controls
    //  before showing the editing control.
    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
        this.Font = dataGridViewCellStyle.Font;
        //this.ForeColor = dataGridViewCellStyle.ForeColor;
        //CUSTOM CODE
        this.ForeColor = DataGridViewMaskedTextBoxOptions.foreClr;
        this.BackColor = dataGridViewCellStyle.BackColor;
        this.TextAlign = translateAlignment(dataGridViewCellStyle.Alignment);
    }

    //  Process input key and determine if the key should be used for the editing control
    //  or allowed to be processed by the grid. Handle cursor movement keys for the DataGridViewMaskedTextBox
    //  control; otherwise if the DataGridView doesn't want the input key then let the editing control handle it.
    public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
    {
        switch (keyData & Keys.KeyCode)
        {
            case Keys.Right:
                //
                // If the end of the selection is at the end of the string
                // let the DataGridView treat the key message
                //
                if (!(this.SelectionLength == 0
                      && this.SelectionStart == this.ToString().Length))
                {
                    return true;
                }
                break;

            case Keys.Left:
                //
                // If the end of the selection is at the begining of the
                // string or if the entire text is selected send this character
                // to the dataGridView; else process the key event.
                //
                if (!(this.SelectionLength == 0
                      && this.SelectionStart == 0))
                {
                    return true;
                }
                break;

            case Keys.Home:
            case Keys.End:
                if (this.SelectionLength != this.ToString().Length)
                {
                    return true;
                }
                break;

            case Keys.Prior:
            case Keys.Next:
                if (this.valueChanged)
                {
                    return true;
                }
                break;

            case Keys.Delete:
                if (this.SelectionLength > 0 || this.SelectionStart < this.ToString().Length)
                {
                    return true;
                }
                break;
        }
        //
        // defer to the DataGridView and see if it wants it.
        //
        return !dataGridViewWantsInputKey;
    }

    //   Get the value of the editing control for formatting.
    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
    {
        return this.Text;
    }

    //  Prepare the editing control for edit.
    public void PrepareEditingControlForEdit(bool selectAll)
    {
        if (selectAll)
        {
            SelectAll();
        }
        else
        {
            //
            // Do not select all the text, but position the caret at the
            // end of the text.
            //
            this.SelectionStart = this.ToString().Length;
        }
    }

    #endregion IDataGridViewEditingControl Members

    ///   Routine to translate between DataGridView
    ///   content alignments and text box horizontal alignments.
    private static HorizontalAlignment translateAlignment(DataGridViewContentAlignment align)
    {
        switch (align)
        {
            case DataGridViewContentAlignment.TopLeft:
            case DataGridViewContentAlignment.MiddleLeft:
            case DataGridViewContentAlignment.BottomLeft:
                return HorizontalAlignment.Left;

            case DataGridViewContentAlignment.TopCenter:
            case DataGridViewContentAlignment.MiddleCenter:
            case DataGridViewContentAlignment.BottomCenter:
                return HorizontalAlignment.Center;

            case DataGridViewContentAlignment.TopRight:
            case DataGridViewContentAlignment.MiddleRight:
            case DataGridViewContentAlignment.BottomRight:
                return HorizontalAlignment.Right;
        }
        throw new ArgumentException("Error: Invalid Content Alignment!");
    }
}