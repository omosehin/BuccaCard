using System;
using System.Threading.Tasks;

namespace Buccacard.Services.ReportService
{
    public interface IReportJob
    {
        void GetCreatedCard();
    }
    public class ReportJob : IReportJob
    {
        public void GetCreatedCard()
        {
            //do anthing here and send email report
            return;
        }
    }
}
