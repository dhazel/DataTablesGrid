
export function initializeDataTable(element) {
    return new DataTable(element);
};

export function updateData(dataTable, data) {
    dataTable.clear();
    dataTable.rows.add(data);
    dataTable.draw();
}
