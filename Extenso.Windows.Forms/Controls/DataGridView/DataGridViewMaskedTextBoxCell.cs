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

internal class DataGridViewMaskedTextBoxCell : DataGridViewTextBoxCell
{
    private DataGridViewTriState includeLiterals;
    private DataGridViewTriState includePrompt;
    private string mask;
    private char promptChar;
    private Type validatingType;

    //=------------------------------------------------------------------=
    // DataGridViewMaskedTextBoxCell
    //=------------------------------------------------------------------=
    /// <summary>
    ///   Initializes a new instance of this class.  Fortunately, there's
    ///   not much to do here except make sure that our base class is
    ///   also initialized properly.
    /// </summary>
    ///
    public DataGridViewMaskedTextBoxCell() : base()
    {
        this.mask = "";
        this.promptChar = '_';
        this.includePrompt = DataGridViewTriState.NotSet;
        this.includeLiterals = DataGridViewTriState.NotSet;
        this.validatingType = typeof(string);
        //CUSTOM CODE
        //this.Style.ForeColor = DataGridViewMaskedTextBoxOptions.foreClr;
    }

    //  Returns the type of the control that will be used for editing
    //  cells of this type.  This control must be a valid Windows Forms
    //  control and must implement IDataGridViewEditingControl.
    public override Type EditType => typeof(DataGridViewMaskedTextBoxEditingControl);

    //  A boolean value indicating whether to include literal characters
    //  in the Text property's output value.
    public virtual DataGridViewTriState IncludeLiterals
    {
        get => this.includeLiterals;
        set => this.includeLiterals = value;
    }

    //  A boolean indicating whether to include prompt characters in
    //  the Text property's value.
    public virtual DataGridViewTriState IncludePrompt
    {
        get => this.includePrompt;
        set => this.includePrompt = value;
    }

    //   A string value containing the Mask against input for cells of
    //   this type will be verified.
    public virtual string Mask
    {
        get => this.mask;
        set => this.mask = value;
    }

    //  The character to use for prompting for new input.
    //
    public virtual char PromptChar
    {
        get => this.promptChar;
        set => this.promptChar = value;
    }

    //  A Type object for the validating type.
    public virtual Type ValidatingType
    {
        get => this.validatingType;
        set => this.validatingType = value;
    }

    ///   Whenever the user is to begin editing a cell of this type, the editing
    ///   control must be created, which in this column type's
    ///   case is a subclass of the DataGridViewMaskedTextBox control.
    ///
    ///   This routine sets up all the properties and values
    ///   on this control before the editing begins.
    public override void InitializeEditingControl(int rowIndex,
        object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        DataGridViewMaskedTextBoxEditingControl mtbec;
        DataGridViewMaskedTextBoxColumn mtbcol;
        DataGridViewColumn dgvc;

        base.InitializeEditingControl(rowIndex, initialFormattedValue,
            dataGridViewCellStyle);

        mtbec = DataGridView.EditingControl as DataGridViewMaskedTextBoxEditingControl;
        //
        // set up props that are specific to the DataGridViewMaskedTextBox
        //
        dgvc = this.OwningColumn;   // this.DataGridView.Columns[this.ColumnIndex];
        if (dgvc is DataGridViewMaskedTextBoxColumn)
        {
            mtbcol = dgvc as DataGridViewMaskedTextBoxColumn;
            //
            // get the mask from this instance or the parent column.
            //
            mtbec.Mask = string.IsNullOrEmpty(this.mask) ? mtbcol.Mask : this.mask;
            //
            // prompt char.
            //
            mtbec.PromptChar = this.PromptChar;
            //
            // IncludePrompt
            //
            if (this.includePrompt == DataGridViewTriState.NotSet)
            {
                //mtbec.IncludePrompt = mtbcol.IncludePrompt;
            }
            else
            {
                //mtbec.IncludePrompt = BoolFromTri(this.includePrompt);
            }
            //
            // IncludeLiterals
            //
            if (this.includeLiterals == DataGridViewTriState.NotSet)
            {
                //mtbec.IncludeLiterals = mtbcol.IncludeLiterals;
            }
            else
            {
                //mtbec.IncludeLiterals = BoolFromTri(this.includeLiterals);
            }
            //
            // Finally, the validating type ...
            //
            mtbec.ValidatingType = this.ValidatingType ?? mtbcol.ValidatingType;

            mtbec.Text = (string)this.Value;
        }
    }

    //   Quick routine to convert from DataGridViewTriState to boolean.
    //   True goes to true while False and NotSet go to false.
    protected static bool BoolFromTri(DataGridViewTriState tri) => (tri == DataGridViewTriState.True) ? true : false;
}