using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Expense;
using System;
using System.Collections.Generic;

namespace CTGroup.Service.Expense.Interfaces
{
    public interface iExpenseEntryMgt
    {
        object[] GetExpenseByID(vmCmnParameters objcmnParam, Int64 BuildingId);
        string SaveUpdateExpense(vmCmnParameters objcmnParam, vmExpenseEntry objExEntry, List<vmExpenseEntry> expenseEntries);

        IEnumerable<vmExpenseEntry> GetExpenseMaster(vmCmnParameters objcmnParam, out long recordsTotal);
    }
}
