using System;
using UserInterface.ViewModels.ModelRepresantations;
using System.Collections.ObjectModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace UserInterface.Commands
{
	public static class EventExporter
	{
        private static int HEADER_SIZE = 5;
        private static int DEFAULT_SIZE = 45;

        public static async Task Export(EventViewModel eventClass)
        {
            var excelApp = new Excel.Application();
            excelApp.ScreenUpdating = false;

            excelApp.Workbooks.Add();
            Excel._Worksheet transactionSheet = (Excel.Worksheet)excelApp.ActiveSheet;

            int height = eventClass.Transactions.Count + HEADER_SIZE > 45 ? eventClass.Transactions.Count + HEADER_SIZE : DEFAULT_SIZE;
            Excel.Range rowCells;

            int row;

            //Column widths
            ((Excel.Range)transactionSheet.Columns["A"]).ColumnWidth = 5;
            ((Excel.Range)transactionSheet.Columns["B"]).ColumnWidth = 7;
            ((Excel.Range)transactionSheet.Columns["C"]).ColumnWidth = 10;
            ((Excel.Range)transactionSheet.Columns["D"]).ColumnWidth = 20;
            ((Excel.Range)transactionSheet.Columns["E"]).ColumnWidth = 12;

            // Header
            rowCells = transactionSheet.Range["B" + 1, "H" + 1];

            rowCells[1, 1] = "Pokladní kniha";
            rowCells.Range["A1:C1"].Merge();
            ((Excel.Range)rowCells[1, 1]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Range["B1:H2"].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            // header row 2
            transactionSheet.Cells[2, "B"] = "Akce: " + eventClass.Name;
            transactionSheet.Range["B2:D2"].Merge();

            // header row 3
            rowCells = transactionSheet.Range["A" + 3, "H" + 3];
            rowCells.Range["B1:C1"].Font.Size = 10;
            rowCells.Range["D1:H1"].Font.Size = 8;

            rowCells[1, 2] = "Dne";
            ((Excel.Range)rowCells[1, 2]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            rowCells[1, 3] = "Dokl.";
            ((Excel.Range)rowCells[1, 3]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            rowCells[1, 4] = "Účel platby";
            ((Excel.Range)rowCells[1, 4]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            rowCells[1, 5] = "Kategorie";
            ((Excel.Range)rowCells[1, 5]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            rowCells[1, 6] = "Příjem";
            ((Excel.Range)rowCells[1, 6]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            rowCells[1, 7] = "Výdej";
            ((Excel.Range)rowCells[1, 7]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            rowCells[1, 8] = "Zůstatek";
            ((Excel.Range)rowCells[1, 8]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            rowCells = transactionSheet.Range["B4:H4"];
            for (int idx = 1; idx <= 7; idx++)
            {
                rowCells[1, idx] = idx.ToString();
                ((Excel.Range)rowCells[1, idx]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)rowCells[1, idx]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }

            row = 4;
            for (int idx = 1; idx <= height - 5; idx++)
            {
                row++;

                rowCells = transactionSheet.Range["A" + row, "H" + row];

                Excel.Range leftCell = (Excel.Range)rowCells[1, 1];
                leftCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                leftCell.Value = idx.ToString();
                leftCell.Font.Size = 10;

                rowCells.Font.Size = 8;
                string remainingMoneyFormula = "= +F" + row + "-G" + row;

                if (idx != 1)
                    remainingMoneyFormula += "+ H" + (row - 1);

                rowCells[1, 8] = remainingMoneyFormula;

                // Borders
                ((Excel.Range)rowCells[1, 2]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)rowCells[1, 3]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)rowCells[1, 4]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)rowCells[1, 5]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)rowCells[1, 6]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)rowCells[1, 7]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)rowCells[1, 8]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;


                if (eventClass.Transactions.Count <= (idx - 1))
                    continue;

                TransactionViewModel transaction = eventClass.Transactions[idx - 1];

                rowCells[1, 2] = transaction.Date.Day + ". " + transaction.Date.Month + ".";
                ((Excel.Range)rowCells[1, 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                rowCells[1, 3] = transaction.DocumentName;
                rowCells[1, 4] = transaction.Name;

                if (transaction.Type != null)
                    rowCells[1, 5] = transaction.Type.ToString();

                if (transaction.Type.IsExpense)
                    rowCells[1, 6] = transaction.Amount.ToString();
                else
                    rowCells[1, 7] = transaction.Amount.ToString();

            }

            // Footer
            transactionSheet.Cells[height, "B"] = "Celkem";
            transactionSheet.Range["B" + height, "D" + height].Merge();

            transactionSheet.Cells[height, "F"] = "=SUM(F5:F" + (height - 1) + ")";
            ((Excel.Range)transactionSheet.Cells[height, "F"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[height, "F"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Cells[height, "G"] = "=SUM(G5:G" + (height - 1) + ")";
            ((Excel.Range)transactionSheet.Cells[height, "G"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[height, "G"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.get_Range("B45:H45").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            //***********************************************************************************
            // Save logic
            //***********************************************************************************

            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Output"; // Default file name
            dlg.DefaultExt = ".xlsx"; // Default file extension
            dlg.Filter = "Excel documents (.xlsx)|*.xlsx"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();


            // Process save file dialog box results
            if (result.Value)
            {
                // Save document
                string savePath = dlg.FileName;
                transactionSheet.SaveAs(savePath);
            }


            excelApp.Quit();

        }

    }
}
