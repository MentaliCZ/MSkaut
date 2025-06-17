using System;
using UserInterface.ViewModels.ModelRepresantations;
using System.Collections.ObjectModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace UserInterface.Commands
{
	public static class EventExporter
	{
        public static async Task Export(EventViewModel eventClass)
        {
            var excelApp = new Excel.Application();
            excelApp.ScreenUpdating = false;

            excelApp.Workbooks.Add();
            Excel._Worksheet transactionSheet = (Excel.Worksheet)excelApp.ActiveSheet;

            int height = eventClass.Transactions.Count + 5 > 45 ? eventClass.Transactions.Count + 5 : 45;

            char column;
            int row;

            // Header
            transactionSheet.Cells[1, "B"] = "Pokladní kniha";
            transactionSheet.Range["B1:D1"].Merge();
            ((Excel.Range)transactionSheet.Cells[1, "B"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Cells[2, "B"] = "Akce: " + eventClass.Name;
            transactionSheet.Range["B2:D2"].Merge();

            transactionSheet.Range["B1:H2"].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            ((Excel.Range)transactionSheet.Columns["A"]).ColumnWidth = 5;

            transactionSheet.Cells[3, "B"] = "Dne";
            ((Excel.Range)transactionSheet.Cells[3, "B"]).Font.Size = 10;
            ((Excel.Range)transactionSheet.Columns["B"]).ColumnWidth = 7;
            ((Excel.Range)transactionSheet.Cells[3, "B"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


            transactionSheet.Cells[3, "C"] = "Dokl.";
            ((Excel.Range)transactionSheet.Cells[3, "C"]).Font.Size = 10;
            ((Excel.Range)transactionSheet.Columns["C"]).ColumnWidth = 10;
            ((Excel.Range)transactionSheet.Cells[3, "C"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


            transactionSheet.Cells[3, "D"] = "Účel platby";
            ((Excel.Range)transactionSheet.Cells[3, "D"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Columns["D"]).ColumnWidth = 20;
            ((Excel.Range)transactionSheet.Cells[3, "D"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Cells[3, "E"] = "Kategorie";
            ((Excel.Range)transactionSheet.Cells[3, "E"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Columns["E"]).ColumnWidth = 12;
            ((Excel.Range)transactionSheet.Cells[3, "E"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Cells[3, "F"] = "Příjem";
            ((Excel.Range)transactionSheet.Cells[3, "F"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[3, "F"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


            transactionSheet.Cells[3, "G"] = "Výdej";
            ((Excel.Range)transactionSheet.Cells[3, "G"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[3, "G"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            transactionSheet.Cells[3, "H"] = "Zůstatek";
            ((Excel.Range)transactionSheet.Cells[3, "H"]).Font.Size = 8;
            ((Excel.Range)transactionSheet.Cells[3, "H"]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


            column = 'B';
            for (int idx = 1; idx <= 7; idx++)
            {
                transactionSheet.Cells[4, column.ToString()] = idx.ToString();
                ((Excel.Range)transactionSheet.Cells[4, column.ToString()]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[4, column.ToString()]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                column++;
            }

            row = 4;
            for (int idx = 1; idx <= height - 5; idx++)
            {
                row++;
                Excel.Range leftCell = (Excel.Range)transactionSheet.Cells[row, "A"];
                leftCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                leftCell.Value = idx.ToString();
                leftCell.Font.Size = 10;

                (transactionSheet.Range["B" + row, "H" + row]).Font.Size = 8;

                string remainingMoneyFormula = "= +F" + row + "-G" + row;

                if (idx != 1)
                    remainingMoneyFormula += "+ H" + (row - 1);

                transactionSheet.Cells[row, "H"] = remainingMoneyFormula;

                // Borders
                ((Excel.Range)transactionSheet.Cells[row, "B"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "C"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "D"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "E"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "F"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "G"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)transactionSheet.Cells[row, "H"]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;



                if (eventClass.Transactions.Count <= (idx - 1))
                    continue;

                TransactionViewModel transaction = eventClass.Transactions[idx - 1];

                transactionSheet.Cells[row, "B"] = transaction.Date.Day + ". " + transaction.Date.Month + ".";
                ((Excel.Range)transactionSheet.Cells[row, "B"]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                transactionSheet.Cells[row, "C"] = transaction.DocumentName;

                transactionSheet.Cells[row, "D"] = transaction.Name;

                if (transaction.Type != null)
                    transactionSheet.Cells[row, "E"] = transaction.Type.ToString();


                if (transaction.Type.IsExpense)
                    transactionSheet.Cells[row, "G"] = transaction.Amount.ToString();
                else
                    transactionSheet.Cells[row, "F"] = transaction.Amount.ToString();

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
