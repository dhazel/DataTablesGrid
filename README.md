
DataTableGrid
=============

A simple Blazor wrapper for DataTables.net data grid.

## Usage

Example:

```razor
@rendermode InteractiveServer
@using DataTableGrid.Components.DataTableGrid

<DataTableGrid TGridItem="MyGridItem" Items="@items.AsQueryable()">
    <Columns>
        <GridColumn Property="@(p => p.Prop1)" Title="My Title"/>
        <GridColumn Property="@(p => p.Prop2)" Title="My Number"/>
        <GridColumn Property="@(p => p.Prop3)" Title="My Date" Format="yyyy-MM-dd"/>
    </Columns>
</DataTableGrid>


@code {
    public class MyGridItem
    {
        public string   Prop1 { get; set; }
        public int      Prop2 { get; set; }
        public DateTime Prop3 { get; set; }
    }

    public List<MyGridItem> items = new List<MyGridItem> 
    {
        new MyGridItem
        {
            Prop1 = "here",
            Prop2 = 20,
            Prop3 = DateTime.Now,
        },
        new MyGridItem
        {
            Prop1 = "and",
            Prop2 = 25,
            Prop3 = DateTime.Now,
        },
    };
}
```

