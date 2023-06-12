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

namespace Extenso.Windows.Forms.Controls;

//  The base object for the custom column type.  Programmers manipulate
//  the column types most often when working with the DataGridView, and
//  this one sets the basics and Cell Template values controlling the
//  default behaviour for cells of this column type.
public class DataGridViewMaskedTextBoxColumn : DataGridViewColumn
{
    //CUSTOM CODE
    private Color foreColor;

    private bool includeLiterals;
    private bool includePrompt;
    private string mask;
    private char promptChar;
    private Type validatingType;

    //  Initializes a new instance of this class, making sure to pass
    //  to its base constructor an instance of a DataGridViewMaskedTextBoxCell
    //  class to use as the basic template.
    public DataGridViewMaskedTextBoxColumn() : base(new DataGridViewMaskedTextBoxCell())
    {
    }

    //  The template cell that will be used for this column by default,
    //  unless a specific cell is set for a particular row.
    //
    //  A DataGridViewMaskedTextBoxCell cell which will serve as the template cell
    //  for this column.
    public override DataGridViewCell CellTemplate
    {
        get
        {
            return base.CellTemplate;
        }
        set
        {
            //  Only cell types that derive from DataGridViewMaskedTextBoxCell are supported as the cell template.
            if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewMaskedTextBoxCell)))
            {
                string s = "Cell type is not based upon the DataGridViewMaskedTextBoxCell.";//CustomColumnMain.GetResourceManager().GetString("excNotDataGridViewMaskedTextBox");
                throw new InvalidCastException(s);
            }

            base.CellTemplate = value;
        }
    }

    public Color ForeColor
    {
        get
        {
            return foreColor;
        }
        set
        {
            foreColor = value;
            DataGridViewMaskedTextBoxOptions.foreClr = foreColor;
        }
    }

    //  Controls whether or not literal (non-prompt) characters should
    //  be included in the output of the Text property for newly entered
    //  data in a cell of this type.
    //
    //  See the DataGridViewMaskedTextBox control documentation for more details.
    public virtual bool IncludeLiterals
    {
        get
        {
            return this.includeLiterals;
        }
        set
        {
            DataGridViewMaskedTextBoxCell mtbc;
            DataGridViewCell dgvc;
            int rowCount;

            if (this.includeLiterals != value)
            {
                this.includeLiterals = value;
                //
                // first, update the value on the template cell.
                //
                mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                mtbc.IncludeLiterals = TriBool(value);
                //
                // now set it on all cells in other rows as well.
                //
                if (this.DataGridView != null && this.DataGridView.Rows != null)
                {
                    rowCount = this.DataGridView.Rows.Count;
                    for (int x = 0; x < rowCount; x++)
                    {
                        dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                        if (dgvc is DataGridViewMaskedTextBoxCell)
                        {
                            mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                            mtbc.IncludeLiterals = TriBool(value);
                        }
                    }
                }
            }
        }
    }

    //   Indicates whether any unfilled characters in the mask should be
    //   be included as prompt characters when somebody asks for the text
    //   of the DataGridViewMaskedTextBox for a particular cell programmatically.
    //
    //   See the DataGridViewMaskedTextBox control documentation for more details.
    public virtual bool IncludePrompt
    {
        get
        {
            return this.includePrompt;
        }
        set
        {
            DataGridViewMaskedTextBoxCell mtbc;
            DataGridViewCell dgvc;
            int rowCount;

            if (this.includePrompt != value)
            {
                this.includePrompt = value;

                //
                // first, update the value on the template cell.
                //
                mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                mtbc.IncludePrompt = TriBool(value);

                //
                // now set it on all cells in other rows as well.
                //
                if (this.DataGridView != null && this.DataGridView.Rows != null)
                {
                    rowCount = this.DataGridView.Rows.Count;
                    for (int x = 0; x < rowCount; x++)
                    {
                        dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                        if (dgvc is DataGridViewMaskedTextBoxCell)
                        {
                            mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                            mtbc.IncludePrompt = TriBool(value);
                        }
                    }
                }
            }
        }
    }

    //  Indicates the Mask property that is used on the DataGridViewMaskedTextBox
    //  for entering new data into cells of this type.
    //
    //  See the DataGridViewMaskedTextBox control documentation for more details.
    public virtual string Mask
    {
        get
        {
            return this.mask;
        }
        set
        {
            DataGridViewMaskedTextBoxCell mtbc;
            DataGridViewCell dgvc;
            int rowCount;

            if (this.mask != value)
            {
                this.mask = value;
                //
                // first, update the value on the template cell.
                //
                mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                mtbc.Mask = value;

                //
                // now set it on all cells in other rows as well.
                //
                if (this.DataGridView != null && this.DataGridView.Rows != null)
                {
                    rowCount = this.DataGridView.Rows.Count;
                    for (int x = 0; x < rowCount; x++)
                    {
                        dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                        if (dgvc is DataGridViewMaskedTextBoxCell)
                        {
                            mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                            mtbc.Mask = value;
                        }
                    }
                }
            }
        }
    }

    //  By default, the DataGridViewMaskedTextBox uses the underscore (_) character
    //  to prompt for required characters.  This propertly lets you
    //  choose a different one.
    //
    //  See the DataGridViewMaskedTextBox control documentation for more details.
    public virtual char PromptChar
    {
        get
        {
            return this.promptChar;
        }
        set
        {
            DataGridViewMaskedTextBoxCell mtbc;
            DataGridViewCell dgvc;
            int rowCount;

            if (this.promptChar != value)
            {
                this.promptChar = value;
                //
                // first, update the value on the template cell.
                //
                mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                mtbc.PromptChar = value;
                //
                // now set it on all cells in other rows as well.
                //
                if (this.DataGridView != null && this.DataGridView.Rows != null)
                {
                    rowCount = this.DataGridView.Rows.Count;
                    for (int x = 0; x < rowCount; x++)
                    {
                        dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                        if (dgvc is DataGridViewMaskedTextBoxCell)
                        {
                            mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                            mtbc.PromptChar = value;
                        }
                    }
                }
            }
        }
    }

    //  Indicates the type against any data entered in the DataGridViewMaskedTextBox
    //  should be validated.  The DataGridViewMaskedTextBox control will attempt to
    //  instantiate this type and assign the value from the contents of
    //  the text box.  An error will occur if it fails to assign to this
    //  type.
    //
    //  See the DataGridViewMaskedTextBox control documentation for more details.
    public virtual Type ValidatingType
    {
        get
        {
            return this.validatingType;
        }
        set
        {
            DataGridViewMaskedTextBoxCell mtbc;
            DataGridViewCell dgvc;
            int rowCount;

            if (this.validatingType != value)
            {
                this.validatingType = value;
                //
                // first, update the value on the template cell.
                //
                mtbc = (DataGridViewMaskedTextBoxCell)this.CellTemplate;
                mtbc.ValidatingType = value;
                //
                // now set it on all cells in other rows as well.
                //
                if (this.DataGridView != null && this.DataGridView.Rows != null)
                {
                    rowCount = this.DataGridView.Rows.Count;
                    for (int x = 0; x < rowCount; x++)
                    {
                        dgvc = this.DataGridView.Rows.SharedRow(x).Cells[x];
                        if (dgvc is DataGridViewMaskedTextBoxCell)
                        {
                            mtbc = (DataGridViewMaskedTextBoxCell)dgvc;
                            mtbc.ValidatingType = value;
                        }
                    }
                }
            }
        }
    }

    //CUSTOM CODE
    public override object Clone()
    {
        DataGridViewMaskedTextBoxColumn col = (DataGridViewMaskedTextBoxColumn)base.Clone();
        col.foreColor = this.foreColor;
        return col;
    }

    //  Routine to convert from boolean to DataGridViewTriState.
    private static DataGridViewTriState TriBool(bool value)
    {
        return value ? DataGridViewTriState.True : DataGridViewTriState.False;
    }
}