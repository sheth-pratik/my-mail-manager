using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPMailCommunicator.Model
{
    public class MailMessageListModel
    {
        public MailMessageListModel(int index, int pageing, int count, int totalRecords, List<MailMessageWrapper> mailMessageList)
        {
            this.CurrentPage = pageing;
            this.Count = count;
            this.TotalRecords = totalRecords;
            this.mailMessageList = mailMessageList;
            this.Index = index;
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Floor((double)TotalRecords / Count);
            }
        }

        public int CurrentPage
        {
            get;
            set;
        }

        public int TotalRecords
        {
            get;
            set;
        }

        public List<int> PageList
        {
            get
            {
                List<int> pageList = new List<int>();
                if (TotalPages > 1)
                {
                    pageList.Add(0);
                    pageList.Add(1);
                    if (!pageList.Contains(CurrentPage - 1) && CurrentPage - 1 > 0)
                    {
                        pageList.Add(CurrentPage - 1);
                    }
                    if (!pageList.Contains(CurrentPage) && CurrentPage > 0)
                    {
                        pageList.Add(CurrentPage);
                    }
                    if (!pageList.Contains(CurrentPage + 1) && CurrentPage + 1 > 0 && CurrentPage + 1 < TotalPages)
                    {
                        pageList.Add(CurrentPage + 1);
                    }
                    if (!pageList.Contains(TotalPages - 2) && TotalPages - 2 > 0)
                    {
                        pageList.Add(TotalPages - 2);
                    }
                    if (!pageList.Contains(TotalPages - 1) && TotalPages - 1 > 0)
                    {
                        pageList.Add(TotalPages - 1);
                    }

                }
                return pageList;
            }
        }

        private List<MailMessageWrapper> mailMessageList = new List<MailMessageWrapper>();
        public List<MailMessageWrapper> MailMessageList
        {
            get { return mailMessageList; }
        }

        public int Count { get; set; }

        public int Index { get; private set; }
    }
}
