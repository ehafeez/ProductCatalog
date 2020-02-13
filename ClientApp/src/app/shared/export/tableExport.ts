import * as XLSX from "xlsx";

export class TableExport {
  static exportToExcel(tableId: string, fileName: string) {
    var table = document.getElementById(tableId);

    var sheet = XLSX.utils.table_to_book(table, <XLSX.Table2SheetOpts>{ sheet: "sheet1" });
    XLSX.writeFile(sheet, `${fileName}.xlsx`);
  }
}
