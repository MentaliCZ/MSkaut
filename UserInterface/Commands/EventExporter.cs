using System;
using UserInterface.ViewModels.ModelRepresantations;
using Excel = Microsoft.Office.Interop.Excel;

namespace UserInterface.Commands
{
	public static class EventExporter
	{
        public static async Task Export(EventViewModel eventClass)
        {
            var excelApp = new Excel.Application();
            excelApp.Workbooks.Add();
            Excel._Worksheet transactionSheet = (Excel.Worksheet)excelApp.ActiveSheet;

            char column;
            int row;

            // Header
            transactionSheet.Cells[1, "B"] = "Pokladní kniha";
            transactionSheet.Range["B1", "D1"].Merge();

            ((Excel.Range)transactionSheet.Columns["A"]).ColumnWidth = 5;

            transactionSheet.Cells[3, "B"] = "Dne";
            ((Excel.Range)transactionSheet.Columns["B"]).ColumnWidth = 7;

            transactionSheet.Cells[3, "C"] = "Dokl.";
            ((Excel.Range)transactionSheet.Columns["C"]).ColumnWidth = 7;

            transactionSheet.Cells[3, "D"] = "Účel platby";
            ((Excel.Range)transactionSheet.Columns["D"]).ColumnWidth = 20;

            transactionSheet.Cells[3, "E"] = "Příjem";
            transactionSheet.Cells[3, "F"] = "Výdej";
            transactionSheet.Cells[3, "G"] = "Zůstatek";

            column = 'B';
            for (int idx = 1; idx <= 6; idx++)
            {
                transactionSheet.Cells[4, column] = idx.ToString();
                column++;
            }

            row = 4;
            for (int idx = 1; idx <= 40; idx++)
            {
                row++;
                transactionSheet.Cells[row, "A"] = idx.ToString();

                string remainingMoneyFormula = "= +E" + row + "-F" + row;
                if (idx != 1)
                    remainingMoneyFormula += "+ G" + (row - 1);
                transactionSheet.Cells[row, "G"] = remainingMoneyFormula;

                if (eventClass.Transactions.Count <= (idx - 1))
                    continue;

                TransactionViewModel transaction = eventClass.Transactions[idx - 1];

                transactionSheet.Cells[row, "B"] = transaction.Date.Day + ". " + transaction.Date.Month + ".";
                transactionSheet.Cells[row, "D"] = transaction.Name;

                if (transaction.Type.IsExpense)
                    transactionSheet.Cells[row, "F"] = transaction.Amount.ToString();
                else
                    transactionSheet.Cells[row, "E"] = transaction.Amount.ToString();

            }

            // Footer
            transactionSheet.Cells[45, "B"] = "Celkem";
            transactionSheet.Range["B45", "D45"].Merge();
            transactionSheet.Cells[45, "E"] = "=SUM(E5:E44)";
            transactionSheet.Cells[45, "F"] = "=SUM(F5:F44)";

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
