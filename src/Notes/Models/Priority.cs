using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public enum PriorityEnum
    {
        [Display(Name = "Very Low")]
        VeryLow = 1,

        [Display(Name = "Low")]
        Low = 2,

        [Display(Name = "Normal")]
        Normal = 3,

        [Display(Name = "High")]
        High = 4,

        [Display(Name = "Very High")]
        VeryHigh = 5
    }

    public class Priority
    {
        public static List<SelectListItem> getPriorityList()
        {
            List<SelectListItem> PriorityList = new List<SelectListItem>();
            foreach (PriorityEnum eVal in Enum.GetValues(typeof(PriorityEnum)))
            {
                // TODO proper naming of prios
                PriorityList.Add(new SelectListItem { Text = Enum.GetName(typeof(PriorityEnum), eVal), Value = eVal.ToString() });
            }
            return PriorityList;
        }
    }
}
