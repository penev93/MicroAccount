using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace finalTaskz.Models.FAQs
{
    public class FAQContentViewModel
    {
        public static Expression<Func<FAQ, FAQContentViewModel>> FromFAQsContent
        {
            get
            {
                return Faq => new FAQContentViewModel
                {
                   Content=Faq.Content,
                };
            }
        }
        public string Content { get; set; }
        
    }
}