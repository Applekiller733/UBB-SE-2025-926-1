using LoanShark.Domain;
using LoanShark.EF.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.EF.Mappers
{
    public static class ReportMapper
    {
         public static Report ToDomainReport(ReportEF reportEF)
        {
            if (reportEF == null)
            {
                throw new ArgumentNullException(nameof(reportEF));
            }

            // if ok
            return new Report(reportEF.MessageID, reportEF.ReporterUserID, reportEF.Status, reportEF.Reason, reportEF.Description);
        }
    }
}
