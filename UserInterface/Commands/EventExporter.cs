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
            ((Excel.Range)transactionSheet.Cells[1, "B"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            ((Excel.Range)transactionSheet.Columns["A"]).ColumnWidth = 5;

            transactionSheet.Cells[3, "B"] = "Dne";
            ((Excel.Range)transactionSheet.Cells[3, "B"]).Font.Size = 10;
            ((Excel.Range)transactionSheet.Columns["B"]).ColumnWidth = 7;
            ((Excel.Range)transactionSheet.Cells[3, "B"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


            transactionSheet.Cells[3, "C"] = "Dokl.";
            ((Excel.Range)transactionSheet.Cells[3, "C"]).Font.Size = 10;
            ((Excel.Range)transactionSheet.Columns["C"]).ColumnWidth = 7;
            ((Excel.Range)transactionSheet.Cells[3, "C"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


            transactionSheet.Cells[3, "D"] = "Účel platby";
            ((Excel.Range)transactionSheet.Cells[3, "D"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Columns["D"]).ColumnWidth = 20;
            ((Excel.Range)transactionSheet.Cells[3, "D"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Cells[3, "E"] = "Příjem";
            ((Excel.Range)transactionSheet.Cells[3, "E"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[3, "E"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


            transactionSheet.Cells[3, "F"] = "Výdej";
            ((Excel.Range)transactionSheet.Cells[3, "F"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[3, "F"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Cells[3, "G"] = "Zůstatek";
            ((Excel.Range)transactionSheet.Cells[3, "G"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[3, "G"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


            column = 'B';
            for (int idx = 1; idx <= 6; idx++)
            {
                transactionSheet.Cells[4, column.ToString()] = idx.ToString();
                ((Excel.Range)transactionSheet.Cells[4, column.ToString()]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[4, column.ToString()]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                column++;
            }

            row = 4;
            for (int idx = 1; idx <= 40; idx++)
            {
                row++;
                Excel.Range leftCell = (Excel.Range)transactionSheet.Cells[row, "A"];
                leftCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                leftCell.Value = idx.ToString();
                leftCell.Font.Size = 10;

                string remainingMoneyFormula = "= +E" + row + "-F" + row;

                if (idx != 1)
                    remainingMoneyFormula += "+ G" + (row - 1);

                transactionSheet.Cells[row, "G"] = remainingMoneyFormula;
                ((Excel.Range)transactionSheet.Cells[row, "G"]).Font.Size = 8;

                // Borders
                ((Excel.Range)transactionSheet.Cells[row, "B"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "C"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "D"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "E"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "F"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "G"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;


                if (eventClass.Transactions.Count <= (idx - 1))
                    continue;

                TransactionViewModel transaction = eventClass.Transactions[idx - 1];

                transactionSheet.Cells[row, "B"] = transaction.Date.Day + ". " + transaction.Date.Month + ".";
                ((Excel.Range)transactionSheet.Cells[row, "B"]).Font.Size = 8;
                ((Excel.Range)transactionSheet.Cells[row, "B"]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;



                transactionSheet.Cells[row, "D"] = transaction.Name;
                ((Excel.Range)transactionSheet.Cells[row, "D"]).Font.Size = 8;


                if (transaction.Type.IsExpense)
                {
                    transactionSheet.Cells[row, "F"] = transaction.Amount.ToString();
                    ((Excel.Range)transactionSheet.Cells[row, "F"]).Font.Size = 8;
                }
                else
                {
                    transactionSheet.Cells[row, "E"] = transaction.Amount.ToString();
                    ((Excel.Range)transactionSheet.Cells[row, "E"]).Font.Size = 8;
                }

            }

            // Footer
            transactionSheet.Cells[45, "B"] = "Celkem";
            transactionSheet.Range["B45", "D45"].Merge();

            transactionSheet.Cells[45, "E"] = "=SUM(E5:E44)";
            ((Excel.Range)transactionSheet.Cells[45, "E"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[45, "E"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Cells[45, "F"] = "=SUM(F5:F44)";
            ((Excel.Range)transactionSheet.Cells[45, "F"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[45, "F"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


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
