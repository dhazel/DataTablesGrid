using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DataTableGrid.Components.DataTableGrid;


[CascadingTypeParameter(nameof(TGridItem))]
public partial class DataTableGrid<TGridItem>
{

    private const string JAVASCRIPT_FILE = "./_content/DataTableGrid/Components/DataTableGrid/DataTableGrid.razor.js";

    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;

    /// <summary>
    /// The child element to contain the grid's column definitions
    /// </summary>
    [Parameter]
    public RenderFragment Columns { get; set; }

    [Parameter]
    public IQueryable<TGridItem> Items { get; set; }

    [Parameter]
    public bool Loading { get; set; }

    private IJSObjectReference? _jsModule;
    private IJSObjectReference? _dataTableReference;
    private ElementReference? _gridReference;

    /// <summary>
    /// The columns available to be displayed
    /// </summary>
    public readonly List<Column<TGridItem>> ColumnsToRender = [];

    internal void AddColumn(Column<TGridItem> column)
    {
        ColumnsToRender.Add(column);
    }

    internal void RemoveColumn<TGridItem>(Column<TGridItem> columnBase)
    {
        throw new NotImplementedException();
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_gridReference is not null)
        {
            if (firstRender)
            {
                _jsModule ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
                try
                {
                    _dataTableReference = await _jsModule
                        .InvokeAsync<IJSObjectReference>("initializeDataTable", _gridReference);
                }
                catch (JSException ex)
                {
                    Console.WriteLine("[DataTableGrid] " + ex.Message);
                }
            }
            else
            {
                await _jsModule.InvokeVoidAsync(
                    "updateData",
                    _dataTableReference,
                    GetData()
                );
            }
        }

    }

    private List<List<string>> GetData()
    {
        var returnValue = new List<List<string>>();
        foreach( var item in Items )
        {
            var row = new List<string>();
            foreach( var column in ColumnsToRender )
            {
                row.Add(column.CellContent(item)?.ToString());
            }
            returnValue.Add(row);
        }
        return returnValue;
    }
}
