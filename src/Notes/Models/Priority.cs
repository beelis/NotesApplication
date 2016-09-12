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
        VeryLow,

        [Display(Name = "Low")]
        Low,

        [Display(Name = "Normal")]
        Normal,

        [Display(Name = "High")]
        High,

        [Display(Name = "Very High")]
        VeryHigh
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
