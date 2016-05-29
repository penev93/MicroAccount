using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace finalTaskz.Models.FAQs
{
    public class FaqViewModel
    {
        public static Expression<Func<FAQ, FaqViewModel>> FromFAQs
        {
            get
            {
                return Faq => new FaqViewModel
                {
                    IdQuestion = Faq.IdQuestion,
                    Title = Faq.Title,
                    Content=Faq.Content,
                };
            }
        }
        public string IdQuestion { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        
    }
}