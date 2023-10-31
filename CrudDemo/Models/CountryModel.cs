using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace CrudDemo.Models
{
    public class CountryModel
    {
        [Key]
        public int Id{ get; set; }
        public string CountryName { get; set; }
    }
    public class Employee
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        

        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public CountryModel Country { get; set; }

        public int StateId { get; set; }

        [ForeignKey(nameof(StateId))]
        public State State { get; set; }



    }
    public class EmployeeVM
    {
        public string Id { get; set; }       
        public string FullName { get; set; }     
        public List<SelectListItem> State { get; set; }
        public List<SelectListItem> Districts { get; set; }
        public int SelectedState { get; set; }
        public string SelectedStateName { get; set; }
        public string SelectedDistrictName { get; set; }
        
        public int District { get; set; }



    }

public class State
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string statename { get; set; }

        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))] 
        public CountryModel Country { get; set; }

        [NotMapped]
        public List<SelectListItem> Countries { get; set; }

    }

}
