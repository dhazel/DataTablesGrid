using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DataTablesGrid.Components.DataTablesGrid;


/// <summary>
/// Represents a column in a DataTablesGrid<TGridItem> associated with an object's property.
/// </summary>
/// <typeparam name="TGridItem">The type of object represented by each row in the data grid.</typeparam>
/// <typeparam name="TProperty">The type of the property whose values are displayed in the column's cells.</typeparam>
public partial class GridColumn<TGridItem, TProperty> : Column<TGridItem>
{

    private Expression<Func<TGridItem, TProperty>>? _lastAssignedProperty;
    private Func<TGridItem, string?>? _cellTextFunc;

    public PropertyInfo? PropertyInfo { get; private set; }

    /// <summary>
    /// Defines the value to be displayed in this column's cells.
    /// </summary>
    [Parameter, EditorRequired] public Expression<Func<TGridItem, TProperty>> Property { get; set; } = default!;

    /// <summary>
    /// The format applied to property values.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>
    [Parameter]
    public string? Format { get; set; }


    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        // We have to do a bit of pre-processing on the lambda expression. Only do that if it's new or changed.
        if (_lastAssignedProperty != Property)
        {
            _lastAssignedProperty = Property;
            var compiledPropertyExpression = Property.Compile();

            if (!string.IsNullOrEmpty(Format))
            {
                // TODO: Consider using reflection to avoid having to box every value just to call IFormattable.ToString
                // For example, define a method "string Type<U>(Func<TGridItem, U> property) where U: IFormattable", and
                // then construct the closed type here with U=TProperty when we know TProperty implements IFormattable

                // If the type is nullable, we're interested in formatting the underlying type
                var nullableUnderlyingTypeOrNull = Nullable.GetUnderlyingType(typeof(TProperty));
                if (!typeof(IFormattable).IsAssignableFrom(nullableUnderlyingTypeOrNull ?? typeof(TProperty)))
                {
                    throw new InvalidOperationException($"A '{nameof(Format)}' parameter was supplied, but the type '{typeof(TProperty)}' does not implement '{typeof(IFormattable)}'.");
                }

                _cellTextFunc = item => ((IFormattable?)compiledPropertyExpression!(item))?.ToString(Format, null);
            }
            else
            {
                _cellTextFunc = item => compiledPropertyExpression!(item)?.ToString();
            }

        }

        if (Property.Body is MemberExpression memberExpression)
        {
            if (Title is null)
            {
                PropertyInfo = memberExpression.Member as PropertyInfo;

                /*var daText = memberExpression.Member.DeclaringType?.GetDisplayAttributeString(memberExpression.Member.Name);*/
                /*Title = !string.IsNullOrEmpty(daText) ? daText : memberExpression.Member.Name;*/

                Title = memberExpression.Member.Name;
            }
        }
    }

    /// <inheritdoc />
    protected internal override object CellContent(TGridItem item)
        => _cellTextFunc?.Invoke(item);


    protected internal override Type PropertyType
        => typeof(TProperty);

}

