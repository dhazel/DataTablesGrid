using Microsoft.AspNetCore.Components;

namespace DataTablesGrid.Components.DataTablesGrid;


/// <summary>
/// Represents a grid column
/// </summary>
/// <typeparam name="TGridItem">The kind of item in this column</typeparam>
public abstract partial class Column<TGridItem>
{

    /// <summary>
    /// The data grid which owns this column.
    /// </summary>
    [CascadingParameter]
    public DataTablesGrid<TGridItem> DataGrid { get; set; }

    /// <summary>
    /// The value stored in this column.
    /// </summary>
    [Parameter] public TGridItem Value { get; set; }

    /// <summary>
    /// The display text for this column.
    /// </summary>
    [Parameter]
    public string Title { get; set; }



    protected override void OnInitialized()
    {

        DataGrid?.AddColumn(this);

    }


    /// <summary>
    /// Releases resources used by this column.
    /// </summary>
    public virtual void Dispose()
    {
        DataGrid?.RemoveColumn(this);
    }


    #region Abstract Members



    protected internal abstract object CellContent(TGridItem item);


    protected internal virtual Type PropertyType { get; }

    /*protected internal abstract void SetProperty(object item, object value);*/

    #endregion
}

